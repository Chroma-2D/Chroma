﻿namespace Chroma.Graphics.Accelerated;

using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ShaderException : GraphicsException
{
    public List<string> GlslErrors { get; }

    internal ShaderException(string message, string rawGlslError) 
        : base(message)
    {
        GlslErrors = rawGlslError.Split('\n').ToList();
        GlslErrors.RemoveAll(string.IsNullOrWhiteSpace);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.AppendLine(base.ToString());
        sb.AppendLine("---");
            
        foreach(var l in GlslErrors)
            sb.AppendLine(l);

        return sb.ToString();
    }
}