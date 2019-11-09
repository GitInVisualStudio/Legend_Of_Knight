using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Legend_Of_Knight.Utils;

namespace Legend_Of_Knight.Entities
{
    public class BoundingBox
    {
        private Entity owner;
        private float width;
        private float height;
        private Vector[] corners;

        public event EventHandler<CollisionArgs> Collided;

        internal Entity Owner
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

        public BoundingBox(Entity owner, float width, float height)
        {
            this.owner = owner;
            this.width = width;
            this.height = height;
            owner.Rotated += Owner_Rotated;

            corners = new Vector[4]
            {
                new Vector(owner.X - width / 2, owner.Y - height / 2),
                new Vector(owner.X + width / 2, owner.Y - height / 2),
                new Vector(owner.X + width / 2, owner.Y + height / 2),
                new Vector(owner.X - width / 2, owner.Y + height / 2)
            };
        }

        private void Owner_Rotated(object sender, float angle)
        {
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
            // Achsen dieser Box
            Vector xAxis = new Vector(corners[0][0] - corners[1][0], corners[0][1] - corners[1][1]);
            Vector yAxis = new Vector(corners[1][0] - corners[2][0], corners[1][1] - corners[2][1]);
            
            if (true && Collided != null)
                Collided(this, new CollisionArgs() 
                {
                    Boxes = new BoundingBox[] {this, box},
                    Position = new Vector(2)
                });
            return false;
        }

        
    }
}
