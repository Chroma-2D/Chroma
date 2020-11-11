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
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        internal uint ProgramHandle;
        internal uint VertexShaderObjectHandle;
        internal uint PixelShaderObjectHandle;

        internal SDL_gpu.GPU_ShaderBlock Block;

        public static int MinimumSupportedGlslVersion
        {
            get
            {
                SDL_gpu.GPU_Renderer renderer;

                unsafe
                {
                    renderer = *((SDL_gpu.GPU_Renderer*)SDL_gpu.GPU_GetCurrentRenderer().ToPointer());
                }

                return renderer.min_shader_version;
            }
        }

        public static int MaximumSupportedGlslVersion
        {
            get
            {
                SDL_gpu.GPU_Renderer renderer;

                unsafe
                {
                    renderer = *((SDL_gpu.GPU_Renderer*)SDL_gpu.GPU_GetCurrentRenderer().ToPointer());
                }

                return renderer.max_shader_version;
            }
        }

        public static bool IsDefaultGpuShaderActive
            => SDL_gpu.GPU_IsDefaultShaderProgram(SDL_gpu.GPU_GetCurrentShaderProgram());

        public static Matrix4x4 ModelViewProjection
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
                    return CreateMatrixFromPointer(
                        SDL_gpu.GPU_GetProjection()
                    );
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
                    return CreateMatrixFromPointer(
                        SDL_gpu.GPU_GetView()
                    );
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
                    return CreateMatrixFromPointer(
                        SDL_gpu.GPU_GetModel()
                    );
                }
            }
        }

        public static void Deactivate()
            => SDL_gpu.GPU_ActivateShaderProgram(0, IntPtr.Zero);

        public virtual void Activate()
        {
            EnsureNotDisposed();

            if (ProgramHandle == 0)
            {
                _log.Warning($"Refusing to activate invalid shader.");
                return;
            }
            
            SDL_gpu.GPU_ActivateShaderProgram(ProgramHandle, ref Block);
        }

        public void SetUniform(string name, Texture value, int textureUnit)
        {
            EnsureNotDisposed();

            if (value.Disposed)
                throw new ArgumentException("Texture provided was already diposed.");

            if (textureUnit == 0)
            {
                _log.Error("Cannot set texture unit 0: reserved for use by the rendering system blitting functions.");
                return;
            }

            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                _log.Warning($"Texture sampler '{name}' does not exist.");
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
                _log.Warning($"Float uniform '{name}' does not exist.");
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
                _log.Warning($"Int uniform '{name}' does not exist.");
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
                _log.Warning($"Uint uniform '{name}' does not exist.");
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
                _log.Warning($"Vec2 uniform '{name}' does not exist.");
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
                _log.Warning($"Vec3 uniform '{name}' does not exist.");
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
                _log.Warning($"Vec4 uniform '{name}' does not exist.");
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
                _log.Warning($"Mat4 uniform '{name}' does not exist.");
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
                _log.Warning($"Vec4 uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformfv(loc, 4, 1, value.AsOrderedArray());
        }

        public void SetAttribute(string name, float value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                _log.Warning($"Float attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributef(loc, value);
        }

        public void SetAttribute(string name, int value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                _log.Warning($"Int attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributei(loc, value);
        }

        public void SetAttribute(string name, uint value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                _log.Warning($"Uint attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributeui(loc, value);
        }

        public void SetAttribute(string name, Vector2 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                _log.Warning($"Vec2 attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributefv(loc, 2, new[] {value.X, value.Y});
        }

        public void SetAttribute(string name, Vector3 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                _log.Warning($"Vec3 attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributefv(loc, 3, new[] {value.X, value.Y, value.Z});
        }

        public void SetAttribute(string name, Vector4 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                _log.Warning($"Vec4 attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributefv(loc, 4, new[] {value.X, value.Y, value.Z, value.W});
        }

        public void SetAttribute(string name, Color value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                _log.Warning($"Vec4 attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributefv(loc, 4, value.AsOrderedArray());
        }

        protected void CompileAndSetDefaultVertexShader()
        {
            EnsureNotDisposed();

            var shaderSourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Resources.shader.default.vert");

            using var sr = new StreamReader(shaderSourceStream!);
            VertexShaderObjectHandle =
                SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_VERTEX_SHADER, sr.ReadToEnd());
        }

        protected virtual void CompileAndSetDefaultPixelShader()
        {
            EnsureNotDisposed();

            var shaderSourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("Chroma.Resources.shader.default.frag");

            using var sr = new StreamReader(shaderSourceStream!);
            PixelShaderObjectHandle =
                SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, sr.ReadToEnd());
        }

        protected virtual void TryLinkShaders()
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

        protected void CleanupAfterLinking()
        {
            SDL_gpu.GPU_DetachShader(ProgramHandle, VertexShaderObjectHandle);
            SDL_gpu.GPU_DetachShader(ProgramHandle, PixelShaderObjectHandle);

            SDL_gpu.GPU_FreeShader(VertexShaderObjectHandle);
            SDL_gpu.GPU_FreeShader(PixelShaderObjectHandle);

            VertexShaderObjectHandle = 0;
            PixelShaderObjectHandle = 0;
        }

        protected void CreateShaderBlock()
        {
            EnsureNotDisposed();

            Block = SDL_gpu.GPU_LoadShaderBlock(
                ProgramHandle,
                "gpu_VertexPosition",
                "gpu_TexCoord",
                "gpu_VertexColor",
                "gpu_ModelViewProjection"
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