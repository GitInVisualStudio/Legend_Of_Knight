using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils
{
    struct Vector
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

        public Vector(params float[] p)
        {
            values = new float[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                values[i] = p[i];
            }
        }

        public Vector(int dimensions)
        {
            values = new float[dimensions];
        }

        public Vector Clone()
        {
            return new Vector(values);
        }

        /// <summary>
        /// Berechnet das Produkt zweier Vektoren
        /// </summary>
        public static float operator *(Vector a, Vector b)
        {
            if (a.Values.Length != b.Values.Length)
                throw new VectorSizeNotEqualException();

            float res = 0;
            for (int i = 0; i < a.Values.Length; i++)
                res += a[i] * b[i];

            return res;
        }

        /// <summary>
        /// Skaliert einen Vektor mit einem Faktor
        /// </summary>
        public static Vector operator * (Vector a, float b)
        {
            Vector res = a.Clone();
            for (int i = 0; i < a.Values.Length; i++)
                res[i] *= b;

            return res;
        }

        public static Vector operator + (Vector a, Vector b)
        {
            if (a.Values.Length != b.Values.Length)
                throw new VectorSizeNotEqualException();

            Vector res = new Vector(a.Values.Length);
            for (int i = 0; i < a.Values.Length; i++)
                res[i] = a[i] + b[i];

            return res;
        }
    }
}
