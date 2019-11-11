﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;

namespace Legend_Of_Knight.Entities
{
    public abstract class Entity
    {
        protected Vector position;
        protected Vector velocity;
        protected Vector prevPosition;
        private float rotation;
        private BoundingBox box;
        protected FrameAnimation walkingAnimation;
        
        public event EventHandler<Vector> Moved;
        public event EventHandler<float> Rotated;

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
                return position.X;
            }

            set
            {
                position.X = value;
            }
        }

        public float Y
        {
            get
            {
                return position.Y;
            }

            set
            {
                position.Y = value;
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
                box.Collided += OnCollision;
            }
        }

        public Entity()
        {
            position = new Vector(2);
            velocity = new Vector(2);
            prevPosition = new Vector(2);
        }

        public abstract void OnRender(float partialTicks);

        public void OnTick()
        {
            Move();
        }

        public void Move()
        {
            prevPosition = Position;
            Position += Velocity * 2;
            velocity *= 0.8f;

            if (velocity.Length > 0.2f)
                walkingAnimation.Update();
            else
                walkingAnimation.Reset();


            if (Moved != null)
                Moved(this, position);
        }

        public void SetVelocity(float x, float y)
        {
            velocity.X = x;
            velocity.Y = y;
        }

        public abstract void OnCollision(object sender, CollisionArgs e);
    }
}