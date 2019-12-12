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
        //Einmalige Stopwatch => Performanz für die Messung der Zeit
        private static Stopwatch delay = new Stopwatch();
        //Speichert die letzte Zeit um veränderung zu messen
        private double time;

        public TimeUtils()
        {
            if (!delay.IsRunning)
                delay.Start();
            //Zeit
            time = delay.Elapsed.TotalMilliseconds;
        }

        /// <summary>
        /// Guckt ob eine bestimmte Zeit(in Millisekunden) erreicht wurde
        /// </summary>
        /// <param name="milli"></param>
        /// <param name="autoReset">Setzt die Zeit automatisch wieder zurück</param>
        /// <returns></returns>
        public bool Check(float milli, bool autoReset = true)
        {
            bool result = delay.Elapsed.TotalMilliseconds - time > milli;
            if (autoReset && result)
                Reset();
            return result;
        }

        public void Reset() => time = delay.Elapsed.TotalMilliseconds;
        
    }
}
