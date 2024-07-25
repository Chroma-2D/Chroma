namespace Chroma.Extensibility;

using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class HookAttribute : Attribute
{
    public HookPoint HookPoint { get; }
    public HookAttachment HookAttachment { get; }

    public HookAttribute(HookPoint hookPoint, HookAttachment hookAttachment)
    {
        HookPoint = hookPoint;
        HookAttachment = hookAttachment;
    }
}