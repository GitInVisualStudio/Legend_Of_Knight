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

        public EntityItem(Item item)
        {
            this.item = item;
            this.Box = new BoundingBox(this, item.Image.Width / 3, item.Image.Height / 3);
        }

        public override void OnCollision(object sender, CollisionArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnRender(float partialTicks)
        {
            if (Game.DEBUG)
                RenderBoundingBox();
            Vector position = MathUtils.Interpolate(prevPosition, this.position, partialTicks);
            Vector hover = new Vector(0, (float)Math.Sin(MathUtils.Interpolate(prevDelta, delta, partialTicks)));
            StateManager.Push();
            StateManager.Translate(position);
            StateManager.Rotate(Rotation);
            StateManager.Translate(Size / 2);
            StateManager.Scale(1, Scale);
            StateManager.Translate(-Size);
            StateManager.DrawImage(item.Image, 0, 0);
            StateManager.Pop();
        }

        public override void OnTick() 
        {
            base.OnTick();
            prevDelta = delta;
            delta += 0.1f;
        }
    }
}
