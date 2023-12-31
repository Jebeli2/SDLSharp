﻿namespace SDLSharp.Actors
{
    using SDLSharp.Events;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ActorCommand
    {
        public ActorAction Action { get; set; }
        public float MapDestX { get; set; }
        public float MapDestY { get; set; }
        public Actor? Enemy { get; set; }
        public Power? Power { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Action);
            switch (Action)
            {
                case ActorAction.Move:
                    sb.Append(" to (");
                    sb.Append(MapDestX);
                    sb.Append(",");
                    sb.Append(MapDestY);
                    sb.Append(")");
                    break;
                case ActorAction.Interact:
                    sb.Append(" at (");
                    sb.Append(MapDestX);
                    sb.Append(",");
                    sb.Append(MapDestY);
                    sb.Append(")");
                    break;
                case ActorAction.Attack:
                    if (Power != null)
                    {
                        sb.Append(" using ");
                        sb.Append(Power.Name);
                        if (Enemy != null)
                        {
                            sb.Append(" on ");
                            sb.Append(Enemy);
                        }
                    }
                    else
                    {
                        sb.Append(" ");
                        if (Enemy != null) { sb.Append(Enemy); }
                        else { sb.Append("nothing"); }
                    }
                    break;
            }
            return sb.ToString();
        }

    }

}
