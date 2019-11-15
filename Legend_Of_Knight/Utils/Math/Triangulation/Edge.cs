using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    public struct Edge
    {
        private Vector a;
        private Vector b;
        private float length;

        public Vector A { get => a; set => a = value; }
        public Vector B { get => b; set => b = value; }
        public float Length { get => length; set => length = value; }

        public Edge(Vector pointA, Vector pointB)
        {
            a = pointA;
            b = pointB;

            length = MathUtils.Sqrt(MathUtils.Pow(a.X - b.X, 2) + MathUtils.Pow(a.Y - b.Y, 2));
        }
    }
}
