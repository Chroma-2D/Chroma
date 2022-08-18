using System;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Graphics
{
    public struct Color : IEquatable<Color>
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public float Hue
        {
            get
            {
                var (min, max, d, h) = GetHsvParameters();

                if (d == 0 && h == 0)
                    return 0;

                return 60f * (h - d / (max - min));
            }
        }

        public float Saturation
        {
            get
            {
                var (min, max, _, _) = GetHsvParameters();

                if (Math.Abs(min - max) < 0.01f)
                    return 0;

                return (max - min) / max;
            }
        }

        public float Value
        {
            get
            {
                var (min, max, _, _) = GetHsvParameters();

                if (Math.Abs(min - max) < 0.01f)
                    return min;

                return max;
            }
        }

        public readonly PackedValue Packed;

        public Color(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;

            Packed = new PackedValue();
            Packed.Owner = this;
        }

        public Color(byte r, byte g, byte b)
            : this(r, g, b, 255)
        {
        }

        public Color(float r, float g, float b, float a) : this(
            (byte)(255 * Math.Clamp(r, 0f, 1f)),
            (byte)(255 * Math.Clamp(g, 0f, 1f)),
            (byte)(255 * Math.Clamp(b, 0f, 1f)),
            (byte)(255 * Math.Clamp(a, 0f, 1f)))
        {
        }

        public Color(float r, float g, float b)
            : this(r, g, b, 1f)
        {
        }

        public Color(uint rgbaPacked)
            : this(
                (byte)((rgbaPacked & 0xFF000000) >> 24),
                (byte)((rgbaPacked & 0x00FF0000) >> 16),
                (byte)((rgbaPacked & 0x0000FF00) >> 8),
                (byte)(rgbaPacked & 0x000000FF)
            )
        {
        }

        public Color(Color other)
            : this(other.R, other.G, other.B, other.A)
        {
        }

        public static readonly Color AliceBlue = new(240, 248, 255);
        public static readonly Color AntiqueWhite = new(250, 235, 215);
        public static readonly Color Aqua = new(0, 255, 255);
        public static readonly Color Aquamarine = new(127, 255, 255);
        public static readonly Color Azure = new(240, 255, 255);
        public static readonly Color Beige = new(245, 245, 220);
        public static readonly Color Bisque = new(255, 228, 196);
        public static readonly Color Black = new(0, 0, 0);
        public static readonly Color BlanchedAlmond = new(255, 235, 205);
        public static readonly Color Blue = new(0, 0, 255);
        public static readonly Color BlueViolet = new(138, 43, 226);
        public static readonly Color Brown = new(165, 42, 42);
        public static readonly Color BurlyWood = new(222, 184, 135);
        public static readonly Color CadetBlue = new(95, 158, 160);
        public static readonly Color Chartreuse = new(127, 255, 0);
        public static readonly Color Chocolate = new(210, 105, 30);
        public static readonly Color Coral = new(255, 127, 80);
        public static readonly Color CornflowerBlue = new(100, 149, 237);
        public static readonly Color Cornsilk = new(255, 248, 220);
        public static readonly Color Crimson = new(220, 20, 60);
        public static readonly Color Cyan = new(0, 255, 255);
        public static readonly Color DarkBlue = new(0, 0, 139);
        public static readonly Color DarkCyan = new(0, 139, 139);
        public static readonly Color DarkGolderod = new(184, 134, 11);
        public static readonly Color DarkGray = new(169, 169, 169);
        public static readonly Color DarkGreen = new(0, 100, 0);
        public static readonly Color DarkKhaki = new(189, 183, 107);
        public static readonly Color DarkMagenta = new(139, 0, 139);
        public static readonly Color DarkOliveGreen = new(85, 107, 47);
        public static readonly Color DarkOrange = new(255, 140, 0);
        public static readonly Color DarkOrchid = new(153, 50, 204);
        public static readonly Color DarkRed = new(139, 0, 0);
        public static readonly Color DarkSalmon = new(233, 150, 122);
        public static readonly Color DarkSeaGreen = new(143, 188, 139);
        public static readonly Color DarkSlateBlue = new(72, 61, 139);
        public static readonly Color DarkSlateGray = new(47, 79, 79);
        public static readonly Color DarkTurquoise = new(0, 206, 209);
        public static readonly Color DarkViolet = new(148, 0, 211);
        public static readonly Color DeepPink = new(255, 20, 147);
        public static readonly Color DeepSkyBlue = new(0, 191, 255);
        public static readonly Color DimGray = new(105, 105, 105);
        public static readonly Color DodgerBlue = new(30, 144, 255);
        public static readonly Color Firebrick = new(178, 34, 34);
        public static readonly Color FloralWhite = new(255, 250, 240);
        public static readonly Color ForestGreen = new(34, 139, 34);
        public static readonly Color Fuchsia = new(255, 0, 255);
        public static readonly Color Gainsboro = new(220, 220, 220);
        public static readonly Color GhostWhite = new(248, 248, 255);
        public static readonly Color Gold = new(255, 215, 0);
        public static readonly Color Goldenrod = new(218, 165, 32);
        public static readonly Color Gray = new(128, 128, 128);
        public static readonly Color Green = new(0, 128, 0);
        public static readonly Color GreenYellow = new(173, 255, 47);
        public static readonly Color Honeydew = new(240, 255, 240);
        public static readonly Color HotPink = new(255, 105, 180);
        public static readonly Color IndianRed = new(205, 92, 92);
        public static readonly Color Indigo = new(75, 0, 130);
        public static readonly Color Ivory = new(255, 255, 240);
        public static readonly Color Khaki = new(240, 230, 140);
        public static readonly Color Lavender = new(230, 230, 230);
        public static readonly Color LavenderBlush = new(255, 240, 245);
        public static readonly Color LawnGreen = new(124, 252, 0);
        public static readonly Color LemonChiffon = new(255, 250, 205);
        public static readonly Color LightBlue = new(173, 216, 230);
        public static readonly Color LightCoral = new(240, 128, 128);
        public static readonly Color LightCyan = new(224, 255, 255);
        public static readonly Color LightGoldenrodYellow = new(250, 250, 210);
        public static readonly Color LightGreen = new(144, 238, 144);
        public static readonly Color LightGray = new(211, 211, 211);
        public static readonly Color LightPink = new(255, 182, 193);
        public static readonly Color LightSalmon = new(255, 160, 122);
        public static readonly Color LightSeaGreen = new(32, 178, 170);
        public static readonly Color LightSkyBlue = new(136, 206, 250);
        public static readonly Color LightSlateGray = new(119, 136, 153);
        public static readonly Color LightSteelBlue = new(176, 196, 222);
        public static readonly Color LightYellow = new(255, 255, 224);
        public static readonly Color Lime = new(0, 255, 0);
        public static readonly Color LimeGreen = new(50, 205, 50);
        public static readonly Color Linen = new(250, 240, 230);
        public static readonly Color Magenta = new(255, 0, 255);
        public static readonly Color Maroon = new(128, 0, 0);
        public static readonly Color MediumAquamarine = new(102, 205, 170);
        public static readonly Color MediumBlue = new(0, 0, 205);
        public static readonly Color MediumOrchid = new(186, 85, 211);
        public static readonly Color MediumPurple = new(147, 112, 219);
        public static readonly Color MediumSeaGreen = new(60, 179, 113);
        public static readonly Color MediumSlateBlue = new(123, 104, 238);
        public static readonly Color MediumSpringGreen = new(0, 250, 154);
        public static readonly Color MediumTurquoise = new(72, 209, 204);
        public static readonly Color MediumVioletRed = new(199, 21, 133);
        public static readonly Color MidnightBlue = new(25, 25, 112);
        public static readonly Color MintCream = new(245, 255, 250);
        public static readonly Color MistyRose = new(255, 228, 225);
        public static readonly Color Moccasin = new(255, 228, 181);
        public static readonly Color NavajoWhite = new(255, 222, 173);
        public static readonly Color Navy = new(0, 0, 128);
        public static readonly Color OldLace = new(253, 245, 230);
        public static readonly Color Olive = new(128, 128, 0);
        public static readonly Color OliveDrab = new(107, 142, 35);
        public static readonly Color Orange = new(255, 165, 0);
        public static readonly Color OrangeRed = new(255, 69, 0);
        public static readonly Color Orchid = new(218, 112, 214);
        public static readonly Color PaleGoldenrod = new(238, 232, 170);
        public static readonly Color PaleGreen = new(152, 251, 152);
        public static readonly Color PaleTurquoise = new(175, 238, 238);
        public static readonly Color PaleVioletRed = new(219, 112, 147);
        public static readonly Color PapayaWhip = new(255, 239, 213);
        public static readonly Color PeachPuff = new(255, 218, 185);
        public static readonly Color Peru = new(205, 133, 63);
        public static readonly Color Pink = new(255, 192, 203);
        public static readonly Color Plum = new(221, 160, 221);
        public static readonly Color PowderBlue = new(176, 224, 230);
        public static readonly Color Purple = new(128, 0, 128);
        public static readonly Color Red = new(255, 0, 0);
        public static readonly Color RosyBrown = new(188, 143, 143);
        public static readonly Color RoyalBlue = new(65, 105, 255);
        public static readonly Color SaddleBrown = new(139, 69, 19);
        public static readonly Color Salmon = new(250, 128, 114);
        public static readonly Color SandyBrown = new(244, 164, 96);
        public static readonly Color SeaGreen = new(46, 139, 87);
        public static readonly Color SeaShell = new(255, 245, 238);
        public static readonly Color Sienna = new(160, 82, 45);
        public static readonly Color Silver = new(192, 192, 192);
        public static readonly Color SkyBlue = new(135, 206, 235);
        public static readonly Color SlateBlue = new(106, 90, 205);
        public static readonly Color SlateGray = new(112, 128, 144);
        public static readonly Color Snow = new(255, 250, 250);
        public static readonly Color SpringGreen = new(0, 255, 127);
        public static readonly Color SteelBlue = new(70, 130, 180);
        public static readonly Color Tan = new(210, 180, 140);
        public static readonly Color Teal = new(0, 128, 128);
        public static readonly Color Thistle = new(216, 191, 216);
        public static readonly Color Tomato = new(255, 99, 71);
        public static readonly Color Transparent = new(0, 0, 0, 0);
        public static readonly Color Turquoise = new(64, 224, 208);
        public static readonly Color Violet = new(238, 130, 238);
        public static readonly Color White = new(255, 255, 255);
        public static readonly Color WhiteSmoke = new(245, 245, 245);
        public static readonly Color Yellow = new(255, 255, 0);
        public static readonly Color YellowGreen = new(154, 205, 50);

        public float[] AsNormalizedFloatArray()
            => new[] {R / 255f, G / 255f, B / 255f, A / 255f};
        
        public Color Alpha(byte alpha) 
            => new(R, G, B, alpha);

        public Color Alpha(float alpha)
            => new(R / 255f, G / 255f, B / 255f, alpha);

        public override bool Equals(object obj)
            => obj is Color color && Equals(color);

        public static Color Lerp(Color a, Color b, float t)
        {
            return new(
                255 * (a.R + (b.R - a.R) * t),
                255 * (a.G + (b.G - a.G) * t),
                255 * (a.B + (b.B - a.B) * t),
                255 * (a.A + (b.A - a.A) * t)
            );
        }

        public static Color FromHSV(float hue, float sat = 1f, float val = 1f)
        {
            if (hue < 0)
                hue = Math.Abs(hue);

            var i = (int)(Math.Floor(hue / 60) % 6);
            var f = (hue / 60) - Math.Floor(hue / 60);

            var v = (byte)(255 * val);
            var p = (byte)(v * (1 - sat));
            var q = (byte)(v * (1 - f * sat));
            var t = (byte)(v * (1 - (1 - f) * sat));

            return i switch
            {
                0 => new Color(v, t, p),
                1 => new Color(q, v, p),
                2 => new Color(p, v, t),
                3 => new Color(p, q, v),
                4 => new Color(t, p, v),
                _ => new Color(v, p, q)
            };
        }

        public override int GetHashCode()
            => Packed.RGBA.GetHashCode();

        public bool Equals(Color other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B &&
                   A == other.A &&
                   Packed.RGBA == other.Packed.RGBA;
        }

        public static bool operator ==(Color left, Color right)
            => left.R == right.R &&
               left.G == right.G &&
               left.B == right.B &&
               left.A == right.A;

        public static bool operator !=(Color left, Color right)
            => !(left == right);

        public static implicit operator System.Drawing.Color(Color chromaColor)
        {
            return System.Drawing.Color.FromArgb(
                chromaColor.A,
                chromaColor.R,
                chromaColor.G,
                chromaColor.B
            );
        }

        public static implicit operator Color(System.Drawing.Color systemDrawingColor)
        {
            return new(
                systemDrawingColor.R,
                systemDrawingColor.G,
                systemDrawingColor.B,
                systemDrawingColor.A
            );
        }

        internal static SDL2.SDL_Color ToSdlColor(Color color)
            => new()
            {
                r = color.R,
                g = color.G,
                b = color.B,
                a = color.A
            };

        internal static Color FromSdlColor(SDL2.SDL_Color color)
            => new(color.r, color.g, color.b, color.a);

        private (float, float, float, float) GetHsvParameters()
        {
            var rf = R / 255f;
            var gf = G / 255f;
            var bf = B / 255f;
            
            var minRgb = Math.Min(rf, Math.Min(gf, bf));
            var maxRgb = Math.Max(rf, Math.Max(gf, bf));

            if (Math.Abs(minRgb - maxRgb) < 0.001f)
                return (minRgb, maxRgb, 0, 0);

            float d = (Math.Abs(rf - minRgb) < 0.001f) ? gf - bf : ((Math.Abs(bf - minRgb) < 0.001f) ? rf - gf : bf - rf);
            float h = (Math.Abs(rf - minRgb) < 0.001f) ? 3 : ((Math.Abs(bf - minRgb) < 0.001f) ? 1 : 5);

            return (minRgb, maxRgb, d, h);
        }

        public class PackedValue
        {
            internal Color Owner { get; set; }

            public uint RGBA
            {
                get
                {
                    var value = 0u;

                    value |= (uint)(Owner.R << 24);
                    value |= (uint)(Owner.G << 16);
                    value |= (uint)(Owner.B << 8);
                    value |= Owner.A;

                    return value;
                }
            }

            public uint ARGB
            {
                get
                {
                    var value = 0u;

                    value |= (uint)(Owner.A << 24);
                    value |= (uint)(Owner.R << 16);
                    value |= (uint)(Owner.G << 8);
                    value |= Owner.B;

                    return value;
                }
            }

            public uint BGRA
            {
                get
                {
                    var value = 0u;

                    value |= (uint)(Owner.B << 24);
                    value |= (uint)(Owner.G << 16);
                    value |= (uint)(Owner.R << 8);
                    value |= Owner.A;

                    return value;
                }
            }

            public uint ABGR
            {
                get
                {
                    var value = 0u;

                    value |= (uint)(Owner.A << 24);
                    value |= (uint)(Owner.B << 16);
                    value |= (uint)(Owner.G << 8);
                    value |= Owner.R;

                    return value;
                }
            }

            internal PackedValue()
            {
            }
        }
    }
}