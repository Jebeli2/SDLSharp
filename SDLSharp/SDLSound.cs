namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SDLSound : SDLObject
    {
        private bool disposedValue;

        public SDLSound(IntPtr handle, string name)
            : base(handle, name)
        {
            SDLAudio.Track(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SDLAudio.Untrack(this);
                }
                if (handle != IntPtr.Zero)
                {
                    SDL_mixer.Mix_FreeChunk(handle);
                }
                disposedValue = true;
            }
            base.Dispose(disposing);
        }

    }
}
