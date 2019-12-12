using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Entities
{
    //Für die BlickRichtung um BewegungsAnimationen zu bestimmen
    public enum EnumFacing
    {
        [Facing(1)] RIGHT = 0,
        [Facing(-1)] LEFT = 1,
    }

    public class FacingAttribute : Attribute
    {
        public float offset;

        public FacingAttribute(float offset)
        {
            this.offset = offset;
        }
    }

}
