using System.Collections.Generic;
using System.Numerics;
using Chroma;
using Chroma.Graphics;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input.GameControllers;
using Chroma.Windowing;

namespace GameController.Views
{
    public class NintendoControllerView : GenericControllerView
    {
        public override string ViewName { get; } = "Nintendo controllers";
        public override Vector2 PositionOnScreen => new(_window.Size.Width / 2f, 0);

        public override List<ControllerType> AcceptedControllers { get; } = new()
        {
            ControllerType.NintendoSwitch
        };

        public NintendoControllerView(Window window)
            : base(window)
        {
        }

        protected override void DrawViewSpecific(RenderContext context)
        {
            var quip = "Implement me when you get up, Elijah :)";
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