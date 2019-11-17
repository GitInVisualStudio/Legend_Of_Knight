using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public class AnimationHandler
    {
        private static List<FireableAnimation> animations = new List<FireableAnimation>();

        public void Update()
        {
            for(int i = animations.Count - 1; i >= 0; i--) //Easier to remove animations
            {
                FireableAnimation animation = animations[i];
                animation.Update();
                if (animation.Finished)
                    animations.Remove(animation);
            }
        }

        public void OnRender(float partialTicks)
        {
            for (int i = animations.Count - 1; i >= 0; i--) //Easier to remove animations
            {
                FireableAnimation animation = animations[i];
                animation.OnRender(partialTicks);
            }
        }

        public static void Add(FireableAnimation a)
        {
            if (!animations.Contains(a))
                animations.Add(a);
        }
    }
}
