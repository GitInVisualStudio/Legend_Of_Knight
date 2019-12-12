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
    public class GuiOptions : GuiScreen
    {
        private bool loading;
        public override void Init(Game game)
        {
            base.Init(game);
            GuiButton restart = new GuiButton("Restart", Width / 2 - 50, Height / 2, 100, 20)
            {
                Background = Color.Gray
            };
            restart.OnClick += (object sender, MouseEventArgs args) =>
            {
                if(restart.OnHover(args) && !loading)
                {
                    //In einem neuen Thread, damit die Form nicht hängt
                    new Thread(() =>
                    {
                        loading = true;
                        game.LoadIngame(false);//Laden des IngameSpiels
                        game.SetScreen(null);//Setzten des Ingame-Fokuses
                    }).Start();
                }
            };
            Components.Add(restart);

        }
    }
}
