using Chroma.STB.Image.Decoding;
using System.IO;

namespace Chroma.STB.Image
{
    internal struct ImageInfo
    {
        public int Width;
        public int Height;
        public ColorComponents ColorComponents;
        public int BitsPerChannel;

        public static ImageInfo? FromStream(Stream stream)
        {
            var info = JpgDecoder.Info(stream);
            if (info != null)
            {
                return info;
            }

            info = PngDecoder.Info(stream);
            if (info != null)
            {
                return info;
            }

            info = GifDecoder.Info(stream);
            if (info != null)
            {
                return info;
            }

            info = BmpDecoder.Info(stream);
            if (info != null)
            {
                return info;
            }

            info = PsdDecoder.Info(stream);
            if (info != null)
            {
                return info;
            }

            info = TgaDecoder.Info(stream);
            if (info != null)
            {
                return info;
            }

            return null;
        }
    }
}