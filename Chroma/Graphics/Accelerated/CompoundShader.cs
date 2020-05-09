using System.IO;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.Accelerated
{
    public sealed class CompoundShader : Shader
    {
        public string PixelShaderFilePath { get; }
        public string VertexShaderFilePath { get; }

        public string PixelShaderSourceCode { get; private set; }
        public string VertexShaderSourceCode { get; private set; }

        public CompoundShader(string pixelShaderFilePath, string vertexShaderFilePath)
        {
            PixelShaderFilePath = pixelShaderFilePath;
            VertexShaderFilePath = vertexShaderFilePath;

            TryCompilePixelShader();
            TryCompileVertexShader();

            LinkShaders();
            CreateShaderBlock();
        }

        private void LinkShaders()
        {
            ProgramHandle = SDL_gpu.GPU_LinkShaders(PixelShaderObjectHandle, VertexShaderObjectHandle);

            if (ProgramHandle == 0)
                throw new ShaderException("Compound shader linkage failed.", SDL_gpu.GPU_GetShaderMessage());
        }

        private void TryCompilePixelShader()
        {
            PixelShaderSourceCode = File.ReadAllText(PixelShaderFilePath);
            PixelShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, PixelShaderSourceCode);

            if (PixelShaderObjectHandle == 0)
                throw new ShaderException("Pixel shader compilation failed.", SDL_gpu.GPU_GetShaderMessage());
        }

        private void TryCompileVertexShader()
        {
            VertexShaderSourceCode = File.ReadAllText(VertexShaderFilePath);
            VertexShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_VERTEX_SHADER, VertexShaderSourceCode);

            if (VertexShaderObjectHandle == 0)
                throw new ShaderException("Vertex shader compilation failed.", SDL_gpu.GPU_GetShaderMessage());
        }
    }
}
