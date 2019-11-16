using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils;
using Legend_Of_Knight.Utils.Math;

namespace Legend_Of_Knight.Entities
{
    public class BoundingBox
    {
        private Entity owner;
        private float width;
        private float height;
        private Vector size;
        private Vector[] corners;

        public Vector[] Corners => corners;
        public event EventHandler<CollisionArgs> Collided;

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
                return width;
            }
        }

        public float Height
        {
            get
            {
                return height;
            }
        }

        public Vector Size => size;

        public BoundingBox(Entity owner, float width, float height)
        {
            this.owner = owner;
            this.width = width;
            this.height = height;
            this.size = new Vector(width, height);
            owner.Rotated += Owner_Rotated;

            corners = new Vector[4]
            {
                new Vector(owner.X - width / 2, owner.Y - height / 2),
                new Vector(owner.X + width / 2, owner.Y - height / 2),
                new Vector(owner.X + width / 2, owner.Y + height / 2),
                new Vector(owner.X - width / 2, owner.Y + height / 2)
            };
            Owner_Rotated(this, Owner.Rotation);
        }

        private void Owner_Rotated(object sender, float angle)
        {
            angle = MathUtils.ToRadians(angle);
            for (int i = 0; i < corners.Length; i++)
            {
                // Lösung geklaut von https://gamedev.stackexchange.com/questions/86755/how-to-calculate-corner-positions-marks-of-a-rotated-tilted-rectangle/86784#86784
                Vector cTranslated = new Vector(corners[i][0] - owner.X, corners[i][1] - owner.Y); // punkt relativ zum Mittelpunkt des Rechteckes
                cTranslated[0] = (float)(cTranslated[0] * Math.Cos(angle) - cTranslated[1] * Math.Sin(angle));
                cTranslated[1] = (float)(cTranslated[0] * Math.Sin(angle) + cTranslated[1] * Math.Cos(angle)); // nicht -cTranslated[0] * ... ??
                corners[i] = new Vector(cTranslated[0] + owner.X, cTranslated[1] + owner.Y); // punkt rotiert und absolut
            }
        }

        public bool Collides(BoundingBox box)
        {
            float[] angles;
            if (owner.Rotation == box.Owner.Rotation)
                angles = new float[] { owner.Rotation, owner.Rotation + (float)Math.PI / 2.0f };
            else
                angles = new float[] { owner.Rotation, owner.Rotation + (float)Math.PI / 2.0f, box.Owner.Rotation, box.Owner.Rotation + (float)Math.PI / 2.0f };

            foreach (float angle in angles)
                if (!ProjectionOverlaps(ProjectOnto(angle), box.ProjectOnto(angle)))
                    return false;


            Collided?.Invoke(this, new CollisionArgs()
            {
                Boxes = new BoundingBox[] { this, box },
            });
            return true;
        }

        /// <summary>
        /// Projiziert diese Box auf eine Achse
        /// </summary>
        /// <param name="angle">Der Winkel der Achse im Bogenmaß</param>
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

        private bool ProjectionOverlaps(float[] a, float[] b)
        {
            if ((a[0] < b[0] && a[1] > b[0]) || (b[0] < a[0] && b[1] > a[0]))
                return true;
            return false;
        }
    }
}