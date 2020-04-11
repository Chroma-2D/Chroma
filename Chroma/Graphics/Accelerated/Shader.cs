using System.IO;
using System.Numerics;
using System.Reflection;
using Chroma.Diagnostics;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.Accelerated
{
    public abstract class Shader : DisposableResource
    {
        internal uint ProgramHandle;
        internal uint VertexShaderObjectHandle;
        internal uint PixelShaderObjectHandle;

        internal SDL_gpu.GPU_ShaderBlock Block;

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

            SDL_gpu.GPU_SetUniformfv(loc, 2, 1, new float[] { value.X, value.Y });
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

            SDL_gpu.GPU_SetUniformfv(loc, 3, 1, new float[] { value.X, value.Y, value.Z });
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

            SDL_gpu.GPU_SetUniformfv(loc, 4, 1, new float[] { value.X, value.Y, value.Z, value.W });
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

            SDL_gpu.GPU_SetUniformMatrixfv(loc, 1, 4, 4, false, new float[] {
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

            var shaderSourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Chroma.Resources.shader.default.vert");

            using var sr = new StreamReader(shaderSourceStream);
            VertexShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_VERTEX_SHADER, sr.ReadToEnd());
        }

        protected void CompileAndSetDefaultPixelShader()
        {
            EnsureNotDisposed();

            var shaderSourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Chroma.Resources.shader.default.frag");

            using var sr = new StreamReader(shaderSourceStream);
            PixelShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, sr.ReadToEnd());
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
    }
}
