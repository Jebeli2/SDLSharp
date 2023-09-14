namespace SDLSharp.Maps
{
    using SDLSharp.Graphics;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Tooltip
    {
        private SDLTexture? buffer;
        private bool bufferValid;
        private int lineSpacing = 2;
        private int horizontalMargin = 6;
        private int verticalMargin = 6;
        private int backgroundBorder = 4;
        private SDLTexture? background;
        private SDLRenderer gfx;
        private TooltipData data;
        private int offsetX = 0;
        private int offsetY = 0;
        private int tipWidth;
        private int tipHeight;

        public Tooltip(SDLRenderer gfx)
        {
            this.gfx = gfx;
            //background = this.gfx.GetImage("images/menus/tooltips.png");
            data = new TooltipData();
        }

        public SDLTexture? Background
        {
            get => background;
            set => background = value;
        }
        public bool IsEmpty => data == null || data.IsEmpty;
        public TooltipData Data
        {
            get { return data; }
            set { SetData(value); }
        }
        public void Clear()
        {
            data.Clear();
        }

        private void SetData(TooltipData value)
        {
            if (!TooltipData.Equals(data, value))
            {
                data = value;
                bufferValid = false;
            }
        }

        public void Render(int x, int y)
        {
            Render(gfx, x, y);
        }
        private void Render(SDLRenderer gfx, int x, int y)
        {
            if (gfx == null) return;
            if (data.IsEmpty) return;
            if (buffer == null || !bufferValid) { CreateBuffer(gfx, data); }
            if (buffer != null && bufferValid)
            {
                x -= tipWidth / 2;
                y -= tipHeight;
                gfx.DrawImage(buffer, new Rectangle(0, 0, tipWidth, tipHeight), new Rectangle(x - offsetX, y - offsetY, tipWidth, tipHeight));
            }
        }

        private void CheckBufferSize(SDLRenderer gfx, int width, int height)
        {
            if (buffer == null || buffer.Width < width || buffer.Height < height)
            {
                buffer?.Dispose();
                buffer = gfx.CreateTexture("tooltip", width, height);
            }
        }
        private void CreateBuffer(SDLRenderer gfx, TooltipData data)
        {
            Size textSize = new();
            List<Size> lineSizes = new List<Size>();
            foreach (var line in data.Lines)
            {
                Size size = gfx.MeasureText(data.Font, line);
                lineSizes.Add(size);
                textSize.Width = Math.Max(textSize.Width, size.Width);
                textSize.Height += size.Height;
                textSize.Height += lineSpacing;
            }
            textSize.Height -= lineSpacing;
            tipWidth = textSize.Width + 2 * horizontalMargin;
            tipHeight = textSize.Height + 2 * verticalMargin;
            CheckBufferSize(gfx, tipWidth, tipHeight);
            if (buffer != null)
            {
                gfx.PushTarget(buffer);
                if (background != null)
                {
                    // top left
                    int srcX = 0;
                    int srcY = 0;
                    int dstX = 0;
                    int dstY = 0;
                    int width = tipWidth - backgroundBorder;
                    int height = tipHeight - backgroundBorder;
                    gfx.DrawTexture(background, srcX, srcY, width, height, dstX, dstY, width, height);
                    // right
                    srcX = background.Width - backgroundBorder;
                    srcY = 0;
                    width = backgroundBorder;
                    height = tipHeight - backgroundBorder;
                    dstX = tipWidth - backgroundBorder;
                    dstY = 0;
                    gfx.DrawTexture(background, srcX, srcY, width, height, dstX, dstY, width, height);
                    // bottom
                    srcX = 0;
                    srcY = background.Height - backgroundBorder;
                    width = tipWidth - backgroundBorder;
                    height = backgroundBorder;
                    dstX = 0;
                    dstY = tipHeight - backgroundBorder;
                    gfx.DrawTexture(background, srcX, srcY, width, height, dstX, dstY, width, height);
                    // bottom right
                    srcX = background.Width - backgroundBorder;
                    srcY = background.Height - backgroundBorder;
                    width = backgroundBorder;
                    height = backgroundBorder;
                    dstX = tipWidth - backgroundBorder;
                    dstY = tipHeight - backgroundBorder;
                    gfx.DrawTexture(background, srcX, srcY, width, height, dstX, dstY, width, height);
                }
                int cursorY = verticalMargin;
                int idx = 0;
                foreach (string line in data.Lines)
                {
                    Size size = lineSizes[idx];
                    int cursorX = (tipWidth - size.Width) / 2;
                    gfx.DrawText(data.Font, line, cursorX, cursorY, 0, 0, Color.White, HorizontalAlignment.Left, VerticalAlignment.Top);
                    cursorY += size.Height;
                    cursorY += lineSpacing;
                    idx++;
                }
                gfx.PopTarget();
                bufferValid = true;
            }
        }
    }
}
