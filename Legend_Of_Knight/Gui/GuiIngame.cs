using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Gui
{
    /// <summary>
    /// Ingame-Gui für den Spieler
    /// </summary>
    public class GuiIngame : Gui
    {
        private Game game;
        private CustomAnimation<float> health = CustomAnimation<float>.CreateDefaultAnimation(20.0f);//Animation für das Leben

        public GuiIngame(Game game)
        {
            this.game = game;
            health.Fire();
        }

        public override void OnRender(float partialTicks)
        {
            //Zeichnet die Lebens-Anzeige
            StateManager.Push();
            StateManager.Scale(1.5f);
            StateManager.SetColor(0, 125, 0);
            StateManager.DrawRect(5, 5, 100, 10);
            StateManager.SetColor(0, 255, 0);
            health.End = game.thePlayer.Health;
            float width = health.Value / 40.0f * 100f;
            StateManager.FillRect(5, 5, width, 10);
            StateManager.Pop();
        }
    }
}
