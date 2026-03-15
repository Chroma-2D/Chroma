namespace Chroma.Extensibility;

using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class HookAttribute(
    HookPoint hookPoint,
    HookAttachment hookAttachment
) : Attribute
{
    public HookPoint HookPoint { get; } = hookPoint;
    public HookAttachment HookAttachment { get; } = hookAttachment;
}