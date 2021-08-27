using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;

namespace Chroma.Extensibility
{
    internal static class ExtensionRegistry
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        private static readonly List<HookInfo> _afterUpdateHooks = new();
        private static readonly List<HookInfo> _beforeUpdateHooks = new();
        private static readonly List<HookInfo> _beforeFixedUpdateHooks = new();
        private static readonly List<HookInfo> _afterFixedUpdateHooks = new();
        private static readonly List<HookInfo> _afterDrawHooks = new();
        private static readonly List<HookInfo> _beforeDrawHooks = new();

        internal static void FindAndLoadExtensions(Game game)
        {
            var localDirectory = AppContext.BaseDirectory;
            var dllFiles = Directory.GetFiles(localDirectory, "*.dll");

            for (var i = 0; i < dllFiles.Length; i++)
            {
                var fileName = dllFiles[i];

                // This has a fortunate or unfortunate effect
                // of calling all the module initializers for
                // all managed DLLs that haven't been loaded
                // by the framework just yet.
                if (!TryLoadConformingAssembly(fileName, out Assembly asm))
                    continue;

                if (!IsValidExtensionAssembly(asm))
                    continue;

                if (!TryRetrieveEntryPoint(asm, out Type entryType))
                    continue;

                if (!TryRetrieveConstructor(entryType, out ConstructorInfo constructor))
                    continue;

                if (!ConstructAndRegisterExtension(constructor, game, out object instance))
                    continue;

                RegisterExtensionHooks(entryType, instance);
            }
        }

        internal static bool InvokeBeforeUpdateHooks(float delta)
            => InvokeBeforeHooks(_beforeUpdateHooks, delta);

        internal static void InvokeAfterUpdateHooks(float delta)
            => InvokeAfterHooks(_afterUpdateHooks, delta);

        internal static bool InvokeBeforeFixedUpdateHooks(float delta)
            => InvokeBeforeHooks(_beforeFixedUpdateHooks, delta);

        internal static void InvokeAfterFixedUpdateHooks(float delta)
            => InvokeAfterHooks(_afterFixedUpdateHooks, delta);

        internal static bool InvokeBeforeDrawHooks(RenderContext context)
            => InvokeBeforeHooks(_beforeDrawHooks, context);

        internal static void InvokeAfterDrawHooks(RenderContext context)
            => InvokeAfterHooks(_afterDrawHooks, context);

        private static bool TryLoadConformingAssembly(string filePath, out Assembly asm)
        {
            asm = null;

            if (!File.Exists(filePath))
                return false;

            try
            {
                asm = Assembly.LoadFrom(filePath);
            }
            catch (BadImageFormatException)
            {
                // Don't log messages about invalid images.
                return false;
            }
            catch (Exception ex)
            {
                _log.Exception(ex);
                return false;
            }

            return true;
        }

        private static bool IsValidExtensionAssembly(Assembly asm)
            => asm.GetCustomAttribute<ChromaExtensionAttribute>() != null;

        private static bool TryRetrieveEntryPoint(Assembly asm, out Type entryType)
        {
            entryType = null;

            var types = asm.GetTypes();

            for (var i = 0; i < types.Length; i++)
            {
                var type = types[i];

                if (!type.IsSealed)
                    continue;

                if (type.GetCustomAttribute<EntryPointAttribute>() == null)
                    continue;

                entryType = type;

                return true;
            }

            return false;
        }

        private static bool TryRetrieveConstructor(Type entryType, out ConstructorInfo constructor)
        {
            constructor = entryType.GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(Game) },
                null
            );

            return constructor != null;
        }

        private static bool ConstructAndRegisterExtension(ConstructorInfo constructor, Game game, out object instance)
        {
            try
            {
                instance = constructor.Invoke(new object[] { game });

                if (instance != null)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                instance = null;
                _log.Exception(e);
            }

            return false;
        }

        private static void RegisterExtensionHooks(Type entryType, object instance)
        {
            var methods = entryType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            for (var i = 0; i < methods.Length; i++)
            {
                var method = methods[i];

                if (IsValidHookMethod<BeforeUpdateAttribute, float>(method))
                    _beforeUpdateHooks.Add(new HookInfo(method, instance));
                else if (IsValidHookMethod<AfterUpdateAttribute, float>(method))
                    _afterUpdateHooks.Add(new HookInfo(method, instance));
                else if (IsValidHookMethod<BeforeFixedUpdateAttribute, float>(method))
                    _beforeFixedUpdateHooks.Add(new HookInfo(method, instance));
                else if (IsValidHookMethod<AfterFixedUpdateAttribute, float>(method))
                    _afterFixedUpdateHooks.Add(new HookInfo(method, instance));
                else if (IsValidHookMethod<BeforeDrawAttribute, RenderContext>(method))
                    _beforeDrawHooks.Add(new HookInfo(method, instance));
                else if (IsValidHookMethod<AfterDrawAttribute, RenderContext>(method))
                    _afterDrawHooks.Add(new HookInfo(method, instance));
            }
        }

        private static bool IsValidHookMethod<T, U>(MethodInfo method) where T : Attribute
        {
            if (method.IsStatic)
                return false;

            var attribute = method.GetCustomAttribute<T>();

            if (attribute == null)
                return false;

            if (attribute is BeforeUpdateAttribute
                || attribute is BeforeFixedUpdateAttribute
                || attribute is BeforeDrawAttribute)
            {
                if (!IsReturnTypeValid(method))
                    return false;
            }

            return IsMethodSignatureValid<U>(method);
        }

        private static bool IsReturnTypeValid(MethodInfo method)
            => method.ReturnType.IsAssignableTo(typeof(bool));

        private static bool IsMethodSignatureValid<T>(MethodInfo method)
        {
            var parameters = method.GetParameters();

            if (parameters.Length != 1)
                return false;

            var parameter = parameters[0];

            return !parameter.IsOut
                   && !parameter.IsOptional
                   && parameter.ParameterType.IsAssignableTo(typeof(T));
        }

        private static void InvokeAfterHooks<T>(List<HookInfo> hooks, T parameter)
        {
            for (var i = 0; i < hooks.Count; i++)
            {
                var hookInfo = hooks[i];

                try
                {
                    hookInfo.Hook.Invoke(
                        hookInfo.ExtensionInstance,
                        new object[] { parameter }
                    );
                }
                catch (Exception e)
                {
                    _log.Exception(e);

                    if (e.InnerException != null)
                        _log.Exception(e.InnerException);
                }
            }
        }

        private static bool InvokeBeforeHooks<T>(List<HookInfo> hooks, T parameter)
        {
            var passThrough = true;

            for (var i = 0; i < hooks.Count; i++)
            {
                var hookInfo = hooks[i];

                try
                {
                    passThrough = (bool)hookInfo.Hook.Invoke(
                        hookInfo.ExtensionInstance,
                        new object[] { parameter }
                    )! && passThrough;
                }
                catch (Exception e)
                {
                    _log.Exception(e);

                    if (e.InnerException != null)
                        _log.Exception(e.InnerException);
                }
            }

            return passThrough;
        }
    }
}