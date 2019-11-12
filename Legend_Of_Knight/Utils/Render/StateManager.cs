using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Render
{
    public class StateManager
    {
        private static Graphics g;
        private static Color color;
        private static Font font = new Font("Arial", 12);
        private static float scaleX, scaleY, translateX, translateY;
        private static float rotation;
        private static State state;

        public static float ScaleX => scaleX;
        public static float ScaleY => scaleY;
        public static float TranslateX => translateX;
        public static float TranslateY => translateY;
        public static float Rotation => rotation;
        public static Font Font => font;
        public static Graphics Graphics => g;
        private static State CurrentState => state;


        public static void Update(Graphics g)
        {
            StateManager.g = g;
        }

        public static void DrawString(string text, float x, float y)
        {
            g.DrawString(text, font, new SolidBrush(color), x, y);
        }

        public static void DrawImage(Bitmap img, float x, float y)
        {
            g.DrawImage(img, x, y);
        }

        public static void DrawImage(Bitmap map, Vector pos)
        {
            g.DrawImage(map, pos.X, pos.Y);
        }

        public static void DrawRect(float x, float y, float width, float height)
        {
            g.DrawRectangle(new Pen(new SolidBrush(color)), x, y, width, height);
        }

        public static void Rotate(float angle)
        {
            g.RotateTransform(angle);
            rotation = angle;
        }

        public static void Translate(float x, float y)
        {
            g.TranslateTransform(x, y);
            translateX = x;
            translateY = y;
        }

        public static void Translate(Vector vector)
        {
            g.TranslateTransform(vector.X, vector.Y);
            translateX = vector.X;
            translateY = vector.Y;
        }

        public static void Scale(float x, float y)
        {
            g.ScaleTransform(x, y);
            scaleX = x;
            scaleY = y;
        }

        public static void Scale(float x)
        {
            Scale(x, x);
            scaleX = x;
            scaleY = x;
        }

        public static void Push()
        {
            state = new State();
        }

        public static void Pop()
        {
            state = state.LastState;
            g.Transform.Reset();
            Scale(state.ScaleX, state.ScaleY);
            Rotate(state.Rotation);
            Translate(state.TranslateX, state.TranslateY);
        }

        public static float GetStringWidth(string s)
        {
            return GetStringSize(s).Width;
        }

        public static float GetStringHeight(string s)
        {
            return GetStringSize(s).Height;
        }

        public static SizeF GetStringSize(string s)
        {
            return g.MeasureString(s, font);
        }

        public static void Color(int r, int g, int b)
        {
            Color(r, g, b, 255);
        }

        public static void Color(int r, int g, int b, int a)
        {
            color = System.Drawing.Color.FromArgb(a, r, g, b);
        }

        private class State
        {
            private State state;
            private float rotation;
            private float scaleX, scaleY, translateX, translateY;

            public float Rotation { get => rotation; set => rotation = value; }
            public float ScaleX { get => scaleX; set => scaleX = value; }
            public float ScaleY { get => scaleY; set => scaleY = value; }
            public State LastState { get => state; set => state = value; }
            public float TranslateX { get => translateX; set => translateX = value; }
            public float TranslateY { get => translateY; set => translateY = value; }

            public State()
            {
                ScaleX = StateManager.ScaleX;
                ScaleY = StateManager.ScaleY;
                TranslateX = StateManager.TranslateX;
                TranslateY = StateManager.TranslateY;
                Rotation = StateManager.Rotation;
                LastState = CurrentState;
            }
        }
    }
}
