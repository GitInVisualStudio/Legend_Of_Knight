using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math
{
    public struct Vector
    {
        private float[] values;

        public float[] Values
        {
            get
            {
                return values;
            }

            set
            {
                values = value;
            }
        }

        public float this[int i]
        {
            get
            {
                return values[i];
            }

            set
            {
                values[i] = value;
            }
        }

        public float X
        {
            get
            {
                if (values.Length < 2)
                    throw new VectorNot2DException();

                return values[0];
            }

            set
            {
                if (values.Length < 2)
                    throw new VectorNot2DException();

                values[0] = value;
            }
        }

        public float Y
        {
            get
            {
                if (values.Length < 2)
                    throw new VectorNot2DException();

                return values[1];
            }

            set
            {
                if (values.Length < 2)
                    throw new VectorNot2DException();

                values[1] = value;
            }
        }

        public Vector(params float[] p)
        {
            values = new float[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                values[i] = p[i];
            }
        }

        public float Length
        {
            get
            {
                float value = 0;
                foreach (float f in values)
                    value += f * f;
                return (float)System.Math.Sqrt(value);
            }
        }

        public int Dimensions => values.Length;

        public Vector(int dimensions)
        {
            values = new float[dimensions];
        }

        public void Normalize()
        {
            this /= Length;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] + v2[i];
            return newVector;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] - v2[i];
            return newVector;
        }

        public static Vector operator *(Vector v1, Vector v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] * v2[i];
            return newVector;
        }

        public static Vector operator *(Vector v1, float v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] * v2;
            return newVector;
        }

        public static Vector operator /(Vector v1, float v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] / v2;
            return newVector;
        }

        public override string ToString()
        {
            return "X: " + X + " Y: " + Y;
        }

        public Vector Copy()
        {
            return new Vector(values);
        }

        class VectorNot2DException : Exception
        {
            public VectorNot2DException() : base("Vector did not have 2 or more Dimensions")
            {

            }
        }
    }
}