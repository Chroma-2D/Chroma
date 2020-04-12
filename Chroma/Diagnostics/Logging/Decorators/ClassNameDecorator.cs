using Chroma.Diagnostics.Logging.Base;
using System.Diagnostics;

namespace Chroma.Diagnostics.Logging.Decorators
{
    public class ClassNameDecorator : Decorator
    {
        public override string Decorate(LogLevel logLevel, string input, string originalMessage, Sink sink)
        {
            return new StackTrace().GetFrame(5).GetMethod().DeclaringType.Name;
        }
    }
}
