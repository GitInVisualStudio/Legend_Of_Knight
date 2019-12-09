using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math
{
    public class Rectangle
    {
        private Vector pos;
        private Vector size;
        public float Area
        {
            get
            {
                return Size.X * Size.Y;
            }
        }

        public Vector Size
        {
            get
            {
                return size;
            }
        }

        public Vector Pos { get => pos; set => pos = value; }
        public Vector Size { get => size; set => size = value; }
        public Vector CenterPos
        {
            get
            {
                return pos + size / 2;
            }
        }
        public float Area
        {
            get
            {
                return pos;
            }

            set
            {
                pos = value;
            }
        }

        public Rectangle(Vector pos, Vector size)
        {
            Pos = pos;
            Size = size;
        }

        public bool PointInRectangle(Vector p)
        {
            if (p.X > pos.X && p.Y > pos.Y && p.X < pos.X + size.X && p.Y < pos.Y + size.Y)
                return true;
            return false;
        }
    }
}
