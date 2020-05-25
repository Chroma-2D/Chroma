using System.Numerics;

namespace Chroma.Graphics
{
    public class Matrix
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

        public static float[] ToFloatArray(Matrix4x4 mtx)
        {
            var m = new float[16];

            m[0] = mtx.M11;
            m[1] = mtx.M12;
            m[2] = mtx.M13;
            m[3] = mtx.M14;

            m[4] = mtx.M21;
            m[5] = mtx.M22;
            m[6] = mtx.M23;
            m[7] = mtx.M24;
            
            m[8] = mtx.M31;
            m[9] = mtx.M32;
            m[10] = mtx.M33;
            m[11] = mtx.M34;
            
            m[12] = mtx.M41;
            m[13] = mtx.M42;
            m[14] = mtx.M43;
            m[15] = mtx.M44;

            return m;
        }
    }
}