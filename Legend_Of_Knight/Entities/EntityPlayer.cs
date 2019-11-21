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
        public EntityPlayer() : base()
        {
            Item = new Legend_Of_Knight.Items.Item("sword.png");
        }

        public override void OnCollision(object sender, CollisionArgs e)
        {
        }

        public override void OnTick()
        {
            base.OnTick();
            Vector yaw = InputManager.mousePosition - position;
            Yaw = MathUtils.ToDegree((float)Math.Atan2(yaw.Y, yaw.X)) + 90;
        }
    }
}
