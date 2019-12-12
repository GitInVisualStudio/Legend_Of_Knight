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
using Rectangle = Legend_Of_Knight.Utils.Math.Rectangle;

namespace Legend_Of_Knight.Entities
{
    public class EntityPlayer : EntityLivingBase
    {
        public EntityPlayer(Rectangle[] bounds) : base(bounds)
        {
            Item = new Legend_Of_Knight.Items.Item("sword.png", 10);
            Box = new BoundingBox(this, animation.Image.Width / 3, animation.Image.Height / 3);
        }
    }
}
