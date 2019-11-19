using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Gui
{
    public class GuiButton : Gui
    {
        private string text;
        private Color color = Color.Black;
        private Color background = Color.Transparent;
        private Bitmap image;

        public GuiButton(string text)
        {
            this.Text = text;
        }

        public GuiButton(Bitmap img)
        {
            Image = img;
        }

        public Color Color { get => color; set => color = value; }
        public Color Background { get => background; set => background = value; }
        public string Text { get => text; set => text = value; }
        public Bitmap Image { get => image; set => image = value; }

        public override void OnRender(float partialTicks)
        {
            StateManager.SetColor(Background);
            StateManager.FillRect(Position, Width, Height);
            StateManager.DrawImage(Image, Position);
            StateManager.SetColor(Color);
            StateManager.DrawString(Text, Position);
        }
    }
}
