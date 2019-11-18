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

        public Vector Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
            }
        }

        public Room(Field[] fields, Vector pos, Vector size) : base(fields)
        {
            CenterPos = new Vector(pos.X + size.Y / 2, pos.Y + size.Y / 2);
            Size = size;
        }

        public static Room FindRoomByPosition(IEnumerable<Room> rooms, Vector position)
        {
            foreach (Room r in rooms)
                if (r.CenterPos == position)
                    return r;
            return null;
        }

        public static Vector FindOverlap(Room a, Room b) // TODO: test
        {
            Vector aL = new Vector(a.CenterPos.X - a.Size.X / 2, a.CenterPos.Y - a.Size.Y / 2);
            Vector bL = new Vector(b.CenterPos.X - b.Size.X / 2, b.CenterPos.Y - b.Size.Y / 2);

            Room left = aL.X < bL.X ? a : b;
            Room right = a.Equals(left) ? b : a;
            Room up = aL.Y < bL.Y ? a : b;
            Room down = a.Equals(up) ? b : a;

            float overlapX = left.CenterPos.X + left.Size.X / 2 - right.CenterPos.X - right.Size.X / 2;
            float overlapY = up.CenterPos.Y + up.Size.Y / 2 - down.CenterPos.Y - down.Size.Y / 2;
            return new Vector(overlapX, overlapY);
        }
    }
}
