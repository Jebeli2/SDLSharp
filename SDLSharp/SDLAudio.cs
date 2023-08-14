namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Channels;
    using System.Threading.Tasks;
    using static SDLSharp.SDL_mixer;

    public static class SDLAudio
    {
        public const string GlobalChannel = "_global_";
        private const int NumChannels = 128;

        private static int musicVolume = MIX_MAX_VOLUME;
        private static int soundVolume = MIX_MAX_VOLUME;
        private static SDLMusic? currentMusic;
        private static string? driverName;

        private static readonly SDLObjectTracker<SDLMusic> musicTracker = new(LogCategory.AUDIO, "Music");
        private static readonly SDLObjectTracker<SDLSound> soundTracker = new(LogCategory.AUDIO, "Sound");

        private static readonly Dictionary<int, Playback> playback = new();
        private static readonly Dictionary<string, int> channels = new();
        private static readonly ChannelFinishedDelegate channelFinished = OnChannelFinished;
        private static PointF lastPos;

        public static bool UseTmpFilesForMusic { get; set; } = true;
        public static bool AttemptToDeleteOldTmpFiles { get; set; } = true;
        public static int SoundFallOff { get; set; } = 15;
        public static int MusicVolume
        {
            get => musicVolume;
            set
            {
                if (value > MIX_MAX_VOLUME) { value = MIX_MAX_VOLUME; }
                if (value < 0) { value = 0; }
                if (musicVolume != value)
                {
                    musicVolume = value;
                    _ = Mix_VolumeMusic(musicVolume);
                }
            }
        }

        public static int SoundVolume
        {
            get => soundVolume;
            set
            {
                if (value > MIX_MAX_VOLUME) { value = MIX_MAX_VOLUME; }
                if (value < 0) { value = 0; }
                if (soundVolume != value)
                {
                    soundVolume = value;
                    SetSoundVolume(soundVolume);
                }
            }
        }

        public static bool IsPlaying
        {
            get { return Mix_PlayingMusic() == 1; }
        }
        public static bool IsPaused
        {
            get { return Mix_PausedMusic() == 1; }
        }


        public static void Initialize()
        {
            _ = Mix_Init(MIX_InitFlags.MIX_INIT_MP3 | MIX_InitFlags.MIX_INIT_OGG | MIX_InitFlags.MIX_INIT_MID);
            if (Mix_OpenAudio(22050, MIX_DEFAULT_FORMAT, 2, 1024) != 0)
            {
                SDLLog.Error(LogCategory.AUDIO, "Couldn't open Audio");
            }
            else
            {
                _ = Mix_AllocateChannels(NumChannels);
                driverName = SDL.IntPtr2String(SDL.SDL_GetCurrentAudioDriver());
                SDLLog.Info(LogCategory.AUDIO, $"Audio opened: {driverName}");
            }
        }

        public static void Shutdown()
        {
            _ = Mix_HaltMusic();
            Mix_SetPostMix(IntPtr.Zero, IntPtr.Zero);
            Mix_HookMusicFinished(IntPtr.Zero);
            musicTracker.Dispose();
            Mix_CloseAudio();
            Mix_Quit();
            SDLLog.Info(LogCategory.AUDIO, "Audo closed");
        }

        internal static void Track(SDLMusic music)
        {
            musicTracker.Track(music);
        }

        internal static void Untrack(SDLMusic music)
        {
            musicTracker.Untrack(music);
        }

        internal static void Track(SDLSound sound)
        {
            soundTracker.Track(sound);
        }

        internal static void Untrack(SDLSound sound)
        {
            soundTracker.Untrack(sound);
        }


        public static SDLMusic? LoadMusic(string name)
        {
            SDLMusic? music = musicTracker.Find(name);
            if (music == null && File.Exists(name))
            {
                IntPtr handle = Mix_LoadMUS(name);
                if (handle != IntPtr.Zero)
                {
                    music = new SDLMusic(handle, name);
                    SDLLog.Verbose(LogCategory.AUDIO, $"Music loaded from file '{name}'");
                }
            }
            return music;
        }

        public static SDLMusic? LoadMusic(string name, byte[]? data)
        {
            if (data != null)
            {
                return InternalLoadMusic(name, data);
            }
            return null;
        }

        private static SDLMusic? InternalLoadMusic(string name, byte[] data)
        {
            SDLMusic? music = musicTracker.Find(name);
            if (music == null)
            {
                if (UseTmpFilesForMusic)
                {
                    string fileName = FileUtils.GetTempFile(name, AttemptToDeleteOldTmpFiles);
                    try
                    {
                        File.WriteAllBytes(fileName, data);
                        IntPtr handle = Mix_LoadMUS(fileName);
                        if (handle != IntPtr.Zero)
                        {
                            music = new SDLMusic(handle, name, fileName);
                            SDLLog.Verbose(LogCategory.AUDIO, $"Music loaded from resource '{name}' (via temporary file '{fileName}')");
                        }
                    }
                    catch (Exception ex)
                    {
                        SDLLog.Error(LogCategory.AUDIO, $"Could not load Music from resource '{name}' (via temporary file '{fileName}'): {ex.Message}");
                    }
                }
                else
                {
                    IntPtr rw = SDL.SDL_RWFromMem(data, data.Length);
                    IntPtr handle = Mix_LoadMUS_RW(rw, 1);
                    if (handle != IntPtr.Zero)
                    {
                        music = new SDLMusic(handle, name);
                        SDLLog.Verbose(LogCategory.AUDIO, $"Music loaded from resource '{name}'");
                    }
                }
            }
            return music;
        }

        public static void PlayMusic(SDLMusic? music, int loops = -1, bool forceRestart = false)
        {
            if (music == null) return;
            IntPtr handle = music.Handle;
            if (handle == IntPtr.Zero) return;
            //SDLMusicEventArgs e = new SDLMusicEventArgs(music);
            if (currentMusic != null)
            {
                if (currentMusic != music)
                {
                    //OnMusicFinished(new SDLMusicFinishedEventArgs(currentMusic, MusicFinishReason.Interrupted));
                }
                else if (forceRestart)
                {
                    Mix_RewindMusic();
                    SDLLog.Verbose(LogCategory.AUDIO, $"Music '{music.Name}' restarted");
                    //OnMusicStarted(e);
                    return;
                }
                else
                {
                    SDLLog.Verbose(LogCategory.AUDIO, $"Music '{music.Name}' already playing, continuing...");
                    //OnMusicStarted(e); // or maybe not?
                    return;
                }
            }
            if (Mix_PlayMusic(handle, loops) != 0)
            {
                SDLLog.Error(LogCategory.AUDIO, $"Could not play Music '{music.Name}': {SDL.GetError()}");
            }
            else
            {
                _ = Mix_VolumeMusic(musicVolume);
                SDLLog.Verbose(LogCategory.AUDIO, $"Music '{music.Name}' started");
                //OnMusicStarted(e);
                currentMusic = music;
                //musicDataEventArgs = new SDLMusicDataEventArgs(currentMusic);
            }
        }
        public static void PauseMusic()
        {
            if (currentMusic != null)
            {
                Mix_PauseMusic();
            }
        }

        public static void ResumeMusic()
        {
            if (currentMusic != null)
            {
                Mix_ResumeMusic();
            }
        }

        public static void RewindMusic()
        {
            if (currentMusic != null)
            {
                Mix_RewindMusic();
            }
        }

        public static void StopMusic()
        {
            if (currentMusic != null)
            {
                _ = Mix_HaltMusic();
                currentMusic = null;
            }
        }

        public static SDLSound? LoadSound(string name)
        {
            SDLSound? sound = soundTracker.Find(name);
            if (sound == null && File.Exists(name))
            {
                IntPtr handle = Mix_LoadMUS(name);
                if (handle != IntPtr.Zero)
                {
                    sound = new SDLSound(handle, name);
                    SDLLog.Verbose(LogCategory.AUDIO, $"Sound loaded from file '{name}'");
                }
            }
            return sound;
        }

        public static SDLSound? LoadSound(string name, byte[]? data)
        {
            SDLSound? sound = soundTracker.Find(name);
            if (sound == null && data != null)
            {
                IntPtr rw = SDL.SDL_RWFromMem(data, data.Length);
                if (rw != IntPtr.Zero)
                {
                    IntPtr snd = Mix_LoadWAV_RW(rw, 1);
                    if (snd != IntPtr.Zero)
                    {
                        sound = new SDLSound(snd, name);
                        SDLLog.Verbose(LogCategory.AUDIO, $"Sound loaded from resource '{name}'");
                    }
                }
            }
            return sound;
        }

        public static void PlaySound(SDLSound? sound)
        {
            PlaySound(sound, null, Point.Empty);
        }
        public static void PlaySound(SDLSound? sound, string? channel, PointF pos, bool loop = false)
        {
            if (sound != null)
            {
                Play(new Playback(sound, channel ?? GlobalChannel, pos, loop));
            }
        }

        public static void Update(float x, float y)
        {
            lastPos.X = x;
            lastPos.Y = y;
            List<int> cleanup = new();
            foreach (var it in playback)
            {
                int channel = it.Key;
                Playback play = it.Value;
                if (play.Finished)
                {
                    cleanup.Add(channel);
                    continue;
                }
                if (play.Location.X == 0 && play.Location.Y == 0)
                {
                    continue;
                }
                float v = Distance(x, y, play.Location.X, play.Location.Y) / SoundFallOff;
                if (play.Loop)
                {
                    if (v < 1.0f && play.Paused)
                    {
                        Mix_Resume(channel);
                        play.Paused = false;
                    }
                    else if (v > 1.0f && !play.Paused)
                    {
                        Mix_Pause(channel);
                        play.Paused = true;
                        continue;
                    }
                }
                v = Math.Min(Math.Max(v, 0.0f), 1.0f);
                byte dist = (byte)(255.0f * v);
                _ = Mix_SetPosition(channel, 0, dist);
            }
            while (cleanup.Count > 0)
            {
                int channel = cleanup[0];
                cleanup.RemoveAt(0);
                if (playback.TryGetValue(channel, out Playback? play))
                {
                    playback.Remove(channel);
                    channels.Remove(play.Channel);
                }
            }
        }

        private static void Play(Playback pb)
        {
            bool setChannel = false;
            if (!string.Equals(GlobalChannel, pb.Channel))
            {
                if (channels.TryGetValue(pb.Channel, out int vc))
                {
                    _ = Mix_HaltChannel(vc);
                    channels.Remove(pb.Channel);
                }
                setChannel = true;
            }
            int channel = Mix_PlayChannel(-1, pb.Sound.Handle, pb.Loop ? -1 : 0);
            if (channel == -1)
            {
                SDLLog.Error(LogCategory.AUDIO, $"Failed to play sound '{pb.Sound}', no more channels available");
            }
            else
            {
                _ = Mix_Volume(channel, soundVolume);
                Mix_ChannelFinished(channelFinished);
                SDLLog.Verbose(LogCategory.AUDIO, $"Playing sound '{pb.Sound}' on channel {channel} ({pb.Channel})");
            }
            byte dist;
            if (!pb.Location.IsEmpty)
            {
                float v = 255.0f * (Distance(lastPos, pb.Location)) / SoundFallOff;
                v = MathF.Min(MathF.Max(v, 0.0f), 255.0f);
                dist = (byte)v;
            }
            else
            {
                dist = 0;
            }
            _ = Mix_SetPosition(channel, 0, dist);
            if (setChannel) { channels[pb.Channel] = channel; }
            playback[channel] = pb;

        }
        private static void OnChannelFinished(int channel)
        {
            if (playback.TryGetValue(channel, out Playback? play))
            {
                if (play != null)
                {
                    play.Finished = true;
                }
            }
            _ = Mix_SetPosition(channel, 0, 0);
        }

        private static void SetSoundVolume(int volume)
        {
            _ = Mix_Volume(0, volume);
            foreach (int channel in channels.Values)
            {
                _ = Mix_Volume(channel, volume);
            }
        }

        private static float Distance(PointF x, PointF y)
        {
            return Distance(x.X, x.Y, y.X, y.Y);
        }
        private static float Distance(float x0, float y0, float x1, float y1)
        {
            return MathF.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
        }


        private class Playback
        {
            public Playback(SDLSound sound, string channel, PointF pos, bool loop)
            {
                Sound = sound;
                Channel = channel;
                Location = pos;
                Loop = loop;
            }

            public SDLSound Sound;
            public string Channel;
            public PointF Location;
            public bool Loop;
            public bool Paused;
            public bool Finished;
        }

    }
}
