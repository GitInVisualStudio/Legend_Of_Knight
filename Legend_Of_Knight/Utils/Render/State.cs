using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Render
{
    public class State
    {

        private Color color;
        private Font font = new Font("System", 12);
        private float scaleX = 1, scaleY = 1, translateX = 0, translateY = 0;
        private float rotation = 0;
        private State prevState;

        public State(bool createNew = false)
        {
            if (createNew)
            {
                return;
            }

            ScaleX = StateManager.ScaleX;
            ScaleY = StateManager.ScaleY;
            Font = StateManager.Font;
            Rotation = StateManager.Rotation;
            PrevState = StateManager.State;
            Color = StateManager.Color;
        }

        public Color Color { get => color; set => color = value; }
        public Font Font { get => font; set => font = value; }
        public float ScaleX { get => scaleX; set => scaleX = value; }
        public float ScaleY { get => scaleY; set => scaleY = value; }
        public float TranslateX { get => translateX; set => translateX = value; }
        public float TranslateY { get => translateY; set => translateY = value; }
        public float Rotation { get => rotation; set => rotation = value; }
        public State PrevState { get => prevState; set => prevState = value; }
    }
}
