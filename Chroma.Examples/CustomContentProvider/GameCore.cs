namespace CustomContentProvider;

using System;
using System.IO;
using System.Numerics;
using Chroma;
using Chroma.Audio.Sources;
using Chroma.ContentManagement;
using Chroma.Graphics;
using Chroma.Input;

public class GameCore : Game
{
    private Sound _shotgun;
    private Texture _texture;
    private float _rotation;

    public GameCore() : base(new(false, false))
    {
        RenderSettings.DefaultTextureFilteringMode = TextureFilteringMode.NearestNeighbor;
    }

    protected override IContentProvider InitializeContentPipeline()
    {
        return new ZipContentProvider(
            Path.Combine(AppContext.BaseDirectory, "../../../../_common/assets.zip")
        );
    }
        
    protected override void Initialize(IContentProvider content)
    {
        _texture = content.Load<Texture>("Textures/pentagram.png");
        _shotgun = content.Load<Sound>("doomsg.wav");
    }

    protected override void Draw(RenderContext context)
    {
        context.DrawTexture(
            _texture,
            (Window.Center - _texture.Center),
            new Vector2(8),
            _texture.Center,
            _rotation
        );
    }

    protected override void Update(float delta)
    {
        _rotation += 15 * delta;
    }

    protected override void KeyPressed(KeyEventArgs e)
    {
        if (e.KeyCode == KeyCode.Space)
        {
            _shotgun.Play();
        }
    }
}