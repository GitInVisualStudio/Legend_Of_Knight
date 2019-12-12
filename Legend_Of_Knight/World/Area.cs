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
        private Field[] fields;
        private Rectangle[] bounds;

        public Field[] Fields { get { return fields; } set { fields = value; } }
        public Rectangle[] Bounds { get { return bounds; } set { bounds = value; } }

        public Area(Field[] fields)
        {
            Fields = fields;
            foreach (Field f in fields)
                f.Area = this;
        }
    }
}
