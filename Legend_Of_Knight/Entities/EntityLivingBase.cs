using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Items;
using Legend_Of_Knight.Utils.Math;
using Item = Legend_Of_Knight.Items.Item;
using EntityItem = Legend_Of_Knight.Entities.Items.EntityItem;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Render;

namespace Legend_Of_Knight.Entities
{
    public class EntityLivingBase : Entity
    {
        private FrameAnimation[] animations;
        private float health;
        private Item item;
        private EntityItem entityItem;
        private int hurtTime;
        private bool usingItem;
        private EnumFacing facing;
        private int itemCount; //Wie lange das Item in benutung ist

        public int ItemCount => entityItem.Animation.Index;
        public float Yaw
        {
            get
            {
                return entityItem.Rotation;
            }
            set
            {
                entityItem.Rotation = value;
            }
        }

        public float Health
        {
            get
            {
                return health;
            }

            set
            {
                health = value;
            }
        }

        public Item Item
        {
            get
            {
                return item;
            }

            set
            {
                item = value;
            }
        }

        public int HurtTime
        {
            get
            {
                return hurtTime;
            }

            set
            {
                hurtTime = value;
            }
        }

        public bool IsUsingItem => usingItem;

        public EnumFacing Facing
        {
            get
            {
                return facing;
            }

            set
            {
                facing = value;
            }
        }

        public EntityLivingBase(FrameAnimation right, FrameAnimation left)
        {
            this.animations = new FrameAnimation[]{ right, left};
            Facing = EnumFacing.RIGHT; //Weil immer rechts
            animation = animations[0];
            Box = new BoundingBox(this, animation.Image.Width, animation.Image.Height);
        }

        public override void OnCollision(object sender, CollisionArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnRender(float partialTicks)
        {
            base.OnRender(partialTicks);
            Vector position = MathUtils.Interpolate(prevPosition, this.position, partialTicks);

            if (item == null)
                return;
            float itemOffset = GetAttribute<FacingAttribute>(Facing).offset;
            Vector offset = new Vector(Width * itemOffset);
            entityItem.Position = position;
            entityItem.OnRender(partialTicks);
        }

        public override void OnTick()
        {
            base.OnTick();

            Vector direction = velocity.Normalize();

            if (direction.X > 0)
                Facing = EnumFacing.RIGHT;
            if (direction.X < 0)
                Facing = EnumFacing.LEFT;

            animation = animations[(int)Facing];

            if (hurtTime != 0)
                hurtTime--;

            if (item == null)
                return;

            if (IsUsingItem)
                entityItem?.Animation.Update();
        }

        public void UseItem()
        {
            if(item != null)
                usingItem = true;
        }

        public static TAttribute GetAttribute<TAttribute>(Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}
