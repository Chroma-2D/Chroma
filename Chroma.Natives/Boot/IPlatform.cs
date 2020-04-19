namespace Chroma.Natives.Boot
{
    internal interface IPlatform
    {
        NativeLibraryRegistry Registry { get; }
        
        void Register(string libFilePath);
    }
}
