namespace SDLTest
{
    using SDLSharp;
    using SDLSharp.Applets;
    using SDLSharp.Content.Flare;
    using SDLSharp.GUI;
    using SDLSharp.Maps;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Design;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class TestSDLWindow : SDLWindow
    {
        private static readonly string MODPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @"flare-engine-master\");
        private const string MAPNAME1 = "maps/frontier_outpost.txt";
        private const string MAPNAME2 = "maps/perdition_harbor.txt";
        private const string MAPNAME3 = "maps/lake_kuuma.txt";
        private const string MAPNAME4 = "maps/underworld.txt";
        private const string MAPNAME5 = "maps/frontier_plains.txt";
        private const string MAPNAME6 = "maps/hyperspace.txt";

        private const string DC = @"D:\Users\jebel\Music\iTunes\iTunes Media\Music\Alt-J\Relaxer\05 Deadcrush.mp3";

        private SDLFont? font;
        private Window? window1;
        private Window? window2;

        private Window? winButTest;
        private Window? winPropTest;
        private Window? winStrTest;
        private Requester? requester;

        private Gadget? button1_1;
        private Gadget? button1_2;
        private Gadget? button1_3;

        private Gadget? button2_1;
        private Gadget? button2_2;
        private Gadget? button2_3;
        private Gadget? button2_4;

        private MapApplet? map;
        public TestSDLWindow()
            : base("Test SDL")
        {
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ContentManager.AddResourceManager(Properties.Resources.ResourceManager);
            ContentManager.AddModPath(MODPATH);
            ContentManager.RegisterResourceLoader(new ModMapLoader());
            ContentManager.RegisterResourceLoader(new ModTileSetLoader());
            ContentManager.RegisterResourceLoader(new ModParallaxLoader());
            ContentManager.RegisterResourceLoader(new ModActorLoader());
            ContentManager.RegisterResourceLoader(new ModAnimationSetLoader());

            font = LoadFont(nameof(Properties.Resources.LiberationSans_Regular), 16);
            GetApplet<BackgroundImage>().Image = ContentManager.Load<SDLTexture>(nameof(Properties.Resources.badlands));
            GetApplet<MusicPlayer>().PlayNow(nameof(Properties.Resources.jesu_joy_of_mans_desiring));
            GetApplet<MusicPlayer>().AddToPlayList(DC, "Deathcrush");
            var icons = GetApplet<IconShow>();
            var boxes = GetApplet<RainingBoxesApp>();
            var music = GetApplet<MusicVisualizer>();
            var lines = GetApplet<LinesApp>();
            var gui = GetApplet<GUISystem>();
            icons.RenderPrio = 2000;
            boxes.RenderPrio = -500;
            music.RenderPrio = -600;
            lines.RenderPrio = -750;

            SDLApplication.MaxFramesPerSecond = 120;
            button1_1 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 10, width: -20, height: 40, text: "GUI Test", clickAction: GoToGUITest);
            button1_2 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 60, width: -20, height: 40, text: "Map", clickAction: GoToMap);
            button1_3 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 110, width: -20, height: 40, text: "Particles");

            button2_1 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 10, width: -20, height: 40, text: "Back", clickAction: GoToTitle);
            button2_2 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 60, width: -20, height: 40, text: "Buttons", clickAction: ShowButtonTest);
            button2_3 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 110, width: -20, height: 40, text: "Props", clickAction: ShowPropTest);
            button2_4 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 160, width: -20, height: 40, text: "Strings", clickAction: ShowStrTest);

            GoToTitle();
        }

        protected override void OnClose(EventArgs e)
        {
            base.OnClose(e);
            CloseAllWindows();
            font?.Dispose();
        }

        protected override void OnKeyUp(SDLKeyEventArgs e)
        {
            base.OnKeyUp(e);
            switch (e.ScanCode)
            {
                case ScanCode.SCANCODE_F2:
                    DisplayMode = DisplayMode.Windowed;
                    break;
                case ScanCode.SCANCODE_F3:
                    DisplayMode = DisplayMode.Desktop;
                    break;
                case ScanCode.SCANCODE_F4:
                    DisplayMode = DisplayMode.FullSize;
                    break;
                case ScanCode.SCANCODE_F5:
                    DisplayMode = DisplayMode.MultiMonitor;
                    break;
                case ScanCode.SCANCODE_1:
                    if (map != null)
                    {
                        map.MapName = MAPNAME1;
                    }
                    break;
                case ScanCode.SCANCODE_2:
                    if (map != null)
                    {
                        map.MapName = MAPNAME2;
                    }
                    break;
                case ScanCode.SCANCODE_3:
                    if (map != null)
                    {
                        map.MapName = MAPNAME3;
                    }
                    break;
                case ScanCode.SCANCODE_4:
                    if (map != null)
                    {
                        map.MapName = MAPNAME4;
                    }
                    break;
                case ScanCode.SCANCODE_5:
                    if (map != null)
                    {
                        map.MapName = MAPNAME5;
                    }
                    break;
                case ScanCode.SCANCODE_6:
                    if (map != null)
                    {
                        map.MapName = MAPNAME6;
                    }
                    break;
            }
        }
        private void GoToMap()
        {
            CloseAllWindows();
            ClearApplets();
            map = GetApplet<MapApplet>();
            map.MapName = MAPNAME1;
        }


        private void CloseAllWindows()
        {
            Intuition.CloseWindow(ref window1);
            Intuition.CloseWindow(ref window2);
            Intuition.CloseWindow(ref winButTest);
            Intuition.CloseWindow(ref winPropTest);
            Intuition.CloseWindow(ref winStrTest);
        }
        private void GoToTitle()
        {
            CloseAllWindows();
            window1 = Intuition.OpenWindow(new NewWindow
            {
                LeftEdge = 50,
                TopEdge = 50,
                Width = 300,
                Height = 300,
                Gadgets = new Gadget[] { button1_1!, button1_2!, button1_3! },
                Activate = true,
                SuperBitmap = true,
                Borderless = true,
                Sizing = false,
                Dragging = false,
                Closing = false,
                BackDrop = true,
                Font = font
            });


        }

        private void GoToGUITest()
        {
            CloseAllWindows();
            window2 = Intuition.OpenWindow(new NewWindow
            {
                LeftEdge = 66,
                TopEdge = 66,
                Width = 400,
                Height = 400,
                Title = "Test GUI",
                Gadgets = new Gadget[] { button2_1!, button2_2!, button2_3!, button2_4! },
                MinWidth = 200,
                MinHeight = 200,
                Activate = true,
                SuperBitmap = true,
                Sizing = true,
                Dragging = true,
                Closing = true,
                Font = font,
                CloseAction = GoToTitle
            });
        }

        private void ShowButtonTest()
        {
            if (winButTest == null)
            {
                requester = new Requester();
                requester.PointRel = true;
                requester.Width = 200;
                requester.Height = 200;

                List<Gadget> gadgets = new();
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Text, leftEdge: 10, topEdge: 10, width: -20, height: 30, text: "Button Demo"));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 50, width: -20, height: 30, text: "Toggle Button", toggleSelect: true));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 90, width: -20, height: 30, text: "Icon Button", icon: Icons.YOUKO));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 130, width: -20, height: 30, text: "Disabled Button", disabled: true));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 170, width: -20, height: 30, text: "Color Button", bgColor: Color.Blue));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 210, width: -262, height: 30, text: "Play Prev", clickAction:
                    () => { GetApplet<MusicPlayer>().PrevMusic(); }));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: -248, topEdge: 210, width: 240, height: 30, text: "Play Next", clickAction:
                    () => { GetApplet<MusicPlayer>().NextMusic(); }));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 250, width: 30, height: 30, icon: Icons.MUSIC, toggleSelect: true,
                    selected: GetApplet<MusicVisualizer>().Enabled, clickAction: () => { GetApplet<MusicVisualizer>().Enabled ^= true; }));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 50, topEdge: 250, width: 30, height: 30, icon: Icons.LINE_GRAPH, toggleSelect: true,
                    selected: GetApplet<LinesApp>().Enabled, clickAction: () => { GetApplet<LinesApp>().Enabled ^= true; }));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 90, topEdge: 250, width: 30, height: 30, icon: Icons.BOX, toggleSelect: true,
                    selected: GetApplet<RainingBoxesApp>().Enabled, clickAction: () => { GetApplet<RainingBoxesApp>().Enabled ^= true; }));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 130, topEdge: 250, width: 30, height: 30, icon: Icons.IMAGE, toggleSelect: true,
                    selected: GetApplet<IconShow>().Enabled, clickAction: () => { GetApplet<IconShow>().Enabled ^= true; }));

                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 170, topEdge: 250, width: 30, height: 30, icon: Icons.HELP,
                    clickAction: () =>
                    {
                        if (winButTest != null)
                        {
                            Intuition.InitRequest(requester, winButTest);
                            Intuition.Request(requester, winButTest);
                        }
                    }));

                gadgets.Add(GadTools.CreateGadget(GadgetKind.Checkbox, leftEdge: 10, topEdge: 290, width: -20, height: 30, text: "Debug Borders", _cheked: Intuition.ShowDebugBounds, checkedStateChangedAction:
                    (b) => { Intuition.ShowDebugBounds = b; }));

                winButTest = Intuition.OpenWindow(new NewWindow
                {
                    LeftEdge = 400,
                    TopEdge = 10,
                    Width = 500,
                    Height = 500,
                    Title = "Buttons",
                    Gadgets = gadgets,
                    MinWidth = 200,
                    MinHeight = 200,
                    Activate = true,
                    SuperBitmap = true,
                    Sizing = true,
                    Dragging = true,
                    Closing = true,
                    Maximizing = true,
                    Font = font,
                    CloseAction = () => { Intuition.CloseWindow(ref winButTest); }
                });

                List<Gadget> reqGads = new();
                reqGads.Add(GadTools.CreateGadget(GadgetKind.Text, leftEdge: 10, topEdge: 10, width: -20, height: 20, text: "Question?"));
                reqGads.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: -30, width: -20, height: 20, text: "OK", endGadget: true));
                Intuition.AddGList(winButTest, reqGads, 0, reqGads.Count, requester);
            }
            else
            {
                Intuition.WindowToFront(winButTest);
                Intuition.ActivateWindow(winButTest);
            }
        }

        private void ShowPropTest()
        {
            if (winPropTest == null)
            {

                List<Gadget> gadgets = new List<Gadget>();
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Text, leftEdge: 10, topEdge: 10, width: -20, height: 30, text: "Props Demo"));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Slider, leftEdge: 10, topEdge: 40, width: -20, height: 20, min: 0, max: 128, level: SDLAudio.MusicVolume, valueChangedAction:
                    (level) =>
                    {
                        SDLAudio.MusicVolume = level;
                        SDLLog.Info(LogCategory.APPLICATION, $"Music Volume changed to {level}");
                    }));
                var nu = GadTools.CreateGadget(GadgetKind.Number, leftEdge: 10, topEdge: 70, width: -20, height: 30, text: "Level {0}", intValue: 1);
                gadgets.Add(nu);
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Slider, leftEdge: 10, topEdge: 110, width: -20, height: 20, min: 1, max: 16, valueChangedAction:
                    (level) =>
                    {
                        GadTools.SetAttrs(nu, intValue: level);
                    }));
                var prop1 = new Gadget { GadgetType = GadgetType.PropGadget, LeftEdge = 10, TopEdge = 140, Width = -20, Height = 100 };
                Intuition.ModifyProp(prop1, PropFlags.FreeHoriz | PropFlags.FreeVert, 0x5000, 0x5000, 0x2000, 0x4000);
                gadgets.Add(prop1);
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Scroller, leftEdge: 10, topEdge: 260, width: -20, height: 22, total: 20, visible: 10, top: 3));


                winPropTest = Intuition.OpenWindow(new NewWindow
                {
                    LeftEdge = 400,
                    TopEdge = 10,
                    Width = 500,
                    Height = 500,
                    Title = "Props",
                    Gadgets = gadgets,
                    MinWidth = 200,
                    MinHeight = 200,
                    Activate = true,
                    SuperBitmap = true,
                    Sizing = true,
                    Dragging = true,
                    Closing = true,
                    Maximizing = true,
                    Font = font,
                    CloseAction = () => { Intuition.CloseWindow(ref winPropTest); }
                });
            }
            else
            {
                Intuition.WindowToFront(winPropTest);
                Intuition.ActivateWindow(winPropTest);
            }
        }

        private void ShowStrTest()
        {
            if (winStrTest == null)
            {

                List<Gadget> gadgets = new List<Gadget>();
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Text, leftEdge: 10, topEdge: 10, width: -20, height: 30, text: "String Demo"));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.String, leftEdge: 10, topEdge: 50, width: -20, height: 30, buffer: "Hello World"));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.String, leftEdge: 10, topEdge: 90, width: -20, height: 30, buffer: "This gadget contains quite a large text in its buffer. Be careful..."));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Integer, leftEdge: 10, topEdge: 130, width: -20, height: 30, intValue: 12345));

                winStrTest = Intuition.OpenWindow(new NewWindow
                {
                    LeftEdge = 400,
                    TopEdge = 10,
                    Width = 500,
                    Height = 500,
                    Title = "Strings",
                    Gadgets = gadgets,
                    MinWidth = 200,
                    MinHeight = 200,
                    Activate = true,
                    SuperBitmap = true,
                    Sizing = true,
                    Dragging = true,
                    Closing = true,
                    Maximizing = true,
                    Font = font,
                    CloseAction = () => { Intuition.CloseWindow(ref winStrTest); }
                });
            }
            else
            {
                Intuition.WindowToFront(winStrTest);
                Intuition.ActivateWindow(winStrTest);
            }
        }

    }
}
