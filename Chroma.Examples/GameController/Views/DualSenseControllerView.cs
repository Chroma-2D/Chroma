using System.Collections.Generic;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input.GameControllers;
using Chroma.Windowing;

namespace GameController.Views
{
    public class DualSenseControllerView : GenericControllerView
    {
        public override string ViewName { get; } = "DualSense controllers";

        public override Vector2 PositionOnScreen => new(
            0,
            _window.Size.Height / 2f
        );

        public override List<ControllerType> AcceptedControllers { get; } = new()
        {
            ControllerType.PlayStation5
        };

        public DualSenseControllerView(Window window) : base(window)
        {
        }

        protected override void DrawViewSpecific(RenderContext context)
        {
            var quip = "vdd's poor ass can't afford a DualSense controller yet\nso here's this text instead";
            var quipMeasure = TrueTypeFont.Default.Measure(quip);

            context.DrawString(
                quip,
                new Vector2(
                    _renderTarget.Width / 2 - quipMeasure.Width / 2,
                    _renderTarget.Height / 2 - quipMeasure.Height / 2
                )
            );
        }
    }
}