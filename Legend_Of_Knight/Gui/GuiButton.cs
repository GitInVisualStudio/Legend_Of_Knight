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

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
            }
        }

        public Color Background
        {
            get
            {
                return background;
            }

            set
            {
                background = value;
            }
        }

        public Bitmap Image
        {
            get
            {
                return image;
            }

            set
            {
                image = value;
            }
        }

        public GuiButton(string text)
        {
            this.Text = text;
        }

        public GuiButton(Bitmap img)
        {
            Image = img;
        }

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
