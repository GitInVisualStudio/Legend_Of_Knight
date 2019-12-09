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

namespace Legend_Of_Knight.Entities.Enemies
{
    public abstract class EntityEnemy : EntityLivingBase
    {
        private FrameAnimation idle;
        private FrameAnimation run;
        protected Entity aggro;
        protected float aggroRange;

        public EntityEnemy(Rectangle[] bounds) : base(bounds)
        {   
            idle = GetAnim("Idle");
            run = GetAnim("Run");
            Animation = idle;
        }

        private FrameAnimation GetAnim(string name)
        {
            List<Bitmap> imgs = new List<Bitmap>();
            Bitmap bmp = (Bitmap)Properties.Resources.ResourceManager.GetObject(GetType().Name + "_" + name + "_00");
            for (int i = 1; bmp != null; i++)
            {
                imgs.Add(bmp);
                bmp = (Bitmap)Properties.Resources.ResourceManager.GetObject(GetType().Name + "_" + name + "_" + String.Format("{0:00}", i));
            }
            return new FrameAnimation(0, false, imgs.ToArray());
        }

        public override void OnTick()
        {
            base.OnTick();

            if (MathUtils.Abs(Game.Player.Position - Position).Length <= aggroRange) // IN UNTERKLASSE
                aggro = Game.Player;
            else
                aggro = null;

            if (aggro != null)
            {
                velocity = (aggro.Position - Position).Normalize() * 0.75f;
                if (Animation == idle)
                    Animation = run;
            }
            else if (Animation == run)
                Animation = idle;
                
        }

        public override void OnCollision(object sender, CollisionArgs e)
        {
            BoundingBox other = (e.Boxes.First() == Box ? e.Boxes[1] : e.Boxes[0]);
            if (other.Owner is EntityItem && (EntityItem)other.Owner == Game.Player.EntityItem && !Game.Player.SwingAnimation.Finished)
                Health -= ((EntityItem)other.Owner).Item.Damage;
        }
    }
}
