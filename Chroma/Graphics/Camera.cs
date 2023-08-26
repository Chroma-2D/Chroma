using System.Numerics;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Graphics
{
    public class Camera
    {
        internal SDL_gpu.GPU_Camera GpuCamera;

        public float X
        {
            get => GpuCamera.x;
            set => GpuCamera.x = value;
        }

        public float Y
        {
            get => GpuCamera.y;
            set => GpuCamera.y = value;
        }

        public float Z
        {
            get => GpuCamera.z;
            set => GpuCamera.z = value;
        }

        public Vector3 Position
        {
            get => new(X, Y, Z);

            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public float ZoomX
        {
            get => GpuCamera.zoom_x;
            set => GpuCamera.zoom_x = value;
        }

        public float ZoomY
        {
            get => GpuCamera.zoom_y;
            set => GpuCamera.zoom_y = value;
        }

        public Vector2 Zoom
        {
            get => new(ZoomX, ZoomY);
            
            set
            {
                ZoomX = value.X;
                ZoomY = value.Y;
            }
        }

        public bool UseCenteredOrigin
        {
            get => GpuCamera.use_centered_origin;
            set => GpuCamera.use_centered_origin = value;
        }

        public float Rotation
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

        public static Camera Default
        {
            get
            {
                var c = new Camera
                {
                    GpuCamera = SDL_gpu.GPU_GetDefaultCamera()
                };

                return c;
            }
        }

        public Camera()
        {
            Zoom = Vector2.One;
        }

        public Camera(Camera other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;

            ZoomX = other.ZoomX;
            ZoomY = other.ZoomY;

            UseCenteredOrigin = other.UseCenteredOrigin;
            Rotation = other.Rotation;
            
            FarZ = other.FarZ;
            NearZ = other.NearZ;
        }

        public Camera(Vector2 position)
            : this((int)position.X, (int)position.Y, 0, 0)
        {
        }

        public Camera(Vector3 position)
            : this((int)position.X, (int)position.Y, (int)position.Z, 0)
        {
        }

        public Camera(int x, int y)
            : this(x, y, 0, 0)
        {
        }

        public Camera(int x, int y, int z)
            : this(x, y, z, 0)
        {
        }

        public Camera(int x, int y, int z, float rotation)
        {
            X = x;
            Y = y;
            Z = z;
            Rotation = rotation;

            ZoomX = 1;
            ZoomY = 1;
            FarZ = 100;
            NearZ = 100;
            UseCenteredOrigin = true;
        }
    }
}