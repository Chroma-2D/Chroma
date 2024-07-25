namespace Chroma.Diagnostics.Logging;

using System.Reflection;

internal class LogInfo
{
    internal Assembly OwningAssembly { get; set; }
    internal Log Log { get; set; }
}