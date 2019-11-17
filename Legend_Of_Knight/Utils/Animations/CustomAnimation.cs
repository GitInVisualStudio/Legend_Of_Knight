using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public class CustomAnimation<T> : FireableAnimation where T : struct
    {
        private Animate animate;
        private T start, current, end;
        private T prevCurrent;
        private T interpolated;
        private float speed = 1.0f;

        public T Value => interpolated;
        public T Start { get => start; set => start = value; }
        public T End { get => end; set => end = value; }
        public float Speed { get => speed; set => speed = value; }

        public CustomAnimation(T start, T end, Animate animate)
        {
            this.animate = animate;
            this.start = start;
            this.end = end;
            this.current = start;
            this.interpolated = start;
            this.prevCurrent = start;
        }

        public delegate T Animate(T current, T delta);

        public override void Reset()
        {
            current = Start;
        }

        public override void Update()
        {
            dynamic delta = current;
            dynamic end = End;
            delta = end - current;
            delta *= Speed / 2;
            prevCurrent = current;
            current = animate(current, delta);
        }

        public override void OnRender(float partialTicks)
        {
            interpolated = MathUtils.Interpolate(prevCurrent, current, partialTicks);
        }
    }
}
