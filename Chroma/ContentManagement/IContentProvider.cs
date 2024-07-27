namespace Chroma.ContentManagement;

using System;
using System.IO;
using Chroma.MemoryManagement;

public interface IContentProvider : IDisposable
{
    string ContentRoot { get; }

    T? Load<T>(string relativePath, params object[] args) where T : DisposableResource;
    void Unload<T>(T resource) where T : DisposableResource;

    byte[] Read(string relativePath);
    Stream Open(string relativePath);

    void Track<T>(T resource) where T : DisposableResource;
    void StopTracking<T>(T resource) where T : DisposableResource;

    void RegisterImporter<T>(Func<string, object[], object> importer) where T : DisposableResource;
    void UnregisterImporter<T>() where T : DisposableResource;
    bool IsImporterPresent<T>() where T : DisposableResource;
}