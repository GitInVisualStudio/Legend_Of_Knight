using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Legend_Of_Knight.Utils.Render
{
    public class RenderUtils
    {

        public static Bitmap PaintBitmap(Bitmap b, Color color, bool copy = false)
        {
            Bitmap img = b;

            if (copy)
                img = new Bitmap(b);

            BitmapData bSrc = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadWrite, img.PixelFormat);
            int bytesPerPixel = Bitmap.GetPixelFormatSize(img.PixelFormat) / 8;
            int byteCount = bSrc.Stride * img.Height;
            byte[] pixelSrc = new byte[byteCount];
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bSrc.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
            int heightInPixels = bSrc.Height;
            int widthInBytes = bSrc.Width * bytesPerPixel;
            for (int y = 0; y < heightInPixels; y++)
            {
                int currentLine = y * bSrc.Stride;
                for (int x = 0; x < widthInBytes; x += bytesPerPixel)
                {
                    //Geht die umliegenden Pixel durch und berechnet eine Durchschnitt dadurch
                    if (pixels[currentLine + x + 3] == 0)
                        continue;
                    pixels[currentLine + x + 2] = color.R;
                    pixels[currentLine + x + 1] = color.G;
                    pixels[currentLine + x + 0] = color.B;
                }
            }

            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            img.UnlockBits(bSrc);
            return img;
        }
    }
}
