namespace Chroma.Graphics.TextRendering.Bitmap;

using System;
using System.IO;
using Chroma.Diagnostics.Logging;

internal class BitmapFontPage
{
    private static readonly Log _log = LogManager.GetForCurrentAssembly();

    public int ID { get; }
        
    public string RootDirectory { get; }
    public string FileName { get; }
        
    public Texture? Texture { get; }

    public BitmapFontPage(int id, string rootDirectory, string fileName, Func<string, Texture>? textureLoader = null)
    {
        ID = id;
            
        RootDirectory = rootDirectory;
        FileName = fileName;

        if (textureLoader == null)
        {
            var absoluteFilePath = Path.Combine(RootDirectory, FileName);
                
            if (File.Exists(absoluteFilePath))
            {
                Texture = new Texture(absoluteFilePath);
            }
            else
            {
                _log.Warning($"Page {ID} texture file does not exist.");
            }
        }
        else
        {
            try
            {
                Texture = textureLoader.Invoke(fileName);
            }
            catch (Exception e)
            {
                _log.Exception(e);
            }

            if (Texture == null)
            {
                _log.Warning($"Page {ID} texture loader failed -- no texture was loaded.");
            }
        }
    }
}