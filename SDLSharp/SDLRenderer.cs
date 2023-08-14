namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using static SDL;

    public sealed class SDLRenderer : IDisposable
    {
        private readonly SDLWindow window;
        private readonly StringBuilder stringBuffer = new(512);
        private IntPtr handle;
        private int windowWidth;
        private int windowHeight;
        private int width;
        private int height;
        private BlendMode blendMode;
        private TextureFilter textureFilter = TextureFilter.Nearest;
        private Color color;
        private byte colorR;
        private byte colorG;
        private byte colorB;
        private byte colorA;
        private readonly uint format;
        private readonly Dictionary<int, TextCache> textCache = new();
        private readonly List<int> textCacheKeys = new();
        private int textCacheLimit = 100;
        private readonly SDLObjectTracker<SDLTexture> textureTracker = new(LogCategory.RENDER, "Texture");

        internal SDLRenderer(SDLWindow window)
        {
            format = SDL_PIXELFORMAT_ARGB8888;
            this.window = window;
            windowWidth = window.Width;
            windowHeight = window.Height;
            width = window.Width;
            height = window.Height;
        }

        public void Dispose()
        {
            if (handle != IntPtr.Zero)
            {
                ClearTextCache();
                //ClearIconCache();
                //if (backBuffer != IntPtr.Zero)
                //{
                //    SDL_DestroyTexture(backBuffer);
                //    backBuffer = IntPtr.Zero;
                //}
                textureTracker.Dispose();
                SDL_DestroyRenderer(handle);
                handle = IntPtr.Zero;
                SDLLog.Info(LogCategory.RENDER, $"Renderer {window.WindowId} destroyed");
            }
        }

        public IntPtr Handle => handle;
        public bool HandleCreated => handle != IntPtr.Zero;
        public int Width => width;
        public int Height => height;
        public Color Color
        {
            get => color;
            set => SetColor(value);
        }
        public BlendMode BlendMode
        {
            get => blendMode;
            set => SetBlendMode(value);
        }

        public SDLTexture? LoadTexture(string fileName)
        {
            SDLTexture? texture = textureTracker.Find(fileName);
            if (texture == null && !string.IsNullOrEmpty(fileName))
            {
                _ = SDLApplication.SetHint(SDL_HINT_RENDER_SCALE_QUALITY, (int)textureFilter);
                IntPtr tex = SDL_image.IMG_LoadTexture(handle, fileName);
                if (tex != IntPtr.Zero)
                {
                    texture = new SDLTexture(this, tex, fileName);
                    SDLLog.Verbose(LogCategory.RENDER, $"Texture loaded from file '{fileName}'");
                }
            }
            return texture;
        }

        public SDLTexture? LoadTexture(string name, byte[]? data)
        {
            SDLTexture? texture = textureTracker.Find(name);
            if (texture == null && data != null)
            {
                string format = DetectImgFormat(data);
                IntPtr rw = SDL_RWFromMem(data, data.Length);
                if (rw != IntPtr.Zero)
                {
                    _ = SDLApplication.SetHint(SDL_HINT_RENDER_SCALE_QUALITY, (int)textureFilter);
                    IntPtr tex;
                    if (!string.IsNullOrEmpty(format))
                    {
                        tex = SDL_image.IMG_LoadTextureTyped_RW(handle, rw, 1, format);
                    }
                    else
                    {
                        tex = SDL_image.IMG_LoadTexture_RW(handle, rw, 1);
                    }
                    if (tex != IntPtr.Zero)
                    {
                        texture = new SDLTexture(this, tex, name);
                        SDLLog.Verbose(LogCategory.RENDER, $"Texture loaded from resource '{name}' (format = {format})");
                    }
                }
            }
            return texture;
        }

        private static byte[] pngSig = new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static byte[] jpgSig = new byte[] { 0xFF, 0xD8, 0xFF };
        private static bool Matches(byte[] data, byte[] sig)
        {
            if (data.Length >= sig.Length)
            {
                for (int i = 0; i < sig.Length; i++)
                {
                    if (data[i] != sig[i]) { return false; }
                }
                return true;
            }
            return false;
        }

        private static string DetectImgFormat(byte[] data)
        {
            if (Matches(data, pngSig)) return "PNG";
            else if (Matches(data, jpgSig)) return "JPG";
            return string.Empty;
        }

        public SDLTexture? CreateTexture(string name, int width, int height)
        {
            SDLTexture? texture = textureTracker.Find(name);
            if (texture == null)
            {
                IntPtr tex = SDL_CreateTexture(handle, format, (int)SDL_TextureAccess.SDL_TEXTUREACCESS_TARGET, width, height);
                if (tex != IntPtr.Zero)
                {
                    texture = new SDLTexture(this, tex, name);
                    SDLLog.Verbose(LogCategory.RENDER, $"Texture created from scratch '{name}'");
                }
            }
            return texture;
        }

        public void DrawTexture(SDLTexture? texture)
        {
            if (texture != null)
            {
                _ = SDL_RenderCopy(handle, texture.Handle, IntPtr.Zero, IntPtr.Zero);
            }
        }

        public void DrawTexture(SDLTexture? texture, Rectangle dst)
        {
            if (texture != null)
            {
                _ = SDL_RenderCopy(handle, texture.Handle, IntPtr.Zero, ref dst);
            }
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            _ = SDL_RenderDrawLine(handle, x1, y1, x2, y2);
        }

        public void SetColor(Color value)
        {
            if (color != value)
            {
                color = value;
                colorR = value.R;
                colorG = value.G;
                colorB = value.B;
                colorA = value.A;
                _ = SDL_SetRenderDrawColor(handle, colorR, colorG, colorB, colorA);
            }
        }

        public void SetBlendMode(BlendMode value)
        {
            if (blendMode != value)
            {
                blendMode = value;
                _ = SDL_SetRenderDrawBlendMode(handle, (SDL_BlendMode)blendMode);
            }
        }

        public void DrawText(SDLFont? font, ReadOnlySpan<char> text, float x, float y, float width, float height, Color color, HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center, VerticalAlignment verticalAlignment = VerticalAlignment.Center, float offsetX = 0, float offsetY = 0)
        {
            if (text == null) return;
            if (text.Length == 0) return;
            font ??= SDLApplication.DefaultFont;
            if (font == null) return;
            DrawTextCache(GetTextCache(font, text, color), x, y, width, height, horizontalAlignment, verticalAlignment, offsetX, offsetY);
            //DirectDrawText(font, text, x, y, width, height, color, horizontalAlignment, verticalAlignment, offsetX, offsetY);
        }

        private TextCache? GetTextCache(SDLFont font, ReadOnlySpan<char> text, Color color)
        {
            int hash = string.GetHashCode(text);
            int key = MakeTextCacheKey(font, hash, color);
            CheckTextCache();
            if (textCache.TryGetValue(key, out var tc))
            {
                if (tc.Matches(font, hash, color)) return tc;
            }
            tc = CreateTextCache(font, text, color);
            if (tc != null)
            {
                textCache[key] = tc;
                textCacheKeys.Add(key);
            }
            return tc;
        }

        private TextCache? CreateTextCache(SDLFont? font, ReadOnlySpan<char> text, Color color)
        {
            TextCache? textCache = null;
            if (font != null && text.Length > 0)
            {
                IntPtr fontHandle = font.Handle;
                if (fontHandle != IntPtr.Zero)
                {
                    stringBuffer.Clear();
                    stringBuffer.Append(text);
                    int hash = string.GetHashCode(text);
                    IntPtr surface = SDL_ttf.TTF_RenderUTF8_Blended(fontHandle, stringBuffer, color.ToArgb());
                    if (surface != IntPtr.Zero)
                    {
                        IntPtr texHandle = SDL_CreateTextureFromSurface(handle, surface);
                        if (texHandle != IntPtr.Zero)
                        {
                            _ = SDL_QueryTexture(texHandle, out _, out _, out int w, out int h);
                            _ = SDL_SetTextureAlphaMod(texHandle, color.A);
                            textCache = new TextCache(font, hash, color, w, h, texHandle);
                        }
                        SDL_FreeSurface(surface);
                    }
                }
            }
            return textCache;
        }

        private void DrawTextCache(TextCache? textCache, float x, float y, float width, float height, HorizontalAlignment hAlign, VerticalAlignment vAlign, float offsetX, float offsetY)
        {
            if (textCache == null) return;
            int w = textCache.Width;
            int h = textCache.Height;
            switch (hAlign)
            {
                case HorizontalAlignment.Left:
                    //nop
                    break;
                case HorizontalAlignment.Right:
                    x = x + width - w;
                    break;
                case HorizontalAlignment.Center:
                    x = x + width / 2 - w / 2;
                    break;
            }
            switch (vAlign)
            {
                case VerticalAlignment.Top:
                    // nop
                    break;
                case VerticalAlignment.Bottom:
                    y = y + height - h;
                    break;
                case VerticalAlignment.Center:
                    y = y + height / 2 - h / 2;
                    break;
            }
            RectangleF dstRect = new RectangleF(x + offsetX, y + offsetY, w, h);
            SetBlendMode(BlendMode.Blend);
            _ = SDL_RenderCopyF(handle, textCache.Texture, IntPtr.Zero, ref dstRect);
        }

        private void DirectDrawText(SDLFont font, ReadOnlySpan<char> text, float x, float y, float width, float height, Color color, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, float offsetX, float offsetY)
        {
            IntPtr fontHandle = font.Handle;
            if (fontHandle != IntPtr.Zero)
            {
                stringBuffer.Clear();
                stringBuffer.Append(text);
                IntPtr surface = SDL_ttf.TTF_RenderUTF8_Blended(fontHandle, stringBuffer, color.ToArgb());
                if (surface != IntPtr.Zero)
                {
                    IntPtr texHandle = SDL_CreateTextureFromSurface(handle, surface);
                    if (texHandle != IntPtr.Zero)
                    {
                        SDL_QueryTexture(texHandle, out _, out _, out int w, out int h);
                        SDL_SetTextureAlphaMod(texHandle, color.A);
                        switch (horizontalAlignment)
                        {
                            case HorizontalAlignment.Left:
                                //nop
                                break;
                            case HorizontalAlignment.Right:
                                x = x + width - w;
                                break;
                            case HorizontalAlignment.Center:
                                x = x + width / 2 - w / 2;
                                break;
                        }
                        switch (verticalAlignment)
                        {
                            case VerticalAlignment.Top:
                                // nop
                                break;
                            case VerticalAlignment.Bottom:
                                y = y + height - h;
                                break;
                            case VerticalAlignment.Center:
                                y = y + height / 2 - h / 2;
                                break;
                        }
                        RectangleF dstRect = new RectangleF(x + offsetX, y + offsetY, w, h);
                        SetBlendMode(BlendMode.Blend);
                        _ = SDL_RenderCopyF(handle, texHandle, IntPtr.Zero, ref dstRect);
                        SDL_DestroyTexture(texHandle);
                    }
                    SDL_FreeSurface(surface);
                }
            }
        }

        internal void CreateHandle()
        {
            int driverIndex = SDLApplication.GetDriverIndex(window.Driver);
            SDL_RendererFlags flags = SDL_RendererFlags.SDL_RENDERER_ACCELERATED;
            handle = SDL_CreateRenderer(window.Handle, driverIndex, flags);
            if (handle != IntPtr.Zero)
            {
                _ = SDL_GetRendererInfo(handle, out SDL_RendererInfo info);
                SDLLog.Info(LogCategory.RENDER, $"Renderer {window.WindowId} created: {Marshal.PtrToStringUTF8(info.name)} ({info.max_texture_width}x{info.max_texture_height} max texture size)");

                windowWidth = window.Width;
                windowHeight = window.Height;
            }
        }

        internal void BeginPaint()
        {
            SetColor(Color.Black);
            SetBlendMode(BlendMode.Blend);
            _ = SDL_RenderClear(handle);
        }

        internal void EndPaint()
        {
            SDL_RenderPresent(handle);
        }

        internal void Track(SDLTexture texture)
        {
            textureTracker.Track(texture);
        }

        internal void Untrack(SDLTexture texture)
        {
            textureTracker.Untrack(texture);
        }

        internal void WindowResized(int width, int height)
        {
            windowWidth = width;
            windowHeight = height;
            this.width = width;
            this.height = height;
        }

        private static int MakeTextCacheKey(SDLFont font, int textHash, Color color)
        {
            return HashCode.Combine(font.FontId, textHash, color.GetHashCode());
        }

        private void CheckTextCache()
        {
            if (textCache.Count >= textCacheLimit)
            {
                int len = textCacheKeys.Count / 2;
                var halfKeys = textCacheKeys.GetRange(0, len);
                textCacheKeys.RemoveRange(0, len);
                SDLLog.Verbose(LogCategory.RENDER, $"Text cache limit {textCacheLimit} reached. Cleaning up...");
                ClearTextCache(halfKeys);
            }
        }
        private void ClearTextCache()
        {
            foreach (var kvp in textCache)
            {
                TextCache tc = kvp.Value;
                SDL_DestroyTexture(tc.Texture);
            }
            textCache.Clear();
            textCacheKeys.Clear();
        }
        private void ClearTextCache(IEnumerable<int> keys)
        {
            foreach (var key in keys)
            {
                if (textCache.TryGetValue(key, out var tc))
                {
                    if (textCache.Remove(key))
                    {
                        SDL_DestroyTexture(tc.Texture);
                    }
                }
            }
        }

        private class TextCache
        {
            internal TextCache(SDLFont font, int textHash, Color color, int width, int height, IntPtr texture)
            {
                Font = font;
                TextHash = textHash;
                Color = color;
                Width = width;
                Height = height;
                Texture = texture;
            }
            public SDLFont Font;
            public int TextHash;
            public int Width;
            public int Height;
            public Color Color;
            public IntPtr Texture;

            public bool Matches(SDLFont font, int textHash, Color color)
            {
                return (textHash == TextHash) && (font == Font) && (color == Color);
            }

        }
    }
}
