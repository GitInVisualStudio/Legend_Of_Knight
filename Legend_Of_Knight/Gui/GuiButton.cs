using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Gui
{
    /// <summary>
    /// Anklickbarer Button mit Event
    /// </summary>
    public class GuiButton : GuiLabel
    {
        private Color background = Color.Transparent;
        private Color current;
        private Bitmap image;

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

        public GuiButton(string text, float x, float y, float width, float height)
        {
            this.Text = text;
            this.Size = new Vector(width, height);
            this.Position = new Vector(x, y);
        }

        public GuiButton(Bitmap img)
        {
            Image = img;
        }

        //Zeichnen des Buttons
        public override void OnRender(float partialTicks)
        {
            int r = Background.R - current.R;
            int g = Background.G - current.G;
            int b = Background.B - current.B;
            if (OnHover(InputManager.mouseX, InputManager.mouseY))
            {
                r -= 50;
                g -= 50;
                b -= 50;
                if (r < 0)
                    r = 0;
                if (g < 0)
                    g = 0;
                if (b < 0)
                    b = 0;
            }
            current = Color.FromArgb(current.R + (int)(r / 4f), current.G + (int)(g / 4f), current.B + (int)(b / 4f));
            StateManager.SetColor(current);
            StateManager.FillRect(Position, Width, Height);
            if(Image != null)
                StateManager.DrawImage(Image, Position);
            StateManager.SetColor(Color);
            StateManager.DrawCenteredString(Text, Position.X + Width/2, Position.Y);
        }
    }
}
