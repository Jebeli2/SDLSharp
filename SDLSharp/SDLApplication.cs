namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static SDL;

    public static class SDLApplication
    {
        private static Version version = new();
        private static string platform = "";
        private static SDLWindow? mainWindow;
        private static bool quit;
        private static readonly List<string> drivers = new();
        private static readonly List<SDLWindow> windows = new();

        private const double ONESECONDMS = 1000.0;
        private static bool useStopwatch = false;
        private static long frequency = 10000000;
        private static double ticksToMs = 0.0001;
        private static bool limitFps = true;
        private static double totalElapsedTime;
        private static double elapsedTime;
        private static int framesPerSecond = 60;
        private static int prevFramesPerSecond = 60;
        private static int maxFramesPerSecond = 60;
        private static double maxElapsedTime = 500;
        private static double fpsTime;
        private static double accumulatedElapsedTime;
        private static double previousTime;
        private static double targetTime = ONESECONDMS / maxFramesPerSecond;
        private static int updateFrameLag;
        private static string? fpsText;
        private static int frameCounter;
        private static bool suppressDraw;
        private static bool isRunningSlowly;

        internal static bool Quit => quit;
        public static Version Version => version;
        public static string Platform => platform;

        private static SDLFont? defaultFont;
        private static SDLFont? iconFont;
        private static string appName = "SDLApplication";

        public static void Run(SDLWindow window, LogPriority logPriority = LogPriority.Info)
        {
            mainWindow = window;
            Initialize(logPriority);
            windows.Add(window);
            if (window.Visible) { window.Show(); }
            MainLoop();
            Shutdown();
        }

        public static string AppName
        {
            get => appName;
            set => appName = value;
        }
        public static SDLFont? DefaultFont => defaultFont;
        internal static SDLFont? IconFont => iconFont;

        public static string FPSText
        {
            get
            {
                fpsText ??= BuildFPSText();
                return fpsText;
            }
        }

        private static string BuildFPSText()
        {
            return framesPerSecond.ToString() + " fps";
        }
        public static void Exit()
        {
            quit = true;
        }

        public static void Delay(double ms)
        {
            if (ms < 1) return;

            if (ms > 0)
            {
                uint ums = (uint)ms;
                if (ums > 0)
                {
                    SDL_Delay(ums);
                }
            }
        }

        public static bool SetHint(string name, int value)
        {
            return SDL_SetHint(name, value.ToString());
        }
        private static void Initialize(LogPriority logPriority)
        {
            string dllDir = Path.Combine(Environment.CurrentDirectory, IntPtr.Size == 4 ? "x86" : "x64");
            Environment.SetEnvironmentVariable("PATH", Environment.GetEnvironmentVariable("PATH") + ";" + dllDir);
            SDLLog.InitializeLog(logPriority);
            SDLLog.Info(LogCategory.APPLICATION, "SDL Initialization Starting...");
            SDL_SetMainReady();
            _ = SDL_SetHint(SDL_HINT_WINDOWS_DISABLE_THREAD_NAMING, "1");
            if (SDL_Init(SDL_INIT_EVERYTHING) == SDL_OK)
            {
                _ = SDL_SetHint(SDL_HINT_RENDER_DRIVER, "opengl");
                _ = SDL_SetHint(SDL_HINT_MOUSE_FOCUS_CLICKTHROUGH, "1");
                _ = SDL_SetHint(SDL_HINT_VIDEO_MINIMIZE_ON_FOCUS_LOSS, "0");
                _ = SDL_SetHint(SDL_HINT_RENDER_BATCHING, "1");
                _ = SDL_SetHint(SDL_HINT_GRAB_KEYBOARD, "1");
                _ = SDL_SetHint(SDL_HINT_ALLOW_ALT_TAB_WHILE_GRABBED, "1");
                //_ = SDL_SetHint(SDL_BORDERLESS_WINDOWED_STYLE, "1");
                //_ = SDL_SetHint(SDL_BORDERLESS_RESIZABLE_STYLE, "1");
                SDL_GetVersion(out SDL_version sdlVersion);
                int sdlRevision = SDL_GetRevisionNumber();
                version = new Version(sdlVersion.major, sdlVersion.minor, sdlVersion.patch, sdlRevision);
                platform = GetPlatform() ?? "unknown";
                SDLLog.Info(LogCategory.SYSTEM, $"SDL Platform: {platform}");
                SDLLog.Info(LogCategory.SYSTEM, $"SDL Version: {GetRevsion()}");
                InitTimer();
                GetDriverInfos();
                SDLInput.Initialize();
                SDLAudio.Initialize();
                SDLFont.Initialize();
                SDLTexture.Initialize();
                defaultFont = SDLFont.LoadFont(Properties.Resources.Roboto_Regular, nameof(Properties.Resources.Roboto_Regular), 16);
                iconFont = SDLFont.LoadFont(Properties.Resources.entypo, nameof(Properties.Resources.entypo), 16);
                SDLLog.Info(LogCategory.APPLICATION, "SDL Initialization Done...");
            }
            else
            {
                SDLLog.Critical(LogCategory.APPLICATION, $"SDL Initialization Failed: {GetError()}");
            }
        }

        private static void Shutdown()
        {
            SDLLog.Info(LogCategory.APPLICATION, "SDL Shutdown Starting...");
            foreach (SDLWindow window in windows) { window.Dispose(); }
            windows.Clear();
            mainWindow = null;
            defaultFont?.Dispose();
            defaultFont = null;
            iconFont?.Dispose();
            iconFont = null;
            SDLTexture.Shutdown();
            SDLFont.Shutdown();
            SDLAudio.Shutdown();
            SDLInput.Shutdown();
            SDLLog.Info(LogCategory.APPLICATION, "SDL Shutdown Done...");
            SDLLog.ShutdownLog();
            SDL_Quit();
        }

        private static void MainLoop()
        {
            previousTime = GetCurrentTime();
            UpdateLoop(0, 0);
            while (!quit)
            {
                SDLInput.MessageLoop();
                Tick();
            }
        }

        private static void Tick()
        {
            RetryTick:
            double currentTime = GetCurrentTime();
            accumulatedElapsedTime += currentTime - previousTime;
            previousTime = currentTime;
            if (limitFps && accumulatedElapsedTime < targetTime)
            {
                Delay(targetTime - accumulatedElapsedTime);
                goto RetryTick;
            }
            if (accumulatedElapsedTime > maxElapsedTime) { accumulatedElapsedTime = maxElapsedTime; }
            if (limitFps)
            {
                elapsedTime = targetTime;
                int stepCount = 0;
                while (accumulatedElapsedTime >= targetTime && !quit)
                {
                    totalElapsedTime += targetTime;
                    accumulatedElapsedTime -= targetTime;
                    ++stepCount;
                    UpdateLoop(totalElapsedTime, elapsedTime);
                }
                updateFrameLag += Math.Max(0, stepCount - 1);
                if (isRunningSlowly)
                {
                    if (updateFrameLag == 0)
                    {
                        isRunningSlowly = false;
                        //SDLLog.Verbose(LogCategory.APPLICATION, "Stopped Running Slowly");
                    }
                }
                else if (updateFrameLag >= 5)
                {
                    isRunningSlowly = true;
                    //SDLLog.Verbose(LogCategory.APPLICATION, "Started Running Slowly");
                }
                if (stepCount == 1 && updateFrameLag > 0) { updateFrameLag--; }
                elapsedTime = targetTime * stepCount;
            }
            else
            {
                elapsedTime += accumulatedElapsedTime;
                totalElapsedTime += accumulatedElapsedTime;
                accumulatedElapsedTime = 0;
                UpdateLoop(totalElapsedTime, elapsedTime);
            }
            if (suppressDraw)
            {
                suppressDraw = false;
            }
            else
            {
                PaintLoop(totalElapsedTime, elapsedTime);
            }

        }

        private static void UpdateLoop(double totalTime, double elapsedTime)
        {
            fpsTime += elapsedTime;
            if (fpsTime >= ONESECONDMS)
            {
                framesPerSecond = frameCounter;
                frameCounter = 0;
                fpsTime -= ONESECONDMS;
                if (framesPerSecond != prevFramesPerSecond)
                {
                    fpsText = null;
                    prevFramesPerSecond = framesPerSecond;
                }
            }
            foreach (SDLWindow window in windows)
            {
                if (window.HandleCreated) { window.Update(totalTime, elapsedTime); }
            }
        }

        private static void PaintLoop(double totalTime, double elapsedTime)
        {
            frameCounter++;
            foreach (SDLWindow window in windows)
            {
                if (window.HandleCreated) { window.Paint(totalTime, elapsedTime); }
            }
        }

        internal static SDLWindow? GetWindowFromId(int id)
        {
            foreach (SDLWindow window in windows)
            {
                if (window.WindowId == id)
                {
                    return window;
                }
            }
            return null;
        }
        public static int GetDriverIndex(string name)
        {
            return drivers.IndexOf(name);
        }

        private static void GetDriverInfos()
        {
            int numDrivers = SDL_GetNumRenderDrivers();
            for (int i = 0; i < numDrivers; i++)
            {
                if (SDL_GetRenderDriverInfo(i, out SDL_RendererInfo info) == 0)
                {
                    string? name = IntPtr2String(info.name);
                    if (name != null)
                    {
                        drivers.Add(name);
                        SDLLog.Verbose(LogCategory.RENDER, $"Added Driver: {name}");
                    }
                }
            }
        }


        private static void InitTimer()
        {
            long swfreq = Stopwatch.Frequency;
            ulong sdlfreq = SDL_GetPerformanceFrequency();
            if ((ulong)swfreq != sdlfreq)
            {
                useStopwatch = true;
                SDLLog.Critical(LogCategory.SYSTEM, $"Timer Frequency mismatch: {swfreq} (System) vs {sdlfreq} (SDL)");
            }
            ticksToMs = 1000.0 / swfreq;
            frequency = swfreq;
            SDLLog.Info(LogCategory.SYSTEM, $"Timer Initialized. Frequency = {frequency}, Ticks to Milliseconds = {ticksToMs}");
        }
        public static long GetCurrentTicks()
        {
            return useStopwatch ? Stopwatch.GetTimestamp() : (long)SDL_GetPerformanceCounter();
        }
        public static double GetCurrentTime()
        {
            return ticksToMs * GetCurrentTicks();
        }
    }
}
