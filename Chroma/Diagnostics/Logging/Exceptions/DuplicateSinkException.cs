using System;

namespace Chroma.Diagnostics.Logging.Exceptions
{
    public class DuplicateSinkException : Exception
    {
        public Type SinkType { get; }

        public DuplicateSinkException(Type sinkType) : base("Sink of this type already exists.")
        {
            SinkType = sinkType;
        }
    }
}
