using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Math
{
    public class CRandom : Random
    {
        public CRandom() : base()
        {

        }

        public CRandom(int seed) : base(seed)
        {

        }

        public float NextFloat()
        {
            return (float)NextDouble();
        }

        /// <summary>
        /// Gibt einen zufälligen Funktionswert der Gausschen Funktion e^-x^2 auf dem Intervall [-1; 1] zurück
        /// </summary>
        /// <param name="factor">Der Faktor der Gausschen Funktion. Beeinflusst, welche Ergebnisse möglich sind und wie wahrscheinlich sie sind. Für eine möglichst große Abdeckung des Wertebereichs [0; 1] standardmäßig mit 4 gewählt.</param>
        /// <returns></returns>
        public float NextFloatGaussian(int factor = 4)
        {
            return 4 * (float)System.Math.Pow(System.Math.E, -System.Math.Pow(2 * NextDouble() - 1, 2));
        }
    }
}
