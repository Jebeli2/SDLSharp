﻿namespace SDLTest
{
    using SDLSharp;
    using SDLSharp.Applets;
    using SDLSharp.GUI;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class TestSDLWindow : SDLWindow
    {

        private const string DC = @"D:\Users\jebel\Music\iTunes\iTunes Media\Music\Alt-J\Relaxer\05 Deadcrush.mp3";

        private Icons icon = Icons.MIN;
        private double lastTime;
        private Window? window1;
        private Window? window2;

        private Window? winButTest;
        private Window? winPropTest;

        private Gadget? button1_1;
        private Gadget? button1_2;
        private Gadget? button1_3;

        private Gadget? button2_1;
        private Gadget? button2_2;
        private Gadget? button2_3;
        public TestSDLWindow()
            : base("Test SDL")
        {
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ContentManager.AddResourceManager(Properties.Resources.ResourceManager);
            GetApplet<BackgroundImage>().Image = LoadTexture(nameof(Properties.Resources.badlands));
            GetApplet<MusicPlayer>().PlayNow(nameof(Properties.Resources.jesu_joy_of_mans_desiring));
            GetApplet<MusicPlayer>().AddToPlayList(DC, "Deathcrush");
            var boxes = GetApplet<RainingBoxesApp>();
            var music = GetApplet<MusicVisualizer>();
            var lines = GetApplet<LinesApp>();
            var gui = GetApplet<GUISystem>();
            boxes.RenderPrio = -500;
            music.RenderPrio = -600;
            lines.RenderPrio = -750;

            SDLApplication.MaxFramesPerSecond = 120;
            button1_1 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 10, width: -20, height: 40, text: "GUI Test", clickAction: GoToGUITest);
            button1_2 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 60, width: -20, height: 40, text: "Blocks");
            button1_3 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 110, width: -20, height: 40, text: "Particles");

            button2_1 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 10, width: -20, height: 40, text: "Back", clickAction: GoToTitle);
            button2_2 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 60, width: -20, height: 40, text: "Buttons", clickAction: ShowButtonTest);
            button2_3 = GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 110, width: -20, height: 40, text: "Props & Strings", clickAction: ShowPropTest);

            GoToTitle();
        }

        protected override void OnClose(EventArgs e)
        {
            base.OnClose(e);
            CloseAllWindows();
        }

        private void NextIcon()
        {
            icon++;
            if (icon > Icons.MAX)
            {
                icon = Icons.MIN;
                return;
            }
            while (!Enum.IsDefined(icon))
            {
                icon++;
            }
        }

        protected override void OnUpdate(SDLWindowUpdateEventArgs e)
        {
            base.OnUpdate(e);

            if (e.TotalTime - lastTime > 1000)
            {
                lastTime = e.TotalTime;
                NextIcon();
            }
        }
        protected override void OnPaint(SDLWindowPaintEventArgs e)
        {
            base.OnPaint(e);
            e.Renderer.DrawIcon(icon, Width / 2, Height / 2, Color.White);
            e.Renderer.DrawText(null, icon.ToString(), Width / 2, Height / 2 + 20, Color.White);
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
            }
        }

        private void CloseAllWindows()
        {
            if (window1 != null)
            {
                Intuition.CloseWindow(window1);
                window1 = null;
            }
            if (window2 != null)
            {
                Intuition.CloseWindow(window2);
                window2 = null;
            }
            if (winButTest != null)
            {
                Intuition.CloseWindow(winButTest);
                winButTest = null;
            }
            if (winPropTest != null)
            {
                Intuition.CloseWindow(winPropTest);
                winPropTest = null;
            }
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
                BackDrop = true
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
                Gadgets = new Gadget[] { button2_1!, button2_2!, button2_3! },
                MinWidth = 200,
                MinHeight = 200,
                Activate = true,
                SuperBitmap = true,
                Sizing = true,
                Dragging = true,
                Closing = true
            });
            window2.WindowClose += Window2_WindowClose;
        }

        private void Window2_WindowClose(object? sender, EventArgs e)
        {
            GoToTitle();
        }

        private void ShowButtonTest()
        {
            if (winButTest == null)
            {
                List<Gadget> gadgets = new List<Gadget>();
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Text, leftEdge: 10, topEdge: 10, width: -20, height: 30, text: "Button Demo"));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 50, width: -20, height: 30, text: "Toggle Button", toggleSelect: true));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 90, width: -20, height: 30, text: "Icon Button", icon: Icons.YOUKO));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 130, width: -20, height: 30, text: "Disabled Button", disabled: true));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 170, width: -20, height: 30, text: "Color Button", bgColor: Color.Blue));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: 10, topEdge: 210, width: -262, height: 30, text: "Play Prev", clickAction:
                    () => { GetApplet<MusicPlayer>().PrevMusic(); }));
                gadgets.Add(GadTools.CreateGadget(GadgetKind.Button, leftEdge: -248, topEdge: 210, width: 240, height: 30, text: "Play Next", clickAction:
                    () => { GetApplet<MusicPlayer>().NextMusic(); }));

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
                });
                winButTest.WindowClose += WinButTest_WindowClose;
            }
            else
            {
                Intuition.WindowToFront(winButTest);
                Intuition.ActivateWindow(winButTest);
            }
        }

        private void WinButTest_WindowClose(object? sender, EventArgs e)
        {
            Intuition.CloseWindow(winButTest);
            winButTest = null;
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
                    Title = "Props & Strings",
                    Gadgets = gadgets,
                    MinWidth = 200,
                    MinHeight = 200,
                    Activate = true,
                    SuperBitmap = true,
                    Sizing = true,
                    Dragging = true,
                    Closing = true,
                    Maximizing = true,
                });
                winPropTest.WindowClose += WinPropTest_WindowClose;
            }
            else
            {
                Intuition.WindowToFront(winPropTest);
                Intuition.ActivateWindow(winPropTest);
            }
        }

        private void WinPropTest_WindowClose(object? sender, EventArgs e)
        {
            Intuition.CloseWindow(winPropTest);
            winPropTest = null;
        }
    }
}
