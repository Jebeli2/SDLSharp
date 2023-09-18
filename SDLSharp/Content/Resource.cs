namespace SDLSharp.Content
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Resource : IResource, IDisposable
    {
        private string name;
        private ContentFlags flags;
        private bool disposedValue;

        public Resource(string name, ContentFlags flags)
        {
            this.name = name;
            this.flags = flags;
        }

        public Resource(Resource other)
        {
            name = other.name;
            flags = other.flags;
        }

        public string Name
        {
            get => name;
            internal set => name = value;
        }

        public ContentFlags Flags
        {
            get => flags;
            internal set => flags = value;
        }
        public bool Disposed => disposedValue;
        public override string ToString()
        {
            return "'" + name + "'";
        }

        protected virtual void DisposeUnmanaged()
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                DisposeUnmanaged();
                disposedValue = true;
            }
        }

        ~Resource()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
