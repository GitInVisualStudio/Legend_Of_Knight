using Legend_Of_Knight.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Entities
{
    public abstract class Entity
    {
        private Vector position;
        private Vector velocity;
        private Vector rotation;
        private BoundingBox box;
        private Animation anim;

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

        public Vector Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        public Vector Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
            }
        }

        public float X
        {
            get
            {
                return position[0];
            }

            set
            {
                position[0] = value;
            }
        }

        public float Y
        {
            get
            {
                return position[1];
            }

            set
            {
                position[1] = value;
            }
        }

        public BoundingBox Box
        {
            get
            {
                return box;
            }

            set
            {
                box = value;
            }
        }

        public Animation Anim
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

        public Entity(BoundingBox box)
        {
            this.Box = box;
        }

        public abstract void OnRender(float partialTicks);

        public void OnTick()
        {
            Move();
        }

        public void Move()
        {
            X += velocity[0];
            Y += velocity[1];
        }

        public abstract void OnCollision(object sender, CollisionArgs e);
    }
}
