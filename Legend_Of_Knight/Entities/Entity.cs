using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;

namespace Legend_Of_Knight.Entities
{
    public abstract class Entity
    {
        protected Vector position;
        protected Vector velocity;
        protected Vector prevPosition;
        private float rotation;
        private BoundingBox box;
        protected FrameAnimation animation;
        
        public event EventHandler<Vector> Moved;
        public event EventHandler<float> Rotated;

        public Vector Velocity => velocity;//Kann sowieso nicht geil geändert werden lol
        public float Width => box.Width;
        public float Height => box.Height;
        public Vector Size => box.Size;

        public FrameAnimation Animation
        {
            get
            {
                return animation;
            }
            set
            {
                animation = value;
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
                prevPosition = value;
            }
        }

        /// <summary>
        /// Drehwinkel in Grad (Einfacher für das Rendern)
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
                Rotated?.Invoke(this, rotation);
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

        public virtual void OnRender(float partialTicks)
        {
            Vector position = MathUtils.Interpolate(prevPosition, this.position, partialTicks);
            StateManager.Push();
            StateManager.Translate(position);
            StateManager.Rotate(rotation);
            StateManager.DrawImage(animation.Image, position - Size / 2);
            StateManager.Pop();
        }

        public virtual void OnTick()
        {
            Move();
        }

        public void Move()
        {
            prevPosition = position;
            position += Velocity * 2;
            velocity *= 0.8f;

            if (velocity.Length > 0.2f)
                animation.Update();
            else
                animation.Reset();


            Moved?.Invoke(this, position);
        }

        public void SetVelocity(float x, float y)
        {
            velocity.X = x;
            velocity.Y = y;
        }

        public abstract void OnCollision(object sender, CollisionArgs e);
    }
}