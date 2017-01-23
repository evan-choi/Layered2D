using System;
using SkiaSharp;
using System.Runtime.InteropServices;
using static Layered2D.Interop.UnsafeNativeMethods;

namespace Layered2D
{
    public class LayeredBuffer
    {
        #region [ Property ]
        public SKColorType ColorType { get; }
        public SKAlphaType AlphaType { get; }
        #endregion

        #region [ Resources ]
        internal SKBitmap Bitmap { get; set; }
        internal RawSize Size { get; private set; }

        internal IntPtr screenDc;
        internal IntPtr memDc;
        internal IntPtr native;
        internal IntPtr scan0;
        internal IntPtr oldBitmap;

        internal BLENDFUNCTION blendFunc;
        #endregion

        #region [ Initializer ]
        public LayeredBuffer(int width, int height, SKColorType colorType, SKAlphaType alphaType)
        {
            this.Size = new RawSize(width, height);
            this.ColorType = colorType;
            this.AlphaType = alphaType;

            Initialize();
        }

        private void Initialize()
        {
            this.Bitmap = new SKBitmap();

            SwapChain();
        }
        #endregion

        #region [ User Methods ]
        public void Resize(int width, int height)
        {
            this.Size = new RawSize(width, height);

            ReleaseResources();
            SwapChain();
        }
        #endregion

        #region [ Context Handling ]
        private void SwapChain()
        {
            CreateNativeContext();

            var info = new SKImageInfo(
                Size.Width,
                Size.Height,
                SKColorType.Bgra8888,
                SKAlphaType.Premul);

            var result = this.Bitmap.InstallPixels(
                info, scan0,
                info.RowBytes,
                null, null,
                "RELEASING");
        }

        private void ReleaseResources()
        {
            ReleaseDC(IntPtr.Zero, screenDc);
            SelectObject(memDc, oldBitmap);
            DeleteDC(memDc);

            DeleteObject(native);
            DeleteObject(scan0);
            DeleteObject(oldBitmap);
        }

        internal void CreateNativeContext()
        {
            var bmh = new BITMAPV5HEADER()
            {
                bV5Size = (uint)Marshal.SizeOf(typeof(BITMAPV5HEADER)),
                bV5Width = Size.Width,
                bV5Height = -Size.Height,
                bV5Planes = 1,
                bV5BitCount = 32,
                bV5Compression = BitmapCompressionMode.BI_RGB,
                bV5AlphaMask = ColorMask.Alpha,
                bV5RedMask = ColorMask.Red,
                bV5GreenMask = ColorMask.Green,
                bV5BlueMask = ColorMask.Blue
            };

            blendFunc = new BLENDFUNCTION()
            {
                BlendOp = AC_SRC_OVER,
                BlendFlags = 0,
                SourceConstantAlpha = 255,
                AlphaFormat = AC_SRC_ALPHA
            };

            screenDc = GetDC(IntPtr.Zero);
            memDc = CreateCompatibleDC(screenDc);

            native = CreateDIBSection(screenDc, ref bmh, 0, out scan0, IntPtr.Zero, 0);
            oldBitmap = SelectObject(memDc, native);
        }
        #endregion
    }
}