using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Chroma.Audio;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.Graphics.TextRendering.Bitmap;
using Chroma.MemoryManagement;

namespace Chroma.ContentManagement.FileSystem
{
    public class FileSystemContentProvider : DisposableResource, IContentProvider
    {
        private readonly Game _game;

        private readonly HashSet<DisposableResource> _loadedResources;
        private readonly Dictionary<Type, Func<string, object[], object>> _importers;

        private Log Log { get; } = LogManager.GetForCurrentAssembly();
        
        public string ContentRoot { get; }

        public FileSystemContentProvider(Game game, string contentRoot = null)
        {
            _game = game;

            ContentRoot = contentRoot;

            if (string.IsNullOrEmpty(ContentRoot))
            {
                var appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                ContentRoot = Path.Combine(appDirectory, "Content");
            }

            if (!Directory.Exists(ContentRoot))
                Directory.CreateDirectory(ContentRoot);
            
            Log.Debug($"ContentRoot seems to be at '{ContentRoot}'.");

            _loadedResources = new HashSet<DisposableResource>();
            _importers = new Dictionary<Type, Func<string, object[], object>>();

            RegisterImporters();
        }

        public T Load<T>(string relativePath, params object[] args) where T : DisposableResource
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
            _loadedResources.Add(resource);

            return resource;
        }

        public void Unload<T>(T loadedResource) where T : DisposableResource
        {
            if (!_loadedResources.Contains(loadedResource))
                throw new ContentNotLoadedException("The content you want to unload was never loaded in the first place.");

            loadedResource.Dispose();
        }

        public Stream Open(string relativePath)
            => new FileStream(MakeAbsolutePath(relativePath), FileMode.Open);

        public byte[] Read(string relativePath)
            => File.ReadAllBytes(MakeAbsolutePath(relativePath));

        protected override void FreeManagedResources()
        {
            foreach (var resource in _loadedResources)
                resource.Dispose();
        }

        private void RegisterImporters()
        {
            _importers.Add(typeof(Texture), (path, args) =>
            {
                var texture = new Texture(path);
                texture.Disposing += OnResourceDisposing;
                
                return texture;
            });
            
            _importers.Add(typeof(PixelShader), (path, args) =>
            {
                var ps = new PixelShader(path);
                ps.Disposing += OnResourceDisposing;
                
                return ps;
            });
            
            _importers.Add(typeof(VertexShader), (path, args) =>
            {
                var vs = new VertexShader(path);
                vs.Disposing += OnResourceDisposing;

                return vs;
            });
            
            _importers.Add(typeof(TrueTypeFont), (path, args) =>
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
                ttf.Disposing += OnResourceDisposing;
                
                return ttf;
            });
            
            _importers.Add(typeof(BitmapFont), (path, args) =>
            {
                var imFont = new BitmapFont(path);
                imFont.Disposing += OnResourceDisposing;

                return imFont;
            });
            
            _importers.Add(typeof(Sound), (path, args) =>
            {
                var sound = _game.Audio.CreateSound(path);
                sound.Disposing += OnResourceDisposing;

                return sound;
            });
            
            _importers.Add(typeof(Music), (path, args) =>
            {
                var music = _game.Audio.CreateMusic(path);
                music.Disposing += OnResourceDisposing;

                return music;
            });
        }

        private string MakeAbsolutePath(string relativePath)
            => Path.Combine(ContentRoot, relativePath);

        private void OnResourceDisposing(object sender, EventArgs e)
        {
            if (sender is DisposableResource disposable)
            {
                disposable.Disposing -= OnResourceDisposing;
                _loadedResources.Remove(disposable);
            }
        }
    }
}
