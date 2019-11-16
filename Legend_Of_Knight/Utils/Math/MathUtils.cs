using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using s = System;

namespace Legend_Of_Knight.Utils.Math
{
    public class MathUtils
    {
        private static Random random = new Random();

        public static float Sin(float angle)
        {
            return (float)s::Math.Sin(ToRadians(angle));
        }

        public static float Cos(float angle)
        {
            return (float)s::Math.Cos(ToRadians(angle));
        }

        public static float Tan(float angle)
        {
            return (float)s::Math.Tan(ToRadians(angle));
        }

        public static float Asin(float sin)
        {
            return ToDegree((float)s::Math.Asin(sin));
        }

        public static float Acos(float cos)
        {
            return ToDegree((float)s::Math.Acos(cos));
        }

        public static float Atan(float tan)
        {
            return (float)s::Math.Atan(ToRadians(tan));
        }

        public static float ToRadians(float angle)
        {
            return (float)(angle * s::Math.PI / 180.0f);
        }

        public static float ToDegree(float angle)
        {
            return (float)(angle * 180.0f / s::Math.PI);
        }

        public static float Sqrt(float d)
        {
            return (float)s::Math.Sqrt(d);
        }

        public static float Pow(float basis, float exponent)
        {
            return (float)s::Math.Pow(basis, exponent);
        }

        public static float Random()
        {
            return (float)random.NextDouble();
        }

        public static float Random(float size)
        {
            return Random() * size;
        }
    }
}
