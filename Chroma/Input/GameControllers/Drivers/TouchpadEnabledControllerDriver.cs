using System.Collections.Generic;

namespace Chroma.Input.GameControllers.Drivers
{
    public class TouchpadEnabledControllerDriver : ControllerDriver
    {
        protected ControllerTouchPoint[][] _touchPoints;

        public override string Name => "Generic touchpad-enabled controller driver";
        
        public IReadOnlyCollection<IReadOnlyCollection<ControllerTouchPoint>> TouchPoints => _touchPoints;

        public TouchpadEnabledControllerDriver(ControllerInfo info) 
            : base(info)
        {
            _touchPoints = new ControllerTouchPoint[info.TouchpadCount][];

            for (var i = 0; i < info.TouchpadCount; i++)
            {
                _touchPoints[i] = new ControllerTouchPoint[info.TouchpadFingerLimit[i]];
            }
        }

        internal void OnTouchpadMoved(int touchpadIndex, int fingerIndex, float x, float y)
        {
            _touchPoints[touchpadIndex][fingerIndex].X = x;
            _touchPoints[touchpadIndex][fingerIndex].Y = y;
        }

        internal void OnTouchpadTouched(int touchpadIndex, int fingerIndex, float x, float y)
        {
            _touchPoints[touchpadIndex][fingerIndex].X = x;
            _touchPoints[touchpadIndex][fingerIndex].Y = y;
            _touchPoints[touchpadIndex][fingerIndex].Touching = true;
        }

        internal void OnTouchpadReleased(int touchpadIndex, int fingerIndex, float x, float y)
        {
            _touchPoints[touchpadIndex][fingerIndex].X = x;
            _touchPoints[touchpadIndex][fingerIndex].Y = y;
            _touchPoints[touchpadIndex][fingerIndex].Touching = false;
        }
    }
}
