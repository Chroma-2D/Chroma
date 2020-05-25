using System.Numerics;

namespace Chroma.Graphics
{
    public static class Matrix
    {
        internal static unsafe Matrix4x4 FromFloatPointer(float* ptr)
        {
            var mtx = new Matrix4x4();
            mtx.M11 = ptr[0];
            mtx.M12 = ptr[1];
            mtx.M13 = ptr[2];
            mtx.M14 = ptr[3];
            
            mtx.M21 = ptr[4];
            mtx.M22 = ptr[5];
            mtx.M23 = ptr[6];
            mtx.M24 = ptr[7];
            
            mtx.M31 = ptr[8];
            mtx.M32 = ptr[9];
            mtx.M33 = ptr[10];
            mtx.M34 = ptr[11];
            
            mtx.M41 = ptr[12];
            mtx.M42 = ptr[13];
            mtx.M43 = ptr[14];
            mtx.M44 = ptr[15];

            return mtx;
        }
    }
}