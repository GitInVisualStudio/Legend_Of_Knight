using Legend_Of_Knight.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Items
{
    public class Item
    {
        private Bitmap image;//Bild für das Item
        private string name;
        private float damage; // Schaden, die diese Waffe anrichtet

        public Item(string path, float damage)
        {
            image = ResourceManager.GetImage("Items." + path);
            name = path;
            this.damage = damage;
        }

        public Bitmap Image { get { return image; } set { image = value; } }
        public string Name { get { return name; } set { name = value; } }

        public float Damage { get => damage; set => damage = value; }
    }
}
