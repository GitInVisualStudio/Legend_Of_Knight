using System;
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
        private int enemiesPerRoom;

        /// <summary>
        /// Seed, aufgrunddessen ein Dungeon generiert werden soll (Default: Momentane Zeit in Minuten geteilt durch die überstehenden Milisekunden)
        /// </summary>
        public int Seed { get => seed; set => seed = value; }
        /// <summary>
        /// Größe des Dungeons in Feldern (Default: 100x100)
        /// </summary>
        public Vector Size { get => size; set => size = value; }
        /// <summary>
        /// Anzahl zu generierender Räume. Es ist nicht garantiert, dass alle erstellt werden können und im Endeffekt im Dungeon auftauchen werden. (Default: 4)
        /// </summary>
        public int Rooms { get => rooms; set => rooms = value; }
        /// <summary>
        /// Die durchschnittliche Raumgröße in Feldern. Unterscheidet sich durch Zufall leicht (Default: 30x30)
        /// </summary>
        public Vector RoomSize { get => roomSize; set => roomSize = value; }
        /// <summary>
        /// Prozentzahl an redundanten Verbindungen zwischen Räumen, die erhalten bleiben sollen (Default: 0.1)
        /// </summary>
        public float LeaveConnectionPercentage { get => leaveConnectionPercentage; set => leaveConnectionPercentage = value; }
        /// <summary>
        /// Breite der verbindenden Korridore in Feldern (Default: 3)
        /// </summary>
        public int CorridorWidth { get => corridorWidth; set => corridorWidth = value; }
        /// <summary>
        /// Durchschnittliche Anzahl an Gegnern, die pro Raum spawnen sollen. Variiert leicht. (Default: 2)
        /// </summary>
        public int EnemiesPerRoom { get => enemiesPerRoom; set => enemiesPerRoom = value; }

        public DungeonGenArgs()
        {
            DateTime now = DateTime.Now;
            Seed = (((now.Year * 365 + now.Day) * 24 + now.Hour) * 60 + now.Minute) / ((60 + now.Second) * 1000 + now.Millisecond + 1); // standard seed abhängig von momentaner Zeit 
            Size = new Vector(100, 100);
            Rooms = 4;
            RoomSize = new Vector(30, 30);
            LeaveConnectionPercentage = 0.1f;
            CorridorWidth = 3;
            EnemiesPerRoom = 2;
        }

        
    }
}
