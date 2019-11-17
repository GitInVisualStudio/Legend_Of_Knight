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
            return ToDegree((float)s::Math.Atan(tan));
        }

        public static float ToRadians(float angle)
        {
            return (float)(angle * s::Math.PI / 180.0f);
        }

        public static float ToDegree(float angle)
        {
            return (float)(angle * 180.0f / s::Math.PI);
        }

        public static T Interpolate<T>(T prev, T current, float partialTicks)
        {
            dynamic var1 = prev;
            dynamic var2 = current;
            return var2 + (var1 - var2) * partialTicks;
        }
    }
}
