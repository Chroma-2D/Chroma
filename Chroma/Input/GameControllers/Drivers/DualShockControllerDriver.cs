using System.Collections.Generic;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Natives.SDL;

namespace Chroma.Input.GameControllers.Drivers
{
    public class DualShockControllerDriver : ControllerDriver
    {
        private Vector2[] _touchPoints = new Vector2[] { new(-1, -1), new(-1, -1) };
        
        public override string Name { get; } = "Sony DualShock 4 Chroma Driver";
        
        public IReadOnlyCollection<Vector2> TouchPoints => _touchPoints;

        public DualShockControllerDriver(ControllerInfo info) : base(info)
        {
        }

        public void SetLED(Color color)
        {
            if (SDL2.SDL_GameControllerSetLED(Info.InstancePointer, color.R, color.G, color.B) < 0)
                _log.Error(SDL2.SDL_GetError());
        }
    }
}