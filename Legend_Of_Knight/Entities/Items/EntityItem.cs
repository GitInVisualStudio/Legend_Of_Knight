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
            Vector position = MathUtils.Interpolate(prevPosition, this.position, partialTicks);
            Vector hover = new Vector(0, (float)Math.Sin(MathUtils.Interpolate(prevDelta, delta, partialTicks)));
            StateManager.Push();
            StateManager.Translate(position - Size / 2);
            StateManager.Translate(Width/2, Height);
            StateManager.Rotate(Rotation);
            StateManager.DrawImage(item.Image, -Width/2, -Height);
            StateManager.Translate(-Width/2, -Height);
            if (Game.DEBUG)
            {
                StateManager.SetColor(255, 0, 0);
                StateManager.FillCircle(Width/2, Height/2, 1);
                StateManager.DrawRect(0, 0, Width, Height, 0.1f);
            }
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
