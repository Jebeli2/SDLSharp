namespace SDLSharp.Content.Flare
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ModLoader
    {
        private readonly string name;
        public ModLoader(string name)
        {
            this.name = name;
        }

        public string Name => name;
        public IContentManager? ContentManager { get; set; }


        internal static void UnknownKey<T>(string name, FileParser infile) where T : IResource
        {
            SDLLog.Warn(LogCategory.APPLICATION, $"Unknown entry in {name} loading {typeof(T).Name}: {infile.Section}-{infile.Key} = {infile.Val}");
        }
    }
}
