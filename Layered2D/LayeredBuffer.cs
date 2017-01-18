using SkiaSharp;
using static Layered2D.Interop.UnsafeNativeMethods;

namespace Layered2D
{
    public class LayeredBuffer : SKBitmap
    {
        internal RawSize Size { get; private set; }

        public LayeredBuffer(int width, int height, SKColorType colorType, SKAlphaType alphaType) :
            base(width, height, colorType, alphaType)
        {
            Size = new RawSize(width, height);
        }
    }
}
