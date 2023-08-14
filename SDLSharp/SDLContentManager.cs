namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Resources;
    public class SDLContentManager
    {
        private readonly SDLWindow window;
        private bool allowFromFileSystem = true;
        private string saveDirectory;
        private readonly List<ResourceManager> resourceManagers = new();
        private readonly List<string> knownNames = new();


        internal SDLContentManager(SDLWindow window)
        {
            this.window = window;
            saveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SDLApplication.AppName);

        }
        public string SaveDirectory => saveDirectory;

        public void AddResourceManager(ResourceManager resourceManager)
        {
            if (!resourceManagers.Contains(resourceManager))
            {
                resourceManagers.Add(resourceManager);
                knownNames.AddRange(ListResources(resourceManager));
            }
        }


        public byte[]? FindContent(string? name)
        {
            byte[]? data = null;
            if (!string.IsNullOrEmpty(name))
            {
                if (data == null && allowFromFileSystem) { data = FindInFileSystem(name); }
                if (data == null) { data = FindInResManagers(name); }
                if (data == null) { SDLLog.Warn(LogCategory.APPLICATION, $"Could not find resource '{name}'"); }
            }
            return data;
        }

        private byte[]? FindInResManagers(string name)
        {
            name = FindResName(name);
            foreach (ResourceManager rm in resourceManagers)
            {
                object? obj = rm.GetObject(name);
                if (obj != null)
                {
                    if (obj is byte[] data) { return data; }
                    else if (obj is string str)
                    {
                        return Encoding.UTF8.GetBytes(str);
                    }
                    else if (obj is UnmanagedMemoryStream ums1)
                    {
                        byte[] umsData = new byte[ums1.Length];
                        ums1.Read(umsData, 0, umsData.Length);
                        return umsData;
                    }
                }
                UnmanagedMemoryStream? ums = rm.GetStream(name);
                if (ums != null)
                {
                    byte[] umsData = new byte[ums.Length];
                    ums.Read(umsData, 0, umsData.Length);
                    return umsData;
                }
            }
            return null;
        }

        private static byte[]? FindInFileSystem(string name)
        {
            try
            {
                if (File.Exists(name))
                {
                    return File.ReadAllBytes(name);
                }
            }
            catch (IOException ioe)
            {
                SDLLog.Warn(LogCategory.SYSTEM, $"IOException during file read for '{name}': {ioe.Message}");
            }
            return null;
        }

        private string FindResName(string name)
        {
            if (knownNames.Contains(name)) return name;
            string testName = name.Replace('_', '-');
            if (knownNames.Contains(testName)) return testName;
            testName = name.Replace('_', ' ').Trim();
            if (knownNames.Contains(testName)) return testName;
            return name;
        }

        private static IEnumerable<string> ListResources(ResourceManager rm)
        {
            ResourceSet? rs = rm.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, false);
            if (rs != null)
            {
                foreach (System.Collections.DictionaryEntry e in rs)
                {
                    string? s = e.Key?.ToString();
                    if (!string.IsNullOrEmpty(s))
                    {
                        yield return s;
                    }
                }
            }
        }

    }
}
