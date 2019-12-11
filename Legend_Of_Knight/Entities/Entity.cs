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
        private Vector prevPosition;
        protected float rotation;
        private BoundingBox box;
        protected float movingTime;
        protected FrameAnimation animation;
        private float scale;
        protected Rectangle[] bounds;
        
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
                PrevPosition = position.Copy();
                Moved(this, position);
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

        public Vector PrevPosition { get => prevPosition; set => prevPosition = value; }

        public Entity(Rectangle[] bounds)
        {
            position = new Vector(2);
            velocity = new Vector(2);
            PrevPosition = new Vector(2);
            this.bounds = bounds;
        }

        public virtual void OnRender(float partialTicks)
        {
            Vector position = MathUtils.Interpolate(this.PrevPosition, this.position, partialTicks);
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

            //TODO: Guck ob das Entity außerhalb der Map geht wenn Velocity addiert wird
            PushInBounds();

            PrevPosition = position;
            position += Velocity;
            velocity *= 0.7f;

            if (velocity.Length > 0.2f)
            {
                movingTime += Game.TPT / 1000.0f;
                animation.Update();
            }
            else
            {
                movingTime = 0;
                animation.Reset();
            }
            Moved?.Invoke(this, position);
        }

        private void PushInBounds(Rectangle rectangle = null, int corner = -1)
        {
            if (rectangle == null)
                rectangle = FindCurrentRect();
            if (corner >= box.Corners.Length || rectangle == null)
                return;
            int last = IsOutOfBox(corner);
            if (last != -1)
                PushInBounds(rectangle, last);
            if (corner == -1)
                return;
            Vector pos = rectangle.Pos;
            Vector size = rectangle.Size;
            Vector next = box.Corners[corner] + velocity;
            if (next.X > pos.X + size.X && velocity.X > 0)
                velocity.X = 0;
            if (next.X < pos.X && velocity.X < 0)
                velocity.X = 0;
            if (next.Y > pos.Y + size.Y && velocity.Y > 0)
                velocity.Y = 0;
            if (next.Y < pos.Y && velocity.Y < 0)
                velocity.Y = 0;
        }

        /// <summary>
        /// Guckt ob ein Ecke der BoundingBox im nächsten Tick außerhalb der Map sein wird
        /// Current: Falls mehrere außerhalb der Map liegen
        /// </summary>
        /// <returns></returns>
        private int IsOutOfBox(int current)
        {
            for(int i = 0; i < box.Corners.Length; i++)
            {
                bool isOut = true;
                for (int k = 0; k < bounds.Length; k++)
                {
                    if (bounds[k].PointInRectangle((box.Corners[i] + velocity)))
                    {
                        isOut = false;
                        break;
                    }
                }
                if (isOut && current < i)
                    return i;
            }
            return -1;
        }

        protected Rectangle FindCurrentRect()
        {
            for (int i = 0; i < bounds.Length; i++)
                foreach (Vector c in box.Corners)
                    if (bounds[i].PointInRectangle(c))
                        return bounds[i];
            return null;
        }

        public void AddVelocity(Vector delta)
        {
            velocity += delta;
        }

        public abstract void OnCollision(object sender, CollisionArgs e);
    }
}