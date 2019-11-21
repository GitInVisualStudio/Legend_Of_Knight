﻿using Legend_Of_Knight.Utils.Render;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Gui
{
    public class GuiLabel : Gui
    {
        private string text;
        private Color color = Color.Black;
        public GuiLabel()
        {

        }

        public GuiLabel(string text)
        {

        }

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

        public override void OnRender(float partialTicks)
        {
            StateManager.SetColor(Color);
            StateManager.DrawString(Text, Position);
        }
    }
}
