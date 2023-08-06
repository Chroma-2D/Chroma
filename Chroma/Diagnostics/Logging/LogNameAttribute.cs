using System;

namespace Chroma.Diagnostics.Logging
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class LogNameAttribute : Attribute
    {
        public string Name { get; }

        public LogNameAttribute(string name)
        {
            Name = name;
        }
    }
}