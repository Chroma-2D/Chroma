using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.SoLoud
{
    internal static class SoLoud
    {
        internal const string LibraryName = "soloud";

        internal enum SoLoud_Backend : uint
        {
            AUTO = 0,
            SDL1 = 1,
            SDL2 = 2,
            PORTAUDIO =3,
            WINMM = 4,
            XAUDIO2 = 5,
            WASAPI = 7,
            ALSA = 7,
            JACK = 8,
            OSS = 9,
            OPENAL = 10,
            COREAUDIO = 11,
            OPENSLES = 12,
            VITA_HOMEBREW = 13,
            MINIAUDIO = 14,
            NO_SOUND = 15,
            NULL_DRIVER = 16
        }

        [Flags]
        internal enum SoLoud_InitFlags : uint
        {
            CLIP_ROUNDOFF = 1,
            ENABLE_VISUALIZATION = 2,
            LEFT_HANDED_3D = 4,
            NO_FPU_REGISTER_CHANGE = 8
        }

        internal const int SOLOUD_MAX_FILTERS = 4;

        internal const int SOLOUD_AUTO = 0;
        
        internal const int BASSBOOST_WET = 0;
        internal const int BASSBOOST_BOOST = 1;
        
        internal const int BIQUAD_RESONANT_LOWPASS = 0;
        internal const int BIQUAD_RESONANT_HIGHPASS = 1;
        internal const int BIQUAD_RESONANT_BANDPASS = 2;
        internal const int BIQUAD_RESONANT_WET = 0;
        internal const int BIQUAD_RESONANT_TYPE = 1;
        internal const int BIQUAD_RESONANT_FREQUENCY = 2;
        internal const int BIQUAD_RESONANT_RESONANCE = 3;

        internal const int ECHO_WET = 0;
        internal const int ECHO_DELAY = 1;
        internal const int ECHO_DECAY = 2;
        internal const int ECHO_FILTER = 3;
        
        internal const int FLANGER_WET = 0;
        internal const int FLANGER_DELAY = 1;
        internal const int FLANGER_FREQ = 2;
        
        internal const int FREEVERB_WET = 0;
        internal const int FREEVERB_FREEZE = 1;
        internal const int FREEVERB_ROOMSIZE = 2;
        internal const int FREEVERB_DAMP = 3;
        internal const int FREEVERB_WIDTH = 4;
        
        internal const int LOFI_WET = 0;
        internal const int LOFI_SAMPLERATE = 1;
        internal const int LOFI_BITDEPTH = 2;
        
        internal const int NOISE_WHITE = 0;
        internal const int NOISE_PINK = 1;
        internal const int NOISE_BROWNISH = 2;
        internal const int NOISE_BLUEISH = 3;
        
        internal const int ROBOTIZE_WET = 0;
        internal const int ROBOTIZE_FREQ = 1;
        internal const int ROBOTIZE_WAVE = 2;
        
        internal const int SFXR_PRESET_COIN = 0;
        internal const int SFXR_PRESET_LASER = 1;
        internal const int SFXR_PRESET_EXPLOSION = 2;
        internal const int SFXR_PRESET_POWERUP = 3;
        internal const int SFXR_PRESET_HURT = 4;
        internal const int SFXR_PRESET_JUMP = 5;
        internal const int SFXR_PRESET_BLIP = 6;
        
        internal const int SPEECH_KW_SAW = 0;
        internal const int SPEECH_KW_TRIANGLE = 1;
        internal const int SPEECH_KW_SIN = 2;
        internal const int SPEECH_KW_SQUARE = 3;
        internal const int SPEECH_KW_PULSE = 4;
        internal const int SPEECH_KW_NOISE = 5;
        internal const int SPEECH_KW_WARBLE = 6;
        
        internal const int VIC_PAL = 0;
        internal const int VIC_NTSC = 1;
        internal const int VIC_BASS = 0;
        internal const int VIC_ALTO = 1;
        internal const int VIC_SOPRANO = 2;
        internal const int VIC_NOISE = 3;
        internal const int VIC_MAX_REGS = 4;
        
        internal const int WAVESHAPER_WET = 0;
        internal const int WAVESHAPER_AMOUNT = 1;
        
        #region Core
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Soloud_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Soloud_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_initEx(IntPtr handle, SoLoud_InitFlags flags, SoLoud_Backend backend, uint sampleRate, uint bufferSize, uint channels);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_deinit(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getVersion(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Soloud_getErrorString")]
        private static extern IntPtr Soloud_getErrorString_INTERNAL(IntPtr handle, int errorCode);
        internal static string Soloud_getErrorString(IntPtr handle, int errorCode)
        {
            return Marshal.PtrToStringAnsi(
                Soloud_getErrorString_INTERNAL(handle, errorCode)
            );
        }

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getBackendId(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Soloud_getBackendString")]
        private static extern IntPtr Soloud_getBackendString_INTERNAL(IntPtr handle);
        internal static string Soloud_getBackendString(IntPtr handle)
        {
            return Marshal.PtrToStringAnsi(
                Soloud_getBackendString_INTERNAL(handle)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getBackendChannels(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getBackendSamplerate(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getBackendBufferSize(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_setSpeakerPosition(IntPtr handle, uint channel, float x, float y, float z);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_getSpeakerPosition(IntPtr handle, uint channel, float[] x, float[] y, float[] z);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_playEx(IntPtr handle, IntPtr sound, float volume, float pan, bool paused, uint bus);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_playClockedEx(IntPtr handle, double soundTime, IntPtr sound, float volume, float pan, uint bus);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_play3dEx(IntPtr handle, IntPtr sound, float posX, float posY, float posZ, float velX, float velY, float velZ, float volume, bool paused, uint bus);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_play3dClockedEx(IntPtr handle, double soundTime, IntPtr sound, float posX, float posY, float posZ, float velX, float velY, float velZ, float volume, uint bus);

        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_playBackgroundEx(IntPtr handle, IntPtr sound, float volume, bool paused, uint bus);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_seek(IntPtr handle, uint voiceHandle, double seconds);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_stop(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_stopAll(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_stopAudioSource(IntPtr handle, IntPtr sound);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_countAudioSource(IntPtr handle, IntPtr sound);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setFilterParameter(IntPtr handle, uint voiceHandle, uint filterId, uint attributeId, float value);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getFilterParameter(IntPtr handle, uint voiceHandle, uint filterId, uint attributeId);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_fadeFilterParameter(IntPtr handle, uint voiceHandle, uint filterId, uint attributeId, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_oscillateFilterParameter(IntPtr handle, uint voiceHandle, uint filterId, uint attributeId, float from, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Soloud_getStreamTime(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Soloud_getStreamPosition(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Soloud_getPause(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getVolume(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getOverallVolume(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getPan(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getSamplerate(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Soloud_getProtectVoice(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getActiveVoiceCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getVoiceCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_isValidVoiceHandle(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getRelativePlaySpeed(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getPostClipScaler(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getGlobalVolume(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getMaxActiveVoiceCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool Soloud_getLooping(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Soloud_getLoopPoint(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setLoopPoint(IntPtr handle, uint voiceHandle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setLooping(IntPtr handle, uint voiceHandle, bool looping);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_setMaxActiveVoiceCount(IntPtr handle, uint voiceCount);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setInaudibleBehavior(IntPtr handle, uint voiceHandle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setGlobalVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setPostClipScaler(IntPtr handle, float scaler);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setPause(IntPtr handle, uint voiceHandle, bool pause);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setPauseAll(IntPtr handle, bool pause);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_setRelativePlaySpeed(IntPtr handle, uint voiceHandle, float speed);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setProtectVoice(IntPtr handle, uint voiceHandle, bool protect);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setSamplerate(IntPtr handle, uint voiceHandle, float sampleRate);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setPan(IntPtr handle, uint voiceHandle, float pan);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setPanAbsoluteEx(IntPtr handle, uint voiceHandle, float lVolume, float rVolume, float lbVolume, float rbVolume, float cVolume, float sVolume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setVolume(IntPtr handle, uint voiceHandle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setDelaySamples(IntPtr handle, uint voiceHandle, uint samples);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_fadeVolume(IntPtr handle, uint voiceHandle, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_fadePan(IntPtr handle, uint voiceHandle, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_fadeRelativePlaySpeed(IntPtr handle, uint voiceHandle, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_fadeGlobalVolume(IntPtr handle, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_schedulePause(IntPtr handle, uint voiceHandle, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_scheduleStop(IntPtr handle, uint voiceHandle, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_oscillateVolume(IntPtr handle, uint voiceHandle, float from, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_oscillatePan(IntPtr handle, uint voiceHandle, float from, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_oscillateRelativePlaySpeed(IntPtr handle, uint voiceHandle, float from, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_oscillateGlobalVolume(IntPtr handle, float from, float to, double time);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setGlobalFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_setVisualizationEnable(IntPtr handle, bool enable);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Soloud_calcFFT")]
        internal static extern IntPtr Soloud_calcFFT_INTERNAL(IntPtr handle);
        internal static float[] Soloud_calcFFT(IntPtr handle)
        {
            var ret = new float[256];
            var p = Soloud_calcFFT_INTERNAL(handle);

            var buffer = new byte[4];
            for (var i = 0; i < ret.Length; ++i)
            {
                var bits = Marshal.ReadInt32(p, i * 4);
                buffer[0] = (byte)((bits >> 0) & 0xff);
                buffer[1] = (byte)((bits >> 8) & 0xff);
                buffer[2] = (byte)((bits >> 16) & 0xff);
                buffer[3] = (byte)((bits >> 24) & 0xff);
                
                ret[i] = BitConverter.ToSingle(buffer, 0);
            }
            
            return ret;
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Soloud_getWave")]
        internal static extern IntPtr Soloud_getWave_INTERNAL(IntPtr handle);
        internal static float[] Soloud_getWave(IntPtr handle)
        {
            var ret = new float[256];
            var p = Soloud_getWave_INTERNAL(handle);

            var buffer = new byte[4];
            for (var i = 0; i < ret.Length; ++i)
            {
                var bits = Marshal.ReadInt32(p, i * 4);
                buffer[0] = (byte)((bits >> 0) & 0xff);
                buffer[1] = (byte)((bits >> 8) & 0xff);
                buffer[2] = (byte)((bits >> 16) & 0xff);
                buffer[3] = (byte)((bits >> 24) & 0xff);
                
                ret[i] = BitConverter.ToSingle(buffer, 0);
            }
            return ret;
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getApproximateVolume(IntPtr handle, uint channel);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_getLoopCount(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_getInfo(IntPtr handle, uint voiceHandle, uint infoKey);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Soloud_createVoiceGroup(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_destroyVoiceGroup(IntPtr handle, uint voiceGroupHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_addVoiceToGroup(IntPtr handle, uint voiceGroupHandle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_isVoiceGroup(IntPtr handle, uint voiceGroupHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_isVoiceGroupEmpty(IntPtr handle, uint voiceGroupHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_update3dAudio(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Soloud_set3dSoundSpeed(IntPtr handle, float speed);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Soloud_get3dSoundSpeed(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dListenerParametersEx(IntPtr handle, float posX, float posY, float posZ, float atX, float atY, float atZ, float upX, float upY, float upZ, float velX, float velY, float velZ);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dListenerPosition(IntPtr handle, float posX, float posY, float posZ);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dListenerAt(IntPtr handle, float atX, float atY, float atZ);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dListenerUp(IntPtr handle, float upX, float upY, float upZ);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dListenerVelocity(IntPtr handle, float velX, float velY, float velZ);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dSourceParametersEx(IntPtr handle, uint voiceHandle, float posX, float posY, float posZ, float velX, float velY, float velZ);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dSourcePosition(IntPtr handle, uint voiceHandle, float posX, float posY, float posZ);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dSourceVelocity(IntPtr handle, uint voiceHandle, float velX, float velY, float velZ);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dSourceMinMaxDistance(IntPtr handle, uint voiceHandle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dSourceAttenuation(IntPtr handle, uint voiceHandle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_set3dSourceDopplerFactor(IntPtr handle, uint voiceHandle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_mix(IntPtr handle, float[] buffer, uint samples);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Soloud_mixSigned16(IntPtr handle, IntPtr buffer, uint samples);
        #endregion
        
        #region Bassboost Filter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BassboostFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BassboostFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int BassboostFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "BassboostFilter_getParamName")]
        private static extern IntPtr BassboostFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string BassboostFilter_getParamName(IntPtr handle, uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                BassboostFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint BassboostFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float BassboostFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float BassboostFilter_getParamMin(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int BassboostFilter_setParams(IntPtr handle, float boost);
        #endregion
        
        #region BiquadResonantFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BiquadResonantFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr BiquadResonantFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int BiquadResonantFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "BiquadResonantFilter_getParamName")]
        private static extern IntPtr BiquadResonantFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string BiquadResonantFilter_getParamName(IntPtr handle, uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                BiquadResonantFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint BiquadResonantFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float BiquadResonantFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float BiquadResonantFilter_getParamMin(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int BiquadResonantFilter_setParams(IntPtr handle, int type, float frequency, float resonance);
        #endregion
        
        #region Bus
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Bus_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Bus_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Bus_playEx(IntPtr handle, IntPtr sound, float volume, float pan, bool paused);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Bus_playClockedEx(IntPtr handle, double soundTime, IntPtr sound, float volume, float pan);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Bus_play3dEx(IntPtr handle, IntPtr sound, float posX, float posY, float posZ, float velX, float velY, float velZ, float volume, bool paused);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Bus_play3dClockedEx(IntPtr handle, double soundTime, IntPtr sound, float posX, float posY, float posZ, float velX, float velY, float velZ, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Bus_setChannels(IntPtr handle, uint channels);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_setVisualizationEnable(IntPtr handle, bool enable);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_annexSound(IntPtr handle, uint voiceHandle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Bus_calcFFT")]
        private static extern IntPtr Bus_calcFFT_INTERNAL(IntPtr handle);
        internal static float[] Bus_calcFFT(IntPtr handle)
        {
            var ret = new float[256];
            var p = Bus_calcFFT_INTERNAL(handle);

            var buffer = new byte[4];
            for (var i = 0; i < ret.Length; ++i)
            {
                var bits = Marshal.ReadInt32(p, i * 4);
                buffer[0] = (byte)((bits >> 0) & 0xff);
                buffer[1] = (byte)((bits >> 8) & 0xff);
                buffer[2] = (byte)((bits >> 16) & 0xff);
                buffer[3] = (byte)((bits >> 24) & 0xff);
                
                ret[i] = BitConverter.ToSingle(buffer, 0);
            }
            return ret;
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "Bus_getWave")]
        private static extern IntPtr Bus_getWave_INTERNAL(IntPtr handle);
        internal static float[] Bus_getWave(IntPtr handle)
        {
            var ret = new float[256];
            var p = Bus_getWave_INTERNAL(handle);

            var buffer = new byte[4];
            for (var i = 0; i < ret.Length; ++i)
            {
                var bits = Marshal.ReadInt32(p, i * 4);
                buffer[0] = (byte)((bits >> 0) & 0xff);
                buffer[1] = (byte)((bits >> 8) & 0xff);
                buffer[2] = (byte)((bits >> 16) & 0xff);
                buffer[3] = (byte)((bits >> 24) & 0xff);
                
                ret[i] = BitConverter.ToSingle(buffer, 0);
            }
            return ret;
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float Bus_getApproximateVolume(IntPtr handle, uint channel);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Bus_getActiveVoiceCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Bus_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Bus_stop(IntPtr handle);
        #endregion
        
        #region DCRemovalFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DCRemovalFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr DCRemovalFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DCRemovalFilter_setParamsEx(IntPtr handle, float length);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int DCRemovalFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "DCRemovalFilter_getParamName")]
        private static extern IntPtr DCRemovalFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string DCRemovalFilter_getParamName(IntPtr handle, uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                DCRemovalFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint DCRemovalFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DCRemovalFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float DCRemovalFilter_getParamMin(IntPtr handle, uint paramIndex);
        #endregion
        
        #region EchoFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr EchoFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr EchoFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int EchoFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "EchoFilter_getParamName")]
        private static extern IntPtr EchoFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string EchoFilter_getParamName(IntPtr handle, uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                EchoFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint EchoFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float EchoFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float EchoFilter_getParamMin(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int EchoFilter_setParamsEx(IntPtr handle, float delay, float decay, float filter);
        #endregion
        
        #region FFTFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr FFTFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr FFTFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int FFTFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FFTFilter_getParamName")]
        private static extern IntPtr FFTFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string FFTFilter_getParamName(IntPtr handle, uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                FFTFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint FFTFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float FFTFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float FFTFilter_getParamMin(IntPtr handle, uint paramIndex);
        #endregion
        
        #region FlangerFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr FlangerFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr FlangerFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int FlangerFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FlangerFilter_getParamName")]
        private static extern IntPtr FlangerFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string FlangerFilter_getParamName(IntPtr handle, uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                FlangerFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint FlangerFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float FlangerFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float FlangerFilter_getParamMin(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int FlangerFilter_setParams(IntPtr handle, float delay, float frequency);
        #endregion
        
        #region FreeverbFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr FreeverbFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr FreeverbFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int FreeverbFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "FreeverbFilter_getParamName")]
        private static extern IntPtr FreeverbFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string FreeverbFilter_getParamName(IntPtr handle, uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                FreeverbFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint FreeverbFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float FreeverbFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float FreeverbFilter_getParamMin(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int FreeverbFilter_setParams(IntPtr handle, float mode, float roomSize, float damp, float width);
        #endregion
        
        #region LofiFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr LofiFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr LofiFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int LofiFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "LofiFilter_getParamName")]
        private static extern IntPtr LofiFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string LofiFilter_getParamName(IntPtr handle, uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                LofiFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint LofiFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float LofiFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float LofiFilter_getParamMin(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int LofiFilter_setParams(IntPtr handle, float sampleRate, float bitDepth);
        #endregion
        
        #region Monotone
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Monotone_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Monotone_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Monotone_setParamsEx(IntPtr handle, int hardwareChannels, int waveform);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Monotone_load(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string fileName);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Monotone_loadMemEx(IntPtr handle, IntPtr mem, uint length, bool copy, bool takeOwnership);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Monotone_loadFile(IntPtr handle, IntPtr file);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Monotone_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Monotone_stop(IntPtr handle);
        #endregion
        
        #region Noise
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Noise_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Noise_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_setOctaveScale(IntPtr handle, float oct0, float oct1, float oct2, float oct3, float oct4, float oct5, float oct6, float oct7, float oct8, float oct9);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_setType(IntPtr handle, int type);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Noise_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Noise_stop(IntPtr handle);
        #endregion
        
        #region OpenMPT
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Openmpt_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Openmpt_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Openmpt_load(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string fileName);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Openmpt_loadMemEx(IntPtr handle, IntPtr mem, uint length, bool copy, bool takeOwnership);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Openmpt_loadFile(IntPtr handle, IntPtr file);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Openmpt_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Openmpt_stop(IntPtr handle);
        #endregion
        
        #region Queue
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Queue_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Queue_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Queue_play(IntPtr handle, IntPtr sound);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint Queue_getQueueCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Queue_isCurrentlyPlaying(IntPtr handle, IntPtr sound);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Queue_setParamsFromAudioSource(IntPtr handle, IntPtr sound);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Queue_setParamsEx(IntPtr handle, float sampleRate, uint channels);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Queue_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Queue_stop(IntPtr handle);
        #endregion
        
        #region RobotizeFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr RobotizeFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int RobotizeFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "RobotizeFilter_getParamName")]
        private static extern IntPtr RobotizeFilter_getParamName_INTERNAL(IntPtr handle, uint paramIndex);
        internal static string RobotizeFilter_getParamName(IntPtr handle,uint paramIndex)
        {
            return Marshal.PtrToStringAnsi(
                RobotizeFilter_getParamName_INTERNAL(handle, paramIndex)
            );
        }
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint RobotizeFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float RobotizeFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float RobotizeFilter_getParamMin(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void RobotizeFilter_setParams(IntPtr handle, float frequency, int waveform);
        #endregion
        
        #region SFXR
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Sfxr_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Sfxr_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_resetParams(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sfxr_loadParams(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string fileName);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sfxr_loadParamsMemEx(IntPtr handle, IntPtr mem, uint length, bool copy, bool takeOwnership);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sfxr_loadParamsFile(IntPtr handle, IntPtr file);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Sfxr_loadPreset(IntPtr handle, int presetNo, int randSeed);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Sfxr_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Sfxr_stop(IntPtr handle);
        #endregion
        
        #region Speech
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Speech_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Speech_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Speech_setText(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string text);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Speech_setParamsEx(IntPtr handle, uint baseFrequency, float baseSpeed, float baseDeclination, int baseWaveform);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Speech_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Speech_stop(IntPtr handle);
        #endregion
        
        #region TED/SID
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr TedSid_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr TedSid_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TedSid_load(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string fileName);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TedSid_loadToMem(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string fileName);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TedSid_loadMemEx(IntPtr handle, IntPtr mem, uint length, bool copy, bool takeOwnership);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TedSid_loadFileToMem(IntPtr handle, IntPtr file);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TedSid_loadFile(IntPtr handle, IntPtr file);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double TedSid_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void TedSid_stop(IntPtr handle);
        #endregion
        
        #region VIC
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Vic_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Vic_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_setModel(IntPtr handle, int model);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Vic_getModel(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_setRegister(IntPtr handle, int reg, byte value);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern byte Vic_getRegister(IntPtr handle, int reg);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Vic_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vic_stop(IntPtr handle);
        #endregion
        
        #region VizSN
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Vizsn_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Vizsn_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_setText(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string text);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Vizsn_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Vizsn_stop(IntPtr handle);
        #endregion
        
        #region WAV
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Wav_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Wav_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Wav_load(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string fileName);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Wav_loadMemEx(IntPtr handle, IntPtr mem, uint length, bool copy, bool takeOwnership);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Wav_loadFile(IntPtr handle, IntPtr file);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Wav_loadRawWave8Ex(IntPtr handle, IntPtr mem, uint length, float sampleRate, uint channels);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Wav_loadRawWave16Ex(IntPtr handle, IntPtr mem, uint length, float sampleRate, uint channels);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Wav_loadRawWaveEx(IntPtr handle, float[] mem, uint length, float sampleRate, uint channels, bool copy, bool takeOwnership);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Wav_getLength(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double Wav_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Wav_stop(IntPtr handle);
        #endregion
        
        #region WaveShaperFilter
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr WaveShaperFilter_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr WaveShaperFilter_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WaveShaperFilter_setParams(IntPtr handle, float amount);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WaveShaperFilter_getParamCount(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr WaveShaperFilter_getParamName(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern uint WaveShaperFilter_getParamType(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float WaveShaperFilter_getParamMax(IntPtr handle, uint paramIndex);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float WaveShaperFilter_getParamMin(IntPtr handle, uint paramIndex);
        #endregion

        #region WavStream
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr WavStream_create();
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr WavStream_destroy(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WavStream_load(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string fileName);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WavStream_loadMemEx(IntPtr handle, IntPtr data, uint dataLen, bool copy, bool takeOwnership);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WavStream_loadToMem(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string fileName);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WavStream_loadFile(IntPtr handle, IntPtr file);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int WavStream_loadFileToMem(IntPtr handle, IntPtr file);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double WavStream_getLength(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_setVolume(IntPtr handle, float volume);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_setLooping(IntPtr handle, bool loop);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_set3dMinMaxDistance(IntPtr handle, float minDistance, float maxDistance);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_set3dAttenuation(IntPtr handle, uint attenuationModel, float attenuationRolloffFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_set3dDopplerFactor(IntPtr handle, float dopplerFactor);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_set3dListenerRelative(IntPtr handle, bool listenerRelative);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_set3dDistanceDelay(IntPtr handle, int distanceDelay);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_set3dColliderEx(IntPtr handle, IntPtr collider, int userData);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_set3dAttenuator(IntPtr handle, IntPtr attenuator);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_setInaudibleBehavior(IntPtr handle, bool mustTick, bool kill);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_setLoopPoint(IntPtr handle, double loopPoint);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern double WavStream_getLoopPoint(IntPtr handle);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_setFilter(IntPtr handle, uint filterId, IntPtr filter);
        
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void WavStream_stop(IntPtr handle);
        #endregion
    }
}