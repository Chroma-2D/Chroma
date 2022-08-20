using System;
using System.Drawing;
using Chroma.Natives.Bindings.GL;
using Chroma.Natives.Bindings.SDL;

namespace Chroma.Graphics
{
    public static class RenderSettings
    {
        private static float _lineThickness;
        private static bool _multiSamplingEnabled;
        private static bool _shapeBlendingEnabled;
        private static bool _depthTestingEnabled;
        private static Rectangle _scissor = Rectangle.Empty;

        public static bool AutoClearEnabled { get; set; }
        public static Color AutoClearColor { get; set; }

        public static float LineThickness
        {
            get => _lineThickness;
            set
            {
                var val = value < 0 ? 0 : value;
                _lineThickness = val;

                SDL_gpu.GPU_SetLineThickness(_lineThickness);
            }
        }
        
        public static bool MultiSamplingEnabled
        {
            get => _multiSamplingEnabled;
            set
            {
                _multiSamplingEnabled = value;

                if (_multiSamplingEnabled)
                {
                    Gl.Enable(Gl.GL_MULTISAMPLE);
                }
                else
                {
                    Gl.Disable(Gl.GL_MULTISAMPLE);
                }
            }
        }

        public static bool DepthTestingEnabled
        {
            get => _depthTestingEnabled;
            set
            {
                _depthTestingEnabled = value;

                if (_depthTestingEnabled)
                {
                    Gl.Enable(Gl.GL_DEPTH_TEST);
                }
                else
                {
                    Gl.Disable(Gl.GL_DEPTH_TEST);
                }
            }
        }

        public static bool ShapeBlendingEnabled
        {
            get => _shapeBlendingEnabled;
            set
            {
                _shapeBlendingEnabled = value;
                SDL_gpu.GPU_SetShapeBlending(_shapeBlendingEnabled);
            }
        }
        
        public static Rectangle Scissor
        {
            get => _scissor;
            set
            {
                _scissor = value;

                if (_scissor == Rectangle.Empty)
                {
                    SDL_gpu.GPU_UnsetClip(
                        SDL_gpu.GPU_GetActiveTarget()
                    );
                }
                else
                {
                    SDL_gpu.GPU_SetClip(
                        SDL_gpu.GPU_GetActiveTarget(),
                        (short)_scissor.X,
                        (short)_scissor.Y,
                        (ushort)_scissor.Width,
                        (ushort)_scissor.Height
                    );
                }
            }
        }

        public static TextureFilteringMode DefaultTextureFilteringMode { get; set; } = TextureFilteringMode.Linear;
        public static TextureSnappingMode DefaultTextureSnappingMode { get; set; } = TextureSnappingMode.None;

        public static BlendingFunction ShapeSourceColorBlendingFunction { get; private set; }
        public static BlendingFunction ShapeSourceAlphaBlendingFunction { get; private set; }
        public static BlendingFunction ShapeDestinationColorBlendingFunction { get; private set; }
        public static BlendingFunction ShapeDestinationAlphaBlendingFunction { get; private set; }

        public static BlendingEquation ShapeColorBlendingEquation { get; private set; }
        public static BlendingEquation ShapeAlphaBlendingEquation { get; private set; }

        static RenderSettings()
        {
            if (!Game.WasConstructed)
                throw new InvalidOperationException("Construct your application first.");
            
            AutoClearEnabled = true;
            AutoClearColor = Color.Transparent;

            LineThickness = 1;

            ShapeBlendingEnabled = true;
            MultiSamplingEnabled = false;

            ResetShapeBlending();
        }

        public static void SetShapeBlendingEquations(BlendingEquation colorBlend, BlendingEquation alphaBlend)
        {
            ShapeColorBlendingEquation = colorBlend;
            ShapeAlphaBlendingEquation = alphaBlend;

            SDL_gpu.GPU_SetShapeBlendEquation(
                (SDL_gpu.GPU_BlendEqEnum)colorBlend,
                (SDL_gpu.GPU_BlendEqEnum)alphaBlend
            );
        }

        public static void SetShapeBlendingFunctions(BlendingFunction sourceColorBlend, BlendingFunction sourceAlphaBlend,
            BlendingFunction destinationColorBlend, BlendingFunction destinationAlphaBlend)
        {
            ShapeSourceColorBlendingFunction = sourceColorBlend;
            ShapeSourceAlphaBlendingFunction = sourceAlphaBlend;

            ShapeDestinationColorBlendingFunction = destinationColorBlend;
            ShapeDestinationAlphaBlendingFunction = destinationAlphaBlend;

            SDL_gpu.GPU_SetShapeBlendFunction(
                (SDL_gpu.GPU_BlendFuncEnum)sourceColorBlend,
                (SDL_gpu.GPU_BlendFuncEnum)destinationColorBlend,
                (SDL_gpu.GPU_BlendFuncEnum)sourceAlphaBlend,
                (SDL_gpu.GPU_BlendFuncEnum)destinationAlphaBlend
            );
        }

        public static void SetShapeBlendingPreset(BlendingPreset preset)
        {
            var gpuPreset = (SDL_gpu.GPU_BlendPresetEnum)preset;
            var blendModeInfo = SDL_gpu.GPU_GetBlendModeFromPreset(gpuPreset);

            SetShapeBlendingFunctions(
                (BlendingFunction)blendModeInfo.source_color,
                (BlendingFunction)blendModeInfo.source_alpha,
                (BlendingFunction)blendModeInfo.dest_color,
                (BlendingFunction)blendModeInfo.dest_alpha
            );

            SetShapeBlendingEquations(
                (BlendingEquation)blendModeInfo.color_equation,
                (BlendingEquation)blendModeInfo.alpha_equation
            );
        }

        public static void ResetShapeBlending()
            => SetShapeBlendingPreset(BlendingPreset.Normal);
    }
}