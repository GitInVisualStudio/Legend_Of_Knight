﻿using System;
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

        public Corridor(Room a, Room b, Field[] fields) : base(fields)
        {
            A = a;
            B = b;
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
