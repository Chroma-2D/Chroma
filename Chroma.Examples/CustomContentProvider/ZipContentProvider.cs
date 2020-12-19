using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Chroma;
using Chroma.Audio.Sources;
using Chroma.ContentManagement;
using Chroma.Diagnostics.Logging;
using Chroma.Graphics;
using Chroma.MemoryManagement;

namespace CustomContentProvider
{
    public class ZipContentProvider : DisposableResource, IContentProvider
    {
        private Dictionary<Type, Func<string, object[], object>> _importers = new();

        private Game _game;
        private FileStream _zipFileStream;
        private ZipArchive _zipArchive;

        private Log Log { get; } = LogManager.GetForCurrentAssembly();

        public string ContentRoot { get; }

        public ZipContentProvider(Game game, string zipFile)
        {
            ContentRoot = zipFile;

            _game = game;
            _zipFileStream = new FileStream(zipFile, FileMode.Open);
            _zipArchive = new ZipArchive(_zipFileStream, ZipArchiveMode.Read);

            foreach (var entry in _zipArchive.Entries)
            {
                if (entry.FullName.EndsWith('/'))
                    continue;

                Log.Info(entry.FullName);
            }

            _importers.Add(typeof(Texture), (path, _) =>
            {
                using (var stream = Open(path))
                    return new Texture(stream);
            });

            _importers.Add(typeof(Sound), (path, _) =>
            {
                using (var stream = Open(path))
                    return new Sound(stream);
            });
        }

        public T Load<T>(string relativePath, params object[] args) where T : DisposableResource
        {
            return _importers[typeof(T)](relativePath, args) as T;
        }

        public void Unload<T>(T resource) where T : DisposableResource
        {
            throw new NotImplementedException();
        }

        public void Track<T>(T resource) where T : DisposableResource
        {
            throw new NotImplementedException();
        }

        public void StopTracking<T>(T resource) where T : DisposableResource
        {
            throw new NotImplementedException();
        }

        public byte[] Read(string relativePath)
        {
            using (var stream = _zipArchive.GetEntry(relativePath)?.Open())
            {
                using (var ms = new MemoryStream())
                {
                    stream?.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        public Stream Open(string relativePath)
            => _zipArchive.GetEntry(relativePath)?.Open();

        public void RegisterImporter<T>(Func<string, object[], object> importer) where T : DisposableResource
        {
            throw new NotImplementedException();
        }

        public void UnregisterImporter<T>() where T : DisposableResource
        {
            throw new NotImplementedException();
        }

        public bool IsImporterPresent<T>() where T : DisposableResource
        {
            throw new NotImplementedException();
        }
    }
}