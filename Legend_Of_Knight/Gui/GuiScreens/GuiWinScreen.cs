using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend_Of_Knight.Gui.GuiScreens
{
    //Siehe GuiDeathScreen nur mit Gewinnen
    public class GuiWinScreen : GuiScreen
    {
        private bool loading;
        private string loadingText;
        private TimeUtils timeUtils;
        private bool hard;

        public GuiWinScreen()
        {
            this.timeUtils = new TimeUtils();
        }

        public override void Init(Game game)
        {
            base.Init(game);
            loading = false;
            GuiButton start = new GuiButton("Restart", Width / 2 - 50, Height / 2, 100, 20)
            {
                Background = Color.White
            };
            start.OnClick += (object sender, MouseEventArgs e) =>
            {
                if (loading || !start.OnHover(e))
                    return;
                //Laden des ingame Spieles in einem anderen Thread
                new Thread(() =>
                {
                    loadingText = "Loading";
                    loading = true;
                    game.LoadIngame(hard);
                    game.SetScreen(null);
                }).Start();
            };
            //Erstellen eines Buttons für das Beenden des Spieles
            GuiButton quit = new GuiButton("Quit", Width / 2 - 50, Height / 2 + 30, 100, 20)
            {
                Background = Color.White
            };
            quit.OnClick += (object sender, MouseEventArgs e) => 
            {
                if (loading || !quit.OnHover(e))
                    return;
                Application.Exit();
            };
            //Ob der Modus Hard aktiviert werden soll
            GuiCheckbox box = new GuiCheckbox("Hard", false)
            {
                Position = new Vector(Width / 2 - 50, Height / 2 - 30),
                Size = new Vector(100, 20)
            };
            box.OnClick += (object sender, MouseEventArgs args) =>
            {
                hard = box.State;
            };
            Components.Add(box);
            Components.Add(start);
            Components.Add(quit);
        }

        public override void OnRender(float partialTicks)
        {
            base.OnRender(partialTicks);
            StateManager.Push();
            StateManager.Translate(0, GetAnimation<float>()); //Translation für Start und Close Animation
            if (timeUtils.Check(500) && loading)
            {
                loadingText += ".";
                if (loadingText.Length > 10)
                    loadingText = "Loading";
            }
            StateManager.SetColor(255, 255, 255);
            StateManager.DrawCenteredString(loadingText, Width / 2, Height / 3);
            StateManager.SetFont(new Font("System", 24));
            StateManager.DrawCenteredString("CONGRATULATIONS! YOU WIN", Width / 2, 10);
            StateManager.Pop();
        }

    }
}
