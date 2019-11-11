using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Properties;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;

namespace Legend_Of_Knight.Entities
{
    public class EntityPlayer : Entity
    {
        public EntityPlayer()
        {
            walkingAnimation = new FrameAnimation(500, false, Resources.player_1, Resources.player_2, Resources.player_3);
        }

        public override void OnCollision(object sender, CollisionArgs e)
        {
            
        }

        public override void OnRender(float partialTicks)
        {
            Vector position = prevPosition + (prevPosition - this.position) * partialTicks;
            StateManager.DrawImage(walkingAnimation.Image, position);
        }
    }
}
