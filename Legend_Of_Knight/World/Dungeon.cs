using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;

namespace Legend_Of_Knight.World
{
    public class Dungeon
    {
        private Field[,] fields;

        public Field[,] Fields { get => fields; set => fields = value; }

        public Dungeon(int width, int height, DungeonGenArgs args = null)
        {
            Fields = new Field[width, height];
            for (int x = 0; x < Fields.GetLength(0); x++)
            {
                for (int y = 0; y < Fields.GetLength(1); y++)
                {
                    Fields[x, y] = new Field(null);
                }
            }

            args = args == null ? new DungeonGenArgs() : args;
            Generate(args);
        }

        public void Generate(DungeonGenArgs args)
        {
            List<Room> rooms = new List<Room>();
            CRandom rnd = new CRandom(args.Seed);
            int roomCount = (int)(args.Size.X * args.Size.Y * args.RoomPercentage / args.RoomSize); // wie viele Räume erstellt werden sollen
            int roomSizeAvg = (int)(args.Size.X * args.Size.Y * args.RoomSize); // wie groß ein Raum durchschnittlich zu sein hat
            for (int i = 0; i < roomCount; i++)
            {
                int sizeX = (int)(rnd.NextFloatGaussian() * 0.25f * args.Size.X); // berechnet die Breite des Raumes zufällig
                int sizeY = (int)(roomSizeAvg * rnd.NextFloatGaussian(0.4f) / sizeX); // errechnet anhand der Breite die Höhe, ebenfalls leicht durch Zufall beeinflusst
                Vector size = new Vector(sizeX, sizeY); 

                for (int j = 0; j < 5; j++) // versucht 5 mal, den Raum zu platzieren
                {
                    int posX = (int)(rnd.NextFloat() * args.Size.X);
                    int posY = (int)(rnd.NextFloat() * args.Size.Y);
                    Vector pos = new Vector(posX, posY);
                    Vector centerPos = new Vector(posX + sizeY / 2, posY + sizeY / 2);

                    Field[] occFields = GetFields(pos, size); // alle Felder, die von diesem Raum belegt werden würden
                    Room room = new Room(occFields, centerPos, size);
                    if (occFields.All(x => x.Area == null)) // falls die gesamte Fläche des Raumes noch unbelegt sind
                    {
                        occFields.ToList().ForEach(x => x.Area = room); // belegt die Felder
                        rooms.Add(room);
                    }
                }


            }
        }

        public Field[] GetFields(Vector pos, Vector size)
        {
            List<Field> res = new List<Field>();
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    res.Add(Fields[(int)pos.X + x, (int)pos.Y + y]);
                }
            }
            return res.ToArray();
        }
    }
}
