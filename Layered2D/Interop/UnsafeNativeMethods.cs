using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Layered2D.Interop
{
    internal static class UnsafeNativeMethods
    {
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
        public const int ULW_ALPHA = 0x00000002;

        [DllImport(ExternDLL.User32, EntryPoint = "PeekMessage"), SuppressUnmanagedCodeSecurity]
        public static extern int PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax, int wRemoveMsg);

        [DllImport(ExternDLL.User32, EntryPoint = "GetMessage"), SuppressUnmanagedCodeSecurity]
        public static extern int GetMessage(out NativeMessage lpMsg, IntPtr hWnd, int wMsgFilterMin, int wMsgFilterMax);

        [DllImport(ExternDLL.User32, EntryPoint = "TranslateMessage"), SuppressUnmanagedCodeSecurity]
        public static extern int TranslateMessage(ref NativeMessage lpMsg);

        [DllImport(ExternDLL.User32, EntryPoint = "DispatchMessage"), SuppressUnmanagedCodeSecurity]
        public static extern int DispatchMessage(ref NativeMessage lpMsg);

        [DllImport(ExternDLL.User32, ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref RawPoint pptDst, ref RawSize psize, IntPtr hdcSrc, ref RawPoint pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);
        
        [DllImport(ExternDLL.User32, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport(ExternDLL.User32, ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport(ExternDLL.User32)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport(ExternDLL.User32, SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport(ExternDLL.Gdi32)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BITMAPV5HEADER pbmi, uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport(ExternDLL.Gdi32, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport(ExternDLL.Gdi32, ExactSpelling = true, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hdc);
        
        [DllImport(ExternDLL.Gdi32, ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport(ExternDLL.Gdi32, EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr handle;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public RawPoint p;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RawPoint
        {
            public int X;
            public int Y;

            public RawPoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RawSize
        {
            public int Width;
            public int Height;

            public RawSize(int width, int height)
            {
                Width = width;
                Height = height;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BITMAPV5HEADER
        {
            public uint bV5Size;
            public int bV5Width;
            public int bV5Height;
            public ushort bV5Planes;
            public ushort bV5BitCount;
            public BitmapCompressionMode bV5Compression;
            public uint bV5SizeImage;
            public int bV5XPelsPerMeter;
            public int bV5YPelsPerMeter;
            public uint bV5ClrUsed;
            public uint bV5ClrImportant;
            public uint bV5RedMask;
            public uint bV5GreenMask;
            public uint bV5BlueMask;
            public uint bV5AlphaMask;
            public uint bV5CSType;
            public CIEXYZTRIPLE bV5Endpoints;
            public uint bV5GammaRed;
            public uint bV5GammaGreen;
            public uint bV5GammaBlue;
            public uint bV5Intent;
            public uint bV5ProfileData;
            public uint bV5ProfileSize;
            public uint bV5Reserved;
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct CIEXYZTRIPLE
        {
            public CIEXYZ ciexyzRed;
            public CIEXYZ ciexyzGreen;
            public CIEXYZ ciexyzBlue;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CIEXYZ
        {
            public int ciexyzX;
            public int ciexyzY;
            public int ciexyzZ;
        }

        [Flags]
        public enum BitmapCompressionMode : uint
        {
            BI_RGB = 0,
            BI_RLE8 = 1,
            BI_RLE4 = 2,
            BI_BITFIELDS = 3,
            BI_JPEG = 4,
            BI_PNG = 5
        }

        public sealed class WindowStyles
        {
            public const uint OVERLAPPED = 0x00000000;
            public const uint POPUP = 0x80000000;
            public const uint CHILD = 0x40000000;
            public const uint MINIMIZE = 0x20000000;
            public const uint VISIBLE = 0x10000000;
            public const uint DISABLED = 0x08000000;
            public const uint CLIPSIBLINGS = 0x04000000;
            public const uint CLIPCHILDREN = 0x02000000;
            public const uint MAXIMIZE = 0x01000000;
            public const uint CAPTION = 0x00C00000;
            public const uint BORDER = 0x00800000;
            public const uint DLGFRAME = 0x00400000;
            public const uint VSCROLL = 0x00200000;
            public const uint HSCROLL = 0x00100000;
            public const uint SYSMENU = 0x00080000;
            public const uint THICKFRAME = 0x00040000;
            public const uint GROUP = 0x00020000;
            public const uint TABSTOP = 0x00010000;

            public const uint MINIMIZEBOX = 0x00020000;
            public const uint MAXIMIZEBOX = 0x00010000;

            public const uint TILED = OVERLAPPED;
            public const uint ICONIC = MINIMIZE;
            public const uint SIZEBOX = THICKFRAME;
            public const uint TILEDWINDOW = OVERLAPPEDWINDOW;
            
            public const uint OVERLAPPEDWINDOW =
                (OVERLAPPED |
                  CAPTION |
                  SYSMENU |
                  THICKFRAME |
                  MINIMIZEBOX |
                  MAXIMIZEBOX);

            public const uint POPUPWINDOW =
                (POPUP |
                  BORDER |
                  SYSMENU);

            public const uint CHILDWINDOW = CHILD;
            
            public const uint EX_DLGMODALFRAME = 0x00000001;
            public const uint EX_NOPARENTNOTIFY = 0x00000004;
            public const uint EX_TOPMOST = 0x00000008;
            public const uint EX_ACCEPTFILES = 0x00000010;
            public const uint EX_TRANSPARENT = 0x00000020;
            
            public const uint EX_MDICHILD = 0x00000040;
            public const uint EX_TOOLWINDOW = 0x00000080;
            public const uint EX_WINDOWEDGE = 0x00000100;
            public const uint EX_CLIENTEDGE = 0x00000200;
            public const uint EX_CONTEXTHELP = 0x00000400;

            public const uint EX_RIGHT = 0x00001000;
            public const uint EX_LEFT = 0x00000000;
            public const uint EX_RTLREADING = 0x00002000;
            public const uint EX_LTRREADING = 0x00000000;
            public const uint EX_LEFTSCROLLBAR = 0x00004000;
            public const uint EX_RIGHTSCROLLBAR = 0x00000000;

            public const uint EX_CONTROLPARENT = 0x00010000;
            public const uint EX_STATICEDGE = 0x00020000;
            public const uint EX_APPWINDOW = 0x00040000;

            public const uint EX_OVERLAPPEDWINDOW = (EX_WINDOWEDGE | EX_CLIENTEDGE);
            public const uint EX_PALETTEWINDOW = (EX_WINDOWEDGE | EX_TOOLWINDOW | EX_TOPMOST);

            public const uint EX_LAYERED = 0x00080000;

            public const uint EX_NOINHERITLAYOUT = 0x00100000;
            public const uint EX_LAYOUTRTL = 0x00400000;

            public const uint EX_COMPOSITED = 0x02000000;
            public const uint EX_NOACTIVATE = 0x08000000;
        }

        public enum WindowLongFlags : int
        {
            GWL_EXSTYLE = -20,
            GWLP_HINSTANCE = -6,
            GWLP_HWNDPARENT = -8,
            GWL_ID = -12,
            GWL_STYLE = -16,
            GWL_USERDATA = -21,
            GWL_WNDPROC = -4,
            DWLP_USER = 0x8,
            DWLP_MSGRESULT = 0x0,
            DWLP_DLGPROC = 0x4
        }
    }
}
