using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using s = System;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    public class Triangle
    {
        private Vector a;
        private Vector b;
        private Vector c;

        private Edge ab;
        private Edge bc;
        private Edge ac;

        private float alpha;
        private float beta;
        private float gamma;

        private Vector circumcenter;
        private float radius;

        public Edge[] Edges
        {
            get
            {
                return new Edge[] { Ab, Bc, Ac };
            }
        }

        public Vector A
        {
            get
            {
                return a;
            }

            set
            {
                a = value;
            }
        }

        public Vector B
        {
            get
            {
                return b;
            }

            set
            {
                b = value;
            }
        }

        public Vector C
        {
            get
            {
                return c;
            }

            set
            {
                c = value;
            }
        }

        public Edge Ab
        {
            get
            {
                return ab;
            }

            set
            {
                ab = value;
            }
        }

        public Edge Bc
        {
            get
            {
                return bc;
            }

            set
            {
                bc = value;
            }
        }

        public Edge Ac
        {
            get
            {
                return ac;
            }

            set
            {
                ac = value;
            }
        }

        public Triangle(Vector a, Vector b, Vector c)
        {
            A = a;
            B = b;
            C = c;

            Ab = new Edge(a, b);
            Bc = new Edge(b, c);
            Ac = new Edge(a, c);

            alpha = MathUtils.Acos((MathUtils.Pow(Bc.Length, 2) - MathUtils.Pow(Ac.Length, 2) - MathUtils.Pow(Ab.Length, 2)) / (-2 * Ac.Length * Ab.Length)); // Winkelberechnung mit Kosinussatz
            beta = MathUtils.Acos((MathUtils.Pow(Ac.Length, 2) - MathUtils.Pow(Bc.Length, 2) - MathUtils.Pow(Ab.Length, 2)) / (-2 * Bc.Length * Ab.Length));
            gamma = MathUtils.Acos((MathUtils.Pow(Ab.Length, 2) - MathUtils.Pow(Bc.Length, 2) - MathUtils.Pow(Ac.Length, 2)) / (-2 * Bc.Length * Ac.Length));

            float ccX = (A.X * MathUtils.Sin(2 * alpha) + B.X * MathUtils.Sin(2 * beta) + C.X * MathUtils.Sin(2 * gamma)) / (MathUtils.Sin(2 * alpha) + MathUtils.Sin(2 * beta) + MathUtils.Sin(2 * gamma));
            float ccY = (A.Y * MathUtils.Sin(2 * alpha) + B.Y * MathUtils.Sin(2 * beta) + C.Y * MathUtils.Sin(2 * gamma)) / (MathUtils.Sin(2 * alpha) + MathUtils.Sin(2 * beta) + MathUtils.Sin(2 * gamma));
            circumcenter = new Vector(ccX, ccY);
            radius = MathUtils.Sqrt(MathUtils.Pow(A.X - circumcenter.X, 2) + MathUtils.Pow(A.Y - circumcenter.Y, 2));
        }

        public bool PointInCircumcircle(Vector point)
        {
            return MathUtils.Sqrt(MathUtils.Pow(point.X - circumcenter.X, 2) + MathUtils.Pow(point.Y - circumcenter.Y, 2)) < radius;
        }

        public bool ContainsPoint(Vector point)
        {
            return A.Equals(point) || B.Equals(point) || C.Equals(point);
        }
    }
}
