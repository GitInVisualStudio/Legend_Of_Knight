using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public abstract class FireableAnimation : Animation
    {
        //Ob sie gestarted wurde
        private bool started;

        public bool Started { get { return started; } set { started = value; } }

        /// <summary>
        /// Animation die Einheitlich berechnet werden könenn
        /// </summary>
        /// <returns></returns>
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
