using System;
using System.Numerics;
using Chroma.Graphics;

namespace LerpingCameras
{
    public class LerpCamera : Camera
    {
        private float _time;
        private float _secondsToReachTarget;
        
        public bool IsMoving { get; private set; }

        public Vector3 StartPosition { get; private set; }
        public Vector3 EndPosition { get; private set; }

        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public LerpCamera(Vector2 startPosition)
        {
            Zoom = Vector2.One;
            Position = new(startPosition.X, startPosition.Y, 0);
        }
        
        public void Update(float delta)
        {
            if (IsMoving)
            {
                _time += delta / _secondsToReachTarget;
                
                Position = Vector3.Lerp(
                    StartPosition,
                    EndPosition,
                    _time
                );

                Position = Vector3.Clamp(Position, StartPosition, EndPosition);
                EndTime = DateTime.Now;

                if (Vector3.Distance(Position, EndPosition) <= 0.5f)
                    IsMoving = false;
            }
        }

        public void StartMoving(Vector2 endPosition, float secondsToReachTarget)
        {
            if (IsMoving)
                return;
            
            StartPosition = Position;
            EndPosition = new(endPosition.X, endPosition.Y, 0);
            
            _time = 0;
            _secondsToReachTarget = secondsToReachTarget;
            
            IsMoving = true;
            StartTime = DateTime.Now;
        }
    }
}