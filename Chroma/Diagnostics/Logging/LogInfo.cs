namespace Chroma.Diagnostics.Logging;

using System.Reflection;

internal class LogInfo(Assembly owningAssembly, Log log)
{
    internal Assembly OwningAssembly { get; } = owningAssembly;
    internal Log Log { get; } = log;
}