namespace InstancedSoundPlayback;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chroma.Audio;
using Chroma.Audio.Sources;
using Chroma.MemoryManagement;

public class InstancedSound : DisposableResource
{
    private static Queue<AudioClip> _expiredInstances = new();
    private static List<AudioClip> _liveInstances = new();
    private Stream _template;

    public static int LiveInstanceCount => _liveInstances.Count;
        
    public InstancedSound(string filePath)
        : this(new FileStream(filePath, FileMode.Open))
    {
    }
        
    public InstancedSound(Stream stream)
    {
        GameCore.AudioOutput.AudioSourceFinished += OnAudioSourceFinished;
        _template = stream;
    }

    public static void Update()
    {
        while (_expiredInstances.Any())
        {
            var instance = _expiredInstances.Dequeue();

            if (_liveInstances.Remove(instance))
                instance.Dispose();
        }
    }

    public void PlayInstance()
    {
        if (_template == null)
            return;
            
        using (var ms = new MemoryStream())
        {
            _template.CopyTo(ms);
            _template.Seek(0, SeekOrigin.Begin);
                
            var clip = new AudioClip(ms, true);
            _liveInstances.Add(clip);

            clip.Play();
        }
    }

    protected override void FreeManagedResources()
    {
        GameCore.AudioOutput.AudioSourceFinished -= OnAudioSourceFinished;
        _template.Dispose();
    }

    private void OnAudioSourceFinished(object sender, AudioSourceEventArgs e)
    {
        if (e.IsLooping)
            return;
            
        if (e.Source is AudioClip clip)
        {
            _expiredInstances.Enqueue(clip);
        }
    }
}