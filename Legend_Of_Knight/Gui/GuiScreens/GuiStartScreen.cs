using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Gui.GuiScreens
{
    /// <summary>
    /// Start
    /// </summary>
    public class GuiStartScreen : GuiScreen
    {
        private bool loading;
        private string loadingText;
        private TimeUtils timeUtils;

        public GuiStartScreen()
        {
            this.timeUtils = new TimeUtils();
        }

        public override void Init(Game game)
        {
            base.Init(game);
            loading = false;
            GuiButton start = new GuiButton("Start", Width / 2 - 50, Height / 2, 100, 20)
            {
                Background = Color.Gray
            };
            start.OnClick += (object sender, System.Windows.Forms.MouseEventArgs e) =>
            {
                if (loading || !start.OnHover(e)) //nur ausführen wenn auch über dem Button
                    return;
                //In einem neuen Thread, damit die Form nicht hängt
                new Thread(() =>
                {
                    loadingText = "Loading";
                    loading = true;
                    game.LoadIngame();//Laden des IngameSpiels
                    game.SetScreen(null);//Setzten des Ingame-Fokuses
                }).Start();
            };
            Components.Add(start);
        }

        //Zeichent die Komponenten
        public override void OnRender(float partialTicks)
        {
            base.OnRender(partialTicks);
            StateManager.Push();
            StateManager.Translate(0, GetAnimation<float>());//Translation für Start- und CloseAnimation
            if (timeUtils.Check(500) && loading)
            {
                loadingText += ".";//Addiert Punkte bis 3 erreicht wurden
                if(loadingText.Length > 10)
                    loadingText = "Loading";
            }
            StateManager.SetColor(255, 255, 255);
            StateManager.DrawCenteredString(loadingText, Width / 2, Height / 3);//Zeichnet den Ladestatus mittig
            StateManager.Pop();
        }

    }
}
