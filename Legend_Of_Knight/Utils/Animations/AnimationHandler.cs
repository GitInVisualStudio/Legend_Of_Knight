using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Legend_Of_Knight.Utils.Animations
{
    /// <summary>
    /// Einheitlicher Manager für alle FireableAnimations
    /// Berechnet alle Animationen die gestartet werden
    /// </summary>
    public class AnimationHandler
    {
        //Liste der momentanen Animationen
        private static List<FireableAnimation> animations = new List<FireableAnimation>();

        public void Update()
        {

            for(int i = animations.Count - 1; i >= 0; i--) //Einfacher um Animationen während des Durchgehens zu entfernen
            {
                FireableAnimation animation = animations[i];
                animation.Update();
                if (animation.Finished) //Entfernt die Animation wenn sie beendet ist.
                    animations.Remove(animation);
            }
        }

        public void OnRender(float partialTicks)
        {
            for (int i = animations.Count - 1; i >= 0; i--)
            {
                FireableAnimation animation = animations[i];
                animation.OnRender(partialTicks);
            }
        }

        /// <summary>
        /// Fügt eine Animation hinzu
        /// </summary>
        /// <param name="a"></param>
        public static void Add(FireableAnimation a)
        {
            if (!animations.Contains(a))
                animations.Add(a);
        }
    }
}
