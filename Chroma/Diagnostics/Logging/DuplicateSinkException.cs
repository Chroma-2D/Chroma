using System;

namespace Chroma.Diagnostics.Logging
{
    public class DuplicateSinkException : FrameworkException
    {
        public Type SinkType { get; }

        internal DuplicateSinkException(Type sinkType) 
            : base("Sink of this type already exists.")
        {
            SinkType = sinkType;
        }
    }
}