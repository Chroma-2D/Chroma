using Chroma.Natives.GL;
using Chroma.Natives.SDL;

namespace Chroma.Graphics
{
    public class RenderSettings
    {
        private bool _multiSamplingEnabled;
        private float _lineThickness;
        private bool _shapeBlendingEnabled;

        private bool _pointSmoothingEnabled;
        private bool _lineSmoothingEnabled;

        public bool AutoClearEnabled { get; set; }
        public Color AutoClearColor { get; set; }

        public BlendingFunction ShapeSourceColorBlendingFunction { get; private set; }
        public BlendingFunction ShapeSourceAlphaBlendingFunction { get; private set; }
        public BlendingFunction ShapeDestinationColorBlendingFunction { get; private set; }
        public BlendingFunction ShapeDestinationAlphaBlendingFunction { get; private set; }

        public BlendingEquation ShapeColorBlendingEquation { get; private set; }
        public BlendingEquation ShapeAlphaBlendingEquation { get; private set; }

        public bool MultiSamplingEnabled
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

        public float LineThickness
        {
            get => _lineThickness;
            set
            {
                var val = value < 0 ? 0 : value;
                _lineThickness = val;

                SDL_gpu.GPU_SetLineThickness(_lineThickness);
            }
        }

        public bool ShapeBlendingEnabled
        {
            get => _shapeBlendingEnabled;
            set
            {
                _shapeBlendingEnabled = value;
                SDL_gpu.GPU_SetShapeBlending(_shapeBlendingEnabled);
            }
        }

        internal RenderSettings()
        {
            AutoClearEnabled = true;
            AutoClearColor = Color.Transparent;

            LineThickness = 1;

            ShapeBlendingEnabled = false;
            ResetShapeBlending();

            MultiSamplingEnabled = false;
        }

        public void SetShapeBlendingEquations(BlendingEquation colorBlend, BlendingEquation alphaBlend)
        {
            ShapeColorBlendingEquation = colorBlend;
            ShapeAlphaBlendingEquation = alphaBlend;

            SDL_gpu.GPU_SetShapeBlendEquation(
                (SDL_gpu.GPU_BlendEqEnum)colorBlend,
                (SDL_gpu.GPU_BlendEqEnum)alphaBlend
            );
        }

        public void SetShapeBlendingFunctions(BlendingFunction sourceColorBlend, BlendingFunction sourceAlphaBlend,
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

        public void SetShapeBlendingPreset(BlendingPreset preset)
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

        public void ResetShapeBlending()
            => SetShapeBlendingPreset(BlendingPreset.Normal);
    }
}