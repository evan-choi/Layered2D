using SkiaSharp;
using System.Drawing;

namespace Layered2D
{
    public static class SkiaSharpEx
    {
        public static int ToInt(this SKColor c)
        {
            return c.Alpha << 24 +
                   c.Red << 16 +
                   c.Green << 8 +
                   c.Blue;
        }

        public static SKBitmap ToSKBitmap(this Bitmap bmp)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, bmp.RawFormat);
                
                return SKBitmap.Decode(
                    ms.ToArray(), 
                    new SKImageInfo(bmp.Width, bmp.Height, SKColorType.Rgba8888));
            }
        }
    }
}
