using System;
using System.IO;
using Chroma.MemoryManagement;

namespace Chroma.ContentManagement
{
    public interface IContentProvider : IDisposable
    {
        string ContentRoot { get; }

        T Load<T>(string relativePath, params object[] args) where T : DisposableResource;
        void Unload<T>(T resource) where T : DisposableResource;

        byte[] Read(string relativePath);
        Stream Open(string relativePath);
    }
}