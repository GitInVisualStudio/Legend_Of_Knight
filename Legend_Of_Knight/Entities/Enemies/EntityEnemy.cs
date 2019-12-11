using Legend_Of_Knight.Utils.Animations;
using Rectangle = Legend_Of_Knight.Utils.Math.Rectangle;
using EntityItem = Legend_Of_Knight.Entities.Items.EntityItem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;
using Legend_Of_Knight.Utils;

namespace Legend_Of_Knight.Entities.Enemies
{
    public abstract class EntityEnemy : EntityLivingBase
    {
        private FrameAnimation idle;
        protected Entity aggro;
        protected float aggroRange;

        public EntityEnemy(Rectangle[] bounds) : base(bounds)
        {   
            idle = new FrameAnimation(FPS, false, ResourceManager.GetImages(this, "Idle"));
        }

        public override void OnTick()
        {
            base.OnTick();
            if (aggro != null)
            {
                velocity += (aggro.Position - Position).Normalize() * 0.75f; // -> Pathfinding wäre schön (after release patch)
                //if (Animation == idle)
                //    Animation = animations[(int)Facing];
            }
            else if (Animation != idle)
                Animation = idle;

            if (SwingAnimation.Finished && aggro != null)
            {
                Vector yaw = Game.Player.Position - Position;
                Yaw = MathUtils.ToDegree((float)Math.Atan2(yaw.Y, yaw.X)) + 90;
                Swing();
            }
                
            if (HurtTime == 0 && !Game.Player.SwingAnimation.Finished && Box.Collides(Game.Player.EntityItem.Box))
            {
                Health -= Game.Player.Item.Damage;
                velocity -= (Game.Player.Position - Position).Normalize() * 20; // Rückstoß
                HurtTime = 30;
            }
        }

        protected override void UpdateAnimation()
        {
            if (aggro == null)
                Animation = idle;
            animation.Update();
        }
    }
}
