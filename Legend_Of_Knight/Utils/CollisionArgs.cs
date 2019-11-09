using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Entities;

namespace Legend_Of_Knight.Utils
{
    public class CollisionArgs
    {
        private BoundingBox[] boxes;
        private Vector position;

        internal BoundingBox[] Boxes
        {
            get
            {
                return boxes;
            }

            set
            {
                boxes = value;
            }
        }

        internal Vector Position
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

        public CollisionArgs()
        {

        }
    }
}
