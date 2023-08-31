namespace SDLSharp.Content.Flare
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Mod
    {
        private readonly string name;
        private readonly string path;
        public Mod(string name, string path)
        {
            this.name = name;
            this.path = path;
        }
        public string Name => name;
        public string Path => path;
        public string GetFileName(string fileName)
        {
            if (fileName.StartsWith("\\")) { fileName = fileName.Substring(1); }
            else if (fileName.StartsWith("/")) { fileName = fileName.Substring(1); }
            return System.IO.Path.Combine(path, fileName);
        }

        public override string ToString()
        {
            return $"{name}: {path}";
        }
    }
}
