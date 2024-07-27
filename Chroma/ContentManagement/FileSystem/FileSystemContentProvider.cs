namespace Chroma.ContentManagement.FileSystem;

using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Chroma.Audio.Sources;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Input;
using Chroma.MemoryManagement;

public class FileSystemContentProvider : DisposableResource, IContentProvider
{
    private readonly HashSet<DisposableResource> _loadedResources;
    private readonly Dictionary<Type, Func<string, object[], object>> _importers;

    public string ContentRoot { get; }

    public FileSystemContentProvider(string? contentRoot = null)
    {
        if (string.IsNullOrEmpty(contentRoot))
        {
            contentRoot = Path.Combine(AppContext.BaseDirectory, "Content");
        }
        
        ContentRoot = contentRoot;

        _loadedResources = new HashSet<DisposableResource>();
        _importers = new Dictionary<Type, Func<string, object[], object>>();

        RegisterImporters();
    }

    public T? Load<T>(string relativePath, params object[] args) where T : DisposableResource
    {
        var type = typeof(T);

        if (!_importers.ContainsKey(type))
        {
            throw new UnsupportedContentException(
                "This type of content is not supported by this provider.",
                MakeAbsolutePath(relativePath)
            );
        }

        var resource = _importers[type].Invoke(MakeAbsolutePath(relativePath), args) as T;

        if (resource != null)
        {
            _loadedResources.Add(resource);
            resource.Disposing += OnResourceDisposing;
        }

        return resource;
    }

    public void Unload<T>(T resource) where T : DisposableResource
    {
        if (!_loadedResources.Contains(resource))
            throw new ContentNotLoadedException(
                "The content you want to unload was never loaded in the first place.");

        resource.Dispose();
    }

    public Stream Open(string relativePath)
        => new FileStream(MakeAbsolutePath(relativePath), FileMode.Open);

    public byte[] Read(string relativePath)
        => File.ReadAllBytes(MakeAbsolutePath(relativePath));

    public void Track<T>(T resource) where T : DisposableResource
    {
        if (_loadedResources.Contains(resource))
            throw new InvalidOperationException("The content you want to track is already being tracked.");

        _loadedResources.Add(resource);
        resource.Disposing += OnResourceDisposing;
    }

    public void StopTracking<T>(T resource) where T : DisposableResource
    {
        if (!_loadedResources.Contains(resource))
            throw new ContentNotLoadedException(
                "The content you want to stop tracking was never tracked in the first place.");

        resource.Disposing -= OnResourceDisposing;
        _loadedResources.Remove(resource);
    }

    public void RegisterImporter<T>(Func<string, object[], object> importer) where T : DisposableResource
    {
        var contentType = typeof(T);
            
        if (_importers.ContainsKey(contentType))
        {
            throw new InvalidOperationException(
                $"An importer for type {contentType.Name} was already registered."
            );
        }

        _importers.Add(contentType, importer);
    }

    public void UnregisterImporter<T>() where T : DisposableResource
    {
        var contentType = typeof(T);

        if (!_importers.ContainsKey(contentType))
        {
            throw new InvalidOperationException(
                $"An importer for type {contentType.Name} was never registered, thus it cannot be unregistered.");
        }

        _importers.Remove(contentType);
    }

    public bool IsImporterPresent<T>() where T : DisposableResource
        => _importers.ContainsKey(typeof(T));

    protected override void FreeManagedResources()
    {
        var disposables = new List<IDisposable>(_loadedResources);

        foreach (var resource in disposables)
            resource.Dispose();
    }

    private void RegisterImporters()
    {
        RegisterImporter<Texture>((path, _) => new Texture(path));
        RegisterImporter<PixelShader>((path, _) => PixelShader.FromFile(path));
        RegisterImporter<VertexShader>((path, _) => VertexShader.FromFile(path));
        RegisterImporter<Effect>((path, _) => Effect.FromFile(path));
        RegisterImporter<BitmapFont>((path, _) => new BitmapFont(path));
        RegisterImporter<Sound>((path, _) => new Sound(path));
        RegisterImporter<Music>((path, _) => new Music(path));

        RegisterImporter<Cursor>((path, args) =>
        {
            var hotSpot = new Vector2();

            if (args.Length >= 1)
                hotSpot = (Vector2)args[0];

            var cursor = new Cursor(path, hotSpot);
            return cursor;
        });

        RegisterImporter<TrueTypeFont>((path, args) =>
        {
            TrueTypeFont ttf;
            if (args.Length == 2)
            {
                ttf = new TrueTypeFont(path, (int)args[0], (string)args[1]);
            }
            else if (args.Length == 1)
            {
                ttf = new TrueTypeFont(path, (int)args[0]);
            }
            else
            {
                ttf = new TrueTypeFont(path, 12);
            }

            return ttf;
        });
    }

    private string MakeAbsolutePath(string relativePath)
        => Path.Combine(ContentRoot, relativePath);

    private void OnResourceDisposing(object? sender, EventArgs e)
    {
        if (sender is DisposableResource disposable)
        {
            disposable.Disposing -= OnResourceDisposing;
            _loadedResources.Remove(disposable);
        }
    }
}