using System.IO;
using Chroma.Diagnostics.Logging;

namespace Chroma.Graphics.TextRendering.Bitmap
{
    internal class BitmapFontPage
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        public int ID { get; }
        public string FileName { get; }
        public Texture Texture { get; }

        public BitmapFontPage(int id, string fileName)
        {
            ID = id;
            FileName = fileName;

            if (File.Exists(fileName))
            {
                Texture = new Texture(fileName);
            }
            else
            {
                _log.Warning($"Page {ID} texture file does not exist.");
            }
        }
    }
}