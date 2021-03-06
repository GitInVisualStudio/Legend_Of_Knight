﻿using System;
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
        protected FrameAnimation[] animations; // zwei Laufanimationen, eine für Links und eine für Rechts
        protected FrameAnimation[] hurtTimeAnimation; //Das selbe aber in rot eingefärbt für die Hurttime
        private float health;
        private Item item; // Waffe der Entity
        private EntityItem entityItem; // Waffen-Entity dieser Entity
        private float yaw; // Ausrichtung der Waffe
        private int maxHurtTime = 30; // damit die HurtTime 1 sekunde gezeigt wird
        private int hurtTime;
        private bool usingItem;
        private EnumFacing facing;
        private CustomAnimation<float> swing; // Attackanimation
        private CustomAnimation<float> death; // Todesanimation
        protected List<EntityItem> enemyItems; // Waffen der Gegner dieser Entity (falls Enemy: player Item, falls Player: Items aller Enemies)

        public int ItemCount => EntityItem.Animation.Index;

        //Blickrichtung des Spielers (auf die Mausposition für das Item)
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
                if (health <= 0)
                    death.Fire();
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
                EntityItem = new EntityItem(value, this);
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

        public bool IsDead => death.Finished;

        public EntityItem EntityItem { get { return entityItem; } protected set { entityItem = value; } }
        public CustomAnimation<float> SwingAnimation { get { return swing; } }
        public int MaxHurtTime { get { return maxHurtTime; } set { maxHurtTime = value; } }

        public EntityLivingBase(Rectangle[] bounds) : base(bounds) 
        {
            //TileMaps für die BegweungsAnimationen
            Bitmap[][] images = new Bitmap[][] { ResourceManager.GetImages(this, "Right"), ResourceManager.GetImages(this, "Left") };
            //Einfärben für die Hurttime 
            Bitmap[][] hurtTimeImages = new Bitmap[images.Length][];
            for(int i = 0; i < images.Length; i++)
            {
                hurtTimeImages[i] = new Bitmap[images[i].Length];
                for(int j = 0; j < images[i].Length; j++)
                {
                    hurtTimeImages[i][j] = RenderUtils.PaintBitmap(images[i][j], Color.Red, true);
                }
            }
            //Erstellen der Bewegungs-Animation und 
            this.animations = new FrameAnimation[]{ new FrameAnimation(FPS, false, images[0]), new FrameAnimation(FPS, false, images[1])};
            this.hurtTimeAnimation = new FrameAnimation[] { new FrameAnimation(FPS, false, hurtTimeImages[0]), new FrameAnimation(FPS, false, hurtTimeImages[1]) };

            Facing = EnumFacing.RIGHT;
            animation = animations[0];
            //Erstellen der BoundingBox für Kollisionen
            Box = new BoundingBox(this, animation.Image.Width, animation.Image.Height);
            //Erstellen der Schlag- und TodesAnimation
            swing = CustomAnimation<float>.CreateDefaultAnimation(1.0f);
            swing.Toleranz = 1E-2f;
            swing.OnFinish += (object sender, EventArgs args) =>
            {
                if (swing.Increments)
                    swing.Reverse();
            };
            swing.Reverse();
            death = CustomAnimation<float>.CreateDefaultAnimation(1.0f);
            death.Toleranz = 1e-5f;
            Health = 20;
        }

        public override void OnRender(float partialTicks)
        {
            if (IsDead)//wenn das Entity tod ist soll dies nicht gezeichnet werden
                return;
            float walkingTime = this.movingTime;
            if (walkingTime != 0)
                walkingTime = MathUtils.Interpolate(this.movingTime - Game.TPT/1000.0f, this.movingTime, partialTicks);
            Vector position = MathUtils.Interpolate(PrevPosition, this.position, partialTicks);
            Rotation = MathUtils.Sin(walkingTime * 360 * 3) * 5.5f;//Bewegungs-Animation (leichtes schwenken im laufen=
            StateManager.Push();
            StateManager.Translate(position);
            StateManager.Rotate(Rotation + death.Value * 90.0f);
            StateManager.Translate(Size / -2);
            if (death.Started)
            {
                //Zeichnet die Sterbe-Animation
                StateManager.DrawImage(hurtTimeAnimation[(int)facing].Image, 0, 0, Width, Height, 1 - death.Value);
                StateManager.Pop();
                return;
            }
            StateManager.DrawImage(animation.Image, 0, 0);
            if (hurtTime != 0) // malt rote Treffer-Animation über die Entity, falls sie eben getroffen wurde
            {
                float opacity = hurtTime / (float)MaxHurtTime;
                StateManager.DrawImage(hurtTimeAnimation[(int)facing].Image, 0, 0, Width, Height, opacity);
            }

            if (item == null || swing.Finished)
            {
                StateManager.Pop();
                return;
            }

            // Rendert das Item
            float itemOffset = GetAttribute<FacingAttribute>(Facing).offset;
            float yaw = Yaw + (MathUtils.Sin(90 * swing.Value) * 80 - 80) * itemOffset;//Rotation des Entites
            EntityItem.Scale = (swing.Value * 2.5f) > 1f ? 1 : (swing.Value * 2.5f) + 0.001f; //Skalierung von 0 auf 1 im laufe der Animation
            EntityItem.Rotation = yaw; //Rotation des Items setzen auf die Blickrichtung
            EntityItem.Position = Size / 2;
            //Translation für die Rotation in die Mitte
            EntityItem.Position -= MathUtils.GetRotation(EntityItem.Size / 2, yaw);
            EntityItem.Position += MathUtils.GetRotation(new Vector(EntityItem.Width / 2, 0), yaw);
            EntityItem.OnRender(partialTicks);
            //Für die Kollisions-Detektion
            EntityItem.Position += position - Size / 2;
            StateManager.Pop();
        }

        public override void OnTick()
        {
            if (IsDead)
                return;
            base.OnTick();

            //Setzt die LaufAnimation entsprechend der Bewegungsrichtung
            Vector direction = velocity.Normalize();
            if (direction.X > 0)
                Facing = EnumFacing.RIGHT;
            if (direction.X < 0)
                Facing = EnumFacing.LEFT;
            animation = animations[(int)Facing];

            if (hurtTime != 0)//für die Hurttime-Animations
            {
                hurtTime--;
                hurtTimeAnimation[(int)Facing].Index = animation.Index;
            }

            if (item == null)
                return;

            if (IsUsingItem)
                EntityItem?.Animation?.Update();

            //Für Rückstoß und Lebens-Abzug falls Entity getroffen wird
            List<EntityItem> enemyItems = Game.GetEnemyItems(!(this is EntityPlayer));
            foreach (EntityItem item in enemyItems)
                if (HurtTime == 0 && !item.Owner.SwingAnimation.Finished && Box.Collides(item.Box))
                {
                    HurtTime = MaxHurtTime;
                    Health -= item.Item.Damage;
                    velocity -= (item.Owner.Position - Position).Normalize() * 20f; // Rückstoß
                }
        }

        /// <summary>
        /// Attackiert mit der Waffe
        /// </summary>
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
