using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;
using Item = Legend_Of_Knight.Items.Item;
using Legend_Of_Knight.Utils.Render;

namespace Legend_Of_Knight.Entities.Items
{
    public class EntityItem : Entity
    {
        private Item item;
        private float delta, prevDelta;
        public override void OnCollision(object sender, CollisionArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnRender(float partialTicks)
        {
            Vector position = MathUtils.Interpolate(prevPosition, this.position, partialTicks);
            Vector hover = new Vector(0, (float)Math.Sin(MathUtils.Interpolate(prevDelta, delta, partialTicks)));
            StateManager.DrawImage(animation.Image, position + hover - Size/2);
        }

        public override void OnTick() 
        {
            base.OnTick();
            prevDelta = delta;
            delta += 0.1f;
        }
    }
}
