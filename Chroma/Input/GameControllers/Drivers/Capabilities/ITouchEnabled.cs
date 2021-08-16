using System.Collections.Generic;

namespace Chroma.Input.GameControllers.Drivers.Capabilities
{
    public interface ITouchEnabled
    {
        IReadOnlyCollection<ControllerTouchPoint> TouchPoints { get; }

        internal void OnTouchpadMoved(int touchpadIndex, int fingerIndex, float x, float y);
        internal void OnTouchpadTouched(int touchpadIndex, int fingerIndex, float x, float y);
        internal void OnTouchpadReleased(int touchpadIndex, int fingerIndex, float x, float y);
    }
}
