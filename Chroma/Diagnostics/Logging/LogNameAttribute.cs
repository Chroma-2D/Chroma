namespace Chroma.Diagnostics.Logging;

using System;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LogNameAttribute : Attribute
{
    public string Name { get; }

    public LogNameAttribute(string name)
    {
        Name = name;
    }
}