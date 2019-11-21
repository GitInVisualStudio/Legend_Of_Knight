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
        private dynamic start, current, end;
        private dynamic prevCurrent;
        private dynamic interpolated;
        private float speed = 1.0f;

        public T Value => interpolated;

        public T Start
        {
            get
            {
                return start;
            }

            set
            {
                start = value;
            }
        }

        public T End
        {
            get
            {
                return end;
            }

            set
            {
                if(Finished)
                    Fire();
                end = value;
            }
        }

        public float Speed
        {
            get
            {
                return speed;
            }

            set
            {
                speed = value;
            }
        }

        public T Delta => end - current;

        public CustomAnimation(T start, T end, Animate animate)
        {
            this.animate = animate;
            this.Start = start;
            this.End = end;
            this.current = start;
            this.interpolated = start;
            this.prevCurrent = start;
        }

        public delegate T Animate(T current, T delta);

        public override void Reset()
        {
            current = Start;
        }

        public override void Reverse()
        {
            base.Reverse();
            T prevStart = start;
            Start = end;
            End = start;
        }

        public override void Update()
        {
            dynamic delta = Delta;
            delta *= Speed / 2;
            prevCurrent = current;
            current = animate(current, delta);
            if (current.Equals(End))
                Finish();
        }

        public override void OnRender(float partialTicks)
        {
            interpolated = MathUtils.Interpolate(prevCurrent, current, partialTicks);
        }
    }
}
