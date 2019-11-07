using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Entities
{
    public class BoundingBox
    {
        private Entity owner;
        private float width;
        private float height;

        internal Entity Owner
        {
            get
            {
                return owner;
            }
        }

        public float Width
        {
            get
            {
                return width;
            }
        }

        public float Height
        {
            get
            {
                return height;
            }
        }

        public BoundingBox(Entity owner, float width, float height)
        {
            this.owner = owner;
            this.width = width;
            this.height = height;
        }

        public bool Collides(BoundingBox box)
        {
            //TODO

            return false;
        }
    }
}
