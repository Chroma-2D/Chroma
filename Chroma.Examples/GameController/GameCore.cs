using System.Collections.Generic;
using Chroma;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input.GameControllers;
using GameController.Views;

namespace GameController
{
    public class GameCore : Game
    {
        private List<GenericControllerView> _views;

        public GameCore()
            : base(new(false, false))
        {
            Graphics.LimitFramerate = true;
            Graphics.VerticalSyncMode = VerticalSyncMode.None;

            Window.GoWindowed(1024, 600, true);
            RenderSettings.AutoClearColor = Color.DimGray;

            _views = new List<GenericControllerView>
            {
                new GenericControllerView(Window),
                new DualShockControllerView(Window),
                new DualSenseControllerView(Window),
                new NintendoControllerView(Window)
            };

            // Enabled to simplify label rendering.
            RenderSettings.ShapeBlendingEnabled = true;
        }

        protected override void ControllerDisconnected(ControllerEventArgs e)
        {
            for (var i = 0; i < _views.Count; i++)
                _views[i].OnDisconnected(e);
        }

        protected override void ControllerConnected(ControllerEventArgs e)
        {
            for (var i = 0; i < _views.Count; i++)
                _views[i].OnConnected(e);
        }

        protected override void ControllerAxisMoved(ControllerAxisEventArgs e)
        {
            for (var i = 0; i < _views.Count; i++)
                _views[i].OnAxisMoved(e);
        }

        protected override void ControllerButtonPressed(ControllerButtonEventArgs e)
        {
            for (var i = 0; i < _views.Count; i++)
                _views[i].OnButtonPressed(e);
        }

        protected override void ControllerButtonReleased(ControllerButtonEventArgs e)
        {
            for (var i = 0; i < _views.Count; i++)
                _views[i].OnButtonReleased(e);
        }

        protected override void Update(float delta)
        {
            Window.Title = $"Chroma Framework - {PerformanceCounter.FPS} FPS";

            for (var i = 0; i < _views.Count; i++)
                _views[i].Update(delta);
        }

        protected override void Draw(RenderContext context)
        {
            for (var i = 0; i < _views.Count; i++)
                _views[i].Draw(context);

            context.Line(Window.Size.Width / 2, 0, Window.Size.Width / 2, Window.Size.Height, Color.DimGray);
            context.Line(0, Window.Size.Height / 2, Window.Size.Width, Window.Size.Height / 2, Color.DimGray);
        }
    }
}