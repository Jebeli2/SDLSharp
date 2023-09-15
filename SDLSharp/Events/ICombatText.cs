using SDLSharp.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDLSharp.Events
{
    public interface ICombatText
    {
        void Update(double totalTime, double elapsedTime);
        void Render(SDLRenderer renderer, double totalTime, double elapsedTime);
        void AddFloat(float num, float x, float y, CombatTextType displayType);
        void AddString(string text, float x, float y, CombatTextType displayType);
    }
}
