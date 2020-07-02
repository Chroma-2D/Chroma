using System.Reflection;

namespace Chroma.Diagnostics.Logging
{
    internal class LogInfo
    {
        internal Assembly OwningAssembly { get; set; }
        internal Log Log { get; set; }
    }
}