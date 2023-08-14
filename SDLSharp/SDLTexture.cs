namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SDLTexture : SDLObject
    {
        private readonly SDLRenderer renderer;
        private readonly int width;
        private readonly int height;
        private readonly uint format;
        private readonly int access;

        private TextureFilter textureFilter;
        private byte alphaMod;
        private Color colorMod;
        private BlendMode blendMode;
        private bool disposedValue;

        internal SDLTexture(SDLRenderer renderer, IntPtr handle, string name)
            : base(handle, name)
        {
            this.renderer = renderer;
            _ = SDL.SDL_QueryTexture(this.handle, out format, out access, out width, out height);
            //_=SDL.SDL_GetTextureScaleMode(this.handle, out textureFilter);
            _ = SDL.SDL_GetTextureScaleMode(this.handle, out var tf);
            textureFilter = (TextureFilter)tf;
            _ = SDL.SDL_GetTextureAlphaMod(this.handle, out alphaMod);
            _ = SDL.SDL_GetTextureColorMod(this.handle, out byte r, out byte g, out byte b);
            colorMod = Color.FromArgb(r, g, b);
            _ = SDL.SDL_GetTextureBlendMode(this.handle, out var bm);
            blendMode = (BlendMode)bm;
            this.renderer.Track(this);
        }

        public static void Initialize()
        {
            _ = SDL_image.IMG_Init(SDL_image.IMG_InitFlags.IMG_INIT_PNG | SDL_image.IMG_InitFlags.IMG_INIT_JPG);
        }

        public static void Shutdown()
        {
            SDL_image.IMG_Quit();
        }

        public int Width => width;
        public int Height => height;

        public TextureFilter TextureFilter
        {
            get => textureFilter;
            set
            {
                if (textureFilter != value)
                {
                    textureFilter = value;
                    _ = SDL.SDL_SetTextureScaleMode(handle, (SDL.SDL_ScaleMode)value);
                }
            }
        }

        public byte AlphaMod
        {
            get => alphaMod;
            set
            {
                if (alphaMod != value)
                {
                    alphaMod = value;
                    _ = SDL.SDL_SetTextureAlphaMod(handle, alphaMod);
                }
            }
        }

        public Color ColorMod
        {
            get => colorMod;
            set
            {
                if (colorMod != value)
                {
                    colorMod = value;
                    _ = SDL.SDL_SetTextureColorMod(handle, colorMod.R, colorMod.G, colorMod.B);
                }
            }
        }

        public BlendMode BlendMode
        {
            get => blendMode;
            set
            {
                if (blendMode != value)
                {
                    blendMode = value;
                    _ = SDL.SDL_SetTextureBlendMode(handle, (SDL.SDL_BlendMode)blendMode);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    renderer.Untrack(this);
                }
                if (handle != IntPtr.Zero)
                {
                    SDL.SDL_DestroyTexture(handle);
                }
                disposedValue = true;
            }
            base.Dispose(disposing);
        }

    }
}
