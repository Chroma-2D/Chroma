namespace Chroma.Graphics.Accelerated;

using System.IO;
using System.Reflection;
using Chroma.Diagnostics;
using Chroma.Natives.Bindings.SDL;

public sealed class Effect : Shader
{
    internal uint UserShaderObjectHandle;

    public string SourceCode { get; }

    public Effect(string sourceCode)
    {
        SourceCode = sourceCode;
        Initialize();
    }

    public static Effect FromFile(string filePath)
        => new(File.ReadAllText(filePath));

    public override void Activate()
    {
        base.Activate();
            
        if (SDL_gpu.GPU_GetCurrentShaderProgram() == ProgramHandle)
        {
            var loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, "gpu_Time");
            if (loc >= 0)
                SDL_gpu.GPU_SetAttributef(loc, (float)PerformanceCounter.SumOfDeltaTimes);

            loc = SDL_gpu.GPU_GetAttributeLocation(ProgramHandle, "gpu_ScreenSize");
            if (loc >= 0)
            {
                var windowHandle = SDL2.SDL_GL_GetCurrentWindow();
                SDL2.SDL_GetWindowSize(windowHandle, out var w, out var h);
                
                SDL_gpu.GPU_SetAttributefv(loc, 2, new float[] {w, h});
            }
        }
    }

    protected override void TryLinkShaders()
    {
        ProgramHandle = SDL_gpu.GPU_CreateShaderProgram();

        SDL_gpu.GPU_AttachShader(ProgramHandle, VertexShaderObjectHandle);
        SDL_gpu.GPU_AttachShader(ProgramHandle, UserShaderObjectHandle);
        SDL_gpu.GPU_AttachShader(ProgramHandle, PixelShaderObjectHandle);
        SDL_gpu.GPU_LinkShaderProgram(ProgramHandle);

        var obj = SDL_gpu.GPU_PopErrorCode();

        if (obj.error != SDL_gpu.GPU_ErrorEnum.GPU_ERROR_NONE)
        {
            var msg = SDL_gpu.GPU_GetShaderMessage();

            switch (obj.error)
            {
                case SDL_gpu.GPU_ErrorEnum.GPU_ERROR_BACKEND_ERROR:
                    throw new ShaderException(
                        "Failed to link the effect.\n" +
                        "Make sure your 'in' parameters have correct fucking types.",
                        msg
                    );

                default:
                {
                    // Yes I know.
                    // No, I don't give a shit.
                    if (msg.Contains("#401"))
                    {
                        throw new ShaderException(
                            "Your effect file is missing `vec4 effect(vec4 pixel, vec2 tex_coords) {...}'",
                            string.Empty
                        );
                    }
                    else
                    {
                        throw new ShaderException(
                            "Failed to link the effect.",
                            msg
                        );
                    }
                }
            }
        }
    }
        
    protected override void CompileAndSetDefaultPixelShader()
    {
        EnsureNotDisposed();

        var shaderSourceStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("Chroma.Resources.shader.default_effect.frag");

        using var sr = new StreamReader(shaderSourceStream!);
        PixelShaderObjectHandle =
            SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, sr.ReadToEnd());
    }

    private void Initialize()
    {
        CompileAndSetDefaultPixelShader();
        if (PixelShaderObjectHandle == 0)
        {
            throw new ShaderException(
                "Default Chroma effect pixel shader entry point compilation failed. " +
                "Report an issue - be sure to include GLSL errors or I'll fuck you up just like I did with this framework.",
                SDL_gpu.GPU_GetShaderMessage()
            );
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

        UserShaderObjectHandle = SDL_gpu.GPU_CompileShader(
            SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, 
            SourceCode
        );
            
        if (UserShaderObjectHandle == 0)
        {
            throw new ShaderException(
                "Effect compilation failed.",
                SDL_gpu.GPU_GetShaderMessage()
            );
        }
            
        TryLinkShaders();
        CreateShaderBlock();
    }
}