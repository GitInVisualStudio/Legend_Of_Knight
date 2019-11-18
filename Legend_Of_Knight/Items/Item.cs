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
        private Bitmap image;
        private string name;

        public Item(string path)
        {
            image = ResourceManager.GetImage("Items." + path);
            name = path;
        }

        public Bitmap Image { get { return image; } set { image = value; } }
        public string Name { get { return name; } set { name = value; } }
    }
}
