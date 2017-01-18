using SkiaSharp;

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
    }
}
