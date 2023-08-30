namespace SDLSharp.Maps
{
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FlareMapRenderer : IMapRenderer
    {
        private static readonly Random rnd = new Random();

        private int viewW;
        private int viewH;
        private int viewWHalf;
        private int viewHHalf;
        private int mapW;
        private int mapH;
        private MapProjection projection;
        private TileSet? tset;
        private TileSet? cset;
        private float unitsPerPixelX;
        private float unitsPerPiexlY;
        private int tileW;
        private int tileH;
        private int tileWHalf;
        private int tileHHalf;

        private float camX;
        private float camY;
        private float shakyCamX;
        private float shakyCamY;
        private double shakyCamDuration;
        private float minCamX;
        private float maxCamX;
        private float minCamY;
        private float maxCamY;

        private float smallestX;
        private float smallesY;
        private float biggestX;
        private float biggestY;

        private bool showCollision;
        private byte collisionAlpha = 32 + 16;
        public FlareMapRenderer()
        {
            minCamX = 0;
            minCamY = 0;
            maxCamX = int.MaxValue;
            maxCamY = int.MaxValue;
        }

        public bool ShowCollision
        {
            get => showCollision;
            set => showCollision = value;
        }

        public byte CollisionAlpha
        {
            get => collisionAlpha;
            set => collisionAlpha = value;
        }

        public void PrepareMap(Map map)
        {
            minCamX = 0;
            minCamY = 0;
            maxCamX = int.MaxValue;
            maxCamY = int.MaxValue;
            InitMapValues(map);
            SetCam(map.StartPosX, map.StartPosY);
        }
        public void Update(double totalTime, double elapsedTime, Map map)
        {
            map.Parallax?.Update(totalTime, elapsedTime);
            map.TileSet?.Update(totalTime, elapsedTime);
        }
        public void Render(SDLRenderer graphics, double totalTime, double elapsedTime, Map map, IList<IMapSprite> front, IList<IMapSprite> back)
        {
            if (InitMapValues(map))
            {
                if (InitGfxValues(graphics))
                {
                    if (projection == MapProjection.Isometric)
                    {
                        RenderIso(graphics, map, front, back);
                    }
                    else if (projection == MapProjection.Orthogonal)
                    {
                        RenderOrtho(graphics, map, front, back);
                    }
                }
            }
        }

        public void ShiftCam(int dX, int dY)
        {
            int vX = viewWHalf + dX;
            int vY = viewHHalf + dY;
            ScreenToMap(vX, vY, out float mX, out float mY);
            SetCam(mX, mY);
        }

        public void SetCam(float x, float y)
        {
            if (projection == MapProjection.Orthogonal)
            {
                x = Clamp(x, minCamX, maxCamX);
                y = Clamp(y, minCamY, maxCamY);
            }
            else if (projection == MapProjection.Isometric)
            {
                x = Clamp(x, minCamX, maxCamX);
                y = Clamp(y, minCamY, maxCamY);
            }
            x = MathF.Round(x, 3);
            y = MathF.Round(y, 3);
            if (DifferentFloat(camX, x) || DifferentFloat(camY, y))
            {
                camX = x;
                camY = y;
                smallestX = camX;
                smallesY = camY;
                biggestX = camX;
                biggestY = camY;
                UpdateShakyCam();
                //audio.Update(camX, camY);
            }
        }

        private void UpdateShakyCam(double elapsedTime = 1)
        {
            if (shakyCamDuration > 0)
            {
                shakyCamDuration -= elapsedTime;
            }
            if (shakyCamDuration > 0)
            {
                shakyCamX = camX + (Rand() % 16 - 8) * 0.0078125f;
                shakyCamY = camY + (Rand() % 16 - 8) * 0.0078125f;
            }
            else
            {
                shakyCamX = camX;
                shakyCamY = camY;
            }
        }

        private bool InitMapValues(Map map)
        {
            projection = map.Projection;
            mapW = map.Width;
            mapH = map.Height;
            tset = map.TileSet;
            cset = map.CollisionTileSet;
            if (tset == null) return false;
            tileW = tset.TileWidth;
            tileH = tset.TileHeight;
            if (!(tileW > 0 && tileH > 0))
            {
                tileW = 64;
                tileH = 32;
            }
            tileWHalf = tileW / 2;
            tileHHalf = tileH / 2;
            if (projection == MapProjection.Isometric)
            {
                unitsPerPixelX = 2.0f / tileW;
                unitsPerPiexlY = 2.0f / tileH;
            }
            else if (projection == MapProjection.Orthogonal)
            {
                unitsPerPixelX = 1.0f / tileW;
                unitsPerPiexlY = 1.0f / tileH;
            }

            return true;
        }

        private bool InitGfxValues(SDLRenderer graphics)
        {
            viewW = graphics.Width;
            viewH = graphics.Height;
            viewWHalf = viewW / 2;
            viewHHalf = viewH / 2;
            if (tset == null) return false;
            if (cset != null && showCollision)
            {
                cset.Alpha = collisionAlpha;
            }
            if (projection == MapProjection.Orthogonal)
            {
                float tilesPerX = viewW / (float)tileW;
                float tilesPerY = viewH / (float)tileH;
                if (tilesPerX < mapW)
                {
                    minCamX = viewWHalf / (float)tileW;
                    maxCamX = mapW - minCamX;
                }
                else
                {
                    minCamX = mapW / 2.0f;
                    maxCamX = minCamX;

                }
                if (tilesPerY < mapH)
                {
                    minCamY = viewHHalf / (float)tileH;
                    maxCamY = mapH - minCamY;
                }
                else
                {
                    minCamY = mapH / 2.0f;
                    maxCamY = minCamY;
                }
            }
            else if (projection == MapProjection.Isometric)
            {
                minCamX = viewWHalf;
                minCamX /= tileW;
                minCamY = viewHHalf;
                minCamY /= tileH;
                maxCamX = mapW - minCamX;
                maxCamY = mapH - minCamY;

                minCamX -= 10;
                minCamY -= 10;
                maxCamX += 10;
                maxCamY += 10;
            }
            minCamX = RoundForMap(minCamX);
            minCamY = RoundForMap(minCamY);
            maxCamX = RoundForMap(maxCamX);
            maxCamY = RoundForMap(maxCamY);
            return true;
        }

        private void RenderIso(SDLRenderer graphics, Map map, IList<IMapSprite> front, IList<IMapSprite> back)
        {
            RenderParallax(graphics, map.Parallax, "");
            foreach (MapLayer layer in map.Layers)
            {
                switch (layer.Type)
                {
                    case LayerType.Background:
                        RenderIsoLayer(graphics, layer);
                        break;
                    case LayerType.Object:
                        //RenderIsoLayer(graphics, layer);
                        RenderIsoBackObjects(graphics, back);
                        RenderIsoFrontObjects(graphics, layer, front);
                        if (showCollision) { RenderIsoCollision(graphics, map.Collision); }
                        break;
                    case LayerType.Unknown:
                        RenderIsoLayer(graphics, layer);
                        break;
                }
                RenderParallax(graphics, map.Parallax, layer.Name);
            }
        }

        private void RenderOrtho(SDLRenderer graphics, Map map, IList<IMapSprite> front, IList<IMapSprite> back)
        {

        }

        private void RenderIsoBackObjects(SDLRenderer graphics, IList<IMapSprite> r)
        {
            foreach (var s in r) { DrawSprite(graphics, s); }
        }

        private void RenderIsoFrontObjects(SDLRenderer graphics, MapLayer layer, IList<IMapSprite> r)
        {
            if (tset == null) return;
            int w = layer.Width;
            int h = layer.Height;
            ScreenToMap(0, 0, out float sx, out float sy);
            int upperLeftX = (int)sx;
            int upperLeftY = (int)sy;
            int maxTilesWidth = (viewW / tileW) + 2 * tset.MaxSizeX;
            int maxTilesHeight = ((viewH / tileH) + 2 * (tset.MaxSizeY)) * 2;
            int j = upperLeftY - tset.MaxSizeY / 2 + tset.MaxSizeX;
            int i = upperLeftX - tset.MaxSizeY / 2 - tset.MaxSizeX;
            int rCursor = 0;
            int rEnd = r.Count;
            while (rCursor != rEnd && ((int)r[rCursor].MapPosX + (int)r[rCursor].MapPosY < i + j || (int)r[rCursor].MapPosX < i))
            {
                ++rCursor;
            }
            List<int> renderBehindSW = new List<int>();
            List<int> renderBehindNE = new List<int>();
            List<int> renderBehindNone = new List<int>();
            int[,] drawnTiles = new int[w, h];
            for (uint y = (uint)maxTilesHeight; y != 0; --y)
            {
                int tilesWidth = 0;
                if (i < -1)
                {
                    j += i + 1;
                    tilesWidth -= i + 1;
                    i = -1;
                }

                int d = j - h;
                if (d >= 0)
                {
                    j -= d;
                    tilesWidth += d;
                    i += d;
                }

                int jEnd = Math.Max(j + i - w + 1, Math.Max(j - maxTilesWidth, 0));
                MapToScreen(i, j, out int pX, out int pY);
                CenterTile(ref pX, ref pY);
                bool isLastNeTile = false;
                while (j > jEnd)
                {
                    --j;
                    ++i;
                    ++tilesWidth;
                    pX += tileW;
                    bool drawTile = true;
                    int rPreCursor = rCursor;
                    while (rPreCursor != rEnd)
                    {
                        int rCursorX = (int)r[rPreCursor].MapPosX;
                        int rCursorY = (int)r[rPreCursor].MapPosY;
                        if ((rCursorX - 1 == i && rCursorY + 1 == j) || (rCursorX + 1 == i && rCursorY - 1 == j))
                        {
                            drawTile = false;
                            break;
                        }
                        else if ((rCursorX + 1 > i) || (rCursorY + 1 > j))
                        {
                            break;
                        }
                        ++rPreCursor;
                    }
                    if (drawTile && drawnTiles[i, j] == 0)
                    {
                        DrawTile(graphics, layer, i, j, pX, pY);
                        drawnTiles[i, j] = 1;
                    }
                    if (rCursor == rEnd)
                    {
                        continue;
                    }
                    doLastNETile:
                    GetTileBounds(i - 2, j + 2, layer, out int tileSWBoundsX, out int tileSWBoundsY, out int tileSWBoundsW, out int tileSWBoundsH, out int tileSWCenterX, out int tileSWCenterY);
                    GetTileBounds(i - 1, j + 2, layer, out int tileSBoundsX, out int tileSBoundsY, out int tileSBoundsW, out int tileSBoundsH, out int tileSCenterX, out int tileSCenterY);
                    GetTileBounds(i, j, layer, out int tileNEBoundsX, out int tileNEBoundsY, out int tileNEBoundsW, out int tileNEBoundsH, out int tileNECenterX, out int tileNECenterY);
                    GetTileBounds(i, j + 1, layer, out int tileEBoundsX, out int tileEBoundsY, out int tileEBoundsW, out int tileEBoundsH, out int tileECenterX, out int tileECenterY);
                    bool drawSWTile = false;
                    bool drawNETile = false;
                    while (rCursor != rEnd)
                    {
                        int rCursorX = (int)r[rCursor].MapPosX;
                        int rCursorY = (int)r[rCursor].MapPosY;
                        if ((rCursorX + 1 == i) && (rCursorY - 1 == j))
                        {
                            drawSWTile = true;
                            drawNETile = !isLastNeTile;
                            MapToScreen(r[rCursor].MapPosX, r[rCursor].MapPosY, out int rCursorLeftX, out int rCursorLeftY);
                            rCursorLeftY -= r[rCursor].OffsetY;
                            int rCursorRightX = rCursorLeftX;
                            int rCursorRightY = rCursorLeftY;
                            rCursorLeftX -= r[rCursor].OffsetX;
                            rCursorRightX += r[rCursor].Width - r[rCursor].OffsetX;
                            bool isBehindSW = false;
                            bool isBehindNE = false;
                            if (IsWithinRect(tileSBoundsX, tileSBoundsY, tileSBoundsW, tileSBoundsH, rCursorRightX, rCursorRightY) &&
                                IsWithinRect(tileSWBoundsX, tileSWBoundsY, tileSWBoundsW, tileSWBoundsH, rCursorLeftX, rCursorLeftY))
                            {
                                isBehindSW = true;
                            }
                            if (drawNETile && IsWithinRect(tileEBoundsX, tileEBoundsY, tileEBoundsW, tileEBoundsH, rCursorLeftX, rCursorLeftY) &&
                                IsWithinRect(tileNEBoundsX, tileNEBoundsY, tileNEBoundsW, tileNEBoundsH, rCursorRightX, rCursorRightY))
                            {
                                isBehindNE = true;
                            }
                            if (isBehindSW)
                            {
                                renderBehindSW.Add(rCursor);
                            }
                            else if (!isBehindSW && isBehindNE)
                            {
                                renderBehindNE.Add(rCursor);
                            }
                            else
                            {
                                renderBehindNone.Add(rCursor);
                            }
                            ++rCursor;
                        }
                        else
                        {
                            break;
                        }
                    }

                    while (renderBehindSW.Count > 0)
                    {
                        DrawSprite(graphics, r[renderBehindSW[0]]);
                        renderBehindSW.RemoveAt(0);
                    }

                    if (drawSWTile && i - 2 >= 0 && j + 2 < h && drawnTiles[i - 2, j + 2] == 0)
                    {
                        DrawTile(graphics, layer, i - 2, j + 2, tileSWCenterX, tileSWCenterY);
                        drawnTiles[i - 2, j + 2] = 1;
                    }

                    while (renderBehindNE.Count > 0)
                    {
                        DrawSprite(graphics, r[renderBehindNE[0]]);
                        renderBehindNE.RemoveAt(0);
                    }

                    if (drawNETile && !drawTile && drawnTiles[i, j] == 0)
                    {
                        DrawTile(graphics, layer, i, j, tileNECenterX, tileNECenterY);
                        drawnTiles[i, j] = 1;
                    }

                    while (renderBehindNone.Count > 0)
                    {
                        DrawSprite(graphics, r[renderBehindNone[0]]);
                        renderBehindNone.RemoveAt(0);
                    }

                    if (isLastNeTile)
                    {
                        ++j;
                        --i;
                        isLastNeTile = false;
                    }
                    else if (i == w - 1 || j == 0)
                    {
                        --j;
                        ++i;
                        isLastNeTile = true;
                        goto doLastNETile;
                    }

                }
                j += tilesWidth;
                i -= tilesWidth;
                if ((y % 2) != 0)
                {
                    i++;
                }
                else
                {
                    j++;
                }
                while (rCursor != rEnd && ((int)r[rCursor].MapPosX + (int)r[rCursor].MapPosY < i + j || (int)r[rCursor].MapPosX <= i))
                {
                    ++rCursor;
                }
            }
        }

        private void RenderParallax(SDLRenderer graphics, MapParallax? parallax, string layerName)
        {
            if (parallax != null)
            {
                foreach (var p in parallax.GetMatchingLayers(layerName))
                {
                    RenderParallaxLayer(graphics, p);
                }
            }
        }

        private void RenderParallaxLayer(SDLRenderer graphics, ParallaxLayer layer)
        {
            if (layer.Image != null)
            {
                int width = layer.Image.Width;
                int height = layer.Image.Height;
                float mapCenterX = mapW / 2.0f;
                float mapCenterY = mapH / 2.0f;
                float dpX = mapCenterX + shakyCamX;
                float dpY = mapCenterY + shakyCamY;
                float sdpX = mapCenterX + dpX * layer.Speed + layer.FixedOffsetX;
                float sdpY = mapCenterY + dpY * layer.Speed + layer.FixedOffsetY;
                MapToScreen(sdpX, sdpY, out int sX, out int sY);
                int centerX = sX - width / 2;
                int centerY = sY - height / 2;
                int drawPosX = centerX - (int)(Math.Ceiling((viewW / 2.0f + centerX) / width)) * width;
                int drawPosY = centerY - (int)(Math.Ceiling((viewH / 2.0f + centerY) / height)) * height;
                int startX = drawPosX;
                int startY = drawPosY;
                int x = startX;
                while (x < viewW)
                {
                    int y = startY;
                    while (y < viewH)
                    {
                        graphics.DrawImage(layer.Image, x, y);
                        y += height;
                    }
                    x += width;
                }
            }
        }
        private void RenderIsoCollision(SDLRenderer graphics, IMapCollision? collision)
        {
            if (tset == null) return;
            if (cset == null) return;
            if (collision == null) return;
            int w = collision.Width;
            int h = collision.Height;
            ScreenToMap(0, 0, out float sx, out float sy);
            int upperLeftX = (int)sx;
            int upperLeftY = (int)sy;
            int maxTilesWidth = (viewW / tileW) + 2 * tset.MaxSizeX;
            int maxTilesHeight = (2 * viewH / tileH) + 2 * (tset.MaxSizeY + 1);
            int j = upperLeftY - tset.MaxSizeY / 2 + tset.MaxSizeX;
            int i = upperLeftX - tset.MaxSizeY / 2 - tset.MaxSizeX;
            for (int y = maxTilesHeight; y >= 0; --y)
            {
                int tilesWidth = 0;
                if (i < -1)
                {
                    j += i + 1;
                    tilesWidth -= i + 1;
                    i = -1;
                }

                int d = j - h;
                if (d >= 0)
                {
                    j -= d;
                    tilesWidth += d;
                    i += d;
                }
                int jEnd = Math.Max(j + i - w + 1, Math.Max(j - maxTilesWidth, 0));
                MapToScreen(i, j, out int pX, out int pY);
                CenterTile(ref pX, ref pY);
                while (j > jEnd)
                {
                    --j;
                    ++i;
                    ++tilesWidth;
                    pX += tileW;
                    DrawCollision(graphics, collision, i, j, pX, pY);
                }
                j += tilesWidth;
                i -= tilesWidth;
                if ((y % 2) != 0)
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }
        }

        private void RenderIsoLayer(SDLRenderer graphics, MapLayer layer)
        {
            if (tset == null) return;
            int w = layer.Width;
            int h = layer.Height;
            ScreenToMap(0, 0, out float sx, out float sy);
            int upperLeftX = (int)sx;
            int upperLeftY = (int)sy;
            int maxTilesWidth = (viewW / tileW) + 2 * tset.MaxSizeX;
            int maxTilesHeight = (2 * viewH / tileH) + 2 * (tset.MaxSizeY + 1);
            int j = upperLeftY - tset.MaxSizeY / 2 + tset.MaxSizeX;
            int i = upperLeftX - tset.MaxSizeY / 2 - tset.MaxSizeX;
            for (int y = maxTilesHeight; y >= 0; --y)
            {
                int tilesWidth = 0;
                if (i < -1)
                {
                    j += i + 1;
                    tilesWidth -= i + 1;
                    i = -1;
                }

                int d = j - h;
                if (d >= 0)
                {
                    j -= d;
                    tilesWidth += d;
                    i += d;
                }

                int jEnd = Math.Max(j + i - w + 1, Math.Max(j - maxTilesWidth, 0));
                MapToScreen(i, j, out int pX, out int pY);
                CenterTile(ref pX, ref pY);
                while (j > jEnd)
                {
                    --j;
                    ++i;
                    ++tilesWidth;
                    pX += tileW;
                    DrawTile(graphics, layer, i, j, pX, pY);
                }
                j += tilesWidth;
                i -= tilesWidth;
                if ((y % 2) != 0)
                {
                    i++;
                }
                else
                {
                    j++;
                }
            }

        }

        private void DrawCollision(SDLRenderer graphics, IMapCollision collision, int x, int y, int sX, int sY)
        {
            ISprite? tile = GetTile(collision, x, y);
            if (tile != null)
            {
                graphics.DrawSprite(tile, sX, sY);
            }
        }
        private void DrawTile(SDLRenderer graphics, MapLayer layer, int x, int y, int sX, int sY)
        {
            UpdateCoordinates(x, y);
            ISprite? tile = GetTile(layer, x, y);
            if (tile != null)
            {
                graphics.DrawSprite(tile, sX, sY);
            }
        }

        private void DrawSprite(SDLRenderer graphics, IMapSprite r)
        {
            if (r.Image != null)
            {
                MapToScreen(r.MapPosX, r.MapPosY, out int sx, out int sy);
                graphics.DrawSprite(r, sx, sy);
            }
        }

        private ISprite? GetTile(MapLayer layer, int x, int y)
        {
            if (tset != null)
            {
                int tileId = layer[x, y];
                return tset[tileId];
            }
            return null;
        }

        private ISprite? GetTile(IMapCollision collision, int x, int y)
        {
            if (cset != null)
            {
                int blockId = collision.ColMap[x, y];
                int tileId = 0;
                switch (blockId)
                {
                    case 0:
                        tileId = 17;
                        break;
                    case 1:
                        tileId = 16;
                        break;
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        tileId = 21;
                        break;
                    case 7:
                        tileId = 18;
                        break;
                    case 8:
                        tileId = 20;
                        break;
                }
                return cset[tileId];
            }
            return null;
        }

        private static bool IsWithinRect(int rX, int rY, int rW, int rH, int x, int y)
        {
            return x >= rX && y >= rY && x < rX + rW && y < rY + rH;
        }

        private void GetTileBounds(int x, int y, MapLayer layer, out int boundsX, out int boundsY, out int boundsW, out int boundsH, out int centerX, out int centerY)
        {
            boundsX = 0;
            boundsY = 0;
            boundsW = 0;
            boundsH = 0;
            centerX = 0;
            centerY = 0;
            if ((x >= 0) && (y >= 0) && (x < layer.Width) && (y < layer.Height))
            {
                var tileId = layer[x, y];
                if (tileId > 0)
                {
                    var tile = tset?.GetTile(tileId);
                    if (tile == null || tile.Image == null)
                    {
                        return;
                    }
                    MapToScreen(x, y, out centerX, out centerY);
                    CenterTile(ref centerX, ref centerY);
                    boundsX = centerX;
                    boundsY = centerY;
                    boundsW = tile.Width;
                    boundsH = tile.Height;
                }
            }
        }
        private void UpdateCoordinates(int mapX, int mapY)
        {
            if (mapX < smallestX)
            {
                smallestX = mapX;
            }
            else if (mapX > biggestX)
            {
                biggestX = mapX;
            }
            if (mapY < smallesY)
            {
                smallesY = mapY;
            }
            else if (mapY > biggestY)
            {
                biggestY = mapY;
            }
        }
        public void CenterTile(ref int x, ref int y)
        {
            if (projection == MapProjection.Orthogonal)
            {
                x += tileWHalf;
                y += tileHHalf;
            }
            else if (projection == MapProjection.Isometric)
            {
                y += tileHHalf;
            }
        }
        public void MapToScreen(float x, float y, out int sx, out int sy)
        {
            MapToScreen(x, y, shakyCamX, shakyCamY, out sx, out sy);
        }

        public void MapToScreen(float x, float y, float camX, float camY, out int sx, out int sy)
        {
            float adjustX = (viewWHalf + 0.5f) * unitsPerPixelX;
            float adjustY = (viewHHalf + 0.5f) * unitsPerPiexlY;
            if (projection == MapProjection.Isometric)
            {
                int rx = (int)(Math.Floor(((x - camX - y + camY + adjustX) / unitsPerPixelX) + 0.5f));
                int ry = (int)(Math.Floor(((x - camX + y - camY + adjustY) / unitsPerPiexlY) + 0.5f));
                sx = rx;
                sy = ry;
            }
            else
            {
                int rx = (int)((x - camX + adjustX) / unitsPerPixelX);
                int ry = (int)((y - camY + adjustY) / unitsPerPiexlY);
                sx = rx;
                sy = ry;
            }
        }
        public void ScreenToMap(int x, int y, out float mx, out float my)
        {
            ScreenToMap(x, y, shakyCamX, shakyCamY, out mx, out my);
        }

        public void ScreenToMap(int x, int y, float camX, float camY, out float mx, out float my)
        {
            if (projection == MapProjection.Isometric)
            {
                float srcx = (x - viewWHalf) * 0.5f;
                float srcy = (y - viewHHalf) * 0.5f;
                float rx = (unitsPerPixelX * srcx) + (unitsPerPiexlY * srcy) + camX;
                float ry = (unitsPerPiexlY * srcy) - (unitsPerPixelX * srcx) + camY;
                mx = rx;
                my = ry;
            }
            else
            {
                float rx = (x - viewWHalf) * unitsPerPixelX + camX;
                float ry = (y - viewHHalf) * unitsPerPiexlY + camY;
                mx = rx;
                my = ry;
            }
        }

        public static float RoundForMap(float f)
        {
            return MathF.Round(f * 2, MidpointRounding.AwayFromZero) / 2;
        }
        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            return val;
        }

        public static bool DifferentFloat(float val1, float val2, float epsilon = 0.0000001f)
        {
            float diff = MathF.Abs(val1 - val2);
            return diff > epsilon;
        }

        public static int Rand()
        {
            return rnd.Next();
        }
    }
}
