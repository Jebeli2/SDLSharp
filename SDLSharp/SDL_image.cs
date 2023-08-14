namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    public static class SDL_image
    {
        private const string nativeLibName = "SDL2_image";
        #region SDL_image.h

        public const int SDL_IMAGE_MAJOR_VERSION = 2;
        public const int SDL_IMAGE_MINOR_VERSION = 0;
        public const int SDL_IMAGE_PATCHLEVEL = 6;

        [Flags]
        public enum IMG_InitFlags
        {
            IMG_INIT_JPG = 0x00000001,
            IMG_INIT_PNG = 0x00000002,
            IMG_INIT_TIF = 0x00000004,
            IMG_INIT_WEBP = 0x00000008
        }

        public static void SDL_IMAGE_VERSION(out SDL.SDL_version X)
        {
            X.major = SDL_IMAGE_MAJOR_VERSION;
            X.minor = SDL_IMAGE_MINOR_VERSION;
            X.patch = SDL_IMAGE_PATCHLEVEL;
        }

        [DllImport(nativeLibName, EntryPoint = "IMG_Linked_Version", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_IMG_Linked_Version();
        public static SDL.SDL_version IMG_Linked_Version()
        {
            SDL.SDL_version result;
            IntPtr result_ptr = INTERNAL_IMG_Linked_Version();
            result = Marshal.PtrToStructure<SDL.SDL_version>(result_ptr);
            return result;
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int IMG_Init(IMG_InitFlags flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void IMG_Quit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr IMG_Load([In()][MarshalAs(UnmanagedType.LPUTF8Str)] string file);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_Load_RW(IntPtr src, int freesrc);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr IMG_LoadTyped_RW(IntPtr src, int freesrc, [In()][MarshalAs(UnmanagedType.LPStr)] string type);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr IMG_LoadTexture(IntPtr renderer, [In()][MarshalAs(UnmanagedType.LPStr)] string file);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadTexture_RW(IntPtr renderer, IntPtr src, int freesrc);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr IMG_LoadTextureTyped_RW(IntPtr renderer, IntPtr src, int freesrc, [In()][MarshalAs(UnmanagedType.LPStr)] string type);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_ReadXPMFromArray(
            [In()] [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)]
            string[] xpm
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int IMG_SavePNG(IntPtr surface, string file);
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int IMG_SavePNG_RW(IntPtr surface, IntPtr dst, int freedst);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int IMG_SaveJPG(IntPtr surface, [In()][MarshalAs(UnmanagedType.LPUTF8Str)] string file, int quality);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int IMG_SaveJPG_RW(IntPtr surface, IntPtr dst, int freedst, int quality);

        public static string? IMG_GetError()
        {
            return SDL.GetError();
        }

        public static void IMG_SetError(string fmtAndArglist)
        {
            SDL.SDL_SetError(fmtAndArglist);
        }

        #region Animated Image Support

        public struct IMG_Animation
        {
            public int w;
            public int h;
            public IntPtr frames; /* SDL_Surface** */
            public IntPtr delays; /* int* */
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadAnimation(
            [In()] [MarshalAs(UnmanagedType.LPStr)]
                string file
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadAnimation_RW(IntPtr src, int freesrc);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadAnimationTyped_RW(IntPtr src, int freesrc, [In()][MarshalAs(UnmanagedType.LPStr)] string type);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void IMG_FreeAnimation(IntPtr anim);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr IMG_LoadGIFAnimation_RW(IntPtr src);

        #endregion

        #endregion
    }
}
