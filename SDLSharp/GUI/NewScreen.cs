using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class NewScreen
    {
        public int LeftEdge { get; set; }
        public int TopEdge { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public SDLFont? Font { get; set; }
        public string? DefaultTitle { get; set; }   

    }
}
