using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math.Triangulation
{
    class Point
    {
        private float x;
        private float y;
        private Line[] vLines;
        private Point[] vPoints;

        public float X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        internal Line[] VLines
        {
            get
            {
                return vLines;
            }

            set
            {
                vLines = value;
            }
        }

        internal Point[] VPoints
        {
            get
            {
                return vPoints;
            }

            set
            {
                vPoints = value;
            }
        }

        public Point(float x, float y, Point[] otherPoints = null)
        {
            this.x = x;
            this.y = y;

            if (otherPoints != null)
                CalculateVernoi(otherPoints);
        }

        public void CalculateVernoi(Point[] points)
        {

        }
    }
}
