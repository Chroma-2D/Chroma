using System.Drawing;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;
using Chroma.Input.EventArgs;
using Color = Chroma.Graphics.Color;

namespace Textures
{
    public class GameCore : Game
    {
        private Texture _tileMap;
        private Texture _burger;

        private int _totalTiles;
        private int _currentTileIndex;
        private Vector2 _virtRes;

        public GameCore()
        {
            Content = new FileSystemContentProvider(this, "../../../../_common");
        }

        protected override void LoadContent()
        {
            _tileMap = Content.Load<Texture>("Textures/walls.jpeg");
            _totalTiles = _tileMap.Width / 64;

            _burger = Content.Load<Texture>("Textures/burg.png");
            _virtRes = new Vector2(_burger.Width, _burger.Height);
        }

        protected override void Draw(RenderContext context)
        {
            context.DrawString(
                "Use left/right arrows to switch between tiles on the tile map.\n" +
                "Use num+/num- to adjust burger's virtual resolution by 32 pixels.\n" +
                "Use F1 to reset burger's virtual resolution.\n" +
                "Use F2-F5 to switch between different blending modes.",
                new Vector2(8)
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
                Mouse.GetPosition(),
                Vector2.One,
                Vector2.Zero,
                0f
            );
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
                _burger.VirtualResolution = _virtRes = new Vector2(_virtRes.X + 32, _virtRes.Y + 32);
            }
            else if (e.KeyCode == KeyCode.NumMinus)
            {
                if (_virtRes.X <= 0 || _virtRes.Y <= 0) return;

                _burger.VirtualResolution = _virtRes = new Vector2(_virtRes.X - 32, _virtRes.Y - 32);
            }
            else if (e.KeyCode == KeyCode.F1)
            {
                _burger.VirtualResolution = _virtRes = new Vector2(_burger.Width, _burger.Height);
            }
            else if (e.KeyCode == KeyCode.F2)
            {
                _burger.SetBlendingMode(BlendingPreset.Add);
            }
            else if (e.KeyCode == KeyCode.F3)
            {
                _burger.SetBlendingFunctions(
                    BlendingFunction.One,
                    BlendingFunction.InverseSourceAlpha,
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
        }
    }
}