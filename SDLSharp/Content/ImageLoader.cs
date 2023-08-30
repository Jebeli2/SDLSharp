namespace SDLSharp.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ImageLoader : IResourceLoader<SDLTexture>
    {
        private readonly SDLWindow window;
        public ImageLoader(SDLWindow window)
        {
            this.window = window;
        }
        public string Name => "Window Image Loader";

        public IContentManager? ContentManager
        {
            get { return window.ContentManager; }
            set { }
        }
        public SDLTexture? Load(string name, byte[]? data)
        {
            if (data != null)
            {
                return window.Renderer.LoadTexture(name, data);
            }
            else
            {
                return window.Renderer.LoadTexture(name);
            }
        }
    }
}
