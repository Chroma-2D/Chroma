using System;
using System.Runtime.InteropServices;

namespace Chroma.Natives.Bindings.SDL
{
    internal static partial class SDL2
    {
		public const ushort SDL_AUDIO_MASK_BITSIZE =	0xFF;
		public const ushort SDL_AUDIO_MASK_DATATYPE =	(1 << 8);
		public const ushort SDL_AUDIO_MASK_ENDIAN =	(1 << 12);
		public const ushort SDL_AUDIO_MASK_SIGNED =	(1 << 15);

		public const ushort AUDIO_U8 =		0x0008;
		public const ushort AUDIO_S8 =		0x8008;
		public const ushort AUDIO_U16LSB =	0x0010;
		public const ushort AUDIO_S16LSB =	0x8010;
		public const ushort AUDIO_U16MSB =	0x1010;
		public const ushort AUDIO_S16MSB =	0x9010;
		public const ushort AUDIO_S32LSB =	0x8020;
		public const ushort AUDIO_S32MSB =	0x9020;
		public const ushort AUDIO_F32LSB =	0x8120;
		public const ushort AUDIO_F32MSB =	0x9120;

		public static readonly ushort AUDIO_U16SYS =
			BitConverter.IsLittleEndian ? AUDIO_U16LSB : AUDIO_U16MSB;
		public static readonly ushort AUDIO_S16SYS =
			BitConverter.IsLittleEndian ? AUDIO_S16LSB : AUDIO_S16MSB;
		public static readonly ushort AUDIO_S32SYS =
			BitConverter.IsLittleEndian ? AUDIO_S32LSB : AUDIO_S32MSB;
		public static readonly ushort AUDIO_F32SYS =
			BitConverter.IsLittleEndian ? AUDIO_F32LSB : AUDIO_F32MSB;

		public const uint SDL_AUDIO_ALLOW_FREQUENCY_CHANGE =	0x00000001;
		public const uint SDL_AUDIO_ALLOW_FORMAT_CHANGE =	0x00000002;
		public const uint SDL_AUDIO_ALLOW_CHANNELS_CHANGE =	0x00000004;
		public const uint SDL_AUDIO_ALLOW_SAMPLES_CHANGE =	0x00000008;
		public const uint SDL_AUDIO_ALLOW_ANY_CHANGE = (
			SDL_AUDIO_ALLOW_FREQUENCY_CHANGE |
			SDL_AUDIO_ALLOW_FORMAT_CHANGE |
			SDL_AUDIO_ALLOW_CHANNELS_CHANGE |
			SDL_AUDIO_ALLOW_SAMPLES_CHANGE
		);

		public const int SDL_MIX_MAXVOLUME = 128;

		public enum SDL_AudioStatus
		{
			SDL_AUDIO_STOPPED,
			SDL_AUDIO_PLAYING,
			SDL_AUDIO_PAUSED
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SDL_AudioSpec
		{
			public int freq;
			public ushort format; // SDL_AudioFormat
			public byte channels;
			public byte silence;
			public ushort samples;
			public uint size;
			public SDL_AudioCallback callback;
			public IntPtr userdata; // void*
		}

		/* userdata refers to a void*, stream to a Uint8 */
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void SDL_AudioCallback(
			IntPtr userdata,
			IntPtr stream,
			int len
		);

		[DllImport(LibraryName, EntryPoint = "SDL_AudioInit", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe int INTERNAL_SDL_AudioInit(
			byte* driver_name
		);
		public static unsafe int SDL_AudioInit(string driver_name)
		{
			int utf8DriverNameBufSize = Utf8Size(driver_name);
			byte* utf8DriverName = stackalloc byte[utf8DriverNameBufSize];
			return INTERNAL_SDL_AudioInit(
				Utf8Encode(driver_name, utf8DriverName, utf8DriverNameBufSize)
			);
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_AudioQuit();

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_CloseAudio();

		/* dev refers to an SDL_AudioDeviceID */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_CloseAudioDevice(uint dev);

		/* audio_buf refers to a malloc()'d buffer from SDL_LoadWAV */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_FreeWAV(IntPtr audio_buf);

		[DllImport(LibraryName, EntryPoint = "SDL_GetAudioDeviceName", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GetAudioDeviceName(
			int index,
			bool iscapture
		);
		public static string SDL_GetAudioDeviceName(
			int index,
			bool iscapture
		) {
			return UTF8_ToManaged(
				INTERNAL_SDL_GetAudioDeviceName(index, iscapture)
			);
		}

		/* dev refers to an SDL_AudioDeviceID */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_AudioStatus SDL_GetAudioDeviceStatus(
			uint dev
		);

		[DllImport(LibraryName, EntryPoint = "SDL_GetAudioDriver", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GetAudioDriver(int index);
		public static string SDL_GetAudioDriver(int index)
		{
			return UTF8_ToManaged(
				INTERNAL_SDL_GetAudioDriver(index)
			);
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern SDL_AudioStatus SDL_GetAudioStatus();

		[DllImport(LibraryName, EntryPoint = "SDL_GetCurrentAudioDriver", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_GetCurrentAudioDriver();
		public static string SDL_GetCurrentAudioDriver()
		{
			return UTF8_ToManaged(INTERNAL_SDL_GetCurrentAudioDriver());
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetNumAudioDevices(int iscapture);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetNumAudioDrivers();

		/* audio_buf refers to a malloc()'d buffer, IntPtr to an SDL_AudioSpec* */
		/* THIS IS AN RWops FUNCTION! */
		[DllImport(LibraryName, EntryPoint = "SDL_LoadWAV_RW", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr INTERNAL_SDL_LoadWAV_RW(
			IntPtr src,
			int freesrc,
			out SDL_AudioSpec spec,
			out IntPtr audio_buf,
			out uint audio_len
		);
		public static IntPtr SDL_LoadWAV(
			string file,
			out SDL_AudioSpec spec,
			out IntPtr audio_buf,
			out uint audio_len
		) {
			IntPtr rwops = SDL_RWFromFile(file, "rb");
			return INTERNAL_SDL_LoadWAV_RW(
				rwops,
				1,
				out spec,
				out audio_buf,
				out audio_len
			);
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_LockAudio();

		/* dev refers to an SDL_AudioDeviceID */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_LockAudioDevice(uint dev);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_MixAudio(
			[Out()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)]
				byte[] dst,
			[In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 2)]
				byte[] src,
			uint len,
			int volume
		);

		/* format refers to an SDL_AudioFormat */
		/* This overload allows raw pointers to be passed for dst and src. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_MixAudioFormat(
			IntPtr dst,
			IntPtr src,
			ushort format,
			uint len,
			int volume
		);

		/* format refers to an SDL_AudioFormat */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_MixAudioFormat(
			[Out()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 3)]
				byte[] dst,
			[In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 3)]
				byte[] src,
			ushort format,
			uint len,
			int volume
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_OpenAudio(
			ref SDL_AudioSpec desired,
			out SDL_AudioSpec obtained
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_OpenAudio(
			ref SDL_AudioSpec desired,
			IntPtr obtained
		);

		/* uint refers to an SDL_AudioDeviceID */
		/* This overload allows for IntPtr.Zero (null) to be passed for device. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint SDL_OpenAudioDevice(
			IntPtr device,
			int iscapture,
			ref SDL_AudioSpec desired,
			out SDL_AudioSpec obtained,
			int allowed_changes
		);

		/* uint refers to an SDL_AudioDeviceID */
		[DllImport(LibraryName, EntryPoint = "SDL_OpenAudioDevice", CallingConvention = CallingConvention.Cdecl)]
		private static extern unsafe uint INTERNAL_SDL_OpenAudioDevice(
			byte* device,
			int iscapture,
			ref SDL_AudioSpec desired,
			out SDL_AudioSpec obtained,
			int allowed_changes
		);
		public static unsafe uint SDL_OpenAudioDevice(
			string device,
			int iscapture,
			ref SDL_AudioSpec desired,
			out SDL_AudioSpec obtained,
			int allowed_changes
		) {
			int utf8DeviceBufSize = Utf8Size(device);
			byte* utf8Device = stackalloc byte[utf8DeviceBufSize];
			return INTERNAL_SDL_OpenAudioDevice(
				Utf8Encode(device, utf8Device, utf8DeviceBufSize),
				iscapture,
				ref desired,
				out obtained,
				allowed_changes
			);
		}

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_PauseAudio(int pause_on);

		/* dev refers to an SDL_AudioDeviceID */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_PauseAudioDevice(
			uint dev,
			int pause_on
		);

		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_UnlockAudio();

		/* dev refers to an SDL_AudioDeviceID */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_UnlockAudioDevice(uint dev);

		/* dev refers to an SDL_AudioDeviceID, data to a void*
	    * Only available in 2.0.4 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_QueueAudio(
			uint dev,
			IntPtr data,
			UInt32 len
		);

		/* dev refers to an SDL_AudioDeviceID, data to a void*
	    * Only available in 2.0.5 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern uint SDL_DequeueAudio(
			uint dev,
			IntPtr data,
			uint len
		);
		
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern unsafe uint SDL_DequeueAudio(
			uint dev,
			byte* data,
			uint len
		);

		/* dev refers to an SDL_AudioDeviceID
	    * Only available in 2.0.4 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern UInt32 SDL_GetQueuedAudioSize(uint dev);

		/* dev refers to an SDL_AudioDeviceID
	    * Only available in 2.0.4 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_ClearQueuedAudio(uint dev);

		/* src_format and dst_format refer to SDL_AudioFormats.
	    * IntPtr refers to an SDL_AudioStream*.
	    * Only available in 2.0.7 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr SDL_NewAudioStream(
			ushort src_format,
			byte src_channels,
			int src_rate,
			ushort dst_format,
			byte dst_channels,
			int dst_rate
		);

		/* stream refers to an SDL_AudioStream*, buf to a void*.
	    * Only available in 2.0.7 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_AudioStreamPut(
			IntPtr stream,
			IntPtr buf,
			int len
		);

		/* stream refers to an SDL_AudioStream*, buf to a void*.
	    * Only available in 2.0.7 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_AudioStreamGet(
			IntPtr stream,
			IntPtr buf,
			int len
		);

		/* stream refers to an SDL_AudioStream*.
	    * Only available in 2.0.7 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_AudioStreamAvailable(IntPtr stream);

		/* stream refers to an SDL_AudioStream*.
	    * Only available in 2.0.7 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_AudioStreamClear(IntPtr stream);

		/* stream refers to an SDL_AudioStream*.
	    * Only available in 2.0.7 or higher.
	    */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_FreeAudioStream(IntPtr stream);

		/* Only available in 2.0.16 or higher. */
		[DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int SDL_GetAudioDeviceSpec(
			int index,
			int iscapture,
			out SDL_AudioSpec spec
		);
    }
}