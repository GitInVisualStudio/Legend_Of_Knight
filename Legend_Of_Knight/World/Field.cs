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

        public Animation Anim { get => anim; set => anim = value; }
        public Area Area { get => area; set => area = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public Field(Animation anim, int x, int y)
        {
            this.anim = anim;
            this.x = x;
            this.y = y;
        }

        public FieldType GetFieldType(Field[,] fields)
        {
            Field[] neighbors = new Field[4];
            for (int i = 0; i < neighbors.Length; i++)
                neighbors[i] = GetNeighbor(fields, (Direction)i);

            int neighborCount = neighbors.Count(x => x != null);
            if (neighborCount == 4)
                return FieldType.Floor;
            else if (neighborCount == 3)
                for (int i = 0; i < neighbors.Length; i++)
                {
                    if (neighbors[i] == null)
                        return (FieldType)(i + 1);
                }
            else if (neighborCount == 2)
                for (int i = 0; i < neighbors.Length; i++)
                {
                    if (neighbors[i] == null && neighbors[i+ 1 % neighbors.Length] == null)
                        return (FieldType)(i + 5);
                }

            throw new FieldAloneException();
        }

        private Field GetNeighbor(Field[,] fields, Direction d)
        {
            Predicate<Field> pred = null;
            if (d == Direction.Left)
                if (X - 1 < 0)
                    return null;
                else
                    return fields[X - 1, Y];
            else if (d == Direction.Top)
                if (Y - 1 < 0)
                    return null;
                else
                    return fields[X, Y - 1];
            else if (d == Direction.Right)
                if (X + 1 > fields.GetLength(0))
                    return null;
                else
                    return fields[X + 1, Y];
            else
                if (Y + 1 > fields.GetLength(1))
                    return null;
                else
                    return fields[X, Y + 1];

        }
    }

    public enum Direction
    {
        Left = 0,
        Top = 1,
        Right = 2,
        Down = 3
    }

    public enum FieldType
    {
        Floor = 0,
        WallLeft = 1,
        WallUp = 2,
        WallRight = 3,
        WallDown = 4,
        CornerDownRight = 5,
        CornerLeftDown = 6,
        CornerUpLeft = 7,
        CornerRightUp = 8
    }
}
