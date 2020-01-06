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
        private Room[] rooms;
        private DungeonGenArgs args;
        private Rectangle[] bounds;
        private Room startRoom;

        private CRandom rnd;

        public Field[,] Fields
        {
            get
            {
                return fields;
            }

            set
            {
                fields = value;
            }
        }

        public Room[] Rooms
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

        public DungeonGenArgs Args
        {
            get
            {
                return args;
            }

            set
            {
                args = value;
            }
        }

        public Rectangle[] Bounds
        {
            get
            {
                return bounds;
            }

            set
            {
                bounds = value;
            }
        }

        public Room StartRoom { get => startRoom; set => startRoom = value; }

        public Dungeon(DungeonGenArgs args = null)
        {
            this.Args = args ?? new DungeonGenArgs();
            rnd = new CRandom(this.Args.Seed);
            Fields = new Field[(int)this.Args.Size.X, (int)this.Args.Size.Y];
            for (int x = 0; x < Fields.GetLength(0); x++)
            {
                for (int y = 0; y < Fields.GetLength(1); y++)
                {
                    Fields[x, y] = new Field(x, y, rnd);
                }
            }

            Generate();
        }

        public void Generate()
        {
            // Dungeongeneration implementiert annhähernd nach https://www.gamasutra.com/blogs/AAdonaac/20150903/252889/Procedural_Dungeon_Generation_Algorithm.php
            List<Room> rooms = new List<Room>(); // erstes Erstellen der Räume
            for (int i = 0; i < Args.Rooms; i++)
            {
                Room r = MakeRoom();
                if (r != null)
                    rooms.Add(r);
            }
            
            Vector[] centerPoints = new Vector[rooms.Count];
            for (int i = 0; i < rooms.Count; i++)
                centerPoints[i] = rooms[i].CenterPos;
            DelaunayTriangulation triang = new DelaunayTriangulation(new Vector(Fields.GetLength(0), Fields.GetLength(1)), centerPoints);
            MinimumSpanningTree mst = new MinimumSpanningTree(triang);
            List<Edge> edges = new List<Edge>();
            edges.AddRange(mst.Edges);
            edges.AddRange(rnd.PickElements(triang.Edges.Where(x => !mst.Edges.Contains(x)), Args.LeaveConnectionPercentage)); // lässt manche redundante Verbindungen da, damit Dungeon nicht linear ist
            List<Corridor> corridors = new List<Corridor>();
            foreach (Edge e in edges) // versucht, Räume anhand der bestimmten Verbindungen mit Korridoren zu verbinden
            {
                Corridor c = ConnectRooms(Room.GetRoomByPosition(rooms, e.A), Room.GetRoomByPosition(rooms, e.B));
                if (c != null)
                {
                    corridors.Add(c);
                    c.A.Connections.Add(c);
                    c.B.Connections.Add(c);
                }
            }
            startRoom = FindRoomWithMostConnections(rooms);
            RemoveUnreacheableRooms(rooms, startRoom);
            Rooms = rooms.ToArray();

            for (int x = 0; x < Fields.GetLength(0); x++)
                for (int y = 0; y < Fields.GetLength(1); y++)
                    Fields[x, y].SetFieldTypeAndAnimation(Fields);

            List<Rectangle> b = new List<Rectangle>(); // Bounding-Rechtecke der Räume und Korridore
            foreach (Room r in rooms)
                b.AddRange(r.Bounds);
            foreach (Corridor c in corridors)
                b.AddRange(c.Bounds);
            Bounds = b.ToArray();
        }

        private Room FindRoomWithMostConnections(List<Room> rooms)
        {
            Room best = rooms[0];
            for (int i = 1; i < rooms.Count; i++)
                best = best.Connections.Count < rooms[i].Connections.Count ? rooms[i] : best;
            return best;
        }

        private void RemoveUnreacheableRooms(List<Room> rooms, Room start)
        {
            Crawl(start);
            List<Room> unreacheable = rooms.Where(x => !x.Reachable).ToList();
            unreacheable.ForEach(x => 
            {
                x.Fields.ToList().ForEach(y => y.Area = null);
                x.Connections.ForEach(y => y.Fields.ToList().ForEach(z => z.Area = null));
            });
            rooms.RemoveAll(x => unreacheable.Contains(x));
        }

        private void Crawl(Room start)
        {
            if (start.Reachable)
                return;
            start.Reachable = true;
            foreach (Corridor c in start.Connections)
                Crawl(c.A == start ? c.B : c.A);
        }

        private Room MakeRoom(int depth = 0)
        {
            if (depth > 5)
                return null;

            int posX = (int)(rnd.NextFloat() * Fields.GetLength(0));
            int posY = (int)(rnd.NextFloat() * Fields.GetLength(1));
            MakeOdd(ref posX); // um sicherzustellen, dass das Feld oben links bei ungeraden Koordinaten liegt
            MakeOdd(ref posY);

            int sizeX = (int)(rnd.NextFloatGaussian(1) * Args.RoomSize.X);
            int sizeY = (int)(rnd.NextFloatGaussian(1) * Args.RoomSize.Y);
            MakeOdd(ref sizeX); // um sicherzustellen, dass der Raum ungerade Maße hat
            MakeOdd(ref sizeY);

            Field[] occFields = GetFields(posX, posY, sizeX, sizeY);
            if (occFields == null || occFields.Any(x => x.Area != null))
                return MakeRoom(depth + 1);
            return new Room(occFields, posX, posY, sizeX, sizeY);
        }

        /// <summary>
        /// Verbindet zwei Räume mit einem Korridor. Falls möglich gerade, falls nicht in einer L-Form
        /// </summary>
        /// <returns>Gibt null zurück, wenn Verbindung nicht möglich war</returns>
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

            int avgX = (int)MathUtils.Average(new float[] { a.CenterPos.X, b.CenterPos.X }); // Durchschnittliche X-Position der zwei Mittelpunkte
            if (avgX > a.X + (int)(Args.CorridorWidth / 2) && avgX < a.X + a.SizeX - (int)(Args.CorridorWidth / 2) && avgX > b.X + (int)(Args.CorridorWidth / 2) && avgX < b.X + b.SizeX - (int)(Args.CorridorWidth / 2)) // gerade vertikale Verbindung möglich, wenn durchschnittlicher Mittelpunkt weit genug in beiden Rechtecken liegt, dass ein Korridor platz hätte
            {
                startX = avgX - (int)(Args.CorridorWidth / 2);
                startY = up.Y + up.SizeY;
                sizeX = Args.CorridorWidth;
                sizeY = down.Y - startY;
                Field[] fs = GetFields(startX, startY, sizeX, sizeY);
                return new Corridor(a, b, new Vector(startX, startY), new Vector(sizeX, sizeY), fs);
            }

            int avgY = (int)MathUtils.Average(new float[] { a.CenterPos.Y, b.CenterPos.Y }); // siehe vertikale Verbindung
            if (avgY > a.Y + (int)(Args.CorridorWidth / 2) && avgY < a.Y + a.SizeY - (int)(Args.CorridorWidth / 2) && avgY > b.Y + (int)(Args.CorridorWidth / 2) && avgY < b.Y + b.SizeY  - (int)(Args.CorridorWidth / 2))
            {
                startX = left.X + left.SizeX;
                startY = avgY - (int)(Args.CorridorWidth / 2);
                sizeX = right.X - startX;
                sizeY = Args.CorridorWidth;
                Field[] fs = GetFields(startX, startY, sizeX, sizeY);
                return new Corridor(a, b, new Vector(startX, startY), new Vector(sizeX, sizeY), fs);
            }

            List<Field> cFields = new List<Field>();
            Corridor c;
            if (rnd.NextFloat() > 0.5)
                c = MakeFlippedL(left, up, right, down);
            else
                c = MakeTrueL(left, up, right, down);
            return c;
        }

        /// <param name="left">Der linkere der zwei Räume</param>
        /// <param name="up">Der obere der zwei Räume</param>
        /// <param name="right">Der rechtere der zwei Räume</param>
        /// <param name="down">Der untere der zwei Räume</param>
        /// <param name="beforeFailed">Falls auf den Kopf gedrehte L-Verbindung bereits fehlgeschlagen ist</param>
        private Corridor MakeFlippedL(Room left, Room up, Room right, Room down, bool beforeFailed = false)
        {
            List<Field> cFields = new List<Field>();
            Vector posA;
            Vector posB;
            Vector sizeA;
            Vector sizeB;
            if (left == down) // felder von links nach rechts, dann von unten nach oben nehmen
            {
                int startX = left.X + left.SizeX; // von links nach rechts
                int startY = (int)left.CenterPos.Y - Args.CorridorWidth / 2;
                int sizeX = (int)right.CenterPos.X - startX;
                int sizeY = Args.CorridorWidth;
                posA = new Vector(startX, startY);
                sizeA = new Vector(sizeX, sizeY);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));

                startX += sizeX - Args.CorridorWidth / 2; // von unten nach oben
                startY = (int)up.CenterPos.Y;
                sizeX = Args.CorridorWidth;
                sizeY = (int)down.CenterPos.Y - startY + (int)Math.Ceiling(Args.CorridorWidth / 2.0f);
                posB = new Vector(startX, startY);
                sizeB = new Vector(sizeX, sizeY);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));
            }
            else // felder von links nach rechts, dann von oben nach unten
            {
                int startX = left.X + left.SizeX;
                int startY = (int)left.CenterPos.Y - Args.CorridorWidth / 2;
                int sizeX = (int)right.CenterPos.X - startX;
                int sizeY = Args.CorridorWidth;
                posA = new Vector(startX, startY);
                sizeA = new Vector(sizeX, sizeY);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));

                startX = (int)right.CenterPos.X - Args.CorridorWidth / 2;
                sizeX = Args.CorridorWidth;
                sizeY = right.Y - startY;
                posB = new Vector(startX, startY);
                sizeB = new Vector(sizeX, sizeY);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));
            }

            if (cFields.Any(x => x.Area != null) && !beforeFailed)
                return MakeTrueL(left, up, right, down, true);
            else if (cFields.Any(x => x.Area != null))
                return null;
            else
                return new Corridor(left, right, posA, sizeA, posB, sizeB, cFields.ToArray());
        }

        private Corridor MakeTrueL(Room left, Room up, Room right, Room down, bool beforeFailed = false)
        {
            List<Field> cFields = new List<Field>();
            Vector posA;
            Vector posB;
            Vector sizeA;
            Vector sizeB;
            if (left == down) // felder von unten nach oben, dann von links nach rechts nehmen
            {
                int startX = (int)left.CenterPos.X - Args.CorridorWidth / 2; // von unten nach oben
                int startY = (int)up.CenterPos.Y;
                int sizeX = Args.CorridorWidth;
                int sizeY = down.Y - startY;
                posA = new Vector(startX, startY);
                sizeA = new Vector(sizeX, sizeY);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));

                startY -= Args.CorridorWidth / 2; // von links nach rechts
                sizeX = right.X - startX;
                sizeY = Args.CorridorWidth;
                posB = new Vector(startX, startY);
                sizeB = new Vector(sizeX, sizeY);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));
            }
            else // Felder von oben nach unten, dann von links nach rechts nehmen
            {
                int startX = (int)left.CenterPos.X - Args.CorridorWidth / 2; // von oben nach unten
                int startY = left.Y + left.SizeY;
                int sizeX = Args.CorridorWidth;
                int sizeY = (int)right.CenterPos.Y - startY;
                posA = new Vector(startX, startY);
                sizeA = new Vector(sizeX, sizeY);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));

                startY = (int)right.CenterPos.Y - Args.CorridorWidth / 2; // von links nach rechts
                sizeX = right.X - startX;
                sizeY = Args.CorridorWidth;
                posB = new Vector(startX, startY);
                sizeB = new Vector(sizeX, sizeY);
                cFields.AddRange(GetFieldsInBounds(startX, startY, sizeX, sizeY));
            }

            if (cFields.Any(x => x.Area != null) && !beforeFailed)
                return MakeFlippedL(left, up, right, down, true);
            else if (cFields.Any(x => x.Area != null))
                return null;
            else
                return new Corridor(left, right, posA, sizeA, posB, sizeB, cFields.ToArray());
        }

        /// <summary>
        /// Gibt alle Felder im angegebenen Bereich zurück, die sich innerhalb der Dungeon-Grenzen befinden
        /// </summary>
        private Field[] GetFieldsInBounds(int startX, int startY, int sizeX, int sizeY)
        {
            List<Field> res = new List<Field>();
            for (int x = startX; x < startX + sizeX; x++)
                for (int y = startY; y < startY + sizeY; y++)
                    if (!OutOfBounds(x, y))
                        res.Add(Fields[x, y]);
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
            if (x < 0 || x >= Fields.GetLength(0) || y < 0 || y >= Fields.GetLength(1))
                return true;
            return false;
        }
    }
}
