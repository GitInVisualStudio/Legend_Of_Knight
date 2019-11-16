using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Render
{
    public class StateManager
    {
        private static Graphics g;
        private static State state = new State(true);

        public static State State => state;
        public static Graphics Graphics => g;
        public static float ScaleX => state.ScaleX;
        public static float ScaleY => state.ScaleY;
        public static float TranslateX => state.TranslateX;
        public static float TranslateY => state.TranslateY;
        public static float Rotation => state.Rotation;
        public static Color Color => state.Color;
        public static Font Font => state.Font;

        /// <summary>
        /// Updated die Graphics-Instanz zum Zeichnen
        /// </summary>
        /// <param name="g"></param>
        public static void Update(Graphics g)
        {
            g.InterpolationMode = InterpolationMode.NearestNeighbor; //Muss ich mit miriam noch besprechen
            StateManager.g = g;
        }

        /// <summary>
        /// Zeichnet einen String
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DrawString(string text, float x, float y)
        {
            g.DrawString(text, Font, new SolidBrush(Color), x, y);
        }

        public static void DrawString(string text, Vector position)
        {
            DrawString(text, position.X, position.Y);
        }

        /// <summary>
        /// Zeichnet ein Bild
        /// </summary>
        /// <param name="img"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void DrawImage(Bitmap img, float x, float y)
        {
            g.DrawImage(img, x, y);
        }

        public static void DrawImage(Bitmap map, Vector pos)
        {
            g.DrawImage(map, pos.X, pos.Y);
        }

        /// <summary>
        /// Zeichnet ein Rechteck
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawRect(float x, float y, float width, float height)
        {
            g.DrawRectangle(new Pen(new SolidBrush(Color)), x, y, width, height);
        }

        public static void DrawRect(Vector position, float width, float height)
        {
            DrawRect(position.X, position.Y, width, height);
        }

        /// <summary>
        /// Zeichnet eine Linie
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        public static void DrawLine(float x, float y, float x1, float y1, float width = 1)
        {
            g.DrawLine(new Pen(new SolidBrush(Color), width), x, y, x1, y1);
        }

        public static void DrawLine(Vector v1, Vector v2, float width = 1)
        {
            DrawLine(v1.X, v1.Y, v2.X, v2.Y, width);
        }

        /// <summary>
        /// Dreht die Transformation
        /// </summary>
        /// <param name="angle">In Grad</param>
        public static void Rotate(float angle)
        {
            g.RotateTransform(angle);
            state.Rotation = angle;
        }

        /// <summary>
        /// Transformiert die Matrix zu einem bestimmten Punkt
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Translate(float x, float y)
        {
            g.TranslateTransform(x, y);
            State.TranslateX = x;
            State.TranslateY = y;
        }

        public static void Translate(Vector vector)
        {
            g.TranslateTransform(vector.X, vector.Y);
            State.TranslateX = vector.X;
            State.TranslateY = vector.Y;
        }

        /// <summary>
        /// Skaliert die Transformation
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Scale(float x, float y)
        {
            g.ScaleTransform(x, y);
            state.ScaleX = x;
            state.ScaleY = y;
        }

        public static void Scale(float x)
        {
            Scale(x, x);
        }

        /// <summary>
        /// Erstellt ein neues State, damit die Transformation unverändert wiederhergestellt werden kan
        /// </summary>
        public static void Push()
        {
            State state = new State();
            StateManager.state = state;
        }

        /// <summary>
        /// Stellt die letzt Transformation wieder her
        /// </summary>
        public static void Pop()
        {
            state = state.PrevState;
            g.ResetTransform();
            Translate(-state.TranslateX, -state.TranslateY);
            Scale(state.ScaleX, state.ScaleY);
            Rotate(state.Rotation);
        }

        /// <summary>
        /// Gibt die String-Breite zurück
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float GetStringWidth(string s)
        {
            return GetStringSize(s).Width;
        }

        /// <summary>
        /// Gibt die String-Höhe zurück
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static float GetStringHeight(string s)
        {
            return GetStringSize(s).Height;
        }

        /// <summary>
        /// Gibt die Größe des String wieder
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static SizeF GetStringSize(string s)
        {
            return g.MeasureString(s, Font);
        }

        /// <summary>
        /// Setzt die Farbe mit der Gezeichnet werden soll
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public static void SetColor(int r, int g, int b)
        {
            SetColor(r, g, b, 255);
        }

        public static void SetColor(int r, int g, int b, int a)
        {
            state.Color = Color.FromArgb(a, r, g, b);
        }
    }
}
