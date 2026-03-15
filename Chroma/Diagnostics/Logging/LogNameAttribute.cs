namespace Chroma.Diagnostics.Logging;

using System;

[AttributeUsage(AttributeTargets.Class)]
public sealed class LogNameAttribute(string name) 
    : Attribute
{
    public string Name { get; } = name;
}