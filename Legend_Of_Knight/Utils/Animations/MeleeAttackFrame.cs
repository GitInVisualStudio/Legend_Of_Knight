using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public class MeleeAttackFrame : FrameAnimation
    {
        const int FRAME_DELAY = 20;
        private AttackFrameStatus status;

        public MeleeAttackFrame(AttackFrameStatus status, params Bitmap[] images) : base(FRAME_DELAY, stop: true, images: images) => Status = status;
        

        public AttackFrameStatus Status { get { return status; } set { status = value; } }

        public enum AttackFrameStatus
        {
            PREPARING,
            HITTING,
            AFTERMATH
        }
    }
}
