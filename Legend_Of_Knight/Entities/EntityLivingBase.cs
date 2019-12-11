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
using Rectangle = Legend_Of_Knight.Utils.Math.Rectangle;
using System.Drawing.Imaging;

namespace Legend_Of_Knight.Entities
{
    public abstract class EntityLivingBase : Entity
    {
        protected FrameAnimation[] animations;
        protected FrameAnimation[] hurtTimeAnimation;
        private float health;
        private Item item;
        private EntityItem entityItem;
        private float yaw;
        private int maxHurtTime = 30; //Damit die HurtTime 1 sekunde gezeigt wird
        private int hurtTime;
        private bool usingItem;
        private EnumFacing facing;
        private CustomAnimation<float> swing;

        public int ItemCount => EntityItem.Animation.Index;
        public float Yaw
        {
            get
            {
                return yaw;
            }
            set
            {
                EntityItem.Rotation = value;
                yaw = value;
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

        public CustomAnimation<float> SwingAnimation { get => swing; }

        public EntityLivingBase(Rectangle[] bounds) : base(bounds) 
        {
            Bitmap[][] images = ResourceManager.GetImages(this);
            Bitmap[][] hurtTimeImages = new Bitmap[images.Length][];
            for(int i = 0; i < images.Length; i++)
            {
                hurtTimeImages[i] = new Bitmap[images[i].Length];
                for(int j = 0; j < images[i].Length; j++)
                {
                    hurtTimeImages[i][j] = RenderUtils.PaintBitmap(images[i][j], Color.Red, true);
                }
            }

            this.animations = new FrameAnimation[]{ new FrameAnimation(FPS, false, images[0]), new FrameAnimation(FPS, false, images[1])};
            this.hurtTimeAnimation = new FrameAnimation[] { new FrameAnimation(FPS, false, hurtTimeImages[0]), new FrameAnimation(FPS, false, hurtTimeImages[1]) };

            Facing = EnumFacing.RIGHT; //Weil immer rechts
            animation = animations[0];
            Box = new BoundingBox(this, animation.Image.Width / 3, animation.Image.Height / 3);
            swing = CustomAnimation<float>.CreateDefaultAnimation(1.0f);
            swing.Toleranz = 1E-2f;
            swing.OnFinish += (object sender, EventArgs args) =>
            {
                if (swing.Increments)
                    swing.Reverse();
            };
            swing.Reverse();
            Health = 20;
        }

        public override void OnRender(float partialTicks)
        {
            if (health <= 0)
                return;
            if (Game.DEBUG)
                RenderBoundingBox();

            float walkingTime = this.movingTime;
            if (walkingTime != 0)
                walkingTime = MathUtils.Interpolate(this.movingTime - Game.TPT/1000.0f, this.movingTime, partialTicks);
            Vector position = MathUtils.Interpolate(PrevPosition, this.position, partialTicks);
            Rotation = MathUtils.Sin(walkingTime * 360 * 3) * 5.5f;
            StateManager.Push();
            StateManager.Translate(position);
            StateManager.Rotate(Rotation);
            StateManager.Translate(Size / -2);
            StateManager.DrawImage(animation.Image, 0, 0);

            if (hurtTime != 0)
            {
                float opacity = hurtTime / (float)maxHurtTime;
                StateManager.DrawImage(hurtTimeAnimation[(int)facing].Image, 0, 0, opacity);
            }

            if (item == null || swing.Finished)
            {
                StateManager.Pop();
                return;
            }
            //float offset = Width * itemOffset;
            //StateManager.Translate(EntityItem.Width / 2, 0);
            float itemOffset = GetAttribute<FacingAttribute>(Facing).offset;
            float yaw = Yaw + (MathUtils.Sin(90 * swing.Value) * 80 - 80) * itemOffset;
            EntityItem.Scale = (swing.Value * 2.5f) > 1f ? 1 : (swing.Value * 2.5f) + 0.001f;
            EntityItem.Rotation = yaw;
            EntityItem.Position = Size / 2;
            EntityItem.Position -= MathUtils.GetRotation(EntityItem.Size / 2, yaw);
            EntityItem.Position += MathUtils.GetRotation(new Vector(EntityItem.Width / 2, 0), yaw);
            EntityItem.OnRender(partialTicks);
            StateManager.Pop();
        }

        public override void OnTick()
        {
            if (health <= 0)
                return;
            base.OnTick();

            Vector direction = velocity.Normalize();

            if (direction.X > 0)
                Facing = EnumFacing.RIGHT;
            if (direction.X < 0)
                Facing = EnumFacing.LEFT;

            animation = animations[(int)Facing];

            if (hurtTime != 0)
            {
                hurtTime--;
                hurtTimeAnimation[(int)Facing].Index = animation.Index;
            }

            if (item == null)
                return;

            if (IsUsingItem)
                EntityItem?.Animation?.Update();
        }

        public void Swing()
        {
            if(item != null)
                usingItem = true;
            swing.Reverse();
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
