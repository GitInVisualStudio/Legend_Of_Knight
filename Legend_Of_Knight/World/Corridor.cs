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
        private Room[] connects;

        public Room[] Connects { get => connects; set => connects = value; }

        public Corridor(Field[] fields) : base(fields)
        {

        }
    }
}
