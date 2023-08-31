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


        protected static void UnknownKey(string name, FileParser infile)
        {
            SDLLog.Warn(LogCategory.APPLICATION, $"Unknown entry in {name}: {infile.Section}-{infile.Key} = {infile.Val}"); 
        }
    }
}
