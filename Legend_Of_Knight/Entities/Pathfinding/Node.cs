using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Entities.Pathfinding
{
    public class Node
    {
        private Node parent;
        private Node child;
        private Vector position;
        private int g;
        private int h;

        public Node Parent
        {
            get
            {
                return parent;
            }

            set
            {
                parent = value;
            }
        }

        public Node Child
        {
            get
            {
                return child;
            }

            set
            {
                child = value;
            }
        }

        public Vector Position
        {
            get
            {
                return position;
            }

            set
            {
                position = value;
            }
        }

        public int G
        {
            get
            {
                return g;
            }

            set
            {
                g = value;
            }
        }

        public int H
        {
            get
            {
                return h;
            }

            set
            {
                h = value;
            }
        }

        public int F
        {
            get
            {
                return g + h;
            }
        }

       
        public Node(Vector position, Node parent = null)
        {
            this.position = position;
            this.parent = parent;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Node))
                return false;
            return ((Node)obj).position == position;
        }

        public static bool operator ==(Node a, Node b)
        {
            if (Object.ReferenceEquals(a, null) && Object.ReferenceEquals(b, null))
                return true;
            else if (!Object.ReferenceEquals(a, null))
                return a.Equals(b);
            else
                return false;
        }

        public static bool operator !=(Node a, Node b)
        {
            return !(a == b);
        }

        public int DistanceTo(Node b)
        {
            Vector distance = b.Position - position;
            return (int)(MathUtils.Pow(distance.X, 2) + MathUtils.Pow(distance.Y, 2));
        }
    }
}
