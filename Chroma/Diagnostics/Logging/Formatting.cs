using System;
using System.Reflection;
using System.Text;

namespace Chroma.Diagnostics.Logging
{
    internal static class Formatting
    {
        internal static string ExceptionForLogging(Exception e, bool skipMessage)
        {
            var sb = new StringBuilder();

            if (!skipMessage)
                sb.AppendLine(e.Message);

            if (e.TargetSite != null)
            {
                sb.AppendLine($"   What threw: {e.TargetSite.Name} in {e.TargetSite.DeclaringType.Name}");
            }

            if (!string.IsNullOrEmpty(e.StackTrace))
            {
                sb.AppendLine("   Stack trace:");

                foreach (var s in e.StackTrace.Split('\n'))
                    sb.AppendLine($"      {s}");
            }

            if (e.InnerException != null)
            {
                sb.AppendLine("--- Inner exception below ---");
                sb.AppendLine(
                    ExceptionForLogging(e.InnerException, false)
                );
            }

            return sb.ToString();
        }

        internal static string ReflectionTypeLoadExceptionForLogging(ReflectionTypeLoadException rtle)
        {
            var sb = new StringBuilder();

            sb.AppendLine(
                ExceptionForLogging(rtle, true)
            );

            sb.AppendLine("--- LOADER EXCEPTIONS BELOW ---");

            foreach (var le in rtle.LoaderExceptions)
            {
                sb.AppendLine(
                    ExceptionForLogging(le, false)
                );
            }

            return sb.ToString();
        }
    }
}
