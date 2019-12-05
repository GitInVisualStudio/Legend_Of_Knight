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
                return Size[0] * Size[1];
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

        public Rectangle(Vector pos, Vector size)
        {
            this.pos = pos;
            this.size = size;
        }
    }
}
