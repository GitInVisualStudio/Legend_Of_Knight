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
        private Vector centerPos;

        public Vector CenterPos
        {
            get
            {
                return centerPos;
            }
            set
            {
                centerPos = value;
            }
        }
        public float Area
        {
            get
            {
                return Size.X * Size.Y;
            }
        }

        public Vector Pos
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

        public Vector Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public Rectangle(Vector pos, Vector size)
        {
            Pos = pos;
            Size = size;
            CenterPos = pos + size / 2;
        }

        public bool PointInRectangle(Vector p)
        {
            if (p.X > Pos.X && p.Y > Pos.Y && p.X < Pos.X + Size.X && p.Y < Pos.Y + Size.Y)
                return true;
            return false;
        }
    }
}
