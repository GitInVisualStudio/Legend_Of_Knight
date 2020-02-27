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
using Legend_Of_Knight.Entities.Pathfinding;

namespace Legend_Of_Knight.Entities.Enemies
{
    public abstract class EntityEnemy : EntityLivingBase
    {
        private FrameAnimation idle;
        protected Node nextNode;
        protected Entity aggro;
        protected float aggroRange;
        protected float swingCooldown; // Wartezeit zwischen Angriffen
        protected float maxSwingCooldown; // swingCooldown = maxSwingCooldown wenn gerade angegriffen wurde

        public EntityEnemy(Rectangle[] bounds) : base(bounds)
        {   
            idle = new FrameAnimation(FPS, false, ResourceManager.GetImages(this, "Idle")); // Animation für wenn der Gegner sich nicht bewegt
            maxSwingCooldown = 60;
        }

        public override void OnTick()
        {
            base.OnTick();
            
            if (aggro != null)
            {
                Path path = new Path(GridPosition, Game.Player.GridPosition, Game.D);
                nextNode = path.GetNextNode();
                while (nextNode == CurrentGridNode)
                    nextNode = path.GetNextNode();
                if (nextNode != null)
                    velocity += (nextNode.Position * 15f - position).Normalize() * 0.75f;

                //if ((aggro.Position - Position).Length >= Item.Image.Width * 0.9f)
                //    velocity += (aggro.Position - Position).Normalize() * 0.75f; // verfolgt den Spieler
            }       
            else if (Animation != idle)
                Animation = idle;

            if (swingCooldown > 0)
                swingCooldown--;
            if (SwingAnimation.Finished && aggro != null && swingCooldown == 0) // falls das Schwert momentan nicht geschwungen wird
            {
                Swing();
                Vector yaw = Game.Player.Position - Position; // setzt Ausrichtung des Schwertes in Richtung Spieler
                Yaw = MathUtils.ToDegree((float)Math.Atan2(yaw.Y, yaw.X)) + 90; 
                swingCooldown = maxSwingCooldown;
            }
                
            if (HurtTime == 0 && !Game.Player.SwingAnimation.Finished && Box.Collides(Game.Player.EntityItem.Box)) // falls der Spieler gerade angreift und sein Schwert diesen Gegner trifft // Kollision funktioniert nicht => positionen falsch aber durch translation "richtig" angezeigt // -> IN OBERKLASSE
            { 
                Health -= Game.Player.Item.Damage;
                velocity -= (Game.Player.Position - Position).Normalize() * 20; // Rückstoß
                HurtTime = MaxHurtTime;
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
