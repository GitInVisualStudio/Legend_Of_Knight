using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;
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
        private dynamic start, current, end, toleranz = default(T);
        private float speed = 1.0f;
        public T Value => current;

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

        public T Toleranz
        {
            get
            {
                return toleranz;
            }

            set
            {
                toleranz = value;
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
            End = prevStart;
        }

        public override void OnRender(float partialTicks)
        {
            dynamic delta = Delta;
            delta *= Speed * StateManager.delta * 10;
            current = animate(current, delta);
            if (delta * delta < toleranz * toleranz && !Finished) //Quadrat für den Betrag
                Finish();
        }

        public override void Update()
        {
            
        }

        protected override void Finish()
        {
            current = End;
            base.Finish();
        }
    }
}
