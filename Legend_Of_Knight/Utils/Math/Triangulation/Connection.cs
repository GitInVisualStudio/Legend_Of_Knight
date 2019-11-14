using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    internal class Connection
    {
        private Vector a;
        private Vector b;
        private Line line;

        internal Line Line
        {
            get
            {
                return line;
            }

            set
            {
                line = value;
            }
        }

        public Connection(Vector a, Vector b)
        {
            this.a = a;
            this.b = b;

            float angle = MathUtils.Atan((a.Y - b.Y) / (a.X - b.X));
            line = new Line(a, angle);
        }

        public Line GetNormal()
        {
            float x = (a.X - b.X) / 2 + a.X;
            float y = (a.Y - b.Y) / 2 + a.Y;
            float angle = MathUtils.Atan(-1 / MathUtils.Tan(line.Angle));
            return new Line(new Vector(x, y), angle);
        }

        public bool Connects(Vector a, Vector b)
        {
            if ((a.Equals(this.a) && b.Equals(this.b)) || (b.Equals(this.a) && a.Equals(this.b)))
                return true;
            return false;
        }
    }
}
