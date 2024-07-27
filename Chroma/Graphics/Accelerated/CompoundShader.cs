namespace Chroma.Graphics.Accelerated;

using System.IO;
using Chroma.Natives.Bindings.SDL;

public sealed class CompoundShader : Shader
{
    public string PixelShaderFilePath { get; }
    public string VertexShaderFilePath { get; }

    public string PixelShaderSourceCode { get; }
    public string VertexShaderSourceCode { get; }

    public CompoundShader(string pixelShaderFilePath, string vertexShaderFilePath)
    {
        PixelShaderFilePath = pixelShaderFilePath;
        VertexShaderFilePath = vertexShaderFilePath;

        PixelShaderSourceCode = File.ReadAllText(PixelShaderFilePath);
        VertexShaderSourceCode = File.ReadAllText(VertexShaderFilePath);

        TryCompilePixelShader();
        TryCompileVertexShader();

        TryLinkShaders();
        CreateShaderBlock();
    }

    private void TryCompilePixelShader()
    {
        PixelShaderObjectHandle =
            SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, PixelShaderSourceCode);

        if (PixelShaderObjectHandle == 0)
            throw new ShaderException("Pixel shader compilation failed.", SDL_gpu.GPU_GetShaderMessage());
    }

    private void TryCompileVertexShader()
    {
        VertexShaderObjectHandle =
            SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_VERTEX_SHADER, VertexShaderSourceCode);

        if (VertexShaderObjectHandle == 0)
            throw new ShaderException("Vertex shader compilation failed.", SDL_gpu.GPU_GetShaderMessage());
    }
}