using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math
{
    public class TimeUtils
    {
        private static Stopwatch delay = new Stopwatch();
        private static Dictionary<string, TimeUtils> utils = new Dictionary<string, TimeUtils>();

        private double time;

        public TimeUtils()
        {
            if (!delay.IsRunning)
                delay.Start();
            time = delay.Elapsed.TotalMilliseconds;
        }

        public bool Check(float milli, bool autoReset)
        {
            bool result = delay.Elapsed.TotalMilliseconds - time > milli;
            if (autoReset && result)
                Reset();
            return result;
        }

        public void Reset() => time = delay.Elapsed.TotalMilliseconds;
        

        public static bool Check(float milli, [CallerFilePath] string memberName = "")
        {
            if (!utils.Keys.Contains(memberName))
            {
                utils.Add(memberName, new TimeUtils());
            }
            return utils[memberName].Check(milli, true);
        }
    }
}
