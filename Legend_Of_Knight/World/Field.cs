using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;

namespace Legend_Of_Knight.World
{
    public class Field
    {
        private Animation anim;
        private Area area;
        private int x;
        private int y;
        private FieldType type;

        public Animation Anim { get => anim; set => anim = value; }
        public Area Area { get => area; set => area = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public FieldType Type { get => type; set => type = value; }

        public Field(Animation anim, int x, int y)
        {
            this.anim = anim;
            this.x = x;
            this.y = y;
            type = FieldType.Nothing;
        }

        public void SetFieldType(Field[,] fields)
        {
            if (Area == null)
                return;

            Field[] neighbors = GetNeighbors(fields);
            Rectangle[] maxRects = FindValidMaxRectangles(neighbors);
            // TODO: figure out what role this field plays in rectangle -> its free fieldtype
            // also include case where theres 2 max rectangles, where 
        }

        /// <summary>
        /// Erstellt alle gültigen Rechtecke aus den umliegenden Feldern, deren Fläche maximal ist
        /// </summary>
        /// <param name="origFields">Array der 8 umliegenden Felder</param>
        private Rectangle[] FindValidMaxRectangles(Field[] origFields)
        {
            Field[,] fields = new Field[3, 3];
            fields[1, 1] = this;
            foreach (Field f in origFields)
                fields[f.X - X + 1, f.Y - Y + 1] = f;

            List<Rectangle> rects = new List<Rectangle>();
            for (int x = 0; x < fields.GetLength(0); x++)
                for (int y = 0; y < fields.GetLength(1); y++)
                {
                    int height = 1;
                    int width = 1;

                    for (int i = 1; x + i < fields.GetLength(0) && fields[x + i, y].Area != null; i++)
                        width++;

                    for (int i = 1; y + i < fields.GetLength(1) && fields[x, y + i].Area != null; i++)
                        height++;

                    bool isRectangular = true;
                    for (int x2 = x; x2 < x + width; x2++)
                        for (int y2 = y; y2 < y + height; y2++)
                            if (fields[x2, y2].Area == null)
                                isRectangular = false; // maybe semantically incorrect idk (if sth doesnt work look here)

                    if (isRectangular)
                        rects.Add(new Rectangle(new Vector(x, y), new Vector(width, height)));
                }

            List<Rectangle> maxRects = new List<Rectangle>();
            maxRects.Add(rects[0]);
            foreach (Rectangle r in rects)
                if (r.Area > maxRects[0].Area)
                {
                    maxRects.Clear();
                    maxRects.Add(r);
                }
                else if (r.Area == maxRects[0].Area)
                    maxRects.Add(r);
            maxRects.RemoveAll(x => x.Area < 4); // entfernt alle nicht validen Rechtecke mit irgendeiner Kantenlänge < 2
            return maxRects.ToArray();
        }
        
        private Field[] GetNeighbors(Field[,] fields)
        {
            Field[] neighbors = new Field[8];
            for (int i = 0; i < neighbors.Length; i++)
            {
                int[] coords = GetCoordinatesByDirection((Direction)i);
                neighbors[i] = GetNeighbor(fields, (Direction)i) ?? new Field(null, coords[0], coords[1]);
            }
                
            return neighbors;
        }

        private Field GetNeighbor(Field[,] fields, Direction d)
        {
            int[] coords = GetCoordinatesByDirection(d);
            if (coords[0] < 0 || coords[0] >= fields.GetLength(0) || coords[1] < 0 || coords[1] >= fields.GetLength(1))
                return null;
            return fields[coords[0], coords[1]];
        }

        private int[] GetCoordinatesByDirection(Direction d)
        {
            if (d == Direction.Left)
                return new int[] { X - 1, Y };
            else if (d == Direction.LeftTop)
                return new int[] { X - 1, Y - 1 };
            else if (d == Direction.Top)
                return new int[] { X, Y - 1 };
            else if (d == Direction.TopRight)
                return new int[] { X + 1, Y - 1 };
            else if (d == Direction.Right)
                return new int[] { X + 1, Y };
            else if (d == Direction.RightDown)
                return new int[] { X + 1, Y + 1 };
            else if (d == Direction.Down)
                return new int[] { X, Y + 1 };
            else
                return new int[] { X - 1, Y + 1 };
        }
    }

    public enum Direction
    {
        Left = 0,
        LeftTop = 1,
        Top = 2,
        TopRight = 3,
        Right = 4,
        RightDown = 5,
        Down = 6,
        DownLeft = 7
    }

    public enum FieldType
    {
        Nothing = -1,
        Floor = 0,
        WallLeft = 1,
        WallUp = 2,
        WallRight = 3,
        WallDown = 4,
        CornerDownRight = 5,
        CornerLeftDown = 6,
        CornerUpLeft = 7,
        CornerRightUp = 8,
    }
}
