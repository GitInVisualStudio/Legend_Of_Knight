using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils
{
    public class StateManager
    {
        private static Graphics g;
        private static Color color;
        private static Font font = new Font("Arial", 12);

        public static void Update(Graphics g)
        {
            StateManager.g = g;
        }

        public static void DrawString(string text, float x, float y)
        {
            g.DrawString(text, font, new SolidBrush(color), x, y);
        }

        public static void DrawImage(Image img, float x, float y)
        {
            g.DrawImage(img, x, y);
        }

        public static void DrawRect(float x, float y, float width, float height)
        {
            g.DrawRectangle(new Pen(new SolidBrush(color)), x, y, width, height);
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
            StateManager.Color(r, g, b, 255);
        }

        public static void Color(int r, int g, int b, int a)
        {
            color = System.Drawing.Color.FromArgb(a, r, g, b);
        }
    }
}
