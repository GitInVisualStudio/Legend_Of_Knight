using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.World
{
    /// <summary>
    /// Ein Korridor der einen oder mehrere Räume miteinander verbindet.
    /// </summary>
    public class Corridor : Area
    {
        private Room a;
        private Room b;

        public Corridor(Room a, Room b, Vector posA, Vector sizeA, Vector posB, Vector sizeB, Field[] fields) : base(fields)
        {
            A = a;
            B = b;

            Bounds = new Rectangle[]
            {
                new Rectangle(posA, sizeA),
                new Rectangle(posB, sizeB)
            };
        }

        public Corridor(Room a, Room b, Vector pos, Vector size, Field[] fields) : base(fields)
        {
            A = a;
            B = b;

            Bounds = new Rectangle[]
            {
                new Rectangle(pos, size)
            };
        }

        public Room A
        {
            get
            {
                return a;
            }

            set
            {
                a = value;
            }
        }

        public Room B
        {
            get
            {
                return b;
            }

            set
            {
                b = value;
            }
        }
    }
}
