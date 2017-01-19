using System;
using SkiaSharp;
using System.Runtime.InteropServices;
using static Layered2D.Interop.UnsafeNativeMethods;

namespace Layered2D
{
    public class LayeredContext : SKCanvas
    {
        RawPoint emptyPoint = new RawPoint(0, 0);
        RawSize screenSize;

        LayeredBuffer buffer;
        BLENDFUNCTION blendFunc;
        
        IntPtr target;
        IntPtr native;
        IntPtr scan0;
        
        IntPtr oldBitmap;
        IntPtr screenDc;
        IntPtr memDc;

        public LayeredContext(IntPtr target, LayeredBuffer buffer) : base(buffer)
        {
            this.target = target;
            this.buffer = buffer;
            
            Initialize();
        }

        private void Initialize()
        {
            var bmh = new BITMAPV5HEADER()
            {
                bV5Size = (uint)Marshal.SizeOf(typeof(BITMAPV5HEADER)),
                bV5Width = buffer.Width,
                bV5Height = -buffer.Height,
                bV5Planes = 1,
                bV5BitCount = 32,
                bV5Compression = BitmapCompressionMode.BI_RGB,
                bV5AlphaMask = 0xFF000000,
                bV5RedMask = 0x00FF0000,
                bV5GreenMask = 0x0000FF00,
                bV5BlueMask = 0x000000FF
            };

            blendFunc = new BLENDFUNCTION()
            {
                BlendOp = AC_SRC_OVER,
                BlendFlags = 0,
                SourceConstantAlpha = 255,
                AlphaFormat = AC_SRC_ALPHA
            };

            screenSize = new RawSize(buffer.Width, buffer.Height);

            screenDc = GetDC(IntPtr.Zero);
            memDc = CreateCompatibleDC(screenDc);

            native = CreateDIBSection(screenDc, ref bmh, 0, out scan0, IntPtr.Zero, 0);
            oldBitmap = SelectObject(memDc, native);
        }
        
        public void Present()
        {
            buffer.CopyPixelsTo(scan0, buffer.ByteCount);
            
            UpdateLayeredWindow(
                target,
                screenDc, 
                ref emptyPoint, ref screenSize,
                memDc,
                ref emptyPoint, 0,
                ref blendFunc, 
                ULW_ALPHA);
        }
    }
}