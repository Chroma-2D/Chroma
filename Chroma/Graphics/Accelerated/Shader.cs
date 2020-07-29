using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.Accelerated
{
    public abstract class Shader : DisposableResource
    {
        internal uint ProgramHandle;
        internal uint VertexShaderObjectHandle;
        internal uint PixelShaderObjectHandle;

        internal SDL_gpu.GPU_ShaderBlock Block;

        private Log Log => LogManager.GetForCurrentAssembly();

        public static bool IsDefaultGpuShaderActive
            => SDL_gpu.GPU_IsDefaultShaderProgram(SDL_gpu.GPU_GetCurrentShaderProgram());

        public static Matrix4x4 ModelViewProjectionMatrix
        {
            get
            {
                EnsureCustomShaderActive();

                var m = new float[16];

                unsafe
                {
                    fixed (float* ptr = &m[0])
                    {
                        SDL_gpu.GPU_GetModelViewProjection(ptr);
                        return CreateMatrixFromPointer(ptr);
                    }
                }
            }
        }

        public static Matrix4x4 ProjectionMatrix
        {
            get
            {
                EnsureCustomShaderActive();

                unsafe
                {
                    var mtxptr = SDL_gpu.GPU_GetProjection();
                    return CreateMatrixFromPointer(mtxptr);
                }
            }
        }

        public static Matrix4x4 ViewMatrix
        {
            get
            {
                EnsureCustomShaderActive();

                unsafe
                {
                    var mtxptr = SDL_gpu.GPU_GetView();
                    return CreateMatrixFromPointer(mtxptr);
                }
            }
        }

        public static Matrix4x4 ModelMatrix
        {
            get
            {
                EnsureCustomShaderActive();

                unsafe
                {
                    var mtxptr = SDL_gpu.GPU_GetModel();
                    return CreateMatrixFromPointer(mtxptr);
                }
            }
        }

        public static void Deactivate()
            => SDL_gpu.GPU_ActivateShaderProgram(0, IntPtr.Zero);

        public void Activate()
        {
            EnsureNotDisposed();

            if (ProgramHandle == 0)
            {
                Log.Warning($"Refusing to activate invalid shader.");
                return;
            }

            SDL_gpu.GPU_ActivateShaderProgram(ProgramHandle, ref Block);
        }

        public void SetUniform(string name, Texture value, int textureUnit)
        {
            EnsureNotDisposed();

            if (value.Disposed)
                throw new ArgumentException("Texture provided was already diposed.");

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Float uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetShaderImage(value.ImageHandle, loc, textureUnit);
        }

        public void SetUniform(string name, float value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Float uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformf(loc, value);
        }

        public void SetUniform(string name, int value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Int uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformi(loc, value);
        }

        public void SetUniform(string name, uint value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Uint uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformui(loc, value);
        }

        public void SetUniform(string name, Vector2 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec2 uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformfv(loc, 2, 1, new[] {value.X, value.Y});
        }

        public void SetUniform(string name, Vector3 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec3 uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformfv(loc, 3, 1, new[] {value.X, value.Y, value.Z});
        }

        public void SetUniform(string name, Vector4 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec4 uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformfv(loc, 4, 1, new[] {value.X, value.Y, value.Z, value.W});
        }

        public void SetUniform(string name, Matrix4x4 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Mat4 uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformMatrixfv(loc, 1, 4, 4, false, new[]
            {
                value.M11, value.M12, value.M13, value.M14,
                value.M21, value.M22, value.M23, value.M24,
                value.M31, value.M32, value.M33, value.M34,
                value.M41, value.M42, value.M43, value.M44
            });
        }

        public void SetUniform(string name, Color value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec4 uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformfv(loc, 4, 1, value.AsOrderedArray());
        }

        protected void CompileAndSetDefaultVertexShader()
        {
            EnsureNotDisposed();

            var shaderSourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Resources.shader.default.vert");

            using var sr = new StreamReader(shaderSourceStream);
            VertexShaderObjectHandle =
                SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_VERTEX_SHADER, sr.ReadToEnd());
        }

        protected void CompileAndSetDefaultPixelShader()
        {
            EnsureNotDisposed();

            var shaderSourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Resources.shader.default.frag");

            using var sr = new StreamReader(shaderSourceStream);
            PixelShaderObjectHandle =
                SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, sr.ReadToEnd());
        }

        protected void TryLinkShaders()
        {
            ProgramHandle = SDL_gpu.GPU_CreateShaderProgram();
            
            SDL_gpu.GPU_AttachShader(ProgramHandle, VertexShaderObjectHandle);
            SDL_gpu.GPU_AttachShader(ProgramHandle, PixelShaderObjectHandle);
            SDL_gpu.GPU_LinkShaderProgram(ProgramHandle);

            var obj = SDL_gpu.GPU_PopErrorCode();
            if (obj.error == SDL_gpu.GPU_ErrorEnum.GPU_ERROR_BACKEND_ERROR)
            {
                throw new ShaderException(
                    "Failed to link vertex and pixel shader.\nMake sure your 'in' parameters have correct fucking types.", 
                    string.Empty
                );
            }
        }

        protected void CreateShaderBlock()
        {
            EnsureNotDisposed();

            Block = SDL_gpu.GPU_LoadShaderBlock(
                ProgramHandle,
                "gpu_Vertex",
                "gpu_TexCoord",
                "gpu_Color",
                "gpu_ModelViewProjectionMatrix"
            );
        }

        protected override void FreeNativeResources()
        {
            EnsureNotDisposed();

            if (PixelShaderObjectHandle != 0)
                SDL_gpu.GPU_FreeShader(PixelShaderObjectHandle);

            if (VertexShaderObjectHandle != 0)
                SDL_gpu.GPU_FreeShader(VertexShaderObjectHandle);

            if (ProgramHandle != 0)
                SDL_gpu.GPU_FreeShaderProgram(ProgramHandle);
        }

        protected static void EnsureCustomShaderActive()
        {
            if (IsDefaultGpuShaderActive)
                throw new InvalidOperationException("This operation requires a custom shader to be active.");
        }

        private static unsafe Matrix4x4 CreateMatrixFromPointer(float* mtxptr)
        {
            return new Matrix4x4(
                mtxptr[0], mtxptr[1], mtxptr[2], mtxptr[3],
                mtxptr[4], mtxptr[5], mtxptr[6], mtxptr[7],
                mtxptr[8], mtxptr[9], mtxptr[10], mtxptr[11],
                mtxptr[12], mtxptr[13], mtxptr[14], mtxptr[15]
            );
        }
    }
}