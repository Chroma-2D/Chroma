using Chroma.Graphics;
using Chroma.Natives.SDL;
using System.Collections.Generic;

namespace Chroma.Input.GameControllers.Drivers
{
    public class DualShockControllerDriver : TouchpadEnabledControllerDriver
    {
        public override string Name { get; } = "Sony DualShock 4 Chroma Driver";

        public new IReadOnlyCollection<ControllerTouchPoint> TouchPoints => _touchPoints[0];

        internal DualShockControllerDriver(ControllerInfo info)
            : base(info)
        {
        }

        public void SetLedColor(Color color)
        {
            if (SDL2.SDL_GameControllerSetLED(Info.InstancePointer, color.R, color.G, color.B) < 0)
                _log.Error(SDL2.SDL_GetError());
        }
    }
}