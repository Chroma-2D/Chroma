using System;
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
                sb.AppendLine($"   What threw: {e.TargetSite.Name} in {e.TargetSite?.DeclaringType?.Name ?? "<unknown>"}");
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
    }
}