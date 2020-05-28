namespace Chroma.Graphics
{
    public class SpriteSheetAnimation
    {
        private float _frameTimer;
        private SpriteSheet _spriteSheet;

        public AnimationState State { get; private set; }

        public bool Repeat { get; set; }

        public int CurrentFrame { get; private set; }
        public int BeginFrame { get; }
        public int EndFrame { get; }
        public int LengthInFrames => EndFrame - BeginFrame;

        public float FrameDuration { get; }

        public SpriteSheetAnimation(SpriteSheet spriteSheet, int beginFrame, int endFrame, float frameDuration)
        {
            _spriteSheet = spriteSheet;

            // todo: range handling
            BeginFrame = beginFrame;
            EndFrame = endFrame;

            FrameDuration = frameDuration;
        }

        public void Update(float delta)
        {
            if (State == AnimationState.Playing)
            {
                if (CurrentFrame + 1 >= LengthInFrames)
                {
                    if (!Repeat)
                    {
                        State = AnimationState.Stopped;
                    }

                    CurrentFrame = 0;
                }

                if (_frameTimer >= FrameDuration)
                {
                    _frameTimer = 0;
                    CurrentFrame++;
                }
                else
                {
                    _frameTimer += (delta * 1000);
                }

                _spriteSheet.CurrentFrame = BeginFrame + CurrentFrame;
            }
        }

        public void Draw(RenderContext context)
            => _spriteSheet.Draw(context);

        public void Play()
        {
            State = AnimationState.Playing;
        }

        public void Stop()
        {
            CurrentFrame = 0;
            State = AnimationState.Stopped;
        }

        public void Pause()
        {
            State = AnimationState.Paused;
        }
    }
}