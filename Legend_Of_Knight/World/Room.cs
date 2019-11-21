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
        private Vector size;
        private Corridor right;
        private Corridor up;
        private Corridor left;
        private Corridor down;

        public Corridor[] Connections { get => connections; set => connections = value; }
        public Vector CenterPos { get => centerPos; set => centerPos = value; }
        public Vector Size { get => size; set => size = value; }
        public Corridor Right { get => right; set => right = value; }
        public Corridor Up { get => up; set => up = value; }
        public Corridor Left { get => left; set => left = value; }
        public Corridor Down { get => down; set => down = value; }

        public Room(Field[] fields, Vector centerPos, Vector size) : base(fields)
        {
            CenterPos = centerPos;
            Size = size;
        }

        public static Room FindRoomByPosition(IEnumerable<Room> rooms, Vector position)
        {
            foreach (Room r in rooms)
                if (r.CenterPos == position)
                    return r;
            return null;
        }
    }
}
