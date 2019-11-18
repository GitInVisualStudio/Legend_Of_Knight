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
        public Bitmap Image => images[index];
        public int Index => index;

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
            index = 0;
        }

        public override void Update()
        {
            if (Finished)
                return;

            if (TimeUtils.Check(Delay))
            {
                index += increase ? 1 : -1;
                if(index == images.Length || index < 0)
                {
                    if(stop)
                        Finish();
                    index = increase ? 0 : images.Length - 1;
                }
            }
        }

        public override void Reset()
        {
            index = 0;
        }
    }
}
