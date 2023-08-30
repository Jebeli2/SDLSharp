namespace SDLSharp.Applets
{
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MapApplet : SDLApplet
    {
        private Map? map;
        private IMapRenderer mapRenderer;
        private bool panning;
        private int panDX;
        private int panDY;

        public MapApplet()
            : base("Map")
        {
            mapRenderer = new FlareMapRenderer();
            MousePanning = true;
        }
        public bool MousePanning { get; set; }
        public Map? Map
        {
            get => map;
            set
            {
                if (map != value)
                {
                    map?.Dispose();
                    map = value;
                    if (map != null)
                    {
                        mapRenderer.PrepareMap(map);
                        var music =LoadMusic(map.Music);
                        if (music != null)
                        {
                            SDLAudio.PlayMusic(music);
                        }
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            map?.Dispose();
            map = null;
        }

        protected override void OnWindowUpdate(SDLWindowUpdateEventArgs e)
        {
            if (map != null)
            {
                mapRenderer.Update(e.TotalTime, e.ElapsedTime, map);
                if (MousePanning && panning && (panDX != 0 || panDY != 0))
                {
                    Pan(panDX, panDY);
                    panDX = 0;
                    panDY = 0;
                }
            }
        }

        protected override void OnWindowPaint(SDLWindowPaintEventArgs e)
        {
            if (map != null)
            {
                if (map.BackgroundColor != Color.Black)
                {
                    e.Renderer.ClearScreen(map.BackgroundColor);
                }
                List<IMapSprite> front = new();
                List<IMapSprite> back = new();
                mapRenderer.Render(e.Renderer, e.TotalTime, e.ElapsedTime, map, front, back);
            }
        }

        protected internal override void OnMouseButtonDown(SDLMouseEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                panning = true;
            }
        }

        protected internal override void OnMouseButtonUp(SDLMouseEventArgs e)
        {
            if (e.Button == MouseButton.Right)
            {
                panning = false;
            }
        }

        protected internal override void OnMouseMove(SDLMouseEventArgs e)
        {
            if (panning)
            {
                panDX = -e.RelX;
                panDY = -e.RelY;
            }
        }

        private void Pan(int dx, int dy)
        {
            mapRenderer.ShiftCam(dx, dy);
        }
    }
}
