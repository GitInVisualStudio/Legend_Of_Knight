using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Legend_Of_Knight.Gui
{
    public abstract class Gui
    {
        private Vector size;
        private Vector position;
        private FireableAnimation animation; //Um Animation einhaltlich zu berechnen und abzufragen

        public float Width => size.X;
        public float Height => size.Y;

        public Vector Size { get => size; set => size = value; }
        public Vector Position { get => position; set => position = value; }
        public FireableAnimation Animation { get => animation; set => animation = value; }

        public event EventHandler<MouseEventArgs> OnClick;
        public event EventHandler<MouseEventArgs> OnMove;
        public event EventHandler<MouseEventArgs> OnRelease;

        protected T GetAnimation<T>() where T : struct =>((CustomAnimation<T>)Animation).Value;

        public void Click(MouseEventArgs args) => OnClick?.Invoke(this, args);

        public void Release(MouseEventArgs args) => OnRelease?.Invoke(this, args);

        public void Move(MouseEventArgs args) => OnMove?.Invoke(this, args);

        public abstract void OnRender(float partialTicks);
    }
}
