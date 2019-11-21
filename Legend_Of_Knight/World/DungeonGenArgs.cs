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

        /// <summary>
        /// Deterministischer Seed für die Generierung
        /// </summary>
        public int Seed { get => seed; set => seed = value; }
        /// <summary>
        /// Größe des Dungeon in Feldern
        /// </summary>
        public Vector Size { get => size; set => size = value; }

        public int Rooms { get => rooms; set => rooms = value; }
        public Vector RoomSize { get => roomSize; set => roomSize = value; }
        /// <summary>
        /// Die Prozentzahl an redundanten Räumen, die dagelassen werden soll
        /// </summary>
        public float LeaveConnectionPercentage { get => leaveConnectionPercentage; set => leaveConnectionPercentage = value; }
        public int CorridorWidth { get => corridorWidth; set => corridorWidth = value; }

        public DungeonGenArgs()
        {
            DateTime now = DateTime.Now;
            //Seed = (((now.Year * 365 + now.Day) * 24 + now.Hour) * 60 + now.Minute) * 60 + now.Second; // standard seed als momentane Zeit in Sekunden
            Seed = 22102016;
            Size = new Vector(100, 100);
            Rooms = 4;
            RoomSize = new Vector(10, 10);
            LeaveConnectionPercentage = 0.1f;
            CorridorWidth = 3;
        }
    }
}
