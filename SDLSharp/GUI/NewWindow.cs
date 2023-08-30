using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.GUI
{
    public class NewWindow
    {
        public int LeftEdge { get; set; }
        public int TopEdge { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string? Title { get; set; }
        public Screen? Screen { get; set; }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public int MaxWidth { get; set; }
        public int MaxHeight { get; set; }
        public IList<Gadget>? Gadgets { get; set; }
        public bool Activate { get; set; }
        public bool SuperBitmap { get; set; }
        public bool Borderless { get; set; }
        public bool Sizing { get; set; }
        public bool Dragging { get; set; }
        public bool Closing { get; set; }
        public bool Maximizing { get; set; }
        public bool Minimizing { get; set; }
        public bool BackDrop { get; set; }
        public SDLFont? Font { get; set; }
        public Action? CloseAction { get; set; }
    }
}
