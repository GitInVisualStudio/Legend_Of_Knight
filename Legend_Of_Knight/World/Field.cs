using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Animations;
using Legend_Of_Knight.Utils.Math;

namespace Legend_Of_Knight.World
{
    public class Field
    {
        private Animation anim;
        private Area area;
        private int x;
        private int y;

        public Animation Anim { get => anim; set => anim = value; }
        public Area Area { get => area; set => area = value; }
        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }

        public Field(Animation anim, int x, int y)
        {
            this.anim = anim;
            this.x = x;
            this.y = y;
        }
    }
}
