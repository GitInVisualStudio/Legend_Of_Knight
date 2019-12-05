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

        public Vector Pos { get => pos; set => pos = value; }
        public Vector Size { get => size; set => size = value; }
        public float Area
        {
            get
            {
                return size[0] * size[1];
            }
        }

        public Rectangle(Vector pos, Vector size)
        {
            Pos = pos;
            Size = size;
        }
    }
}
