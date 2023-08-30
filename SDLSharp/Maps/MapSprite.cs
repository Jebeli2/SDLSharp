namespace SDLSharp.Maps
{
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MapSprite : Sprite, IMapSprite
    {
        private float mapPosX;
        private float mapPosY;
        private long basePrio;
        private long prio;

        public MapSprite(ISprite sprite)
            : base(sprite)
        {

        }

        public float MapPosX
        {
            get => mapPosX;
            set => mapPosX = value;
        }

        public float MapPosY
        {
            get => mapPosY;
            set => mapPosY = value;
        }

        public long BasePrio
        {
            get => basePrio;
            set => basePrio = value;
        }
        public long Prio
        {
            get => prio;
            set => prio = value;
        }

        public int CompareTo(MapSprite other)
        {
            return prio.CompareTo(other.prio);
        }

        public int CompareTo(IMapSprite? other)
        {
            if (other == null) return 1;
            if (other is MapSprite mpss) { return CompareTo(mpss); }
            return -1;
        }
    }
}
