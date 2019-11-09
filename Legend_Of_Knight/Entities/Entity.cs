using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils;

namespace Legend_Of_Knight.Entities
{
    abstract class Entity
    {
        private Vector position;
        private Vector velocity;
        private float rotation;
        private BoundingBox box;
        private Animation anim;

        public event EventHandler<Vector> Moved;
        public event EventHandler<float> Rotated;

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

        internal Vector Velocity
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

        /// <summary>
        /// Drehwinkel im Bogenmaß
        /// </summary>
        public float Rotation
        {
            get
            {
                return rotation;
            }

            set
            {
                rotation = value;
                if (Rotated != null)
                    Rotated(this, rotation);
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

        internal BoundingBox Box
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

        internal Animation Anim
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
            this.box = box;
            box.Collided += OnCollision;
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

            if (Moved != null)
                Moved(this, position);
        }

        public abstract void OnCollision(object sender, CollisionArgs e);
    }
}
