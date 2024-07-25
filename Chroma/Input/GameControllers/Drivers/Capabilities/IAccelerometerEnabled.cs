namespace Chroma.Input.GameControllers.Drivers.Capabilities;

using System.Numerics;

public interface IAccelerometerEnabled
{
    bool AccelerometerEnabled { get; set; }
        
    Vector3 ReadAccelerometerSensor();

    internal void OnAccelerometerStateChanged(float x, float y, float z);
}