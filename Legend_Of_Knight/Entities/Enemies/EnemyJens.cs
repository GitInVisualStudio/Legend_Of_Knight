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
            aggroRange = 10;
        }

        public override void OnTick()
        {
            base.OnTick();
        }
    }
}
