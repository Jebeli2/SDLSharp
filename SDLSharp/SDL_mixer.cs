namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public static class SDL_mixer
    {
        private const string nativeLibName = "SDL2_mixer";

        #region SDL_mixer.h

        /* Similar to the headers, this is the version we're expecting to be
		 * running with. You will likely want to check this somewhere in your
		 * program!
		 */
        public const int SDL_MIXER_MAJOR_VERSION = 2;
        public const int SDL_MIXER_MINOR_VERSION = 0;
        public const int SDL_MIXER_PATCHLEVEL = 5;

        /* In C, you can redefine this value before including SDL_mixer.h.
		 * We're not going to allow this in SDL2#, since the value of this
		 * variable is persistent and not dependent on preprocessor ordering.
		 */
        public const int MIX_CHANNELS = 8;

        public static readonly int MIX_DEFAULT_FREQUENCY = 44100;
        public static readonly ushort MIX_DEFAULT_FORMAT = BitConverter.IsLittleEndian ? SDL.AUDIO_S16LSB : SDL.AUDIO_S16MSB;
        public static readonly int MIX_DEFAULT_CHANNELS = 2;
        public static readonly byte MIX_MAX_VOLUME = 128;

        [Flags]
        public enum MIX_InitFlags
        {
            MIX_INIT_FLAC = 0x00000001,
            MIX_INIT_MOD = 0x00000002,
            MIX_INIT_MP3 = 0x00000008,
            MIX_INIT_OGG = 0x00000010,
            MIX_INIT_MID = 0x00000020,
            MIX_INIT_OPUS = 0x00000040
        }

        public struct MIX_Chunk
        {
            public int allocated;
            public IntPtr abuf; /* Uint8* */
            public uint alen;
            public byte volume;
        }

        public enum Mix_Fading
        {
            MIX_NO_FADING,
            MIX_FADING_OUT,
            MIX_FADING_IN
        }

        public enum Mix_MusicType
        {
            MUS_NONE,
            MUS_CMD,
            MUS_WAV,
            MUS_MOD,
            MUS_MID,
            MUS_OGG,
            MUS_MP3,
            MUS_MP3_MAD_UNUSED,
            MUS_FLAC,
            MUS_MODPLUG_UNUSED,
            MUS_OPUS
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MixFuncDelegate(
            IntPtr udata, // void*
            IntPtr stream, // Uint8*
            int len
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Mix_EffectFunc_t(
            int chan,
            IntPtr stream, // void*
            int len,
            IntPtr udata // void*
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Mix_EffectDone_t(
            int chan,
            IntPtr udata // void*
        );

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void MusicFinishedDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ChannelFinishedDelegate(int channel);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int SoundFontDelegate(
            IntPtr a, // const char*
            IntPtr b // void*
        );

        public static void SDL_MIXER_VERSION(out SDL.SDL_version X)
        {
            X.major = SDL_MIXER_MAJOR_VERSION;
            X.minor = SDL_MIXER_MINOR_VERSION;
            X.patch = SDL_MIXER_PATCHLEVEL;
        }

        [DllImport(nativeLibName, EntryPoint = "MIX_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_MIX_Linked_Version();
        public static SDL.SDL_version MIX_Linked_Version()
        {
            SDL.SDL_version result;
            IntPtr result_ptr = INTERNAL_MIX_Linked_Version();
            result = Marshal.PtrToStructure<SDL.SDL_version>(
                result_ptr
            );
            return result;
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_Init(MIX_InitFlags flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_Quit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_OpenAudio(
            int frequency,
            ushort format,
            int channels,
            int chunksize
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_AllocateChannels(int numchans);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_QuerySpec(
            out int frequency,
            out ushort format,
            out int channels
        );

        /* src refers to an SDL_RWops*, IntPtr to a Mix_Chunk* */
        /* THIS IS A PUBLIC RWops FUNCTION! */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_LoadWAV_RW(
            IntPtr src,
            int freesrc
        );

        /* IntPtr refers to a Mix_Chunk* */
        /* This is an RWops macro in the C header. */
        public static IntPtr Mix_LoadWAV(string file)
        {
            IntPtr rwops = SDL.SDL_RWFromFile(file, "rb");
            return Mix_LoadWAV_RW(rwops, 1);
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Mix_LoadMUS_RW(IntPtr rwops, int freesrc);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_LoadMUS(string file);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_QuickLoad_WAV(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1)]
                byte[] mem
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_QuickLoad_RAW(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U1, SizeParamIndex = 1)]
                byte[] mem,
            uint len
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_FreeChunk(IntPtr chunk);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_FreeMusic(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GetNumChunkDecoders();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetChunkDecoder(int index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GetNumMusicDecoders();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetMusicDecoder(int index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Mix_MusicType Mix_GetMusicType(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetMusicTitle(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetMusicTitleTag(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetMusicArtistTag(IntPtr music);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetMusicAlbumTag(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetMusicCopyrightTag(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_SetPostMix(MixFuncDelegate mix_func, IntPtr arg);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_SetPostMix(IntPtr mix_func, IntPtr arg);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_HookMusic(MixFuncDelegate mix_func, IntPtr arg);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_HookMusic(IntPtr mix_func, IntPtr arg);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_HookMusicFinished(MusicFinishedDelegate music_finished);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_HookMusicFinished(IntPtr music_finished);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetMusicHookData();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_ChannelFinished(ChannelFinishedDelegate channel_finished);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_RegisterEffect(int chan, Mix_EffectFunc_t f, Mix_EffectDone_t d, IntPtr arg);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_UnregisterEffect(int channel, Mix_EffectFunc_t f);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_UnregisterAllEffects(int channel);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_SetPanning(int channel, byte left, byte right);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_SetPosition(int channel, short angle, byte distance);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_SetDistance(int channel, byte distance);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_SetReverseStereo(int channel, int flip);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_ReserveChannels(int num);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GroupChannel(int which, int tag);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GroupChannels(int from, int to, int tag);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GroupAvailable(int tag);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GroupCount(int tag);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GroupOldest(int tag);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GroupNewer(int tag);

        public static int Mix_PlayChannel(int channel, IntPtr chunk, int loops)
        {
            return Mix_PlayChannelTimed(channel, chunk, loops, -1);
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_PlayChannelTimed(int channel, IntPtr chunk, int loops, int ticks);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_PlayMusic(IntPtr music, int loops);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_FadeInMusic(IntPtr music, int loops, int ms);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_FadeInMusicPos(IntPtr music, int loops, int ms, double position);

        public static int Mix_FadeInChannel(int channel, IntPtr chunk, int loops, int ms)
        {
            return Mix_FadeInChannelTimed(channel, chunk, loops, ms, -1);
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_FadeInChannelTimed(int channel, IntPtr chunk, int loops, int ms, int ticks);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_Volume(int channel, int volume);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_VolumeChunk(IntPtr chunk, int volume);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_VolumeMusic(int volume);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GetVolumeMusicStream(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_HaltChannel(int channel);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_HaltGroup(int tag);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_HaltMusic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_ExpireChannel(int channel, int ticks);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_FadeOutChannel(int which, int ms);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_FadeOutGroup(int tag, int ms);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_FadeOutMusic(int ms);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Mix_Fading Mix_FadingMusic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern Mix_Fading Mix_FadingChannel(int which);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_Pause(int channel);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_Resume(int channel);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_Paused(int channel);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_PauseMusic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_ResumeMusic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_RewindMusic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_PausedMusic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_SetMusicPosition(double position);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double Mix_GetMusicPosition(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double Mix_MusicDuration(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double Mix_GetMusicLoopStartTime(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double Mix_GetMusicLoopEndTime(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern double Mix_GetMusicLoopLengthTime(IntPtr music);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_Playing(int channel);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_PlayingMusic();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int INTERNAL_Mix_SetMusicCMD(string command);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_SetSynchroValue(int value);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_GetSynchroValue();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_SetSoundFonts(string paths);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetSoundFonts();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_EachSoundFont(SoundFontDelegate function, IntPtr data);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int Mix_SetTimidityCfg([In()][MarshalAs(UnmanagedType.LPStr)] string path);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetTimidityCfg();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr Mix_GetChunk(int channel);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Mix_CloseAudio();

        public static string? Mix_GetError()
        {
            return SDL.GetError();
        }

        public static void Mix_SetError(string fmtAndArglist)
        {
            SDL.SDL_SetError(fmtAndArglist);
        }

        public static void Mix_ClearError()
        {
            SDL.SDL_ClearError();
        }

        #endregion
    }
}
