using System;
using S = System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Math.Triangulation;

namespace Legend_Of_Knight.World
{
    public class Dungeon
    {
        private Field[,] fields;
        private MinimumSpanningTree mst;
        private Room[] rooms;
        public Field[,] Fields { get => fields; set => fields = value; }
        public MinimumSpanningTree Mst { get => mst; set => mst = value; }
        public Room[] Rooms { get => rooms; set => rooms = value; }

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

            args = args ?? new DungeonGenArgs();
            Generate(args);
        }

        public void Generate(DungeonGenArgs args)
        {
            List<Room> tempRooms = new List<Room>();
            CRandom rnd = new CRandom(args.Seed);
            for (int i = 0; i < args.Rooms; i++)
            {
                int sizeX = (int)(rnd.NextFloatGaussian(0.5f) * args.RoomSize.X); // berechnet die Breite des Raumes zufällig
                sizeX += sizeX % 2 == 0 ? 1 : 0;
                int sizeY = (int)(rnd.NextFloatGaussian(0.5f) * args.RoomSize.Y); // errechnet die Höhe des Raumes, ebenfalls leicht durch Zufall beeinflusst
                sizeY += sizeY % 2 == 0 ? 1 : 0;
                Vector size = new Vector(sizeX, sizeY); 

                for (int j = 0; j < 5; j++) // versucht 5 mal, den Raum zu platzieren
                {
                    int posX = (int)(rnd.NextFloat() * args.Size.X);
                    int posY = (int)(rnd.NextFloat() * args.Size.Y);
                    Vector pos = new Vector(posX, posY);
                    Vector centerPos = new Vector(pos.X + (int)(size.X / 2) - 1, pos.Y + (int)(size.Y / 2) -1);

                    Field[] occFields = GetFields(pos, size); // alle Felder, die von diesem Raum belegt werden würden
                    if (occFields == null)
                        continue;

                    Room room = new Room(occFields, centerPos, size);
                    if (occFields.All(x => x.Area == null)) // falls die gesamte Fläche des Raumes noch unbelegt ist
                    {
                        occFields.ToList().ForEach(x => x.Area = room); // belegt die Felder
                        tempRooms.Add(room);
                    }
                }
            }
            rooms = tempRooms.ToArray();

            Vector[] points = new Vector[tempRooms.Count];
            for (int i = 0; i < points.Length; i++)
                points[i] = tempRooms[i].CenterPos;

            DelaunayTriangulation triang = new DelaunayTriangulation(args.Size, points);
            mst = new MinimumSpanningTree(triang);

            List<Room[]> connections = new List<Room[]>();
            foreach (Edge e in mst.Edges)
                if (mst.Edges.Contains(e) || rnd.NextFloat() < args.LeaveConnectionPercentage)
                    connections.Add(new Room[] { Room.FindRoomByPosition(tempRooms, e.A), Room.FindRoomByPosition(tempRooms, e.B) });

            List<Corridor> corridors = new List<Corridor>();
            foreach (Room[] connection in connections)
            {
                Vector aL = new Vector(connection[0].CenterPos.X - (connection[0].Size.X / 2), connection[0].CenterPos.Y - (connection[0].Size.Y / 2));
                Vector bL = new Vector(connection[1].CenterPos.X - (connection[1].Size.X / 2), connection[1].CenterPos.Y - (connection[1].Size.Y / 2));

                Room left = aL.X < bL.X ? connection[0] : connection[1];
                Room right = connection[0].Equals(left) ? connection[1] : connection[0];
                Room up = aL.Y < bL.Y ? connection[0] : connection[1];
                Room down = connection[0].Equals(up) ? connection[1] : connection[0];


                float overlapX = left.CenterPos.X + (left.Size.X / 2) - right.CenterPos.X - (right.Size.X / 2);
                float overlapY = up.CenterPos.Y + (up.Size.Y / 2) - down.CenterPos.Y - (down.Size.Y / 2);
                int overlapPos = -1;

                if (overlapX > args.CorridorWidth + 2)
                {
                    overlapPos = (int)(right.CenterPos.X - right.Size.X / 2);
                    Field[] fields = GetFields(new Vector(overlapPos, up.CenterPos.Y + up.Size.Y / 2), new Vector(args.CorridorWidth, down.CenterPos.Y - down.Size.Y / 2 - up.CenterPos.Y + up.Size.Y / 2));
                    Corridor c = new Corridor(up, down, fields);
                    fields.ToList().ForEach(x => x.Area = c);
                    up.Down = c;
                    down.Up = c;
                    corridors.Add(c);
                }
                else if (overlapY > args.CorridorWidth + 2)
                {
                    overlapPos = (int)(down.CenterPos.Y - down.Size.Y / 2);
                    Field[] fields = GetFields(new Vector(left.CenterPos.X + left.Size.X / 2, overlapPos), new Vector(right.CenterPos.X - right.Size.X / 2 - left.CenterPos.X + left.Size.X, args.CorridorWidth));
                    Corridor c = new Corridor(left, right, fields);
                    fields.ToList().ForEach(x => x.Area = c);
                    left.Right = c;
                    right.Left = c;
                    corridors.Add(c);
                }
            }
        }

        public Field[] GetFields(Vector pos, Vector size)
        {
            List<Field> res = new List<Field>();
            if (pos.X + size.X > fields.GetLength(0) || pos.Y + size.Y > fields.GetLength(1))
                return null;

            for (int x = 0; x < size.X; x++)
                for (int y = 0; y < size.Y; y++)
                    res.Add(Fields[(int)pos.X + x, (int)pos.Y + y]);
            return res.ToArray();
        }
    }
}
