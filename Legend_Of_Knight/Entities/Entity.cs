﻿using System;
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
        private float prevRotation;
        private BoundingBox box;
        protected float movingTime;
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

        public float PrevRotation
        {
            get
            {
                return prevRotation;
            }

            set
            {
                prevRotation = value;
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
            Vector position = MathUtils.Interpolate(this.prevPosition, this.position, partialTicks);
            if (Game.DEBUG)
                RenderBoundingBox();
            StateManager.Push();
            StateManager.Translate(position);
            StateManager.Rotate(MathUtils.Interpolate(PrevRotation, rotation, partialTicks));
            StateManager.Translate(Size / -2);
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
            PrevRotation = rotation;
            Move();
        }

        public virtual void Move()
        {
            prevPosition = position;
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

        public void SetVelocity(float x, float y)
        {
            velocity.X = x;
            velocity.Y = y;
        }

        public abstract void OnCollision(object sender, CollisionArgs e);
    }
}