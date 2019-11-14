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
        private Vector centerPos;

        public Corridor[] Connections { get => connections; set => connections = value; }
        public Vector CenterPos
        {
            get
            {
                return centerPos;
            }

            set
            {
                centerPos = value;
            }
        }

        public Room(Field[] fields, Vector pos, Vector size) : base(fields)
        {
            CenterPos = new Vector(pos.X + size.Y / 2, pos.Y + size.Y / 2);
        }
    }
}
