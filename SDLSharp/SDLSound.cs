namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SDLSound : SDLObject
    {
        public SDLSound(IntPtr handle, string name)
            : base(handle, name, Content.ContentFlags.Sound)
        {
            SDLAudio.Track(this);
        }

        protected override void DisposeUnmanaged()
        {
            SDLAudio.Untrack(this);
            if (handle != IntPtr.Zero)
            {
                SDL_mixer.Mix_FreeChunk(handle);
            }
        }

    }
}
