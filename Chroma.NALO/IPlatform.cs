namespace Chroma.NALO;

internal interface IPlatform
{
    NativeLibraryRegistry Registry { get; }
        
    void Register(string libFilePath);
}