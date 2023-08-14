namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using static SDLSharp.SDL;

    public static class SDLLog
    {
        private static readonly SDL_LogOutputFunction logFunc = LogOutputFunc;
        private static bool logToConsole = true;
        private static LogPriority logPriority;
        private static readonly string[] prioText = new string[(int)LogPriority.Max];
        private static readonly string[] catText = new string[(int)LogCategory.MAX];
        public static void InitializeLog(LogPriority prio)
        {
            logPriority = prio;
            for (int i = 0; i < prioText.Length; i++) { prioText[i] = string.Format("{0,-8}", ((LogPriority)i).ToString().ToUpperInvariant()); }
            for (int i = 0; i < catText.Length; i++) { catText[i] = string.Format("{0,-11}", ((LogCategory)i).ToString().ToUpperInvariant()); }
            SDL_LogSetOutputFunction(logFunc, IntPtr.Zero);
            SDL_LogSetAllPriority((SDL_LogPriority)prio);
        }
        public static void ShutdownLog()
        {
            SDL_LogSetOutputFunction(IntPtr.Zero, IntPtr.Zero);
            SDL_LogResetPriorities();
        }
        public static bool WillLog(LogCategory cat, LogPriority prio)
        {
            if (prio >= logPriority)
            {
                return true;
            }
            return false;
        }
        public static void Log(LogCategory cat, LogPriority prio, string msg)
        {
            if (!WillLog(cat, prio)) return;
            SDL_LogMessage((int)cat, (SDL_LogPriority)prio, msg);
        }
        public static void Verbose(LogCategory cat, string msg)
        {
            Log(cat, LogPriority.Verbose, msg);
        }
        public static void Debug(LogCategory cat, string msg)
        {
            Log(cat, LogPriority.Debug, msg);
        }
        public static void Info(LogCategory cat, string msg)
        {
            Log(cat, LogPriority.Info, msg);
        }
        public static void Warn(LogCategory cat, string msg)
        {
            Log(cat, LogPriority.Warn, msg);
        }
        public static void Error(LogCategory cat, string msg)
        {
            Log(cat, LogPriority.Error, msg);
        }
        public static void Critical(LogCategory cat, string msg)
        {
            Log(cat, LogPriority.Critical, msg);
        }

        private static void LogOutputFunc(IntPtr userData, int category, SDL_LogPriority priority, IntPtr message)
        {
            if (logToConsole)
            {
                ClearConsoleColor();
                Console.Write(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                Console.Write(' ');
                SetConsoleColor(priority);
                Console.Write(prioText[(int)priority]);
                Console.Write(' ');
                Console.Write(catText[category]);
                Console.Write(' ');
                ClearConsoleColor();
                //Console.WriteLine(Marshal.PtrToStringUTF8(message));
                Console.WriteLine(Marshal.PtrToStringAnsi(message));
            }
        }

        private static void ClearConsoleColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }
        private static void SetConsoleColor(SDL_LogPriority level)
        {
            switch (level)
            {
                case SDL_LogPriority.SDL_LOG_PRIORITY_ERROR:
                case SDL_LogPriority.SDL_LOG_PRIORITY_CRITICAL:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case SDL_LogPriority.SDL_LOG_PRIORITY_WARN:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case SDL_LogPriority.SDL_LOG_PRIORITY_DEBUG:
                case SDL_LogPriority.SDL_LOG_PRIORITY_VERBOSE:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case SDL_LogPriority.SDL_LOG_PRIORITY_INFO:
                default:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

    }
}
