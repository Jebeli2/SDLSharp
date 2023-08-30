namespace SDLSharp.Maps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum MapProjection
    {
        Isometric,
        Orthogonal
    }
    public enum LayerType
    {
        Unknown,
        Object,
        Background,
        Collision
    }

    public enum MovementType
    {
        Normal,
        Flying,
        Intangible
    }

    public enum CollisionType
    {
        Normal,
        Player,
        Entity
    }
}
