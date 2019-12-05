using System;
using S = System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Math.Triangulation;
using Legend_Of_Knight.World;
using Legend_Of_Knight.Utils.Animations;

namespace Legend_Of_Knight.World
{
    public class Dungeon
    {
        private Field[,] fields;
        private MinimumSpanningTree mst;
        private Room[] rooms;
        private DungeonGenArgs args;
        private CRandom rnd;

        public Field[,] Fields { get => fields; set => fields = value; }
        public MinimumSpanningTree Mst { get => mst; set => mst = value; }
        public Room[] Rooms { get => rooms; set => rooms = value; }

        public Dungeon(DungeonGenArgs args = null)
        {
            this.args = args ?? new DungeonGenArgs();
            rnd = new CRandom(this.args.Seed);
            Fields = new Field[(int)this.args.Size.X, (int)this.args.Size.Y];
            for (int x = 0; x < Fields.GetLength(0); x++)
            {
                for (int y = 0; y < Fields.GetLength(1); y++)
                {
                    Fields[x, y] = new Field(null, x, y, rnd);
                }
            }

            Generate();
        }

        public void Generate()
        {
            // implementiert annhähernd nach https://www.gamasutra.com/blogs/AAdonaac/20150903/252889/Procedural_Dungeon_Generation_Algorithm.php
            List<Room> rooms = new List<Room>(); // erstes Erstellen der Räume
            for (int i = 0; i < args.Rooms; i++)
            {
                Room r = MakeRoom();
                if (r != null)
                    rooms.Add(r);
            }

            Vector[] centerPoints = new Vector[rooms.Count];
            for (int i = 0; i < rooms.Count; i++)
                centerPoints[i] = rooms[i].CenterPos;
            DelaunayTriangulation triang = new DelaunayTriangulation(new Vector(fields.GetLength(0), fields.GetLength(1)), centerPoints);
            MinimumSpanningTree mst = new MinimumSpanningTree(triang);
            List<Edge> edges = new List<Edge>();
            edges.AddRange(mst.Edges);
            edges.AddRange(rnd.PickElements(triang.Edges.Where(x => !mst.Edges.Contains(x)), args.LeaveConnectionPercentage));
            List<Corridor> corridors = new List<Corridor>();
            foreach (Edge e in edges)
                corridors.Add(ConnectRooms(Room.GetRoomByPosition(rooms, e.A), Room.GetRoomByPosition(rooms, e.B)));

            try
            {
                for (int x = 0; x < fields.GetLength(0); x++)
                    for (int y = 0; y < fields.GetLength(1); y++)
                        fields[x, y].SetFieldTypeAndAnimation(fields);
            }
            catch (FieldAloneException) // DEBUG
            {
                Console.WriteLine("Something went wrong! Seed: " + args.Seed);
            }
        }

        private Room MakeRoom(int depth = 0)
        {
            if (depth > 5)
                return null;

            int posX = (int)(rnd.NextFloat() * fields.GetLength(0));
            int posY = (int)(rnd.NextFloat() * fields.GetLength(1));
            MakeOdd(ref posX); // um sicherzustellen, dass das Feld oben links bei ungeraden Koordinaten liegt
            MakeOdd(ref posY);

            int sizeX = (int)(rnd.NextFloatGaussian(1) * args.RoomSize.X);
            int sizeY = (int)(rnd.NextFloatGaussian(1) * args.RoomSize.Y);
            MakeOdd(ref sizeX); // um sicherzustellen, dass der Raum ungerade Maße hat
            MakeOdd(ref sizeY);

            Field[] occFields = GetFields(posX, posY, sizeX, sizeY);
            if (occFields == null || occFields.Any(x => x.Area != null))
                return MakeRoom(depth + 1);
            return new Room(occFields, posX, posY, sizeX, sizeY);
        }

        private Corridor ConnectRooms(Room a, Room b)
        {
            int startX;
            int startY;
            int sizeX;
            int sizeY;

            Room up = a.Y < b.Y ? a : b;
            Room down = up.Equals(a) ? b : a;

            Room left = a.X < b.X ? a : b;
            Room right = left.Equals(a) ? b : a;

            int avgX = (int)MathUtils.Average(new float[] { a.CenterPos.X, b.CenterPos.X });
            if (avgX > a.X + (int)(args.CorridorWidth / 2) && avgX < a.X + a.SizeX - 1 - (int)(args.CorridorWidth / 2) && avgX > b.X + (int)(args.CorridorWidth / 2) && avgX < b.X + b.SizeX - 1 - (int)(args.CorridorWidth / 2)) // gerade vertikale Verbindung möglich
            {
                startX = avgX - (int)(args.CorridorWidth / 2);
                startY = up.Y + up.SizeY;
                sizeX = args.CorridorWidth;
                sizeY = down.Y - startY;
                Field[] fs = GetFields(startX, startY, sizeX, sizeY);
                return new Corridor(a, b, fs);
            }

            int avgY = (int)MathUtils.Average(new float[] { a.CenterPos.Y, b.CenterPos.Y });
            if (avgY > a.Y + (int)(args.CorridorWidth / 2) && avgY < a.Y + a.SizeY - 1 - (int)(args.CorridorWidth / 2) && avgY > b.Y + (int)(args.CorridorWidth / 2) && avgY < b.Y + b.SizeY - 1 - (int)(args.CorridorWidth / 2))
            {
                startX = left.X + left.SizeX;
                startY = avgY - (int)(args.CorridorWidth / 2);
                sizeX = right.X - startX;
                sizeY = args.CorridorWidth;
                Field[] fs = GetFields(startX, startY, sizeX, sizeY);
                return new Corridor(a, b, fs);
            }

            List<Field> cFields = new List<Field>();
            Corridor c;
            if (rnd.NextFloat() > 0.5)
                c = MakeTrueL(left, up, right, down);
            else
                c = MakeTurnedL(left, up, right, down);
            return c;
        }

        private Corridor MakeTrueL(Room left, Room up, Room right, Room down, bool beforeFailed = false)
        {
            int startX;
            int startY;
            int sizeX;
            int sizeY;
            List<Field> cFields = new List<Field>();
            if (left == down) // felder von links nach rechts, dann von unten nach oben nehmen
            {
                startX = left.X + left.SizeX; // von links nach rechts
                startY = (int)left.CenterPos.Y - args.CorridorWidth / 2;
                sizeX = (int)right.CenterPos.X - startX;
                sizeY = args.CorridorWidth;
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));

                startX += sizeX - args.CorridorWidth / 2; // von unten nach oben
                startY = (int)up.CenterPos.Y;
                sizeX = args.CorridorWidth;
                sizeY = (int)down.CenterPos.Y - startY + (int)Math.Ceiling(args.CorridorWidth / 2.0f);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));
            }
            else
            {
                startX = left.X + left.SizeX;
                startY = (int)left.CenterPos.Y - args.CorridorWidth / 2;
                sizeX = (int)right.CenterPos.X - startX;
                sizeY = args.CorridorWidth;
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));

                startX = (int)right.CenterPos.X - args.CorridorWidth / 2;
                sizeX = args.CorridorWidth;
                sizeY = right.Y - startY;
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));
            }

            if (cFields.Any(x => x.Area != null) && !beforeFailed)
                return MakeTurnedL(left, up, right, down, true);
            else if (cFields.Any(x => x.Area != null))
                return null;
            else
                return new Corridor(left, right, cFields.ToArray());
        }

        private Corridor MakeTurnedL(Room left, Room up, Room right, Room down, bool beforeFailed = false)
        {
            List<Field> cFields = new List<Field>();
            if (left == down) // felder von unten nach oben, dann von links nach rechts nehmen
            {
                int startX = (int)left.CenterPos.X - args.CorridorWidth / 2; // von unten nach oben
                int startY = (int)up.CenterPos.Y;
                int sizeX = args.CorridorWidth;
                int sizeY = down.Y - startY;
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));

                startY -= args.CorridorWidth / 2; // von links nach rechts
                sizeX = right.X - startX;
                sizeY = args.CorridorWidth;
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));
            }
            else
            {
                int startX = (int)left.CenterPos.X - args.CorridorWidth / 2;
                int startY = left.Y + left.SizeY;
                int sizeX = args.CorridorWidth;
                int sizeY = (int)right.CenterPos.Y - startY;
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));

                startY = (int)right.CenterPos.Y - args.CorridorWidth / 2;
                sizeX = right.X - startX;
                sizeY = args.CorridorWidth;
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));
            }

            if (cFields.Any(x => x.Area != null) && !beforeFailed)
                return MakeTrueL(left, up, right, down, true);
            else if (cFields.Any(x => x.Area != null))
                return null;
            else
                return new Corridor(left, right, cFields.ToArray());
        }

        private Field[] GetFieldsInBounds(int startX, int startY, int sizeX, int sizeY)
        {
            List<Field> res = new List<Field>();
            for (int x = startX; x < startX + sizeX; x++)
                for (int y = startY; y < startY + sizeY; y++)
                    if (!OutOfBounds(x, y))
                        res.Add(fields[x, y]);
            return res.ToArray();
        }

        private void MakeOdd(ref int i)
        {
            i += i % 2 == 0 ? 1 : 0;
        }

        public Field[] GetFields(int startX, int startY, int sizeX, int sizeY)
        {
            List<Field> res = new List<Field>();
            if (OutOfBounds(startX + sizeX, startY + sizeY))
                return null;

            for (int x = 0; x < sizeX; x++)
                for (int y = 0; y < sizeY; y++)
                    res.Add(Fields[(int)startX + x, (int)startY + y]);
            return res.ToArray();
        }

        private bool OutOfBounds(int x, int y)
        {
            if (x < 0 || x >= fields.GetLength(0) || y < 0 || y >= fields.GetLength(1))
                return true;
            return false;
        }
    }
}
