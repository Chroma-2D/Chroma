using System.IO;
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
            {
                var gpuMessage = SDL_gpu.GPU_GetShaderMessage();
                throw new ShaderException("Compilation failed.", gpuMessage);
            }

            CompileAndSetDefaultVertexShader();

            if (VertexShaderObjectHandle == 0)
            {
                throw new ShaderException(
                    "Default Chroma vertex shader compilation failed. " +
                    "Report an issue - be sure to include GLSL errors or I'll fuck you up just like I did with this framework.", SDL_gpu.GPU_GetShaderMessage());
            }

            ProgramHandle = SDL_gpu.GPU_LinkShaders(PixelShaderObjectHandle, VertexShaderObjectHandle);

            if (ProgramHandle == 0)
                throw new ShaderException("Linkage failed.", SDL_gpu.GPU_GetShaderMessage());

            CreateShaderBlock();
        }
    }
}
