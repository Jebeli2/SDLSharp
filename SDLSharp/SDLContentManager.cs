namespace SDLSharp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Resources;
    using SDLSharp.Content;

    public class SDLContentManager : IContentManager
    {
        private readonly SDLWindow window;
        private bool allowFromFileSystem = true;
        private string saveDirectory;
        private readonly List<ResourceManager> resourceManagers = new();
        private readonly List<Content.Flare.ModManager> modManagers = new();
        private readonly List<string> knownNames = new();
        private readonly List<IResLoader> loaders = new();


        internal SDLContentManager(SDLWindow window)
        {
            this.window = window;
            saveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), SDLApplication.AppName);
            AddResourceManager(Properties.Resources.ResourceManager);
            RegisterResourceLoader(new ImageLoader(this.window));
            RegisterResourceLoader(new MusicLoader());
            RegisterResourceLoader(new SoundLoader());

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

        public void AddModPath(string path)
        {
            var modManager = new Content.Flare.ModManager(path);
            if (modManager.ModCount > 0)
            {
                modManagers.Add(modManager);
            }
        }

        public void RegisterResourceLoader<T>(IResourceLoader<T> loader) where T : IResource
        {
            loaders.Add(loader);
        }

        public T? Load<T>(string name) where T : IResource
        {
            byte[]? data = FindContent(name);
            foreach (var loader in GetResourceLoaders<T>())
            {
                loader.ContentManager = this;
                T? result = loader.Load(name, data);
                if (result != null) return result;
            }
            if (data == null)
            {
                SDLLog.Error(LogCategory.APPLICATION, $"No resource found for '{name}'");
            }
            else
            {
                SDLLog.Error(LogCategory.APPLICATION, $"No loader found for '{name}'");
            }
            return default;
        }

        private IEnumerable<IResourceLoader<T>> GetResourceLoaders<T>() where T : IResource
        {
            foreach (var loader in loaders)
            {
                if (loader is IResourceLoader<T> resourceLoader) { yield return resourceLoader; }
            }
        }
        public byte[]? FindContent(string? name)
        {
            byte[]? data = null;
            if (!string.IsNullOrEmpty(name))
            {
                if (data == null && allowFromFileSystem) { data = FindInFileSystem(name); }
                if (data == null) { data = FindInResManagers(name); }
                if (data == null) { data = FindInModManagers(name); }
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

        private byte[]? FindInModManagers(string name)
        {
            foreach (var modManager in modManagers)
            {
                string? fileName = modManager.Locate(name);
                if (!string.IsNullOrEmpty(fileName))
                {
                    return FindInFileSystem(fileName);
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
