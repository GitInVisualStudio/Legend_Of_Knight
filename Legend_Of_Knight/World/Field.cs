using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils.Animations;

namespace Legend_Of_Knight.World
{
    public class Field
    {
        private Animation anim;
        private Area area;

        public Animation Anim { get => anim; set => anim = value; }
        public Area Area { get => area; set => area = value; }

        public Field(Animation anim)
        {
            this.anim = anim;
        }
    }
}
