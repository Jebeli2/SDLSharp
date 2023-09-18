using SDLSharp.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.Events
{
    public class CombatText : ICombatText
    {
        private readonly IMapEngine engine;
        private float duration;
        private float fadeDuration;
        private float speed;
        private int offset;
        private SDLFont? font;
        private readonly List<CombatTextItem> combatTexts = new();

        private readonly Color defaultColor = Color.FromArgb(250, Color.White);
        private readonly Color[] colors = new Color[]
        {
            Color.FromArgb(250,Color.White), // GiveDmg
            Color.FromArgb(250,Color.Red), // TakeDmg
            Color.FromArgb(250,Color.Yellow), // Crit
            Color.FromArgb(250,Color.Gray), // Miss
            Color.FromArgb(250,Color.Green) // Buff
        };
        public CombatText(IMapEngine engine)
        {
            this.engine = engine;
            duration = 1000;
            fadeDuration = 0;
            speed = 1;
            offset = 48;
        }

        public SDLFont? Font
        {
            get => font;
            set => font = value;
        }

        public void Update(double totalTime, double elapsedTime)
        {
            int i = 0;
            while (i < combatTexts.Count)
            {
                CombatTextItem item = combatTexts[i];
                if (item.LifeSpan >= 0)
                {
                    item.LifeSpan -= (float)elapsedTime;
                    item.FloatingOffset += speed;
                    engine.Camera.MapToScreen(item.Pos.X, item.Pos.Y, out int sx, out int sy);
                    sy -= (int)item.FloatingOffset;
                    item.ScreenX = sx;
                    item.ScreenY = sy;
                    i++;
                }
                else
                {
                    combatTexts.RemoveAt(i);
                }
            }
        }

        public void Render(SDLRenderer renderer, double totalTime, double elapsedTime)
        {
            foreach (CombatTextItem item in combatTexts)
            {
                RenderItem(renderer, item);
            }
        }

        private void RenderItem(SDLRenderer renderer, CombatTextItem item)
        {
            renderer.DrawText(item.Font, item.Text, item.ScreenX, item.ScreenY, GetColor(item.DisplayType));
        }

        private Color GetColor(CombatTextType type)
        {
            int idx = (int)type;
            if (idx >= 0 && idx < colors.Length)
            {
                return colors[(int)type];
            }
            return defaultColor;
        }


        public void Clear()
        {
            combatTexts.Clear();
        }

        public void AddString(string text, float x, float y, CombatTextType displayType)
        {
            CombatTextItem item = new CombatTextItem();
            item.Text = text;
            item.LifeSpan = duration;
            item.FloatingOffset = offset;
            item.Pos = new PointF(x, y);
            item.DisplayType = displayType;
            item.Font = font;
            combatTexts.Add(item);
        }
        public void AddFloat(float num, float x, float y, CombatTextType displayType)
        {
            AddString(num.ToString(), x, y, displayType);
        }

        private class CombatTextItem
        {
            public string? Text;
            public float LifeSpan;
            public PointF Pos;
            public float FloatingOffset;
            public CombatTextType DisplayType;
            public SDLFont? Font;
            public bool IsNumber;
            public float NumberValue;
            public int ScreenX;
            public int ScreenY;
        }

    }
}
