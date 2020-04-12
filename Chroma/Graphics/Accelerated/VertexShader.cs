using System.IO;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.Exceptions;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.Accelerated
{
    public sealed class VertexShader : Shader
    {
        private Log Log => LogManager.GetForCurrentAssembly();

        public string FilePath { get; }
        public string SourceCode { get; }

        public VertexShader(string filePath)
        {
            FilePath = filePath;
            SourceCode = File.ReadAllText(FilePath);

            VertexShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_VERTEX_SHADER, SourceCode);

            if (VertexShaderObjectHandle == 0)
                throw new ShaderException("Compilation failed.", SDL_gpu.GPU_GetShaderMessage());

            CompileAndSetDefaultPixelShader();
            if (PixelShaderObjectHandle == 0)
            {
                throw new ShaderException(
                    "Default Chroma pixel shader compilation failed. " +
                    "Report an issue - be sure to include GLSL errors or I'll fuck you up just like I did with this framework.", SDL_gpu.GPU_GetShaderMessage());
            }

            ProgramHandle = SDL_gpu.GPU_LinkShaders(PixelShaderObjectHandle, VertexShaderObjectHandle);

            if (ProgramHandle == 0)
                throw new ShaderException("Linkage failed.", SDL_gpu.GPU_GetShaderMessage());

            CreateShaderBlock();
        }

        public void SetAttribute(string name, float value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Float attribute '{name}' does not exist.");
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
                Log.Warning($"Int attribute '{name}' does not exist.");
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
                Log.Warning($"Uint attribute '{name}' does not exist.");
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
                Log.Warning($"Vec2 attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributefv(loc, 2, new float[] { value.X, value.Y });
        }

        public void SetAttribute(string name, Vector3 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec3 attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributefv(loc, 3, new float[] { value.X, value.Y, value.Z });
        }

        public void SetAttribute(string name, Vector4 value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec4 attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributefv(loc, 4, new float[] { value.X, value.Y, value.Z, value.W });
        }

        public void SetAttribute(string name, Color value)
        {
            EnsureNotDisposed();

            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec4 attribute '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetAttributefv(loc, 4, value.AsOrderedArray());
        }
    }
}
