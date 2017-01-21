using SkiaSharp;
using System.Drawing;

namespace Layered2D
{
    public static class SkiaSharpEx
    {
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
