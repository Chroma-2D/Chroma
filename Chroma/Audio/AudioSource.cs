using System;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.OpenAL;

namespace Chroma.Audio
{
    public abstract class AudioSource : DisposableResource
    {
        private static readonly Log _log = LogManager.GetForCurrentAssembly();

        internal uint Handle;
        internal uint Buffer;

        internal OutputDevice Device { get; }

        public bool IsValid => Handle != 0;
        public bool HasValidBuffer => Buffer != 0;

        public float MaximumGain
        {
            get
            {
                return ExecuteOpenAlCommand(() =>
                {
                    Al.alGetSourcef(
                        Handle,
                        Al.AL_MAX_GAIN,
                        out var value
                    );

                    return value;
                }, -1, "Failed to retrieve maximum gain: ");
            }

            set
            {
                ExecuteOpenAlCommand(() =>
                {
                    if (value < 0)
                    {
                        _log.Warning("Refusing to set negative maximum gain.");
                        return;
                    }

                    Al.alSourcef(
                        Handle,
                        Al.AL_MAX_GAIN,
                        value
                    );
                }, "Failed to set maximum gain: ");
            }
        }

        public float MinimumGain
        {
            get
            {
                return ExecuteOpenAlCommand(() =>
                {
                    Al.alGetSourcef(
                        Handle,
                        Al.AL_MIN_GAIN,
                        out var value
                    );

                    return value;
                }, -1, "Failed to retrieve minimum gain: ");
            }

            set
            {
                ExecuteOpenAlCommand(() =>
                {
                    if (value < 0)
                    {
                        _log.Warning("Refusing to set negative minimum gain.");
                        return;
                    }

                    Al.alSourcef(
                        Handle,
                        Al.AL_MIN_GAIN,
                        value
                    );
                }, "Failed to set minimum gain: ");
            }
        }

        public float Gain
        {
            get
            {
                return ExecuteOpenAlCommand(() =>
                {
                    Al.alGetSourcef(
                        Handle,
                        Al.AL_GAIN,
                        out var value
                    );

                    return value;
                }, -1, "Failed to retrieve gain: ");
            }

            set
            {
                ExecuteOpenAlCommand(() =>
                {
                    if (value < 0)
                    {
                        _log.Warning("Refusing to set negative minimum gain.");
                        return;
                    }

                    Al.alSourcef(
                        Handle,
                        Al.AL_GAIN,
                        value
                    );
                }, "Failed to set minimum gain: ");
            }
        }

        public float Pitch
        {
            get
            {
                return ExecuteOpenAlCommand(() =>
                {
                    Al.alGetSourcef(
                        Handle,
                        Al.AL_PITCH,
                        out var value
                    );

                    return value;
                }, -1, "Failed to retrieve the pitch multiplier: ");
            }

            set
            {
                ExecuteOpenAlCommand(() =>
                {
                    if (value < 0)
                    {
                        _log.Warning("Refusing to set negative pitch multiplier.");
                        return;
                    }

                    Al.alSourcef(
                        Handle,
                        Al.AL_PITCH,
                        value
                    );
                }, "Failed to set the pitch multiplier: ");
            }
        }

        public bool RelativeToListener
        {
            get
            {
                return ExecuteOpenAlCommand(() =>
                {
                    Al.alGetSourcei(
                        Handle,
                        Al.AL_SOURCE_RELATIVE,
                        out var value
                    );

                    return value != 0;
                }, false, "Failed to retrieve relativity flag: ");
            }

            set
            {
                ExecuteOpenAlCommand(() =>
                {
                    Al.alSourcei(
                        Handle,
                        Al.AL_SOURCE_RELATIVE,
                        value ? 1 : 0
                    );
                }, "Failed to set relativity flag: ");
            }
        }

        public AudioSourceState State
        {
            get
            {
                return ExecuteOpenAlCommand(() =>
                {
                    Al.alGetSourcei(
                        Handle,
                        Al.AL_SOURCE_STATE,
                        out var state
                    );

                    return (AudioSourceState)state;
                }, AudioSourceState.Invalid,
                "Failed to retrieve state: ");
            }
        }

        public Vector2 Position
        {
            get
            {
                return ExecuteOpenAlCommand(() =>
                {
                    Al.alGetSource3f(
                        Handle,
                        Al.AL_POSITION,
                        out var v1, out var v2, out var v3
                    );

                    return new Vector2(v1, v3);
                }, Vector2.Zero, "Failed to retrieve position: ");
            }

            set
            {
                ExecuteOpenAlCommand(() =>
                {
                    Al.alSource3f(
                        Handle,
                        Al.AL_POSITION,
                        value.X, 0, value.Y
                    );
                }, "Failed to set position: ");
            }
        }
        
        public Vector2 Velocity
        {
            get
            {
                return ExecuteOpenAlCommand(() =>
                {
                    Al.alGetSource3f(
                        Handle,
                        Al.AL_VELOCITY,
                        out var v1, out var v2, out var v3
                    );

                    return new Vector2(v1, v3);
                }, Vector2.Zero, "Failed to retrieve velocity: ");
            }

            set
            {
                ExecuteOpenAlCommand(() =>
                {
                    Al.alSource3f(
                        Handle,
                        Al.AL_VELOCITY,
                        value.X, 0, value.Y
                    );
                }, "Failed to set velocity: ");
            }
        }

        internal AudioSource(OutputDevice device)
        {
            Device = device;

            if (!CreateBuffer())
                return;

            CreateSource();
        }

        private bool CreateBuffer()
        {
            var buffer = Al.alGenBuffer();
            var error = Device.Manager.LogOpenAlError("Failed to create audio source: ");

            if (error != Al.AL_NO_ERROR)
                return false;

            Buffer = buffer;
            return true;
        }

        private void CreateSource()
        {
            var src = Al.alGenSource();

            var error = Device.Manager.LogOpenAlError("Failed to create audio source: ");
            if (error != Al.AL_NO_ERROR)
                return;

            Handle = src;
        }

        protected bool AttachBuffer()
        {
            return ExecuteOpenAlCommand(
                () => Al.alSourcei(Handle, Al.AL_BUFFER, (int)Buffer),
                "Failed to attach buffer to audio source: "
            );
        }

        protected void EnsureHandleValid()
        {
            if (!IsValid)
                throw new InvalidOperationException("The requested operation requires a valid handle.");
        }

        protected void EnsureBufferValid()
        {
            if (!HasValidBuffer)
                throw new InvalidOperationException("The requested operation requested a valid buffer.");
        }

        protected T ExecuteOpenAlCommand<T>(Func<T> openAlLogic, T faultValue, string msg)
        {
            EnsureHandleValid();
            EnsureBufferValid();

            var result = openAlLogic();
            var error = Device.Manager.LogOpenAlError(msg);

            if (error != Al.AL_NO_ERROR)
                return faultValue;

            return result;
        }

        protected bool ExecuteOpenAlCommand(Action openAlLogic, string msg)
        {
            EnsureHandleValid();
            EnsureBufferValid();

            openAlLogic();
            return Device.Manager.LogOpenAlError(msg) != Al.AL_NO_ERROR;
        }
    }
}