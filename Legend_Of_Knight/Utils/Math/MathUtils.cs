﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using formath = System;

namespace Legend_Of_Knight.Utils.Math
{
    public class MathUtils
    {

        public static float Sin(float angle)
        {
            return (float)formath::Math.Sin(ToRadians(angle));
        }

        public static float Cos(float angle)
        {
            return (float)formath::Math.Cos(ToRadians(angle));
        }

        public static float Tan(float angle)
        {
            return (float)formath::Math.Tan(ToRadians(angle));
        }

        public static float Asin(float sin)
        {
            return ToDegree((float)formath::Math.Asin(sin));
        }

        public static float Acos(float cos)
        {
            return ToDegree((float)formath::Math.Acos(cos));
        }

        public static float Atan(float tan)
        {
            return (float)formath::Math.Atan(ToRadians(tan));
        }

        public static float ToRadians(float angle)
        {
            return (float)(angle * formath::Math.PI / 180.0f);
        }

        public static float ToDegree(float angle)
        {
            return (float)(angle * 180.0f / formath::Math.PI);
        }

        public static float Sqrt(float d)
        {
            return (float)formath::Math.Sqrt(d);
        }

        public static float Pow(float basis, float exponent)
        {
            return (float)formath::Math.Pow(basis, exponent);
        }
    }
}
