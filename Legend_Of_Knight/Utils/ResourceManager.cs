using Legend_Of_Knight.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Manager = System.Resources.ResourceManager;

namespace Legend_Of_Knight.Utils
{
    public class ResourceManager
    {
        public static Bitmap GetImage(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Legend_Of_Knight.Resources." + path;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                if (stream == null)
                    return null;
                else
                    return new Bitmap(stream);
        }

        public static Bitmap[] GetImages<T>(T t, string name)
        {
            List<Bitmap> images = new List<Bitmap>();
            string pathName = t.GetType().Name;
            Bitmap bmp = GetImage($"{pathName}.{name}.00.png");
            for (int i = 1; bmp != null; i++)
            {
                images.Add(bmp);
                bmp = GetImage($"{pathName}.{name}.{String.Format("{0:00}", i)}.png");
            }
            return images.ToArray();
        }
    }
}
