using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
using Legend_Of_Knight.Entities.Pathfinding;

namespace Legend_Of_Knight.Entities
{
    public abstract class Entity
    {
        protected const int FPS = (int)(1000.0f / 10); // FPS für alle Animationen

        protected Vector position;
        private Vector gridPosition;
        private Node currentGridNode;
        protected Vector velocity;
        private Vector prevPosition; // für Interpolation
        protected float rotation;
        private BoundingBox box;
        protected float movingTime;
        protected FrameAnimation animation; //FrameAnimation für Bewegung etc.
        private float scale;
        protected Rectangle[] bounds;
        
        public event EventHandler<Vector> Moved;
        public event EventHandler<float> Rotated;

        /// <summary>
        /// Momentangeschwindigkeit der Entity. Read-Only -> SetVelocity() sollte genutzt werden
        /// </summary>
        public Vector Velocity => velocity;
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
                UpdateGridPosition();
            }
        }

        /// <summary>
        /// Drehwinkel in Grad
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
                UpdateGridPosition();
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
                UpdateGridPosition();
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

        public Vector PrevPosition { get { return prevPosition; } set { prevPosition = value; } }

        public Vector GridPosition
        {
            get
            {
                return gridPosition;
            }

            protected set
            {
                gridPosition = value;
                currentGridNode = new Node(value);
            }
        }

        public Node CurrentGridNode
        {
            get
            {
                return currentGridNode;
            }
        }

        /// <param name="bounds">Rechtecke, in denen die Entity sich bewegen darf</param>
        public Entity(Rectangle[] bounds)
        {
            position = new Vector(2);
            velocity = new Vector(2);
            PrevPosition = new Vector(2);
            this.bounds = bounds;
            UpdateGridPosition();
        }

        /// <summary>
        /// Zeichnet nur das Entity, berechnet keine neuen Positionen
        /// </summary>
        /// <param name="partialTicks"></param>
        public virtual void OnRender(float partialTicks)
        {
            //Interpolation damit es flüssig ist
            Vector position = MathUtils.Interpolate(this.PrevPosition, this.position, partialTicks);
            StateManager.Push();
            //Translation um Mittig zu Rotieren
            StateManager.Translate(position);
            StateManager.Rotate(rotation);
            StateManager.Translate(Size / -2);
            //Skalierung vom Entity
            StateManager.Scale(Scale);
            StateManager.DrawImage(animation.Image, 0, 0);
            StateManager.Pop();
        }

        /// <summary>
        /// Wird jeden Tick aufgerufen, dient zur berechnung von Position und Interaktionen
        /// </summary>
        public virtual void OnTick()
        {
            Move();
        }

        public void Move()
        {
            //TODO: Guck ob das Entity außerhalb der Map geht wenn Velocity addiert wird
            //Wenn dies der Fall ist, Velocity auf 0 setzen
            PushInBounds();

            PrevPosition = position;//Für die Interpolation
            position += Velocity;
            velocity *= 0.7f;//Damit das Entity sich nicht Linear gewegt
            UpdateGridPosition();

            UpdateAnimation();
        }

        private void UpdateGridPosition()
        {
            GridPosition = position.Copy();
            GridPosition.ForEach(i => { return (int)(i / 15f); });
        }

        /// <summary>
        /// Pusht das Entity in die Begrenzungen der Map
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="corner"></param>
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

        /// <summary>
        /// Aktualisiert die Animation des Entitys
        /// </summary>
        protected virtual void UpdateAnimation()
        {
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
        }

        /// <summary>
        /// Findet das Bounding-Rechteck, in dem sich die Entity momentan befindet
        /// </summary>
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
    }
}