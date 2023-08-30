namespace SDLSharp.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MusicLoader : IResourceLoader<SDLMusic>
    {
        public string Name => "Music Loader";
        public IContentManager? ContentManager {  get; set; }   
        public SDLMusic? Load(string name, byte[]? data)
        {
            if (data != null)
            {
                return SDLAudio.LoadMusic(name, data);
            }
            else
            {
                return SDLAudio.LoadMusic(name);
            }
        }
    }
}
