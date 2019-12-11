using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    public class FrameAnimation : Animation
    {
        private Bitmap[] images;
        private int index;
        private int delay;
        private bool stop;
        public int Delay { get { return delay; } set { delay = value; } }
        public int Length => images.Length;
        public Bitmap Image => images[Index];

        public int Index { get => index; set => index = value; }

        /// <summary>
        /// goes through the images
        /// </summary>
        /// <param name="delay">in millis</param>
        /// <param name="images"></param>
        public FrameAnimation(int delay, bool stop = true, params Bitmap[] images)
        {
            this.images = images;
            this.stop = stop;
            Delay = delay;
            Index = 0;
        }

        public override void Update()
        {
            if (Finished)
                return;

            if (TimeUtils.Check(Delay))
            {
                Index += increase ? 1 : -1;
                if(Index == images.Length || Index < 0)
                {
                    if(stop)
                        Finish();
                    Index = increase ? 0 : images.Length - 1;
                }
            }
        }

        public override void Reset() => Index = 0;
    }
}
