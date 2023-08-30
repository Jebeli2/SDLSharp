namespace SDLSharp.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public interface IResLoader
    {
        string Name { get; }
        IContentManager? ContentManager { get; set; }
    }
    public interface IResourceLoader<T> : IResLoader where T : IResource
    {
        T? Load(string name, byte[]? data);
    }
}
