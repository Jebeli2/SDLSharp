namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using static SDL_ttf;

    public class SDLFont : SDLObject
    {
        private static readonly SDLObjectTracker<SDLFont> fontTracker = new(LogCategory.APPLICATION, "Font");
        private static readonly StringBuilder stringBuffer = new(512);
        private static int nextFontId;
        private readonly int fontId;
        private readonly IntPtr mem;
        private readonly int ySize;
        private FontStyle fontStyle;
        private int fontOutline;
        private FontHinting fontHinting;
        private int fontHeight;
        private int fontAscent;
        private int fontDescent;
        private int fontLineSkip;
        private int fontKerning;
        private string familyName;
        private string styleName;
        private bool disposedValue;

        internal SDLFont(IntPtr handle, int ySize)
            : this(handle, ySize, IntPtr.Zero)
        {

        }
        internal SDLFont(IntPtr handle, int ySize, IntPtr mem)
            : base(handle, BuildFontName(handle, ySize))
        {
            fontId = ++nextFontId;
            this.mem = mem;
            this.ySize = ySize;
            fontStyle = (FontStyle)TTF_GetFontStyle(this.handle);
            fontOutline = TTF_GetFontOutline(this.handle);
            fontHinting = (FontHinting)TTF_GetFontHinting(this.handle);
            fontHeight = TTF_FontHeight(this.handle);
            fontAscent = TTF_FontAscent(this.handle);
            fontDescent = TTF_FontDescent(this.handle);
            fontLineSkip = TTF_FontLineSkip(this.handle);
            fontKerning = TTF_GetFontKerning(this.handle);
            familyName = SDL.IntPtr2String(TTF_FontFaceFamilyName(this.handle)) ?? "unknown";
            styleName = SDL.IntPtr2String(TTF_FontFaceStyleName(this.handle)) ?? "regular";
            fontTracker.Track(this);
        }

        public static string BuildFontName(IntPtr fontHandle, int ySize)
        {
            string familyName = SDL.IntPtr2String(TTF_FontFaceFamilyName(fontHandle)) ?? "unknown";
            string styleName = SDL.IntPtr2String(TTF_FontFaceStyleName(fontHandle)) ?? "regular";
            return familyName + "-" + styleName + "-" + ySize;
        }
        public int FontId => fontId;
        public int YSize => ySize;
        public string FamilyName => familyName;
        public string StyleName => styleName;
        public FontStyle FontStyle => fontStyle;
        public int Outline => fontOutline;
        public FontHinting Hinting => fontHinting;
        public int Height => fontHeight;
        public int Ascent => fontAscent;
        public int Descent => fontDescent;
        public int LineSkip => fontLineSkip;
        public int Kerning => fontKerning;

        public Size MeasureText(ReadOnlySpan<char> text)
        {
            int w = 0;
            int h = 0;
            if (text != null && text.Length > 0)
            {
                stringBuffer.Clear();
                stringBuffer.Append(text);
                _ = TTF_SizeUTF8(handle, stringBuffer, out w, out h);
            }
            return new Size(w, h);
        }

        public void GetGlyphMetrics(char c, out int minx, out int maxx, out int miny, out int maxy, out int advance)
        {
            _ = TTF_GlyphMetrics32(handle, (uint)c, out minx, out maxx, out miny, out maxy, out advance);
        }

        public static void Initialize()
        {
            _ = TTF_Init();
        }

        public static void Shutdown()
        {
            fontTracker.Dispose();
            TTF_Quit();
        }
        public static SDLFont? LoadFont(string fileName, int ptSize)
        {
            SDLFont? font = null;
            if (!string.IsNullOrEmpty(fileName))
            {
                IntPtr fnt = TTF_OpenFont(fileName, ptSize);
                if (fnt != IntPtr.Zero)
                {
                    font = new SDLFont(fnt, ptSize);
                    SDLLog.Verbose(LogCategory.APPLICATION, $"Font {font.Name} loaded from file '{fileName}'");
                }
            }
            return font;
        }

        public static SDLFont? LoadFont(byte[] data, string name, int ptSize)
        {
            SDLFont? font = null;
            if (data != null)
            {
                int size = data.Length;
                IntPtr mem = Marshal.AllocHGlobal(size);
                Marshal.Copy(data, 0, mem, size);
                IntPtr rw = SDL.SDL_RWFromMem(mem, size);
                IntPtr handle = TTF_OpenFontRW(rw, 1, ptSize);
                if (handle != IntPtr.Zero)
                {
                    font = new SDLFont(handle, ptSize, mem);
                    SDLLog.Verbose(LogCategory.APPLICATION, $"Font {font.Name} loaded from resource '{name}'");
                }
            }
            return font;
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    fontTracker.Untrack(this);
                }
                if (handle != IntPtr.Zero)
                {
                    TTF_CloseFont(handle);
                }
                if (mem != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(mem);
                }
                disposedValue = true;
            }
            base.Dispose(disposing);
        }

    }
}
