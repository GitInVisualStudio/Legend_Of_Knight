using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public abstract class FireableAnimation : Animation
    {
        public virtual FireableAnimation Fire()
        {
            Finished = false;
            AnimationHandler.Add(this);
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
