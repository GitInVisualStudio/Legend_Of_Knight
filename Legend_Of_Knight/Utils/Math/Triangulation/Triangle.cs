﻿using System;
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

        public Vector A { get => a; set => a = value; }
        public Vector B { get => b; set => b = value; }
        public Vector C { get => c; set => c = value; }
        public Edge Ab { get => ab; set => ab = value; }
        public Edge Bc { get => bc; set => bc = value; }
        public Edge Ac { get => ac; set => ac = value; }
        public Edge[] Edges
        {
            get
            {
                return new Edge[] { ab, bc, ac };
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

            alpha = s::Math.Abs(GetGradientAngle(A, B) - GetGradientAngle(A, C));
            beta = s::Math.Abs(GetGradientAngle(B, A) - GetGradientAngle(B, C));
            gamma = 180 - alpha - beta;

            float ccX = (A.X * MathUtils.Sin(2 * alpha) + B.X * MathUtils.Sin(2 * beta) + C.X * MathUtils.Sin(2 * gamma)) / (MathUtils.Sin(2 * alpha) + MathUtils.Sin(2 * beta) + MathUtils.Sin(2 * gamma));
            float ccY = (A.Y * MathUtils.Sin(2 * alpha) + B.Y * MathUtils.Sin(2 * beta) + C.Y * MathUtils.Sin(2 * gamma)) / (MathUtils.Sin(2 * alpha) + MathUtils.Sin(2 * beta) + MathUtils.Sin(2 * gamma));
            circumcenter = new Vector(ccX, ccY);
            radius = MathUtils.Sqrt(MathUtils.Pow(A.X - circumcenter.X, 2) + MathUtils.Pow(A.Y - circumcenter.Y, 2));
        }

        private float GetGradientAngle(Vector v1, Vector v2)
        {
            return MathUtils.Atan((v1.Y - v2.Y) / (v1.X / v2.X));
        }

        public bool PointInCircumcircle(Vector point)
        {
            return MathUtils.Sqrt(MathUtils.Pow(point.X - circumcenter.X, 2) + MathUtils.Pow(point.Y - circumcenter.Y, 2)) < radius;
        }

        public bool ContainsPoint(Vector point)
        {
            return a.Equals(point) || b.Equals(point) || c.Equals(point);
        }
    }
}