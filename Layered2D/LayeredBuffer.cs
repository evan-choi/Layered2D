using SkiaSharp;
using static Layered2D.Interop.UnsafeNativeMethods;

namespace Layered2D
{
    public class LayeredBuffer
    {
        internal SKBitmap Bitmap { get; set; }
        internal RawSize Size { get; private set; }

        public LayeredBuffer(int width, int height, SKColorType colorType, SKAlphaType alphaType)
        {
            this.Size = new RawSize(width, height);

            this.Bitmap = new SKBitmap(width, height, colorType, alphaType);
        }

        public void Resize(int width, int height)
        {
            this.Size = new RawSize(width, height);

            var info = this.Bitmap.Info;
            this.Bitmap.Dispose();
            this.Bitmap = new SKBitmap(width, height, info.ColorType, info.AlphaType);
        }
    }
}