using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class Camera
    {
        internal SDL_gpu.GPU_Camera GpuCamera;

        public int X
        {
            get => (int)GpuCamera.x;
            set => GpuCamera.x = value;
        }

        public int Y
        {
            get => (int)GpuCamera.y;
            set => GpuCamera.y = value;
        }

        public int Z
        {
            get => (int)GpuCamera.z;
            set => GpuCamera.z = value;
        }

        public int ZoomX
        {
            get => (int)GpuCamera.zoom_x;
            set => GpuCamera.zoom_x = value;
        }

        public int ZoomY
        {
            get => (int)GpuCamera.zoom_y;
            set => GpuCamera.zoom_y = value;
        }

        public bool UseCenteredOrigin
        {
            get => GpuCamera.use_centered_origin;
            set => GpuCamera.use_centered_origin = value;
        }

        public float Angle
        {
            get => GpuCamera.angle;
            set => GpuCamera.angle = value;
        }

        public float FarZ
        {
            get => GpuCamera.z_far;
            set => GpuCamera.z_far = value;
        }

        public float NearZ
        {
            get => GpuCamera.z_near;
            set => GpuCamera.z_near = value;
        }

        public Camera()
        {
            GpuCamera = SDL_gpu.GPU_GetDefaultCamera();
        }

        public Camera(int x, int y)
            : this(x, y, 0, 0)
        {
        }

        public Camera(int x, int y, int z)
            : this(x, y, z, 0)
        {
        }

        public Camera(int x, int y, int z, float angle)
        {
            X = x;
            Y = y;
            Z = z;
            Angle = angle;

            ZoomX = 1;
            ZoomY = 1;
            FarZ = 100;
            NearZ = 100;
            UseCenteredOrigin = true;
        }
    }
}