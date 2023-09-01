﻿namespace SDLSharp.Applets
{
    using SDLSharp.Actors;
    using SDLSharp.Content;
    using SDLSharp.Events;
    using SDLSharp.Maps;
    using SDLSharp.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    public class MapApplet : SDLApplet
    {

        private enum MapState
        {
            None,
            Load,
            Loading,
            Loaded,
            Display
        }

        private string mapName = "";
        private string playerName = "";
        private Map? map;
        private readonly IMapRenderer mapRenderer;
        private readonly ActorManager actorManager;
        private readonly EventManager eventManager;
        private bool panning;
        private int panDX;
        private int panDY;
        private MapState mapState;
        private SDLTexture? image;
        private SDLMusic? music;
        private SDLTexture? dialogBox;
        private Actor? player;

        public MapApplet()
            : this(null)
        {

        }
        public MapApplet(IMapRenderer? renderer = null)
            : base("Map")
        {
            mapRenderer = renderer ?? new FlareMapRenderer();
            actorManager = new ActorManager();
            eventManager = new EventManager();
            actorManager.EventManager = eventManager;
            MousePanning = true;
            CommandMoving = true;
            FollowPlayer = true;
            mapState = MapState.None;
            PlayerName = "";
        }

        public bool MousePanning { get; set; }
        public bool CommandMoving { get; set; }
        public bool FollowPlayer { get; set; }

        public string MapName
        {
            get => mapName;
            set
            {
                if (mapName != value)
                {
                    mapName = value;
                    mapState = MapState.None;
                }
            }
        }

        public string PlayerName
        {
            get => playerName;
            set => SetPlayerName(value);
        }

        public Map? Map
        {
            get => map;
        }
        private void SetPlayerName(string? name, int x = -1, int y = -1)
        {
            if (string.IsNullOrEmpty(name)) { name = "male"; }
            playerName = name;
            actorManager.PlayerInfo = new ActorInfo { Id = playerName, Name = "Player", PosX = x, PosY = y };
        }
        private static string GetRandomBackground()
        {
            switch (MathUtils.Rand() % 3)
            {
                case 1: return "images/menus/backgrounds/ice_palace.png";
                case 2: return "images/menus/backgrounds/fire_temple.png";
                default: return "images/menus/backgrounds/badlands.png";
            }
        }

        protected override void OnWindowLoad(SDLWindowLoadEventArgs e)
        {
            actorManager.ContentManager = ContentManager;
        }

        private void LoadMap()
        {
            map = ContentManager?.Load<Map>(mapName);
            if (map != null)
            {
                SetMusic(map.Music);

                actorManager.Clear();
                actorManager.Map = map;
                actorManager.Camera = mapRenderer;
                PlayerName = "";
                actorManager.SpawnPlayer(map);
                actorManager.SpawnMapActors(map);
                player = actorManager.Player;
                eventManager.Map = map;
                eventManager.Camera = mapRenderer;
                eventManager.ExecuteOnLoadEvents();
            }
        }

        private bool CheckTravel()
        {
            if (eventManager.Travel && !string.IsNullOrEmpty(eventManager.TravelMap))
            {
                SetPlayerName(playerName,eventManager.TravelX,eventManager.TravelY);
                eventManager.Travel = false;
                MapName = eventManager.TravelMap;
                return true;
            }
            return false;
        }
        private void Update(double totalTime, double elapsedTime)
        {
            switch (mapState)
            {
                case MapState.None:
                    if (!string.IsNullOrEmpty(mapName))
                    {
                        ClearImage();
                        SetImage(GetRandomBackground());
                        mapState = MapState.Load;
                    }
                    break;
                case MapState.Load:
                    mapState = MapState.Loading;
                    break;
                case MapState.Loading:
                    LoadMap();
                    mapState = MapState.Loaded;
                    break;
                case MapState.Loaded:
                    if (map != null)
                    {
                        mapRenderer.PrepareMap(map);
                        mapState = MapState.Display;
                    }
                    break;
                case MapState.Display:
                    if (map != null)
                    {
                        actorManager.Update(totalTime, elapsedTime);
                        mapRenderer.Update(totalTime, elapsedTime, map);
                        if (MousePanning && panning && (panDX != 0 || panDY != 0))
                        {
                            if (Pan(panDX, panDY))
                            {
                                panDX = 0;
                                panDY = 0;
                            }
                        }
                        else if (FollowPlayer && player != null && player.HasMoved)
                        {
                            if (!Move(player.PosX, player.PosY))
                            {
                                player.HasMoved = false;
                            }
                        }
                        else
                        {
                            CheckTravel();
                        }
                    }
                    break;
            }
        }

        private void Paint(SDLRenderer renderer, double totalTime, double elapsedTime)
        {
            switch (mapState)
            {
                case MapState.None:
                    break;
                case MapState.Load:
                    RenderLoadScreen(renderer);
                    break;
                case MapState.Loading:
                    RenderLoadScreen(renderer);
                    break;
                case MapState.Loaded:
                    RenderLoadScreen(renderer);
                    break;
                case MapState.Display:
                    if (map != null)
                    {
                        if (map.BackgroundColor != Color.Black)
                        {
                            renderer.ClearScreen(map.BackgroundColor);
                        }
                        List<IMapSprite> front = new(actorManager.GetLivingSprites());
                        List<IMapSprite> back = new(actorManager.GetDeadSprites());
                        mapRenderer.Render(renderer, totalTime, elapsedTime, map, front, back);
                    }
                    break;
            }
        }

        private void RenderLoadScreen(SDLRenderer renderer)
        {
            renderer.DrawTexture(image);
            dialogBox ??= ContentManager?.Load<SDLTexture>("images/menus/dialog_box.png");
            if (dialogBox != null)
            {
                int w = renderer.Width / 2 - dialogBox.Width / 2;
                int h = renderer.Height - dialogBox.Height * 2;
                Rectangle box = new Rectangle(w, h, dialogBox.Width, dialogBox.Height);
                renderer.DrawTexture(dialogBox, box);
                renderer.DrawText(null, "Loading...", box.X, box.Y, box.Width, box.Height, Color.White);
            }
        }

        private void SetMusic(string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                SDLMusic? newMusic = LoadMusic(name);
                if (newMusic != music)
                {
                    music?.Dispose();
                    music = newMusic;
                    SDLAudio.PlayMusic(music);
                }
            }
        }

        private void SetImage(string? name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                SDLTexture? newImg = LoadTexture(name);
                if (newImg != image)
                {
                    image?.Dispose();
                    image = newImg;
                }
            }
        }

        private void ClearImage()
        {
            image?.Dispose();
            image = null;
        }

        protected override void OnDispose()
        {
            map?.Dispose();
            map = null;
            dialogBox?.Dispose();
            dialogBox = null;
            image?.Dispose();
            image = null;
        }

        protected override void OnWindowUpdate(SDLWindowUpdateEventArgs e)
        {
            Update(e.TotalTime, e.ElapsedTime);
        }

        protected override void OnWindowPaint(SDLWindowPaintEventArgs e)
        {
            Paint(e.Renderer, e.TotalTime, e.ElapsedTime);
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
            else if (e.Button == MouseButton.Left)
            {
                if (CommandMoving && player != null)
                {
                    actorManager.MakeCommands(player, e.X, e.Y);
                }
            }
        }

        protected internal override void OnMouseMove(SDLMouseEventArgs e)
        {
            if (panning)
            {
                panDX = -e.RelX;
                panDY = -e.RelY;
            }
            mapRenderer.ScreenToMap(e.X, e.Y, out float mx, out float my);
            mx = MathUtils.RoundForMap(mx);
            my = MathUtils.RoundForMap(my);
            eventManager.CheckHotSpotEvents(mx, my);
        }

        private bool Move(float x, float y)
        {
            return mapRenderer.MoveCam(x, y);
        }

        private bool Pan(int dx, int dy)
        {
            return mapRenderer.ShiftCam(dx, dy);
        }
    }
}