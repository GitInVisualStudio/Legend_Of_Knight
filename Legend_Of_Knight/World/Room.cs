using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;

namespace Legend_Of_Knight.World
{
    /// <summary>
    /// Ein Raum im Dungeon
    /// </summary>
    public class Room : Area
    {
        private Corridor[] connections;

        public Corridor[] Connections { get => connections; set => connections = value; }

        public Room(Field[] fields) : base(fields)
        {

        }
    }
}
