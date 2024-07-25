namespace Chroma;

using System.Numerics;

public static partial class Extensions
{
    public static float[] ToFloatArray(this Matrix4x4 mtx)
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