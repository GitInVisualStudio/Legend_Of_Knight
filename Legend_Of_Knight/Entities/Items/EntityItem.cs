using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Math;
using Item = Legend_Of_Knight.Items.Item;
using Legend_Of_Knight.Utils.Render;
using Legend_Of_Knight.Items;

namespace Legend_Of_Knight.Entities.Items
{
    public class EntityItem : Entity
    {
        private Item item;
        private float delta, prevDelta;
        private EntityLivingBase owner;

        public Item Item { get => item; protected set => item = value; }
        public EntityLivingBase Owner { get => owner; }

        public EntityItem(Item item, EntityLivingBase owner) : base(new Rectangle[0])
        {
            this.Item = item;
            this.Box = new BoundingBox(this, item.Image.Width / 3, item.Image.Height / 3);
            this.owner = owner;
        }

        public override void OnRender(float partialTicks)
        {
            Vector position = MathUtils.Interpolate(PrevPosition, this.position, partialTicks);
            Vector hover = new Vector(0, (float)Math.Sin(MathUtils.Interpolate(prevDelta, delta, partialTicks)));
            StateManager.Push();
            StateManager.Translate(position);
            StateManager.Rotate(Rotation);
            StateManager.Translate(Size / 2);
            StateManager.Scale(1, Scale);
            StateManager.Translate(-Size);
            StateManager.DrawImage(Item.Image, 0, 0);
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
