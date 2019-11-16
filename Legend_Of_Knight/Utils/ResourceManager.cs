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
        private const int ANIMATION_LENGTH = 2;

        public static Bitmap GetImage(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Legend_Of_Knight.Resources." + path;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                return new Bitmap(stream);
        }

        public static Bitmap[][] GetImages<T>(T t)
        {
            Bitmap[][] images = new Bitmap[ANIMATION_LENGTH][];
            for(int i = 0; i < images.Length; i++)
                images[i] = new Bitmap[ANIMATION_LENGTH];
            string pathName = t.GetType().Name;
            for (int i = 0; i < ANIMATION_LENGTH; i++)
                images[0][i] = GetImage($"{pathName}.Right.{i}.png");
            for (int i = 0; i < ANIMATION_LENGTH; i++)
                images[1][i] = GetImage($"{pathName}.Left.{i}.png");
            return images;
        }
    }
}
