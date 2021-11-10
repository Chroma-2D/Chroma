using System.IO;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.Accelerated
{
    public sealed class VertexShader : Shader
    {
        public string SourceCode { get; }

        public VertexShader(string sourceCode)
        {
            SourceCode = sourceCode;
            Initialize();
        }

        public static VertexShader FromFile(string filePath)
            => new(File.ReadAllText(filePath));
        
        private void Initialize()
        {
            VertexShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_VERTEX_SHADER, SourceCode);

            if (VertexShaderObjectHandle == 0)
                throw new ShaderException("Compilation failed.", SDL_gpu.GPU_GetShaderMessage());

            CompileAndSetDefaultPixelShader();
            if (PixelShaderObjectHandle == 0)
            {
                throw new ShaderException(
                    "Default Chroma pixel shader compilation failed. " +
                    "Report an issue - be sure to include GLSL errors or I'll fuck you up just like I did with this framework.",
                    SDL_gpu.GPU_GetShaderMessage());
            }

            TryLinkShaders();
            CreateShaderBlock();
        }
    }
}