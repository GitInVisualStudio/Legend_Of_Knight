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
    }
}
