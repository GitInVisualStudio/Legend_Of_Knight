using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    /// <summary>
    /// Animations-Basis für alle Animationen die im Spiel existieren
    /// </summary>
    public abstract class Animation
    {
        private bool finished;
        public bool Finished { get { return finished; } protected set { finished = value; } }
        public event EventHandler OnFinish; //Events welche ausgelösten werden, wenn die Animation beendet wird
        protected bool increase;
        public bool Increments => increase;

        public Animation()
        {
            increase = true;
        }

        //Abstarkt, soll nur die Basis Funktionen geben
        public abstract void Update();

        public abstract void Reset();

        protected virtual void Finish()
        {
            finished = true;
            OnFinish?.Invoke(this, null);
        }

        /// <summary>
        /// Dreht die Animation um
        /// </summary>
        public virtual void Reverse()
        {
            Finished = false;
            increase = !increase;
        }
    }
}
