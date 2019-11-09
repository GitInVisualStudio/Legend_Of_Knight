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
        public bool Finished { get => finished; set => finished = value; }
        public event EventHandler OnFinish;
        protected bool increase;

        public Animation()
        {
            increase = true;
        }

        public abstract void Update();

        protected void Finish()
        {
            OnFinish?.Invoke(this, null);
        }

        public virtual void Reverse()
        {
            Finished = false;
            increase = !increase;
        }
    }
}
