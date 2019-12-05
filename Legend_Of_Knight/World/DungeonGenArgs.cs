﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;

namespace Legend_Of_Knight.World
{
    public class DungeonGenArgs
    {
        private int seed;
        private Vector size;
        private int rooms;
        private Vector roomSize;
        private float leaveConnectionPercentage;
        private int corridorWidth;

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

        public int Rooms
        {
            get
            {
                return rooms;
            }

            set
            {
                rooms = value;
            }
        }

        public Vector RoomSize
        {
            get
            {
                return roomSize;
            }

            set
            {
                roomSize = value;
            }
        }

        public float LeaveConnectionPercentage
        {
            get
            {
                return leaveConnectionPercentage;
            }

            set
            {
                leaveConnectionPercentage = value;
            }
        }

        public int CorridorWidth
        {
            get
            {
                return corridorWidth;
            }

            set
            {
                corridorWidth = value;
            }
        }

        public int Seed
        {
            get
            {
                return seed;
            }

            set
            {
                seed = value;
            }
        }

        /// <summary>
        /// Deterministischer Seed für die Generierung
        /// </summary>
        /// <summary>
        /// Größe des Dungeon in Feldern
        /// </summary>

        /// <summary>
        /// Die Prozentzahl an redundanten Räumen, die dagelassen werden soll
        /// </summary>

        public DungeonGenArgs()
        {
            DateTime now = DateTime.Now;
            Seed = ((((now.Year * 365 + now.Day) * 24 + now.Hour) * 60 + now.Minute) * 60 + now.Second) * 1000 + now.Millisecond; // standard seed als momentane Zeit in Miilsekunden
            //Seed = 21022002;
            Size = new Vector(100, 100);
            Rooms = 4;
            RoomSize = new Vector(30, 30);
            LeaveConnectionPercentage = 0.1f;
            CorridorWidth = 3;
        }
    }
}
