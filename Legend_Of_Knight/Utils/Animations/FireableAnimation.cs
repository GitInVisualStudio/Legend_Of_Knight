using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public abstract class FireableAnimation : Animation
    {
        private bool started;

        public bool Started { get => started; set => started = value; }

        public virtual FireableAnimation Fire()
        {
            Finished = false;
            started = true;
            AnimationHandler.Add(this);
            OnFinish += (object sender, EventArgs args) =>
            {
                started = false;
            };
            return this;
        }

        public override void Reverse()
        {
            base.Reverse();
            Fire();
        }

        public abstract void OnRender(float partialTicks);
    }
}
