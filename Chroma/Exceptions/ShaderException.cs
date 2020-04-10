using System;
using System.Collections.Generic;
using System.Linq;

namespace Chroma.Exceptions
{
    public class ShaderException : Exception
    {
        public List<string> GlslErrors { get; }

        internal ShaderException(string message, string rawGlslError) : base(message)
        {
            GlslErrors = rawGlslError.Split('\n').ToList();
            GlslErrors.RemoveAll(x => string.IsNullOrWhiteSpace(x));
        }
    }
}
