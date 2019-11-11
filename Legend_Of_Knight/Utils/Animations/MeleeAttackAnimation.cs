using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Legend_Of_Knight.Utils.Animations.MeleeAttackFrame;

namespace Legend_Of_Knight.Utils.Animations
{
    public class MeleeAttackAnimation : Animation
    {
        private MeleeAttackFrame[] attackFrames;
        private MeleeAttackFrame currentFrame;
        private int index;
        public AttackFrameStatus Status => currentFrame.Status;
        public Bitmap Image => currentFrame.Image;

        public MeleeAttackAnimation(MeleeAttackFrame[] attackFrames)
        {
            this.attackFrames = attackFrames;
            this.index = 0;
            this.currentFrame = attackFrames[index];

            for(int i = 0; i < attackFrames.Length; i++)
            {
                attackFrames[i].OnFinish += (object sender, EventArgs args) =>
                {
                    if (index == attackFrames.Length)
                    {
                        Finish();
                        return;
                    }
                    index++;
                    currentFrame = attackFrames[index];
                };
            }
        }

        public override void Update()
        {
            currentFrame.Update();
        }
    }
}
