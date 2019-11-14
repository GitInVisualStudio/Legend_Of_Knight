using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using s = System;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    class Line
    {
        private float offset;
        private float angle;

        public float Offset
        {
            get
            {
                return offset;
            }

            set
            {
                offset = value;
            }
        }

        public float Angle
        {
            get
            {
                return angle;
            }

            set
            {
                angle = value;
            }
        }

        public Line(float offset, float angle)
        {
            this.offset = offset;
            this.angle = angle;
        }

        public Line(Vector point, float angle)
        {
            this.offset = -MathUtils.Tan(angle) * point.X + point.Y;
            this.angle = angle;
        }

        public Vector FindIntersect(Line b)
        {
            if (b.angle % 360 == this.angle % 360)
                throw new LinesParallelException();

            float x = (b.offset - this.offset) / (MathUtils.Tan(this.angle) - MathUtils.Tan(b.angle));
            float y = MathUtils.Tan(this.angle) * x + offset;
            return new Vector(x, y);
        }

        class LinesParallelException : Exception 
        {
            public LinesParallelException() : base("Lines were parallel, so an intersect could not be determined")
            {

            }
        }

    }
}
