using System.Numerics;

namespace Chroma.Input.GameControllers.Drivers.Capabilities
{
    public interface IGyroscopeEnabled
    {
        bool GyroscopeEnabled { get; set; }

        Vector3 ReadGyroscopeSensor();

        internal void OnGyroscopeStateChanged(float x, float y, float z);
    }
}
