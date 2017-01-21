using System.Windows.Forms;

using Layered2D.Interop;
using WindowLong = Layered2D.Interop.UnsafeNativeMethods.WindowLongFlags;

namespace Layered2D
{
    internal static class FormEx
    {
        public static int GetWindowStyle(this Form frm)
        {
            return UnsafeNativeMethods.GetWindowLong(frm.Handle, (int)WindowLong.GWL_STYLE);
        }

        public static int GetWindowExtendedStyle(this Form frm)
        {
            return UnsafeNativeMethods.GetWindowLong(frm.Handle, (int)WindowLong.GWL_EXSTYLE);
        }

        public static void SetWindowStyle(this Form frm, int style)
        {
            UnsafeNativeMethods.SetWindowLong(frm.Handle, (int)WindowLong.GWL_STYLE, style);
        }

        public static void SetWindowExtendedStyle(this Form frm, int exStyle)
        {
            UnsafeNativeMethods.SetWindowLong(frm.Handle, (int)WindowLong.GWL_EXSTYLE, exStyle);
        }

    }
}