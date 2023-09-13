namespace SDLSharp.Content.Flare
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ModManager
    {
        private readonly Dictionary<string, string> locCache = new();
        private readonly List<Mod> modList;
        public ModManager(string path)
        {
            var modDirs = GetModDirs(path);
            modList = GetModList(path, modDirs);
        }

        public int ModCount => modList.Count;

        public string? Locate(string fileName)
        {
            fileName = CleanFileName(fileName);
            if (string.IsNullOrEmpty(fileName)) return null;
            if (locCache.TryGetValue(fileName, out string? fn))
            {
                return fn;
            }
            string testPath;
            foreach (var mod in modList)
            {
                testPath = mod.GetFileName(fileName);
                testPath = Path.GetFullPath(testPath);
                if (File.Exists(testPath))
                {
                    locCache[fileName] = testPath;
                    return testPath;
                }
            }
            return null;
        }

        public IList<string> List(string path, bool fullPaths = true)
        {
            List<string> list = new();
            string testPath;
            foreach (var mod in modList)
            {
                testPath = mod.GetFileName(path);
                if (File.Exists(testPath))
                {
                    list.Add(testPath);
                }
                else if (Directory.Exists(testPath))
                {
                    GetFileList(testPath, "txt", list);
                }
            }
            if (list.Count > 0 && !fullPaths)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = list[i].Substring(list[i].IndexOf(path));
                }
                for (int i = 0; i < list.Count; i++)
                {
                    int j = i + 1;
                    while (j < list.Count)
                    {
                        if (list[i].Equals(list[j]))
                        {
                            list.RemoveAt(j);
                        }
                        else
                        {
                            j++;
                        }
                    }
                }
            }
            return list;
        }
        private static List<Mod> GetModList(string path, IList<string> modDirs)
        {
            List<Mod> list = new();
            string place1 = Path.Combine(path, "mods.txt");
            string place2 = Path.Combine(path, "mods", "mods.txt");
            string place = "";
            if (File.Exists(place1)) { place = place1; }
            else if (File.Exists(place2)) { place = place2; }
            if (!string.IsNullOrEmpty(place))
            {
                using var infile = File.OpenText(place);
                string? line = "";
                while (line != null)
                {
                    line = infile.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;
                    if (line.StartsWith("#")) continue;
                    string? dir = modDirs.FirstOrDefault(x => x.EndsWith(line));
                    if (dir != null)
                    {
                        Mod? mod = LoadMod(line, dir);
                        if (mod != null)
                        {
                            list.Add(mod);
                        }
                    }
                }
            }
            return list;
        }

        private static void GetFileList(string dir, string ext, IList<string> files)
        {
            if (Directory.Exists(dir))
            {
                foreach (var d in Directory.GetFiles(dir, "*." + ext, SearchOption.TopDirectoryOnly))
                {
                    string file = Path.GetFileName(d);
                    files.Add(dir + "/" + file);
                }
            }
        }

        private static Mod? LoadMod(string name, string dir)
        {
            string settingsFile = Path.Combine(dir, "settings.txt");
            if (File.Exists(settingsFile))
            {
                return new Mod(name, dir);
            }
            return null;
        }
        private static List<string> GetModDirs(string path)
        {
            var list = new List<string>();
            string modDir = Path.Combine(path, "mods");
            if (Directory.Exists(modDir))
            {
                foreach (string dir in Directory.GetDirectories(modDir))
                {
                    list.Add(dir);
                }
            }
            return list;
        }

        private static string CleanFileName(string filename)
        {
            if (filename == null) return "";
            if (filename == "") return "";
            return filename.Replace("\\", "/");
        }
    }
}
