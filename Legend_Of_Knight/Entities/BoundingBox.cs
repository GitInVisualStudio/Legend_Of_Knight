using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Math;

namespace Legend_Of_Knight.Entities
{
    /// <summary>
    /// Stellt eine Hitbox für eine Entity dar.
    /// </summary>
    public class BoundingBox
    {
        private Entity owner;
        private Vector size;
        private Vector[] corners; // Ecken der Box absolut
        private Vector[] original; // Ecken der Box relativ zum Mittelpunkt

        public Vector[] Corners => corners;

        public Entity Owner
        {
            get
            {
                return owner;
            }
        }

        public float Width
        {
            get
            {
                return size.X;
            }
        }

        public float Height
        {
            get
            {
                return size.Y;
            }
        }

        public Vector Size
        {
            get
            {
                return size;
            }
        }

        public BoundingBox(Entity owner, float width, float height)
        {
            this.owner = owner;
            this.size = new Vector(width, height);
            owner.Rotated += Owner_Rotated;
            owner.Moved += Owner_Moved;

            original = new Vector[4] //Nicht absolut, nur die Größe sonst wird beim bewegen alles verfälscht
            {
                new Vector(-width / 2, -height / 2),
                new Vector(width / 2, -height / 2),
                new Vector(width / 2, height / 2),
                new Vector(-width / 2, height / 2)
            };
            corners = new Vector[4];
            UpdateCorners(owner.Rotation);
        }

        private void Owner_Moved(object sender, Vector e)
        {
            UpdateCorners(owner.Rotation);
        }

        private void Owner_Rotated(object sender, float angle)
        {
            UpdateCorners(angle);
        }

        /// <summary>
        /// Setzt die Ecken der Box absolut, wenn die Besitzer-Entity sich bewegt oder rotiert hat
        /// </summary>
        /// <param name="angle">Der Winkel, in dem die Box gedreht ist</param>
        private void UpdateCorners(float angle)
        {
            for (int i = 0; i < corners.Length; i++)
            {
                Vector current = original[i];
                float x = owner.X + current.X * MathUtils.Cos(angle) - current.Y * MathUtils.Sin(angle);
                float y = owner.Y + current.X * MathUtils.Sin(angle) + current.Y * MathUtils.Cos(angle);
                corners[i] = new Vector(x, y);
            }
        }

        /// <summary>
        /// Überprüft mithilfe des Separating-Axes-Theorem, ob diese und eine andere Box kollidieren
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        public bool Collides(BoundingBox box)
        {
            float[] angles; // Array der Winkel der Achsen, die überprüft werden müssen
            if (owner.Rotation == box.Owner.Rotation) // falls beide Boxen gleich ausgerichtet sind, sind ihre Achsen die selben
                angles = new float[] { owner.Rotation, owner.Rotation + (float)Math.PI / 2.0f };
            else
                angles = new float[] { owner.Rotation, owner.Rotation + (float)Math.PI / 2.0f, box.Owner.Rotation, box.Owner.Rotation + (float)Math.PI / 2.0f };

            foreach (float angle in angles)
                if (!ProjectionOverlaps(ProjectOnto(angle), box.ProjectOnto(angle)))
                    return false;

            return true;
        }

        /// <summary>
        /// Projiziert diese Box auf eine Achse
        /// </summary>
        /// <param name="angle">Der Winkel der Achse im Bogenmaß</param>
        /// <returns>Array von zwei floats, die die relativ gesehen linkste und rechteste Stelle auf der Achse darstellen</returns>
        public float[] ProjectOnto(float angle)
        {
            float min = Int32.MaxValue;
            float max = Int32.MinValue;

            foreach (Vector c in corners)
            {
                float cuttingAngle = (float)Math.Atan(c.Y / c.X) - angle;
                float projection = (float)(Math.Cos(cuttingAngle) * Math.Sqrt(Math.Pow(c.X, 2) + Math.Pow(c.Y, 2)));
                min = projection < min ? projection : min;
                max = projection > max ? projection : max;
            }

            return new float[] { min, max };
        }

        /// <summary>
        /// Überprüft, ob sich zwei Projektionen überlappen
        /// </summary>
        private bool ProjectionOverlaps(float[] a, float[] b)
        {
            if ((a[0] < b[0] && a[1] > b[0]) || (b[0] < a[0] && b[1] > a[0]))
                return true;
            return false;
        }
    }
}