namespace SDLSharp.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SoundLoader : IResourceLoader<SDLSound>
    {
        public string Name => "Sound Loader";
        public IContentManager? ContentManager { get; set; }

        public SDLSound? Load(string name, byte[]? data)
        {
            if (data != null)
            {
                return SDLAudio.LoadSound(name, data);
            }
            else
            {
                return SDLAudio.LoadSound(name);
            }
        }
    }
}
