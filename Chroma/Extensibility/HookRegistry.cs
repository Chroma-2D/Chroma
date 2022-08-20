using System;
using System.Collections.Generic;
using System.Reflection;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.GameControllers;

namespace Chroma.Extensibility
{
    public static class HookRegistry
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();
        private static Game _owner;

        private static readonly Dictionary<HookPoint, List<MethodInfo>> _prefixHooks = new();
        private static readonly Dictionary<HookPoint, List<MethodInfo>> _postfixHooks = new();

        internal static void Initialize(Game owner)
        {
            _owner = owner;
            InitializeHookCollections();
        }

        public static void AttachAll()
        {
            AttachAll(Assembly.GetCallingAssembly());
        }
        
        public static void AttachAll(Assembly assembly)
        {
            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                var methods = type.GetMethods(
                    BindingFlags.Static
                    | BindingFlags.NonPublic
                    | BindingFlags.Public
                );

                foreach (var method in methods)
                {
                    var attribute = method.GetCustomAttribute<HookAttribute>();

                    if (attribute == null)
                        continue;

                    Attach(attribute.HookPoint, attribute.HookAttachment, method);
                }
            }
        }

        public static void Attach(HookPoint hookPoint, HookAttachment hookAttachment, Delegate method)
        {
            Attach(hookPoint, hookAttachment, method.GetMethodInfo());
        }

        public static void Detach(HookPoint hookPoint, HookAttachment hookAttachment, Delegate method)
        {
            Detach(hookPoint, hookAttachment, method.GetMethodInfo());
        }

        internal static void WrapCall<T>(HookPoint hookPoint, T argument, Action<T> action)
        {
            if (InvokePrefixHooks(hookPoint, argument))
            {
                action(argument);
                InvokePostfixHooks(hookPoint, argument);
            }
        }

        private static void Attach(HookPoint hookPoint, HookAttachment hookAttachment, MethodInfo method)
        {
            var type = method.DeclaringType!;            

            if (hookAttachment == HookAttachment.Prefix)
            {
                if (!IsValidPrefixHook(method, hookPoint))
                {
                    _log.Warning(
                        $"Method '{method.Name}' from '{type.FullName}' is marked as a prefix hook " +
                        $"for '{hookPoint}' but its return value or signature is invalid."
                    );

                    return;
                }

                if (_prefixHooks[hookPoint].Contains(method))
                {
                    _log.Warning(
                        $"Attempted to create a duplicate prefix hook with method '{method.Name}' " +
                        $"from '{type.FullName}' for '{hookPoint}'. The request has been ignored."
                    );
                    
                    return;
                }
                
                _prefixHooks[hookPoint].Add(method);
            }
            else if (hookAttachment == HookAttachment.Postfix)
            {
                if (!IsValidHook(method, hookPoint))
                {
                    _log.Warning(
                        $"Method '{method.Name}' from '{type.FullName}' is marked as a postfix hook " +
                        $"for '{hookPoint}' but its signature is invalid."
                    );

                    return;
                }
                
                if (_postfixHooks[hookPoint].Contains(method))
                {
                    _log.Warning(
                        $"Attempted to create a duplicate postfix hook with method '{method.Name}' " +
                        $"from '{type.FullName}' for '{hookPoint}'. The request has been ignored."
                    );
                    
                    return;
                }

                _postfixHooks[hookPoint].Add(method);
            }
            else
            {
                throw new NotSupportedException(
                    $"Hook attachment '{hookAttachment}' is not supported.");
            }
        }

        private static void Detach(HookPoint hookPoint, HookAttachment hookAttachment, MethodInfo method)
        {
            bool wasRemoved;

            if (hookAttachment == HookAttachment.Prefix)
            {
                wasRemoved = _prefixHooks[hookPoint].Remove(method);
            }
            else if (hookAttachment == HookAttachment.Postfix)
            {
                wasRemoved = _postfixHooks[hookPoint].Remove(method);
            }
            else
            {
                throw new NotSupportedException($"Unsupported hook attachment '{hookAttachment}'.");
            }

            if (!wasRemoved)
            {
                _log.Warning(
                    $"Attempted to unregister a '{hookAttachment.ToString().ToLower()} hook " +
                    $"'{method.Name}' for '{hookPoint}' " +
                    $"declared by '{method.DeclaringType?.FullName}', however none was registered beforehand."
                );
            }
        }

        private static bool InvokePrefixHooks<T>(HookPoint hookPoint, T argument)
        {
            var continueToMainBody = true;
            var hookCollection = _prefixHooks[hookPoint];

            for (var i = 0; i < hookCollection.Count; i++)
            {
                continueToMainBody &= (bool)hookCollection[i].Invoke(null, new object[] { _owner, argument })!;
            }

            return continueToMainBody;
        }

        private static void InvokePostfixHooks<T>(HookPoint hookPoint, T argument)
        {
            var hookCollection = _postfixHooks[hookPoint];

            for (var i = 0; i < hookCollection.Count; i++)
            {
                hookCollection[i].Invoke(null, new object[] { _owner, argument });
            }
        }

        private static void InitializeHookCollections()
        {
            var enumEntryCount = Enum.GetNames(typeof(HookPoint)).Length;

            for (var i = 0; i < enumEntryCount; i++)
            {
                _prefixHooks.Add((HookPoint)i, new List<MethodInfo>());
                _postfixHooks.Add((HookPoint)i, new List<MethodInfo>());
            }
        }

        private static bool IsValidPrefixHook(MethodInfo method, HookPoint hookPoint)
        {
            return HasBooleanReturnType(method)
                   && IsValidHook(method, hookPoint);
        }

        private static bool HasBooleanReturnType(MethodInfo method)
        {
            return method.ReturnType == typeof(bool);
        }

        private static bool MatchesSignature(MethodInfo method, params Type[] parameterTypes)
        {
            var parameters = method.GetParameters();

            if (parameters.Length != parameterTypes.Length)
                return false;

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType != parameterTypes[i])
                    return false;
            }

            return true;
        }

        private static bool IsValidHook(MethodInfo method, HookPoint hookPoint)
        {
            return hookPoint switch
            {
                HookPoint.Draw
                    => MatchesSignature(method, typeof(Game), typeof(RenderContext)),

                HookPoint.Update
                    => MatchesSignature(method, typeof(Game), typeof(float)),

                HookPoint.FixedUpdate
                    => MatchesSignature(method, typeof(Game), typeof(float)),

                HookPoint.MouseMoved
                    => MatchesSignature(method, typeof(Game), typeof(MouseMoveEventArgs)),

                HookPoint.MousePressed
                    => MatchesSignature(method, typeof(Game), typeof(MouseButtonEventArgs)),

                HookPoint.MouseReleased
                    => MatchesSignature(method, typeof(Game), typeof(MouseButtonEventArgs)),

                HookPoint.WheelMoved
                    => MatchesSignature(method, typeof(Game), typeof(MouseWheelEventArgs)),

                HookPoint.KeyPressed
                    => MatchesSignature(method, typeof(Game), typeof(KeyEventArgs)),

                HookPoint.KeyReleased
                    => MatchesSignature(method, typeof(Game), typeof(KeyEventArgs)),

                HookPoint.TextInput
                    => MatchesSignature(method, typeof(Game), typeof(TextInputEventArgs)),

                HookPoint.ControllerConnected
                    => MatchesSignature(method, typeof(Game), typeof(ControllerEventArgs)),

                HookPoint.ControllerDisconnected
                    => MatchesSignature(method, typeof(Game), typeof(ControllerEventArgs)),

                HookPoint.ControllerButtonPressed
                    => MatchesSignature(method, typeof(Game), typeof(ControllerButtonEventArgs)),

                HookPoint.ControllerButtonReleased
                    => MatchesSignature(method, typeof(Game), typeof(ControllerButtonEventArgs)),

                HookPoint.ControllerAxisMoved
                    => MatchesSignature(method, typeof(Game), typeof(ControllerAxisEventArgs)),

                HookPoint.ControllerTouchpadMoved
                    => MatchesSignature(method, typeof(Game), typeof(ControllerTouchpadEventArgs)),

                HookPoint.ControllerTouchpadTouched
                    => MatchesSignature(method, typeof(Game), typeof(ControllerTouchpadEventArgs)),

                HookPoint.ControllerTouchpadReleased
                    => MatchesSignature(method, typeof(Game), typeof(ControllerTouchpadEventArgs)),

                HookPoint.ControllerGyroscopeStateChanged
                    => MatchesSignature(method, typeof(Game), typeof(ControllerSensorEventArgs)),

                HookPoint.ControllerAccelerometerStateChanged
                    => MatchesSignature(method, typeof(Game), typeof(ControllerSensorEventArgs)),

                _ => throw new NotSupportedException($"Hook point {hookPoint} is not supported.")
            };
        }
    }
}