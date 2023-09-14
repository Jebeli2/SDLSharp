namespace SDLSharp.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PostEffect
    {
        public string Id { get; set; }
        public int Magnitude { get; set; }
        public int Duration { get; set; }
        public int Chance { get; set; }
        public bool TargetSrc { get; set; }

        public PostEffect()
        {
            Id = "";
            Magnitude = 0;
            Duration = 0;
            Chance = 100;
            TargetSrc = false;
        }
    }
}
