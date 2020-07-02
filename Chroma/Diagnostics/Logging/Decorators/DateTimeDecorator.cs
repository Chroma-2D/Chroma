using Chroma.Diagnostics.Logging.Base;
using System;
using System.Globalization;

namespace Chroma.Diagnostics.Logging.Decorators
{
    public class DateTimeDecorator : Decorator
    {
        private DisplayMode Mode { get; }

        public DateTimeDecorator()
            : this(DisplayMode.TimeString24h)
        {
        }

        public DateTimeDecorator(DisplayMode mode)
        {
            Mode = mode;
        }

        public override string Decorate(LogLevel logLevel, string input, string originalMessage, Sink sink)
        {
            return Mode switch
            {
                DisplayMode.TimeString24h => DateTime.Now.ToString("HH:mm:ss"),
                DisplayMode.TimeString12h => DateTime.Now.ToString("hh:mm:ss tt"),
                DisplayMode.DefaultDateTimeString => DateTime.Now.ToString(CultureInfo.InvariantCulture),

                _ => "??:??:??"
            };
        }

        public enum DisplayMode
        {
            TimeString24h,
            TimeString12h,
            DefaultDateTimeString
        }
    }
}