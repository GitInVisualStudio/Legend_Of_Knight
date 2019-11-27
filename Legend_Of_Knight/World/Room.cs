﻿using System;
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
        public Corridor[] Connections { get => connections; set => connections = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public Vector CenterPos
        {
            get
            {
                return new Vector(x + (int)(sizeX / 2), y + (int)(sizeY / 2));
            }
        }
        public int SizeX { get => sizeX; set => sizeX = value; }
        public int SizeY { get => sizeY; set => sizeY = value; }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="x">X-Koordinate des Feldes oben links</param>
        /// <param name="y">Y-Koordinate des Feldes oben links</param>
        /// <param name="size"></param>
        public Room(Field[] fields, int x, int y, int sizeX, int sizeY) : base(fields)
        {
            this.x = x;
            this.y = y;
            this.sizeX = sizeX;
            this.sizeY = sizeY;
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