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
        public Field[] Fields { get => fields; set => fields = value; }

        public Area(Field[] fields)
        {
            Fields = fields;
        }
    }
}
