namespace Chroma.Input.GameControllers.Drivers.Capabilities;

using System.Numerics;

public interface IGyroscopeEnabled
{
    bool GyroscopeEnabled { get; set; }

    Vector3 ReadGyroscopeSensor();

    internal void OnGyroscopeStateChanged(float x, float y, float z);
}