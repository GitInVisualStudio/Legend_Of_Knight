using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public abstract class FireableAnimation : Animation
    {
        public void Fire()
        {
            AnimationHandler.Add(this);
        }
    }
}
