namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    internal static class SDL_ttf
    {
        private const string nativeLibName = "SDL2_ttf";

        #region SDL_ttf.h

        /* Similar to the headers, this is the version we're expecting to be
		 * running with. You will likely want to check this somewhere in your
		 * program!
		 */
        public const int SDL_TTF_MAJOR_VERSION = 2;
        public const int SDL_TTF_MINOR_VERSION = 0;
        public const int SDL_TTF_PATCHLEVEL = 16;

        public const int UNICODE_BOM_NATIVE = 0xFEFF;
        public const int UNICODE_BOM_SWAPPED = 0xFFFE;

        public const int TTF_STYLE_NORMAL = 0x00;
        public const int TTF_STYLE_BOLD = 0x01;
        public const int TTF_STYLE_ITALIC = 0x02;
        public const int TTF_STYLE_UNDERLINE = 0x04;
        public const int TTF_STYLE_STRIKETHROUGH = 0x08;

        public const int TTF_HINTING_NORMAL = 0;
        public const int TTF_HINTING_LIGHT = 1;
        public const int TTF_HINTING_MONO = 2;
        public const int TTF_HINTING_NONE = 3;
        public const int TTF_HINTING_LIGHT_SUBPIXEL = 4; /* >= 2.0.16 */

        public static void SDL_TTF_VERSION(out SDL.SDL_version X)
        {
            X.major = SDL_TTF_MAJOR_VERSION;
            X.minor = SDL_TTF_MINOR_VERSION;
            X.patch = SDL_TTF_PATCHLEVEL;
        }

        [DllImport(nativeLibName, EntryPoint = "TTF_LinkedVersion", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_TTF_LinkedVersion();
        public static SDL.SDL_version TTF_LinkedVersion()
        {
            SDL.SDL_version result;
            IntPtr result_ptr = INTERNAL_TTF_LinkedVersion();
            result = Marshal.PtrToStructure<SDL.SDL_version>(
                result_ptr
            );
            return result;
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_ByteSwappedUNICODE(int swapped);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_Init();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr TTF_OpenFont(string file, int ptsize);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_OpenFontRW(IntPtr src, int freesrc, int ptsize);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr INTERNAL_TTF_OpenFontIndex(string file, int ptsize, long index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_OpenFontIndexRW(IntPtr src, int freesrc, int ptsize, long index);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_SetFontSize(IntPtr font, int ptsize);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GetFontStyle(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_SetFontStyle(IntPtr font, int style);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GetFontOutline(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_SetFontOutline(IntPtr font, int outline);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GetFontHinting(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_SetFontHinting(IntPtr font, int hinting);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_FontHeight(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_FontAscent(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_FontDescent(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_FontLineSkip(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GetFontKerning(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_SetFontKerning(IntPtr font, int allowed);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_FontFaces(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_FontFaceIsFixedWidth(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr TTF_FontFaceFamilyName(IntPtr font);
        public static string? FontFaceFamilyName(IntPtr font)
        {
            return SDL.IntPtr2String(TTF_FontFaceFamilyName(font));
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr TTF_FontFaceStyleName(IntPtr font);
        public static string? FontFaceStyleName(IntPtr font)
        {
            return SDL.IntPtr2String(TTF_FontFaceStyleName(font));
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GlyphIsProvided(IntPtr font, ushort ch);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GlyphIsProvided32(IntPtr font, uint ch);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GlyphMetrics(
            IntPtr font,
            ushort ch,
            out int minx,
            out int maxx,
            out int miny,
            out int maxy,
            out int advance
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GlyphMetrics32(
            IntPtr font,
            uint ch,
            out int minx,
            out int maxx,
            out int miny,
            out int maxy,
            out int advance
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_SizeText(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
            string text,
            out int w,
            out int h
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int TTF_SizeUTF8(IntPtr font, [In()][MarshalAs(UnmanagedType.LPUTF8Str)] StringBuilder text, out int w, out int h);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_SizeUNICODE(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPWStr)]
            string text,
            out int w,
            out int h
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_MeasureText(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
            string text,
            int measure_width,
            out int extent,
            out int count
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_MeasureUTF8(
            IntPtr font,
            [In()][MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            int measure_width,
            out int extent,
            out int count
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_MeasureUNICODE(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPWStr)]
            string text,
            int measure_width,
            out int extent,
            out int count
        );

        /* IntPtr refers to an SDL_Surface*, font to a TTF_Font* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderText_Solid(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
            string text,
            SDL.SDL_Color fg
        );

        /* IntPtr refers to an SDL_Surface*, font to a TTF_Font* */
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUTF8_Solid(
            IntPtr font,
            [In()][MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            SDL.SDL_Color fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderUNICODE_Solid(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPWStr)]
            string text,
            SDL.SDL_Color fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderText_Solid_Wrapped(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
            string text,
            SDL.SDL_Color fg,
            uint wrapLength
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderUTF8_Solid_Wrapped(
            IntPtr font,
            [In()][MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            SDL.SDL_Color fg,
            uint wrapLength
        );
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderUNICODE_Solid_Wrapped(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPWStr)]
            string text,
            SDL.SDL_Color fg,
            uint wrapLength
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderGlyph_Solid(
            IntPtr font,
            ushort ch,
            SDL.SDL_Color fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderGlyph32_Solid(
            IntPtr font,
            uint ch,
            SDL.SDL_Color fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderText_Shaded(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
            string text,
            SDL.SDL_Color fg,
            SDL.SDL_Color bg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUTF8_Shaded(
            IntPtr font,
            [In()][MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            SDL.SDL_Color fg,
            SDL.SDL_Color bg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderUNICODE_Shaded(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPWStr)]
            string text,
            SDL.SDL_Color fg,
            SDL.SDL_Color bg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderText_Shaded_Wrapped(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
            string text,
            SDL.SDL_Color fg,
            SDL.SDL_Color bg,
            uint wrapLength
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderUTF8_Shaded_Wrapped(
            IntPtr font,
            [In()][MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            SDL.SDL_Color fg,
            SDL.SDL_Color bg,
            uint wrapLength
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderUNICODE_Shaded_Wrapped(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPWStr)]
            string text,
            SDL.SDL_Color fg,
            SDL.SDL_Color bg,
            uint wrapLength
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderGlyph_Shaded(
            IntPtr font,
            ushort ch,
            SDL.SDL_Color fg,
            SDL.SDL_Color bg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderGlyph32_Shaded(
            IntPtr font,
            uint ch,
            SDL.SDL_Color fg,
            SDL.SDL_Color bg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderText_Blended(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
            string text,
            SDL.SDL_Color fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr TTF_RenderUTF8_Blended(
            IntPtr font,
            [In()][MarshalAs(UnmanagedType.LPUTF8Str)] StringBuilder text,
            int fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderUNICODE_Blended(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPWStr)]
            string text,
            SDL.SDL_Color fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderText_Blended_Wrapped(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPStr)]
            string text,
            SDL.SDL_Color fg,
            uint wrapped
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TTF_RenderUTF8_Blended_Wrapped(
            IntPtr font,
            [In()][MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            SDL.SDL_Color fg,
            uint wrapped
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderUNICODE_Blended_Wrapped(
            IntPtr font,
            [In()] [MarshalAs(UnmanagedType.LPWStr)]
            string text,
            SDL.SDL_Color fg,
            uint wrapped
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderGlyph_Blended(
            IntPtr font,
            ushort ch,
            SDL.SDL_Color fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TTF_RenderGlyph32_Blended(
            IntPtr font,
            uint ch,
            SDL.SDL_Color fg
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_SetDirection(int direction);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_SetScript(int script);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_CloseFont(IntPtr font);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TTF_Quit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_WasInit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_GetFontKerningSize(
            IntPtr font,
            int prev_index,
            int index
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GetFontKerningSizeGlyphs(
            IntPtr font,
            ushort previous_ch,
            ushort ch
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TTF_GetFontKerningSizeGlyphs32(
            IntPtr font,
            ushort previous_ch,
            ushort ch
        );

        public static string? TTF_GetError()
        {
            return SDL.GetError();
        }

        public static void TTF_SetError(string fmtAndArglist)
        {
            SDL.SDL_SetError(fmtAndArglist);
        }

        #endregion
    }
}
