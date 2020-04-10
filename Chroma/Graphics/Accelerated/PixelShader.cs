using System.IO;
using System.Reflection;
using Chroma.Exceptions;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.Accelerated
{
    public class PixelShader : ShaderBase
    {
        public string FilePath { get; }
        public string SourceCode { get; }

        public PixelShader(string filePath)
        {
            FilePath = filePath;

            using var sr = new StreamReader(filePath);
            PixelShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_PIXEL_SHADER, sr.ReadToEnd());

            if (PixelShaderObjectHandle == 0)
                throw new ShaderException("Compilation failed.", SDL_gpu.GPU_GetShaderMessage());

            CompileDefaultVertexShader();
            ProgramHandle = SDL_gpu.GPU_LinkShaders(PixelShaderObjectHandle, VertexShaderObjectHandle);

            if (ProgramHandle == 0)
                throw new ShaderException("Linkage failed.", SDL_gpu.GPU_GetShaderMessage());

            CreateShaderBlock();
        }

        protected override void FreeNativeResources()
        {
            base.FreeNativeResources();
        }

        private void CompileDefaultVertexShader()
        {
            var shaderSourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Chroma.Resources.shader.default.vert");

            using var sr = new StreamReader(shaderSourceStream);
            VertexShaderObjectHandle = SDL_gpu.GPU_CompileShader(SDL_gpu.GPU_ShaderEnum.GPU_VERTEX_SHADER, sr.ReadToEnd());
        }

        private void CreateShaderBlock()
        {
            Block = SDL_gpu.GPU_LoadShaderBlock(
                ProgramHandle,
                "gpu_Vertex",
                "gpu_TexCoord",
                "gpu_Color",
                "gpu_ModelViewProjectionMatrix"
            );
        }
    }
}
