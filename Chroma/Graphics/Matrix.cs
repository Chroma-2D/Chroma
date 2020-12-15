using System.Numerics;

namespace Chroma.Graphics
{
    public static class Matrix
    {
        internal static unsafe Matrix4x4 FromFloatPointer(float* ptr)
        {
            var mtx = new Matrix4x4
            {
                M11 = ptr[0],
                M12 = ptr[1],
                M13 = ptr[2],
                M14 = ptr[3],
                M21 = ptr[4],
                M22 = ptr[5],
                M23 = ptr[6],
                M24 = ptr[7],
                M31 = ptr[8],
                M32 = ptr[9],
                M33 = ptr[10],
                M34 = ptr[11],
                M41 = ptr[12],
                M42 = ptr[13],
                M43 = ptr[14],
                M44 = ptr[15]
            };

            return mtx;
        }
    }
}