using Legend_Of_Knight.Utils.Math;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Animations
{
    /// <summary>
    /// FrameAnimation für ein Durchgehen von einzelnen Bitmaps
    /// </summary>
    public class FrameAnimation : Animation
    {
        private Bitmap[] images;
        private int index;
        private int delay;
        private bool stop;
        public int Delay { get { return delay; } set { delay = value; } } //Delay in Millisekunden bis zum nächsten Bitmap
        public int Length => images.Length;
        public Bitmap Image => images[Index];
        private TimeUtils timeUtils;

        public int Index { get => index; set => index = value; }

        /// <summary>
        /// Geht durch ein Bitmap-Array als Animation
        /// </summary>
        /// <param name="delay">in millis</param>
        /// <param name="images"></param>
        public FrameAnimation(int delay, bool stop = true, params Bitmap[] images)
        {
            this.images = images;
            this.stop = stop;
            Delay = delay;
            Index = 0;
            this.timeUtils = new TimeUtils();
        }

        public override void Update()
        {
            if (Finished)
                return;
            //Wenn Delay um ist, wird der Index addiert um das Nächste Bild zu setzen
            if (timeUtils.Check(Delay))
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
