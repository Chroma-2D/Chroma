using System.Numerics;
using Chroma.Extensions;
using Chroma.Natives.SDL;
using Chroma.Windowing;

namespace Chroma.Graphics
{
    public static class RenderTransform
    {
        public static Matrix4x4 Matrix
        {
            get
            {
                unsafe
                {
                    return Graphics.Matrix.FromFloatPointer(SDL_gpu.GPU_GetCurrentMatrix());
                }
            }

            set => SDL_gpu.GPU_LoadMatrix(value.ToFloatArray());
        }

        public static void Frustum(float left, float top, float right, float bottom, float z_near, float z_far)
        {
            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixFrustum(mtx, left, right, bottom, top, z_near, z_far);

            LoadMatrixByFloatPointer(mtx);
        }

        public static void Shear(Vector2 vec)
        {
            var mtx = Matrix;
            {
                mtx.M21 = vec.X;
                mtx.M12 = vec.Y;
            }
            Matrix = mtx;
        }

        public static void Scale(Vector3 vec)
        {
            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixScale(mtx, vec.X, vec.Y, vec.Z);

            LoadMatrixByFloatPointer(mtx);
        }

        public static void Scale(Vector2 vec)
            => Scale(new Vector3(vec, 1));

        public static void Translate(Vector3 vec)
        {
            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixTranslate(mtx, vec.X, vec.Y, vec.Z);

            LoadMatrixByFloatPointer(mtx);
        }

        public static void Translate(Vector2 vec)
            => Translate(new Vector3(vec, 0));

        public static void Rotate(float angle, Vector3 pivot)
        {
            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixRotate(mtx, angle, pivot.X, pivot.Y, pivot.Z);

            LoadMatrixByFloatPointer(mtx);
        }

        public static void Push()
            => SDL_gpu.GPU_PushMatrix();

        public static void Pop()
            => SDL_gpu.GPU_PopMatrix();

        public static void SetMatrixMode(MatrixMode mode, RenderTarget target)
            => SDL_gpu.GPU_MatrixMode(
                target.TargetHandle,
                (SDL_gpu.GPU_MatrixModeEnum)mode
            );
        
        public static void SetMatrixMode(MatrixMode mode, Window window)
            => SDL_gpu.GPU_MatrixMode(
                window.RenderTargetHandle,
                (SDL_gpu.GPU_MatrixModeEnum)mode
            );

        public static void LoadIdentity()
            => SDL_gpu.GPU_LoadIdentity();

        public static void Orthographic(float left, float top, float right, float bottom, float z_near, float z_far)
        {
            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixOrtho(mtx, left, right, bottom, top, z_near, z_far);

            LoadMatrixByFloatPointer(mtx);
        }

        public static void LookAt(Vector3 eyePosition, Vector3 targetPosition, Vector3 up)
        {
            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixLookAt(
                mtx, 
                eyePosition.X, 
                eyePosition.Y, 
                eyePosition.Z,
                targetPosition.X,
                targetPosition.Y,
                targetPosition.Z,
                up.X,
                up.Y,
                up.Z
            );
        }

        private static unsafe void LoadMatrixByFloatPointer(float[] mtx)
        {
            fixed (float* mtxptr = &mtx[0])
            {
                Matrix = Graphics.Matrix.FromFloatPointer(mtxptr);
            }
        }
    }
}