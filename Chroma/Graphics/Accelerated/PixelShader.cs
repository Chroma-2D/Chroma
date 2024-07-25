namespace Chroma.Graphics.Accelerated;

using System.IO;
using Chroma.Natives.Bindings.SDL;

public sealed class PixelShader : Shader
{
    public string SourceCode { get; }

    public PixelShader(string sourceCode)
    {
        SourceCode = sourceCode;
        Initialize();
    }

    public static PixelShader FromFile(string filePath)
        => new(File.ReadAllText(filePath));

    private void Initialize()
    {
        PixelShaderObjectHandle = SDL_gpu.GPU_CompileShader(
            SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, 
            SourceCode
        );

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
                "Report an issue - be sure to include GLSL errors or I'll fuck you up just like I did with this framework.",
                SDL_gpu.GPU_GetShaderMessage()
            );
        }

        TryLinkShaders();
        CreateShaderBlock();
    }
}