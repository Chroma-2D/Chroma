using System;
using System.Collections.Generic;
using Chroma.Hardware;

namespace Chroma.Input
{
    public class ControllerInfo
    {
        internal IntPtr InstancePointer { get; }
        internal int InstanceId { get; }

        public Guid Guid { get; }
        public int PlayerIndex { get; }
        public string Name { get; }
        public ProductInfo ProductInfo { get; }
        public Dictionary<ControllerAxis, ushort> DeadZones { get; }

        internal ControllerInfo(IntPtr instancePointer, int instanceId, Guid guid, int playerIndex, string name, ProductInfo productInfo)
        {
            InstancePointer = instancePointer;
            InstanceId = instanceId;

            Guid = guid;
            PlayerIndex = playerIndex;
            Name = name;
            ProductInfo = productInfo;

            DeadZones = new Dictionary<ControllerAxis, ushort>();
        }
    }
}