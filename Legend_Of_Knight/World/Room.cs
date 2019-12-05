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
        private int x;
        private int y;
        private int sizeX;
        private int sizeY;
        public Vector CenterPos
        {
            get
            {
                return new Vector(X + (int)(SizeX / 2), Y + (int)(SizeY / 2));
            }
        }

        public int SizeY
        {
            get
            {
                return sizeY;
            }

            set
            {
                sizeY = value;
            }
        }

        public Corridor[] Connections
        {
            get
            {
                return connections;
            }

            set
            {
                connections = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public int SizeX
        {
            get
            {
                return sizeX;
            }

            set
            {
                sizeX = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="x">X-Koordinate des Feldes oben links</param>
        /// <param name="y">Y-Koordinate des Feldes oben links</param>
        /// <param name="size"></param>
        public Room(Field[] fields, int x, int y, int sizeX, int sizeY) : base(fields)
        {
            this.X = x;
            this.Y = y;
            this.SizeX = sizeX;
            this.SizeY = sizeY;

            Bounds = new Rectangle[] 
            {
                new Rectangle(new Vector(x, y), new Vector(sizeX, sizeY))
            };
        }

        public static Room GetRoomByPosition(IEnumerable<Room> rooms, Vector pos)
        {
            foreach (Room r in rooms)
                if (r.CenterPos == pos)
                    return r;
            return null;
        }
    }
}
