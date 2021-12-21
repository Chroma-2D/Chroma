using System.Diagnostics;
using System.Reflection;
using Chroma.Diagnostics.Logging.Base;

namespace Chroma.Diagnostics.Logging.Decorators
{
    public class ClassNameDecorator : Decorator
    {
        public override string Decorate(LogLevel logLevel, string input, string originalMessage, Sink sink)
        {
            var stackTrace = new StackTrace();

            var type = stackTrace
                .GetFrame(5)!
                .GetMethod()!
                .DeclaringType!;

            while (type.IsNested && type.Name.Contains("<"))
            {
                type = type.DeclaringType!;
            }

            return type.GetCustomAttribute<LogNameAttribute>()?.Name 
                   ?? type.Name;
        }
    }
}