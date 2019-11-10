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

        public BoundingBox[] Boxes
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
    }
}
