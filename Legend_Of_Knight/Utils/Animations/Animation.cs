using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public abstract class Animation
    {

        private bool finished;
        public bool Finished { get { return finished; } set { finished = value; } }
        public event EventHandler OnFinish;
        protected bool increase;

        public Animation()
        {
            increase = true;
        }

        public abstract void Update();

        public abstract void Reset();

        protected void Finish()
        {
            finished = true;
            OnFinish?.Invoke(this, null);
        }

        public virtual void Reverse()
        {
            Finished = false;
            increase = !increase;
        }
    }
}
