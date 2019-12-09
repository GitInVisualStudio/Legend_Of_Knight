using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using d = System.Drawing;
using Rectangle = Legend_Of_Knight.Utils.Math.Rectangle;

namespace Legend_Of_Knight.World
{
    public class Field
    {
        private FrameAnimation anim;
        private Area area;
        private int x;
        private int y;
        private FieldType type;

        private CRandom rnd;

        public FrameAnimation Anim
        {
            get
            {
                return anim;
            }

            set
            {
                anim = value;
            }
        }

        public Area Area
        {
            get
            {
                return area;
            }

            set
            {
                area = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }

            set
            {
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }

            set
            {
                y = value;
            }
        }

        public FieldType Type
        {
            get
            {
                return type;
            }

            set
            {
                type = value;
            }
        }

        public Field(FrameAnimation anim, int x, int y, CRandom rnd)
        {
            this.Anim = anim;
            this.X = x;
            this.Y = y;
            this.rnd = rnd;
            Type = FieldType.Nothing;
        }

        public void SetFieldTypeAndAnimation(Field[,] fields)
        {
            if (Area == null)
                return;

            Field[] neighbors = GetNeighbors(fields);

            Rectangle[] maxRects = FindValidMaxRectangles(neighbors);
            if (maxRects.Length == 0)
                throw new FieldAloneException();

            if (maxRects.Length > 1)
            {
                if (maxRects[0].Area < 6)
                    throw new FieldAloneException();
                    

                for (int i = 1; i < neighbors.Length; i += 2) // Betrachten aller diagonalen Ecken bei zwei Rechtecken mit Größe 6
                { 
                    if (neighbors[i].Area == null)
                        this.Type = (FieldType)(9 + (((i - 1) / 2 + 2) % 4)); // bestimmt manche Ecken
                }
                    
            }
            else
            {
                Rectangle rect = maxRects[0];
                if (rect.Area == 4) // Feld ist eine Ecke
                {
                    for (int i = 0; i < neighbors.Length; i += 2)
                        if (neighbors[i].Area == null && neighbors[(i + 2) % neighbors.Length].Area == null)
                            Type = (FieldType)(5 + i / 2);
                }
                else if (rect.Area == 6)
                {
                    for (int i = 0; i < neighbors.Length; i += 2)
                        if (neighbors[i].Area == null)
                            Type = (FieldType)(1 + i / 2);
                }
                else if (rect.Area == 9)
                    Type = FieldType.Floor;
            }
            if (Type != FieldType.Nothing)
                SetAnimation();
        }

        private void SetAnimation()
        {
            Bitmap[] imgs = ResourceManager.GetImages(this, Type.ToString());
            Anim = new FrameAnimation(0, true, new Bitmap[] { rnd.PickElements(imgs, 1)[0] });
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
                    if (fields[x, y].Area == null)
                        continue;

                    int width = 1;
                    int height = 1;

                    for (int i = 1; x + i < fields.GetLength(0) && AllFieldsInRectangleDefined(fields, new Rectangle(new Vector(x, y), new Vector(width + 1, height))); i++)
                        width++;

                    for (int i = 1; y + i < fields.GetLength(1) && AllFieldsInRectangleDefined(fields, new Rectangle(new Vector(x, y), new Vector(width, height + 1))); i++)
                        height++;

                    Rectangle rectHorizontal = new Rectangle(new Vector(x, y), new Vector(width, height));

                    width = 1;
                    height = 1;

                    for (int i = 1; y + i < fields.GetLength(1) && AllFieldsInRectangleDefined(fields, new Rectangle(new Vector(x, y), new Vector(width, height + 1))); i++)
                        height++;

                    for (int i = 1; x + i < fields.GetLength(0) && AllFieldsInRectangleDefined(fields, new Rectangle(new Vector(x, y), new Vector(width + 1, height))); i++)
                        width++;

                    Rectangle rectVertical = new Rectangle(new Vector(x, y), new Vector(width, height));
                    if (rectHorizontal.Size != rectVertical.Size)
                        rects.Add(rectVertical);
                    rects.Add(rectHorizontal);
                }

            rects.RemoveAll(x => x.Area < 4); // entfernt alle nicht validen Rechtecke mit irgendeiner Kantenlänge < 2
            if (rects.Count == 0)
                return rects.ToArray();

            List<Rectangle> maxRects = new List<Rectangle>();
            maxRects.Add(rects[0]);
            for (int i = 1; i < rects.Count; i++)
                if (rects[i].Area > maxRects[0].Area)
                {
                    maxRects.Clear();
                    maxRects.Add(rects[i]);
                }
                else if (rects[i].Area == maxRects[0].Area)
                    maxRects.Add(rects[i]);
            
            return maxRects.ToArray();
        }

        private bool AllFieldsInRectangleDefined(Field[,] fields, Rectangle rect)
        {
            for (int x = (int)rect.Pos.X; x < rect.Pos.X + rect.Size.X; x++)
                for (int y = (int)rect.Pos.Y; y < rect.Pos.Y + rect.Size.Y; y++)
                    if (fields[x, y].Area == null)
                        return false;
            return true;
        }
        
        private Field[] GetNeighbors(Field[,] fields)
        {
            Field[] neighbors = new Field[8];
            for (int i = 0; i < neighbors.Length; i++)
            {
                int[] coords = GetCoordinatesByDirection((Direction)i);
                neighbors[i] = GetNeighbor(fields, (Direction)i) ?? new Field(null, coords[0], coords[1], rnd);
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
        CornerWallDownRight = 9,
        CornerWallLeftDown = 10,
        CornerWallUpLeft = 11,
        CornerWallRightUp = 12
    }
}
