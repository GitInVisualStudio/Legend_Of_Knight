﻿using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend_Of_Knight.Gui
{
    /// <summary>
    /// Basis für alle GUI
    /// </summary>
    public abstract class Gui
    {
        private Vector size = new Vector(2);
        private Vector position = new Vector(2);
        private FireableAnimation animation; //Um Animation einhaltlich zu berechnen und abzufragen

        public float X => position.X;
        public float Y => position.Y;
        public float Width => Size.X;
        public float Height => Size.Y;

        public Vector Size
        {
            get
            {
                return size;
            }

            set
            {
                size = value;
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
            }
        }

        public FireableAnimation Animation
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

        //Basis-Events für Interaktionen
        public event EventHandler<MouseEventArgs> OnClick;
        public event EventHandler<MouseEventArgs> OnMove;
        public event EventHandler<MouseEventArgs> OnRelease;
        public event EventHandler<KeyEventArgs> OnKeyPressed;

        protected T GetAnimation<T>() where T : struct =>((CustomAnimation<T>)Animation).Value;

        public void Click(MouseEventArgs args) => OnClick?.Invoke(this, args);

        public void Release(MouseEventArgs args) => OnRelease?.Invoke(this, args);

        public void Move(MouseEventArgs args) => OnMove?.Invoke(this, args);

        public void KeyPressed(KeyEventArgs args) => OnKeyPressed?.Invoke(this, args);

        //Prüft ob der Spieler über dem GUI ist
        public bool OnHover(MouseEventArgs args)
        {
            return args.X > X && args.Y > Y && args.X < X + Width && args.Y < Y + Height;
        }

        //Prüft ob der Spieler über dem GUI ist
        public bool OnHover(int x, int y)
        {
            return x > X && y > Y && x < X + Width && y < Y + Height;
        }

        public abstract void OnRender(float partialTicks);
    }
}
