using System.Reflection;

namespace Chroma.Extensibility
{
    public struct HookInfo
    {
        public MethodInfo Hook { get; }
        public object ExtensionInstance { get; }

        public HookInfo(MethodInfo hook, object extensionInstance)
        {
            Hook = hook;
            ExtensionInstance = extensionInstance;
        }
    }
}