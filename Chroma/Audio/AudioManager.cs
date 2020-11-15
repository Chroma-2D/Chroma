using System;
using System.Collections.Generic;
using System.Numerics;
using Chroma.Diagnostics.Logging;
using Chroma.MemoryManagement;
using Chroma.Natives.OpenAL;

namespace Chroma.Audio
{
    public class AudioManager : DisposableResource
    {
        private readonly Log _log = LogManager.GetForCurrentAssembly();

        private bool HasActiveContext
            => Alc.alcGetCurrentContext() != IntPtr.Zero;

        internal List<OutputDevice> OpenOutputDevices { get; } = new List<OutputDevice>();
        public OutputDevice CurrentOutputDevice { get; private set; }

        public string DefaultOutputDeviceName => Alc.alcGetString(
            IntPtr.Zero,
            Alc.ALC_DEFAULT_DEVICE_SPECIFIER
        );

        public string DefaultInputDeviceName => Alc.alcGetString(
            IntPtr.Zero,
            Alc.ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER
        );

        public bool CanEnumerateDevices => Alc.alcIsExtensionPresent(
            IntPtr.Zero,
            "ALC_ENUMERATION_EXT"
        );

        public Vector2 ListenerPosition
        {
            get
            {
                var values = new[] {0.0f, 0.0f, 0.0f};

                if (!HasActiveContext)
                {
                    _log.Error("Cannot retrieve listener position when no audio context is active.");
                }
                else
                {
                    Al.alGetListenerfv(Al.AL_POSITION, values);
                    LogOpenAlError("Failed to set listener position: ");
                }

                return new Vector2(values[0], values[2]);
            }

            set
            {
                if (!HasActiveContext)
                {
                    _log.Error("Cannot set listener position when no audio context is active.");
                    return;
                }

                var values = new[] {value.X, 0, value.Y};
                Al.alListenerfv(Al.AL_POSITION, values);
                LogOpenAlError("Failed to set listener position: ");
            }
        }
        
        public Vector3 ListenerVelocity
        {
            get
            {
                var values = new[] {0.0f, 0.0f, 0.0f};

                if (!HasActiveContext)
                {
                    _log.Error("Cannot retrieve listener velocity when no audio context is active.");
                }
                else
                {
                    Al.alGetListenerfv(Al.AL_VELOCITY, values);
                    LogOpenAlError("Failed to set listener velocity: ");
                }

                return new Vector3(values[0], values[1], values[2]);
            }

            set
            {
                if (!HasActiveContext)
                {
                    _log.Error("Cannot set listener velocity when no audio context is active.");
                    return;
                }

                var values = new[] {value.X, value.Y, value.Z};
                Al.alListenerfv(Al.AL_VELOCITY, values);
                LogOpenAlError("Failed to set listener velocity: ");
            }
        }

        public Vector3[] Orientation
        {
            get
            {
                var vec = new Vector3[2];
                var fl = new float[6];

                Al.alGetListenerfv(
                    Al.AL_ORIENTATION,
                    fl
                );

                vec[0] = new Vector3(fl[0], fl[1], fl[2]);
                vec[1] = new Vector3(fl[3], fl[4], fl[5]);

                return vec;
            }
        }

        public DistanceModel DistanceModel
        {
            get => (DistanceModel)Al.alGetInteger(Al.AL_DISTANCE_MODEL);
            set => Al.alDistanceModel((int)value);
        }

        internal AudioManager()
        {
            var defaultDev = InitializeDevice(DefaultOutputDeviceName, true);

            if (defaultDev.IsValid && defaultDev.HasValidContext)
                OpenOutputDevices.Add(defaultDev);
            
            Al.alListenerfv(
                Al.AL_ORIENTATION,
                new [] { 0, -1, 0, -1, 0.5f, -1}
            );
        }

        public OutputDevice InitializeDevice(string deviceName, bool useAfterInit = false)
        {
            var dev = new OutputDevice(this, deviceName);
            dev.Open();

            if (useAfterInit)
                UseDevice(dev);

            return dev;
        }

        public void UseDevice(OutputDevice device)
        {
            if (!device.HasValidContext)
            {
                _log.Error($"Refusing to use device '{device.Name}' without a valid context.");
                return;
            }

            var prevDevice = CurrentOutputDevice;

            if (CurrentOutputDevice != null)
                SuspendDeviceUsage();

            if (!Alc.alcMakeContextCurrent(device.ContextHandle))
            {
                LogOpenAlError("Failed to activate output device: ");

                if (prevDevice != null)
                    UseDevice(prevDevice);

                return;
            }

            CurrentOutputDevice = device;
        }

        public void DestroyDevice(OutputDevice device)
        {
            if (!device.IsValid)
            {
                _log.Error($"Refusing to destroy invalid device '{device.Name}'.");
                return;
            }

            if (device == CurrentOutputDevice)
                SuspendDeviceUsage();

            OpenOutputDevices.Remove(device);
            device.Close();
        }

        public List<string> EnumerateOutputDevices()
        {
            var ret = new List<string>();

            unsafe
            {
                var unsafeStringList = Alc.alcGetString_UNSAFE(
                    IntPtr.Zero,
                    Alc.ALC_DEVICE_SPECIFIER
                );

                var next = unsafeStringList + 1;
                int length;

                while (*unsafeStringList != 0 && *next != 0)
                {
                    ret.Add(ExtractStringFromList(unsafeStringList, out length));
                    unsafeStringList += length + 1;
                    next += length + 2;
                }
            }

            return ret;
        }

        public Sound NewSound(string filePath)
        {
            var sound = new Sound(CurrentOutputDevice);
            sound.LoadDataFromFile(filePath);

            return sound;
        }
        
        internal int LogOpenAlError(string message = null)
        {
            var error = Al.alGetError();
            if (error != Al.AL_NO_ERROR)
            {
                _log.Error($"{message ?? string.Empty}{Al.GetErrorMessage(error)} (0x{error:X4})");
            }

            return error;
        }

        protected override void FreeNativeResources()
        {
            SuspendDeviceUsage();

            for (var i = OpenOutputDevices.Count - 1; i >= 0; i--)
                DestroyDevice(OpenOutputDevices[i]);
        }

        private void SuspendDeviceUsage()
        {
            CurrentOutputDevice = null;
            Alc.alcMakeContextCurrent(IntPtr.Zero);
        }

        private unsafe string ExtractStringFromList(byte* str, out int length)
        {
            var s = string.Empty;
            var len = 0;

            while (*str != 0)
            {
                s += (char)*str;
                len++;
                str++;
            }

            length = len;
            return s;
        }
        
        protected T ExecuteOpenAlCommand<T>(Func<T> openAlLogic, T faultValue, string msg)
        {
            var result = openAlLogic();
            var error = LogOpenAlError(msg);

            if (error != Al.AL_NO_ERROR)
                return faultValue;

            return result;
        }

        protected bool ExecuteOpenAlCommand(Action openAlLogic, string msg)
        {
            openAlLogic();
            return LogOpenAlError(msg) != Al.AL_NO_ERROR;
        }
    }
}