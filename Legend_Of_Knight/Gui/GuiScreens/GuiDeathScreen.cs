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
    public class GuiDeathScreen : GuiScreen
    {
        private bool loading;
        private string loadingText;
        private TimeUtils timeUtils;

        public GuiDeathScreen()
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
                new Thread(() =>
                {
                    loadingText = "Loading";
                    loading = true;
                    game.LoadIngame();
                    game.SetScreen(null);
                }).Start();
            };
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
            Components.Add(start);
            Components.Add(quit);
        }

        public override void OnRender(float partialTicks)
        {
            base.OnRender(partialTicks);
            StateManager.Push();
            StateManager.Translate(0, GetAnimation<float>());
            if (timeUtils.Check(500) && loading)
            {
                loadingText += ".";
                if (loadingText.Length > 10)
                    loadingText = "Loading";
            }
            StateManager.SetColor(255, 255, 255);
            StateManager.DrawCenteredString(loadingText, Width / 2, Height / 3);
            StateManager.Push();
            StateManager.SetFont(new Font("System", 24));
            StateManager.DrawCenteredString("YOU ARE DEAD", Width / 2, 10);
            StateManager.Pop();
            StateManager.Pop();
        }

    }
}
