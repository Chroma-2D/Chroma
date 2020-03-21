using Chroma.Hardware;
using System;

namespace Chroma.Input
{
    public class ControllerInfo
    {
        internal IntPtr InstancePointer { get; }
        internal int InstanceId { get; }

        public int PlayerIndex { get; }
        public string Name { get; }
        public ProductInfo ProductInfo { get; }

        internal ControllerInfo(IntPtr instancePointer, int instanceId, int playerIndex, string name, ProductInfo productInfo)
        {
            InstancePointer = instancePointer;
            InstanceId = instanceId;

            PlayerIndex = playerIndex;
            Name = name;
            ProductInfo = productInfo;
        }
    }
}
