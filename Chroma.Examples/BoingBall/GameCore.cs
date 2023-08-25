using System;
using System.Drawing;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.Audio.Sources;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Input;

namespace BoingBall
{
    public class GameCore : Game
    {
        private enum EffectMode
        {
            Shadow,
            Sphere,
            Shade
        }
        
        private static readonly Vector2 _maxPos = new(175, 96);
        private Vector2 _pos = Vector2.Zero;
        private Vector2 _dir = Vector2.One;
        private Vector2 _vel = new(45, 0);
        private float _grav = 3.2f;
        
        private float _texshift;
        private RenderTarget _target;
        private Texture _texture;
        private Texture _shade;
        private Cursor _cursor;
        private Effect _effect;
        
        private Sound _bounce1;
        private Sound _bounce2;

        public GameCore()
            : base(new(false, false))
        {
            Window.Mode.SetWindowed(960, 600, true);
            FixedTimeStepTarget = 60;
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(AppContext.BaseDirectory, "../../../../_common")
            );
        }

        protected override void LoadContent()
        {
            _target = new RenderTarget(320, 200);
            _target.VirtualResolution = new(Window.Width, Window.Height);
            _target.FilteringMode = TextureFilteringMode.NearestNeighbor;

            _texture = Content.Load<Texture>("Textures/boing.png");
            _texture.FilteringMode = TextureFilteringMode.NearestNeighbor;
            _texture.VerticalWrappingMode = TextureWrappingMode.Repeat;
            _texture.HorizontalWrappingMode = TextureWrappingMode.Repeat;

            _shade = Content.Load<Texture>("Textures/shade.png");
            _shade.FilteringMode = TextureFilteringMode.NearestNeighbor;

            _cursor = Content.Load<Cursor>("Cursors/ami13.png");
            _cursor.SetCurrent();

            _effect = Content.Load<Effect>("Shaders/distort.frag");
            _bounce1 = Content.Load<Sound>("Sounds/bounce1.ogg");
            _bounce2 = Content.Load<Sound>("Sounds/bounce2.ogg");
        }

        protected override void FixedUpdate(float delta)
        {
            _texshift += delta / 1.66f * _dir.X;
            
            if (_dir.Y > 0)
            {
                _vel.Y += _grav * delta;
            }
            else
            {
                _vel.Y -= _grav * delta;
                
                if (_vel.Y < 0)
                {
                    _vel.Y = 0;
                    _dir.Y *= -1;
                }
            }

            _pos.X += _dir.X * _vel.X * delta;
            _pos.Y += _dir.Y * _vel.Y;

            if (_pos.X > _maxPos.X)
            {
                _dir.X *= -1;
                _pos.X = _maxPos.X - 1;
                
                _bounce2.Play();
            }

            if (_pos.X < 0)
            {
                _dir.X *= -1;
                _pos.X = 0;
                
                _bounce2.Play();
            }

            if (_pos.Y > _maxPos.Y)
            {
                _dir.Y *= -1;
                _pos.Y = _maxPos.Y - 1;
                _vel.Y = 2.8f;
                
                _bounce1.Play();
            }
        }

        protected override void Draw(RenderContext context)
        {
            var scale = new Vector2(4f);
            var basePosition = new Vector2(72, 47);
            var shadowOffset = new Vector2(16, 4);
            var sphereTilt = 25f;

            context.RenderTo(_target, (ctx, tgt) =>
            {
                ctx.Clear(127, 127, 127);

                DrawGrid(new(40, 16), ctx);

                _effect.Activate();
                _effect.SetUniform("mode", (int)EffectMode.Shadow);
                _texture.ColorMask = new(0, 0, 0, 127);
                ctx.DrawTexture(
                    _texture, 
                    basePosition + shadowOffset + _pos, 
                    scale,
                    _texture.AbsoluteCenter, 25f
                );
                
                _effect.SetUniform("mode", (int)EffectMode.Sphere);
                _effect.SetUniform("texshift", _texshift);
                ctx.DrawTexture(
                    _texture, 
                    basePosition + _pos, 
                    scale, 
                    _texture.AbsoluteCenter,
                    sphereTilt
                );
                Shader.Deactivate();
                
                _effect.Activate();
                _effect.SetUniform("mode", (int)EffectMode.Shade);
                ctx.DrawTexture(
                    _shade, 
                    basePosition + _pos, 
                    scale, 
                    _shade.AbsoluteCenter,
                    sphereTilt
                );
                Shader.Deactivate();
            });

            context.DrawTexture(_target, Vector2.Zero);
        }

        private void DrawGrid(Vector2 origin, RenderContext context)
        {
            for (var i = 0; i < 16; i++)
            {
                context.Line(
                    origin.X + 16 * i,
                    origin.Y,
                    origin.X + 16 * i,
                    origin.Y + 160,
                    new(170, 0, 170)
                );

                if (i < 11)
                {
                    context.Line(
                        origin.X - 0.5f,
                        origin.Y + 16 * i,
                        origin.X + 240,
                        origin.Y + 16 * i,
                        new(170, 0, 170)
                    );
                }
            }
        }
    }
}