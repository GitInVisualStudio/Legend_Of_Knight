using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;

namespace Legend_Of_Knight.World
{
    /// <summary>
    /// Abstrakte darstellung entweder eines Korridores oder eines Raumes
    /// </summary>
    public abstract class Area
    {
        // evtl. noch Array mit Entities / Gegnern?
        private Field[] fields;
        private Rectangle[] bounds;

        public Area(Field[] fields)
        {
            Fields = fields;
            foreach (Field f in fields)
                f.Area = this;
        }

        public bool PointInBounds(Vector point)
        {
            foreach (Rectangle r in bounds)
                if (r.PointInRectangle(point))
                    return true;
            return false;
        }

        public Field[] Fields
        {
            get
            {
                return fields;
            }

            set
            {
                fields = value;
            }
        }

        public Rectangle[] Bounds
        {
            get
            {
                return bounds;
            }

            set
            {
                bounds = value;
            }
        }
    }
}
