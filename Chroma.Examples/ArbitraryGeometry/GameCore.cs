using System;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.TrueType;

namespace ArbitraryGeometry
{
    public class GameCore : Game
    {
        private Texture _texture;
        private PixelShader _ps;
        private float _t;

        private float[] _box = new[]
        {
            -0.25f, -0.25f, -0.25f,
            -0.25f, -0.25f, 0.25f,
            -0.25f, 0.25f, 0.25f,
            0.25f, 0.25f, -0.25f,
            -0.25f, -0.25f, -0.25f,
            -0.25f, 0.25f, -0.25f,
            0.25f, -0.25f, 0.25f,
            -0.25f, -0.25f, -0.25f,
            0.25f, -0.25f, -0.25f,
            0.25f, 0.25f, -0.25f,
            0.25f, -0.25f, -0.25f,
            -0.25f, -0.25f, -0.25f,
            -0.25f, -0.25f, -0.25f,
            -0.25f, 0.25f, 0.25f,
            -0.25f, 0.25f, -0.25f,
            0.25f, -0.25f, 0.25f,
            -0.25f, -0.25f, 0.25f,
            -0.25f, -0.25f, -0.25f,
            -0.25f, 0.25f, 0.25f,
            -0.25f, -0.25f, 0.25f,
            0.25f, -0.25f, 0.25f,
            0.25f, 0.25f, 0.25f,
            0.25f, -0.25f, -0.25f,
            0.25f, 0.25f, -0.25f,
            0.25f, -0.25f, -0.25f,
            0.25f, 0.25f, 0.25f,
            0.25f, -0.25f, 0.25f,
            0.25f, 0.25f, 0.25f,
            0.25f, 0.25f, -0.25f,
            -0.25f, 0.25f, -0.25f,
            0.25f, 0.25f, 0.25f,
            -0.25f, 0.25f, -0.25f, 
            -0.25f, 0.25f, 0.25f,
            0.25f, 0.25f, 0.25f,
            -0.25f, 0.25f, 0.25f,
            0.25f, -0.25f, 0.25f,
        };

        public GameCore() : base(new(false, true, 4))
        {
            RenderSettings.MultiSamplingEnabled = true;
        }

        protected override void Initialize(IContentProvider content)
        {
            _texture = new Texture(1, 1);
            _texture[0, 0] = Color.White;

            _ps = Content.Load<PixelShader>("Shaders/untextured.frag");
        }

        private void With3D(Action action)
        {
            RenderTransform.SetMatrixMode(MatrixMode.Model, Window);
            RenderTransform.Push();
            RenderTransform.LoadIdentity();

            RenderTransform.SetMatrixMode(MatrixMode.View, Window);
            RenderTransform.Push();
            RenderTransform.LoadIdentity();

            RenderTransform.SetMatrixMode(MatrixMode.Projection, Window);
            RenderTransform.Push();
            RenderTransform.LoadIdentity();

            action();

            RenderTransform.SetMatrixMode(MatrixMode.Projection, Window);
            RenderTransform.Pop();

            RenderTransform.SetMatrixMode(MatrixMode.View, Window);
            RenderTransform.Pop();

            RenderTransform.SetMatrixMode(MatrixMode.Model, Window);
            RenderTransform.Pop();
        }

        protected override IContentProvider InitializeContentPipeline()
        {
            return new FileSystemContentProvider(
                Path.Combine(
                    AppContext.BaseDirectory, 
                    "../../../../_common"
                )
            );
        }


        protected override void Update(float delta)
        {
            _t += delta;
        }

        protected override void Draw(RenderContext context)
        {
            _ps.Activate();
            RenderSettings.DepthTestingEnabled = true;
            {
                With3D(() =>
                {
                    RenderTransform.Translate(
                        new Vector3(
                            0,
                            MathF.Sin(_t) * 0.5f,
                            0
                        )
                    );

                    RenderTransform.Rotate(111 * _t, new Vector3(0, 0.5f, 0.5f));
                    RenderTransform.Rotate(27 * _t, new Vector3(0.5f, 0.5f, 0.0f));

                    _ps.SetUniform("tri_color", Color.Red);
                    context.RenderArbitraryGeometry(
                        _texture,
                        VertexFormat.XYZ,
                        36,
                        _box,
                        new ushort[]
                        {
                            3, 4, 5,
                            9, 10, 11
                        }
                    );

                    _ps.SetUniform("tri_color", Color.Magenta);
                    context.RenderArbitraryGeometry(
                        _texture,
                        VertexFormat.XYZ,
                        36,
                        _box,
                        new ushort[]
                        {
                            0, 1, 2,
                            12, 13, 14,
                        }
                    );

                    _ps.SetUniform("tri_color", Color.Yellow);
                    context.RenderArbitraryGeometry(
                        _texture,
                        VertexFormat.XYZ,
                        36,
                        _box,
                        new ushort[]
                        {
                            6, 7, 8,
                            15, 16, 17,
                        }
                    );

                    _ps.SetUniform("tri_color", Color.CornflowerBlue);
                    context.RenderArbitraryGeometry(
                        _texture,
                        VertexFormat.XYZ,
                        36,
                        _box,
                        new ushort[]
                        {
                            18, 19, 20,
                            33, 34, 35
                        }
                    );

                    _ps.SetUniform("tri_color", Color.White);
                    context.RenderArbitraryGeometry(
                        _texture,
                        VertexFormat.XYZ,
                        36,
                        _box,
                        new ushort[]
                        {
                            21, 22, 23,
                            24, 25, 26,
                        }
                    );

                    _ps.SetUniform("tri_color", Color.Lime);
                    context.RenderArbitraryGeometry(
                        _texture,
                        VertexFormat.XYZ,
                        36,
                        _box,
                        new ushort[]
                        {
                            27, 28, 29,
                            30, 31, 32,
                        }
                    );
                });
            }
            RenderSettings.DepthTestingEnabled = false;
            Shader.Deactivate();

            var text = "I'm just some random text don't mind me :)";
            var measure = TrueTypeFont.Default.Measure(text);
            
            context.DrawString(
                text,
                new Vector2(
                    Window.Center.X - (measure.Width / 2),
                    Window.Center.Y
                ),
                (d, _, i, p) =>
                {
                    d.Color = Color.Orange;
                    d.Position = p + new Vector2(0, MathF.Sin(_t * 4 + i) * 4);
                }
            );
        }
    }
}