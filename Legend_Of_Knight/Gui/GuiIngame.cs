using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Gui
{
    public class GuiIngame : Gui
    {
        private Game game;
        private CustomAnimation<float> health = CustomAnimation<float>.CreateDefaultAnimation(20.0f);

        public GuiIngame(Game game)
        {
            this.game = game;
            health.Fire();
        }

        public override void OnRender(float partialTicks)
        {
            StateManager.SetColor(0, 0, 0);
            StateManager.DrawRect(5, 5, 100, 10);
            StateManager.SetColor(0, 255, 0);
            health.End = game.thePlayer.Health;
            float width = health.Value / 20.0f * 100f;
            StateManager.FillRect(5, 5, width, 10);
        }
    }
}
