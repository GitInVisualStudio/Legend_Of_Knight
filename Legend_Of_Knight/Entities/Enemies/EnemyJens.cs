using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Entities.Enemies
{
    public class EnemyJens : EntityEnemy
    {
        public EnemyJens(Rectangle[] bounds) : base(bounds)
        {
            aggroRange = 50;
            Item = new Legend_Of_Knight.Items.Item("sword.png", 10);
        }

        public override void OnTick()
        {
            if ((Game.Player.Position - Position).Length <= aggroRange) // IN UNTERKLASSE
                aggro = Game.Player;
            else
                aggro = null;

            base.OnTick();
        }
    }
}
