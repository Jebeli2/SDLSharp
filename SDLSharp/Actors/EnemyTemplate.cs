namespace SDLSharp.Actors
{
    using SDLSharp.Content;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class EnemyTemplate : Resource
    {
        public EnemyTemplate(string name)
            : base(name, ContentFlags.Data)
        {
            DisplayName = name;
            Rarity = "common";
            Categories = new List<string>();
            AnimationParts = new Dictionary<string, string>();
            LayerOrder = new Dictionary<int, IList<string>>();
            LifeForm = true;
        }
        
        public string DisplayName { get; set; }
        //public string File { get; set; }
        public IList<string> Categories { get; set; }
        public string Rarity { get; set; }
        public int Level { get; set; }
        public IDictionary<string, string> AnimationParts { get; set; }
        public IDictionary<int, IList<string>> LayerOrder { get; set; }
        public bool LifeForm { get; set; }
    }
}
