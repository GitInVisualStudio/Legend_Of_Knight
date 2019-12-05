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
using System.Drawing;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Entities.Items;

namespace Legend_Of_Knight.Entities
{
    public class EntityLivingBase : Entity
    {
        protected FrameAnimation[] animations;
        private float health;
        private Item item;
        private EntityItem entityItem;
        private int hurtTime;
        private bool usingItem;
        private EnumFacing facing;
        private int itemCount; //Wie lange das Item in benutung ist
        private int swingProgress;

        public int ItemCount => EntityItem.Animation.Index;
        public float Yaw
        {
            get
            {
                return EntityItem.Rotation;
            }
            set
            {
                EntityItem.Rotation = value;
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
                EntityItem = new EntityItem(value);
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

        public EntityItem EntityItem { get { return entityItem; } protected set { entityItem = value; } }

        public EntityLivingBase()
        {
            Bitmap[][] images = ResourceManager.GetImages(this);
            this.animations = new FrameAnimation[]{ new FrameAnimation(FPS, false, images[0]), new FrameAnimation(FPS, false, images[1])};
            Facing = EnumFacing.RIGHT; //Weil immer rechts
            animation = animations[0];
            Box = new BoundingBox(this, animation.Image.Width / 3, animation.Image.Height / 3);
        }

        public override void OnCollision(object sender, CollisionArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnRender(float partialTicks)
        {
            if (Game.DEBUG)
                RenderBoundingBox();

            float walkingTime = this.movingTime;
            if (walkingTime != 0)
                walkingTime = MathUtils.Interpolate(this.movingTime - Game.TPT/1000.0f, this.movingTime, partialTicks);
            Rotation = MathUtils.Sin(walkingTime * 360 * 3) * 5.5f;

            Vector position = MathUtils.Interpolate(this.prevPosition, this.position, partialTicks);
            StateManager.Push();
            StateManager.Translate(position);
            StateManager.Rotate(Rotation);
            StateManager.Translate(Size / -2);
            StateManager.DrawImage(animation.Image, 0, 0);

            if (item == null)
            {
                StateManager.Pop();
                return;
            }
            //float itemOffset = GetAttribute<FacingAttribute>(Facing).offset;
            //float offset = Width * itemOffset;
            //StateManager.Translate(EntityItem.Width / 2, 0);
            //EntityItem.Position = new Vector(2);
            StateManager.Pop();
            EntityItem.Position = position.Copy();
            EntityItem.Position -= MathUtils.GetRotation(EntityItem.Size / 2, Yaw);
            EntityItem.Position += MathUtils.GetRotation(new Vector(EntityItem.Width / 2, 0), Yaw);
            EntityItem.OnRender(partialTicks);
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
                EntityItem?.Animation.Update();
        }

        public void Swing()
        {
            if(item != null)
                usingItem = true;
            swingProgress = 10;
        }

        public static TAttribute GetAttribute<TAttribute>(Enum value) where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name)
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
}
