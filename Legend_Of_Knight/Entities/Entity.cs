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
        protected const int FPS = (int)(1000.0f / 5);

        protected Vector position;
        protected Vector velocity;
        protected Vector prevPosition;
        protected float rotation;
        private BoundingBox box;
        protected float movingTime;
        protected FrameAnimation animation;
        private float scale;
        protected Rectangle[] bounds;
        protected bool outOfBounds;
        
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
                prevPosition = position;
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
            protected set
            {
                box = value;
                box.Collided += OnCollision;
            }
        }

        public float Scale
        {
            get
            {
                return scale;
            }

            set
            {
                scale = value;
            }
        }

        public Entity()
        {
            position = new Vector(2);
            velocity = new Vector(2);
            prevPosition = new Vector(2);
            this.bounds = bounds;
        }

        public virtual void OnRender(float partialTicks)
        {
            Vector position = MathUtils.Interpolate(this.prevPosition, this.position, partialTicks);
            if (Game.DEBUG)
                RenderBoundingBox();
            StateManager.Push();
            StateManager.Translate(position);
            StateManager.Rotate(rotation);
            StateManager.Translate(Size / -2);
            StateManager.Scale(Scale);
            StateManager.DrawImage(animation.Image, 0, 0);
            StateManager.Pop();
        }

        protected virtual void RenderBoundingBox()
        {
            Vector prev = Box.Corners.Last();
            StateManager.SetColor(255, 0, 0);
            for (int i = 0; i < Box.Corners.Length; i++)
            {
                Vector current = Box.Corners[i];
                StateManager.DrawLine(prev, current, 0.1f);
                prev = current;
            }
        }

        public virtual void OnTick()
        {
            Move();
        }

        public void Move()
        {
            if (outOfBounds)
                return;
            prevPosition = position;
            position += Velocity;
            velocity *= 0.7f;

            if (velocity.Length > 0.2f)
            {
                movingTime += Game.TPT / 1000.0f;
                //animation.Update();
            }
            else
            {
                movingTime = 0;
                animation.Reset();
            }

            int oobCounter = 0;
            while (!InBounds())
            {
                Rectangle closest = null;
                if (oobCounter > 50)
                {
                    closest = FindClosestBoundingRect();
                    while (!InBounds())
                    {
                        position += new Vector(closest.CenterPos.X * 16 - position.X, closest.CenterPos.Y * 16 - position.Y).Normalize();
                        Moved?.Invoke(this, position);
                    }
                }
                else
                {
                    outOfBounds = true;
                    position -= velocity.Normalize() * 1;
                    Moved?.Invoke(this, position);
                    oobCounter++;
                }
            }
            outOfBounds = false;
            Moved?.Invoke(this, position);
        }

        protected bool InBounds()
        {
            foreach (Vector corner in box.Corners)
                if (bounds.All(r => !r.PointInRectangle(corner / 16)))
                    return false;
            return true;
        }

        protected Rectangle FindClosestBoundingRect()
        {
            
            for (int i = 0; i < bounds.Length; i++)
                foreach (Vector c in box.Corners)
                    if (bounds[i].PointInRectangle(c / 16))
                        return bounds[i];

            Rectangle closest = bounds[0];
            for (int i = 1; i < bounds.Length; i++) // falls keiner der Ecken in irgendeinem Rechteck ist
                if (MathUtils.Sqrt(MathUtils.Pow(closest.CenterPos.X - position.X, 2) + MathUtils.Pow(closest.CenterPos.Y - position.Y, 2)) < MathUtils.Sqrt(MathUtils.Pow(bounds[i].CenterPos.X - position.X, 2) + MathUtils.Pow(bounds[i].CenterPos.Y - position.Y, 2)))
                    closest = bounds[i];
            Console.WriteLine("Kein Punkt InBounds, nehme nächstes Rechteck");
            return closest;
        }

        public void AddVelocity(Vector delta)
        {
            if (InBounds())
                velocity += delta;
        }

        public abstract void OnCollision(object sender, CollisionArgs e);
    }
}