using SDLSharp.Maps;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        private readonly List<CombatTextItem> combatTexts = new();
        public CombatText(IMapEngine engine)
        {
            this.engine = engine;
            duration = 1000;
            fadeDuration = 0;
            speed = 1;
            offset = 48;
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
            renderer.DrawText(null, item.Text, item.ScreenX, item.ScreenY, GetColor(item.DisplayType));
        }

        private Color GetColor(CombatTextType type)
        {
            switch (type)
            {
                case CombatTextType.GiveDmg: return Color.White;
                case CombatTextType.TakeDmg: return Color.Red;
                case CombatTextType.Crit: return Color.Yellow;
                case CombatTextType.Buff: return Color.Green;
                case CombatTextType.Miss: return Color.Gray;
            }
            return Color.White;
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
            public bool IsNumber;
            public float NumberValue;
            public int ScreenX;
            public int ScreenY;
        }

    }
}
