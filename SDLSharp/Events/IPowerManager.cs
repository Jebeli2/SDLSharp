namespace SDLSharp.Events
{
    using SDLSharp.Actors;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IPowerManager
    {
        Power? GetPower(int index);
        bool Activate(Power power, Actor source, PointF target);
        bool Activate(Power power, Event evt);
    }
}
