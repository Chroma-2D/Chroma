using System.IO;
using Chroma.Exceptions;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.Accelerated
{
    public sealed class PixelShader : Shader
    {
        public string FilePath { get; }
        public string SourceCode { get; }

        public PixelShader(string filePath)
        {
            FilePath = filePath;
            SourceCode = File.ReadAllText(FilePath);

            PixelShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, SourceCode);

            if (PixelShaderObjectHandle == 0)
                throw new ShaderException("Compilation failed.", SDL_gpu.GPU_GetShaderMessage());

            CompileAndSetDefaultVertexShader();
            ProgramHandle = SDL_gpu.GPU_LinkShaders(PixelShaderObjectHandle, VertexShaderObjectHandle);

            if (ProgramHandle == 0)
                throw new ShaderException("Linkage failed.", SDL_gpu.GPU_GetShaderMessage());

            CreateShaderBlock();
        }
    }
}
