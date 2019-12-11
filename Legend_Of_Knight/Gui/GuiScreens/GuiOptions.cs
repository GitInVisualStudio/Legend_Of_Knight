using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Gui.GuiScreens
{
    public class GuiOptions : GuiScreen
    {

        public override void Init(Game game)
        {
            base.Init(game);
            Components.Add(new GuiButton("TEST1", Width / 2 - 50, Height / 2, 100, 20)
            {
                Background = Color.Gray
            });
            Components.Add(new GuiCheckbox("TEST2", true)
            {
                Position = new Utils.Math.Vector(Width / 2 - 50, Height / 2 - 20),
                Size = new Utils.Math.Vector(100, 20)
            });
        }
    }
}
