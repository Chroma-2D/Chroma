using System;
using System.Numerics;

namespace MusicAndSounds
{
    public class FFT
    {
        //
        // Found somewhere on the internets.
        // Fuck if I know where.
        //
        // Adapted for use with SDL_mixer and .NET 5.
        //
        public static void CalculateFFT(Complex[] samples, float[] result)
        {
            var power = (int)Math.Log(samples.Length, 2);
            var count = 1;
            for (var i = 0; i < power; i++)
                count <<= 1;

            var mid = count >> 1;
            var j = 0;
            for (var i = 0; i < count - 1; i++)
            {
                if (i < j)
                {
                    var tmp = samples[i];
                    samples[i] = samples[j];
                    samples[j] = tmp;
                }
                var k = mid;
                while (k <= j)
                {
                    j -= k;
                    k >>= 1;
                }
                j += k;
            }

            var r = new Complex(-1, 0);
            var l2 = 1;
            for (var l = 0; l < power; l++)
            {
                var l1 = l2;
                l2 <<= 1;
                var r2 = new Complex(1, 0);
                for (var n = 0; n < l1; n++)
                {
                    for (var i = n; i < count; i += l2)
                    {
                        var i1 = i + l1;
                        var tmp = r2 * samples[i1];
                        samples[i1] = samples[i] - tmp;
                        samples[i] += tmp;
                    }
                    r2 = r2 * r;
                }
                r = new Complex(Math.Sqrt((1d + r.Real) / 2d), -Math.Sqrt((1d - r.Real) / 2d));
            }
            
            var scale = 1d / count;

            for (var i = 0; i < count; i++)
                samples[i] *= scale;

            for (var i = 0; i < samples.Length / 2; i++)
                result[i] = (float)samples[i].Magnitude;
        }
    }
}