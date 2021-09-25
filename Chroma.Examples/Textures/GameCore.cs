using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Diagnostics;
using Chroma.Graphics;
using Chroma.Input;
using Color = Chroma.Graphics.Color;

namespace Textures
{
    public class GameCore : Game
    {
        private Texture _tileMap;
        private Texture _burger;
        private Texture _dynTex;

        private int _totalTiles;
        private int _currentTileIndex;
        private Size _virtRes;

        private Random _rnd = new();
        private float _rotation;
        private float _wave;

        private Color[] _colors = new[]
        {
            Color.Aqua,
            Color.Yellow,
            Color.Green,
            Color.CornflowerBlue,
            Color.Violet,
            Color.Thistle,
            Color.Orange
        };

        public GameCore() : base(new(false, false))
        {           
            Graphics.LimitFramerate = false;
            Cursor.IsVisible = false;
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void LoadContent()
        {
            _tileMap = Content.Load<Texture>("Textures/walls.jpeg");
            _totalTiles = _tileMap.Width / 64;

            _burger = Content.Load<Texture>("Textures/burg.png");
            _virtRes = new Size(_burger.Width, _burger.Height);

            _dynTex = new Texture(256, 256);
            _dynTex.FilteringMode = TextureFilteringMode.NearestNeighbor;
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "Use left/right arrows to switch between tiles on the tile map.\n" +
                "Use num+/num- to adjust burger's virtual resolution by 32 pixels.\n" +
                "Use F1 to reset burger's virtual resolution.\n" +
                "Use F2-F5 to switch between different blending modes.\n" +
                $"{Color.Brown.R}, {Color.Brown.G}, {Color.Brown.B}: {Color.Brown.Hue}, {Color.Brown.Saturation}, {Color.Brown.Value}",
                new Vector2(8)
            );

            _dynTex.Flush();
            context.DrawTexture(
                _dynTex,
                Window.Center + new Vector2(127, 0),
                Vector2.One,
                Vector2.Zero, 
                0
            );

            context.DrawTexture(
                _tileMap,
                new Vector2(32, 128),
                Vector2.One,
                Vector2.Zero,
                rotation: 0f,
                new Rectangle(_currentTileIndex * 64, 0, 64, 64)
            );

            context.Rectangle(
                ShapeMode.Fill,
                new Vector2(256),
                128, 128, Color.HotPink
            );

            context.DrawTexture(
                _burger,
                Mouse.GetPosition() + _burger.Center + new Vector2(0, 5 * MathF.Sin(_wave)),
                Vector2.One,
                _burger.Center,
                _rotation
            );
        }

        protected override void Update(float delta)
        {
            Window.Title = PerformanceCounter.FPS.ToString("F2");

            _wave += 10 * delta;
            _rotation += 50 * delta;

            for (var y = 0; y < _dynTex.Height; y++)
            {
                for (var x = 0; x < _dynTex.Width; x++)
                {
                    var mod = x / (y + 1);

                    if (y > 127)
                        mod = (x / 2) * (y / 2);

                    if (x > 127)
                        mod = (x + y) / 2;

                    if (x > 127 && y < 127)
                        mod = (int)(Math.Abs(x / 2 - y / 2) + (5 * MathF.Sin(_wave)));

                    _dynTex[x, y] = _colors[Math.Abs(((int)PerformanceCounter.LifetimeFrames + mod) % _colors.Length)];
                }
            }
        }

        protected override void KeyPressed(KeyEventArgs e)
        {
            if (e.KeyCode == KeyCode.Right)
            {
                _currentTileIndex++;
                _currentTileIndex %= _totalTiles;
            }
            else if (e.KeyCode == KeyCode.Left)
            {
                _currentTileIndex--;

                if (_currentTileIndex < 0)
                    _currentTileIndex = _totalTiles - 1;
            }
            else if (e.KeyCode == KeyCode.NumPlus)
            {
                _burger.VirtualResolution = _virtRes = new Size(_virtRes.Width + 32, _virtRes.Height + 32);
            }
            else if (e.KeyCode == KeyCode.NumMinus)
            {
                if (_virtRes.Width <= 0 || _virtRes.Height <= 0) return;

                _burger.VirtualResolution = _virtRes = new Size(_virtRes.Width - 32, _virtRes.Height - 32);
            }
            else if (e.KeyCode == KeyCode.F1)
            {
                _burger.VirtualResolution = _virtRes = new Size(_burger.Width, _burger.Height);
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                _burger.SetBlendingMode(BlendingPreset.Add);
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                _burger.SetBlendingFunctions(
                    BlendingFunction.One,
                    BlendingFunction.OneMinusSourceAlpha,
                    BlendingFunction.DestinationAlpha,
                    BlendingFunction.One
                );

                _burger.SetBlendingEquations(
                    BlendingEquation.ReverseSubtract,
                    BlendingEquation.ReverseSubtract
                );
            }
            else if (e.KeyCode == KeyCode.F4)
            {
                _burger.SetBlendingMode(BlendingPreset.Multiply);
            }
            else if (e.KeyCode == KeyCode.F5)
            {
                _burger.SetBlendingMode(BlendingPreset.Normal);
            }
            else if (e.KeyCode == KeyCode.F6)
            {
                _dynTex.Save("dyntex.png", ImageFileFormat.PNG);
            }
        }
    }
}