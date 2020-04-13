using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Chroma.ContentManagement.Exceptions;
using Chroma.Graphics;
using Chroma.Graphics.Accelerated;
using Chroma.Graphics.TextRendering;
using Chroma.MemoryManagement;

namespace Chroma.ContentManagement.FileSystem
{
    public class FileSystemContentProvider : DisposableResource, IContentProvider
    {
        private readonly HashSet<DisposableResource> _loadedResources;
        private readonly Dictionary<Type, Func<string, object[], object>> _importers;

        public string ContentRoot { get; }

        public FileSystemContentProvider(string contentRoot = null)
        {
            ContentRoot = contentRoot;

            if (string.IsNullOrEmpty(ContentRoot))
            {
                var appDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                ContentRoot = Path.Combine(appDirectory, "Content");
            }

            if (!Directory.Exists(ContentRoot))
                Directory.CreateDirectory(ContentRoot);

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
            _loadedResources.Remove(loadedResource);
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
            _importers.Add(typeof(Texture), (path, args) => { return new Texture(path); });
            _importers.Add(typeof(PixelShader), (path, args) => { return new PixelShader(path); });
            _importers.Add(typeof(VertexShader), (path, args) => { return new VertexShader(path); });
            _importers.Add(typeof(TrueTypeFont), (path, args) =>
            {
                if (args.Length == 2)
                {
                    return new TrueTypeFont(path, (int)args[0], (string)args[1]);
                }
                else if (args.Length == 1)
                {
                    return new TrueTypeFont(path, (int)args[0], null);
                }
                else
                {
                    return new TrueTypeFont(path, 12, null);
                }
            });
            _importers.Add(typeof(ImageFont), (path, args) => { return new ImageFont(path, (string)args[0]); });
        }

        private string MakeAbsolutePath(string relativePath)
            => Path.Combine(ContentRoot, relativePath);
    }
}
