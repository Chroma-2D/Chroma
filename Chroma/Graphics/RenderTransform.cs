using System.Numerics;
using Chroma.Extensions;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class RenderTransform
    {
        private readonly RenderContext _context;

        public Matrix4x4 Matrix
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

        public bool AutoLoadIdentity { get; set; } = true;

        internal RenderTransform(RenderContext context)
            => _context = context;

        public void Frustum(float left, float top, float right, float bottom, float z_near, float z_far)
        {
            if (AutoLoadIdentity)
                SDL_gpu.GPU_LoadIdentity();

            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixFrustum(mtx, left, right, bottom, top, z_near, z_far);

            LoadMatrixByFloatPointer(mtx);
        }

        public void Shear(Vector2 vec)
        {
            if (AutoLoadIdentity)
                SDL_gpu.GPU_LoadIdentity();

            var mtx = Matrix;
            {
                mtx.M21 = vec.X;
                mtx.M12 = vec.Y;
            }
            Matrix = mtx;
        }

        public void Scale(Vector3 vec)
        {
            if (AutoLoadIdentity)
                SDL_gpu.GPU_LoadIdentity();

            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixScale(mtx, vec.X, vec.Y, vec.Z);

            LoadMatrixByFloatPointer(mtx);
        }

        public void Scale(Vector2 vec)
            => Scale(new Vector3(vec, 1));

        public void Translate(Vector3 vec)
        {
            if (AutoLoadIdentity)
                SDL_gpu.GPU_LoadIdentity();

            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixTranslate(mtx, vec.X, vec.Y, vec.Z);

            LoadMatrixByFloatPointer(mtx);
        }

        public void Translate(Vector2 vec)
            => Translate(new Vector3(vec, 0));

        public void Rotate(float angle, Vector3 pivot)
        {
            if (AutoLoadIdentity)
                SDL_gpu.GPU_LoadIdentity();

            var mtx = Matrix.ToFloatArray();
            SDL_gpu.GPU_MatrixRotate(mtx, angle, pivot.X, pivot.Y, pivot.Z);

            LoadMatrixByFloatPointer(mtx);
        }

        public void Push()
            => SDL_gpu.GPU_PushMatrix();

        public void Pop()
            => SDL_gpu.GPU_PopMatrix();

        public void SetMatrixMode(MatrixMode mode)
            => SDL_gpu.GPU_MatrixMode(
                _context.CurrentRenderTarget,
                (SDL_gpu.GPU_MatrixModeEnum)mode
            );

        private unsafe void LoadMatrixByFloatPointer(float[] mtx)
        {
            fixed (float* mtxptr = &mtx[0])
            {
                Matrix = Graphics.Matrix.FromFloatPointer(mtxptr);
            }
        }
    }
}