namespace SDLSharp.Actors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Enemy : Actor
    {
        public Enemy(string name)
            : base(name)
        {
            IsEnemy = true;
        }
    }
}
