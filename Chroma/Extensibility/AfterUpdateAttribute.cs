using System;

namespace Chroma.Extensibility
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AfterUpdateAttribute : Attribute
    {
    }
}