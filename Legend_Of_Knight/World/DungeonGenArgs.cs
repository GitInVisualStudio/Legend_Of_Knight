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
        private float roomPercentage;
        private float roomSize;

        /// <summary>
        /// Deterministischer Seed für die Generierung
        /// </summary>
        public int Seed { get => seed; set => seed = value; }
        /// <summary>
        /// Größe des Dungeon in Feldern
        /// </summary>
        public Vector Size { get => size; set => size = value; }
        
        /// <summary>
        /// Näherungswert, wie viel Prozent des Dungeons am Ende mit Räumen belegt werden soll
        /// </summary>
        public float RoomPercentage { get => roomPercentage; set => roomPercentage = value; }
        /// <summary>
        /// Durchschnittswert, wie viel Prozent des Dungeons ein Raum einnehmen soll
        /// </summary>
        public float RoomSize { get => roomSize; set => roomSize = value; }

        public DungeonGenArgs()
        {
            DateTime now = DateTime.Now;
            Seed = (((now.Year * 365 + now.Day) * 24 + now.Hour) * 60 + now.Minute) * 60 + now.Second; // standard seed als momentane Zeit in Sekunden
            Size = new Vector(100, 100);
            RoomPercentage = 0.5f;
            RoomSize = 0.005f;
        }
    }
}
