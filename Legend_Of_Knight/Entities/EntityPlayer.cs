using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Properties;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils.Render;

namespace Legend_Of_Knight.Entities
{
    public class EntityPlayer : EntityLivingBase
    {
        public EntityPlayer() : base(new FrameAnimation(FPS, false, Resources.right_0000, Resources.right_0001), new FrameAnimation(FPS, false, Resources.left_0000, Resources.left_0001))
        {
            
        }

        public override void OnCollision(object sender, CollisionArgs e)
        {
            
        }

        public override void OnRender(float partialTicks)
        {
            base.OnRender(partialTicks: partialTicks);
        }
    }
}
