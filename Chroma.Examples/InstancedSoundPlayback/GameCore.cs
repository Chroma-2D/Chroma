namespace InstancedSoundPlayback;

using System;
using System.IO;
using Chroma;
using Chroma.Audio;
using Chroma.ContentManagement;
using Chroma.ContentManagement.FileSystem;
using Chroma.Graphics;
using Chroma.Input;

public class GameCore : Game
{
    private InstancedSound _instancedSound;
        
    internal static AudioOutput AudioOutput { get; set; }
        
    public GameCore() 
        : base(new(false, false))
    {
        AudioOutput = Audio.Output;
    }

    protected override IContentProvider InitializeContentPipeline()
    {
        var pipeline = new FileSystemContentProvider(
            Path.Combine(AppContext.BaseDirectory, "../../../../_common")
        );

        pipeline.RegisterImporter<InstancedSound>((path, _) => new InstancedSound(path));
            
        return pipeline;
    }
        
    protected override void Initialize(IContentProvider content)
    {
        _instancedSound = content.Load<InstancedSound>("Sounds/doomsg.wav");
    }

    protected override void Update(float delta)
    {
        InstancedSound.Update();
    }

    protected override void Draw(RenderContext context)
    {
        context.DrawString(
            "Press and/or hold any key to play a new instance of the same sound.\n" +
            $"{InstancedSound.LiveInstanceCount} live instance(s).",
            8, 8
        );
    }

    protected override void KeyPressed(KeyEventArgs e)
    {
        _instancedSound.PlayInstance();
    }
}