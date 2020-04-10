using Chroma.Diagnostics;
using Chroma.Natives.SDL;

namespace Chroma.Graphics.Accelerated
{
    public abstract class ShaderBase : DisposableResource
    {
        internal uint ProgramHandle { get; set; }
        internal uint VertexShaderObjectHandle { get; set; }
        internal uint PixelShaderObjectHandle { get; set; }

        internal SDL_gpu.GPU_ShaderBlock Block { get; set; }

        public void SetUniform(string name, float value)
        {
            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Float uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformf(loc, value);
        }

        public void SetUniform(string name, Vector2 value)
        {
            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec2 uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformfv(loc, 2, 1, value.AsOrderedArray());
        }

        public void SetUniform(string name, Color value)
        {
            var loc = SDL_gpu.GPU_GetUniformLocation(ProgramHandle, name);

            if (loc == -1)
            {
                Log.Warning($"Vec4 uniform '{name}' does not exist.");
                return;
            }

            SDL_gpu.GPU_SetUniformfv(loc, 4, 1, value.AsOrderedArray());
        }
    }
}
