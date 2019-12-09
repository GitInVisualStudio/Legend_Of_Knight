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
        private Vector[] original;

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
            owner.Moved += Owner_Moved;

            original = new Vector[4] //Nicht absolut, nur die Größe sonst wird beim bewegen alles verfälscht
            {
                new Vector(-width / 2, -height / 2),
                new Vector(width / 2, -height / 2),
                new Vector(width / 2, height / 2),
                new Vector(-width / 2, height / 2)
            };
            corners = new Vector[4];
            Owner_Rotated(this, Owner.Rotation);
        }

        private void Owner_Moved(object sender, Vector e)
        {
            Owner_Rotated(owner, owner.Rotation);
        }

        private void Owner_Rotated(object sender, float angle)
        {

            for (int i = 0; i < corners.Length; i++)
            {
                Vector current = original[i];
                float x = owner.X + current.X * MathUtils.Cos(angle) - current.Y * MathUtils.Sin(angle);
                float y = owner.Y + current.X * MathUtils.Sin(angle) + current.Y * MathUtils.Cos(angle);
                corners[i] = new Vector(x, y);
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