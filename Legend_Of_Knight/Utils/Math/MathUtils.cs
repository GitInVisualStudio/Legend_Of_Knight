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

        public static float Sin(float angle) => (float)s::Math.Sin(ToRadians(angle));
        

        public static float Cos(float angle) => (float)s::Math.Cos(ToRadians(angle));
        

        public static float Tan(float angle) => (float)s::Math.Tan(ToRadians(angle));
        

        public static float Asin(float sin) => ToDegree((float)s::Math.Asin(sin));
        

        public static float Acos(float cos) => ToDegree((float)s::Math.Acos(cos));
        

        public static float Atan(float tan) => ToDegree((float)s::Math.Atan(tan));
        

        public static float ToRadians(float angle) => (float)(angle * s::Math.PI / 180.0f);
        

        public static float ToDegree(float angle) => (float)(angle * 180.0f / s::Math.PI);

        public static Vector GetRotation(Vector position, float angle)
        {
            return new Vector(position.X * Cos(angle) - position.Y * Sin(angle), position.X * Sin(angle) + position.Y * Cos(angle));
        }

        public static dynamic Interpolate(dynamic prev, dynamic current, float partialTicks)
        {
            return current + (prev - current) * partialTicks;
        }

        public static float Sqrt(float d)
        {
            return (float)s::Math.Sqrt(d);
        }

        public static float Pow(float basis, float exponent)
        {
            return (float)s::Math.Pow(basis, exponent);
        }

        public static float Average(IEnumerable<float> list)
        {
            return list.Sum() / list.Count();
        }

        public static Vector Abs(Vector a)
        {
            float[] values = new float[a.Values.Length];
            for (int i = 0; i < values.Length; i++)
                values[i] = s::Math.Abs(a[i]);
            return new Vector(values);
        }
    }
}
