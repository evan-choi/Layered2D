using System;
using System.Globalization;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Layered2D.Interop;
using NativeMessage = Layered2D.Interop.UnsafeNativeMethods.NativeMessage;

namespace Layered2D.Windows
{
    public class RenderLoop : IDisposable
    {
        public delegate void RenderCallback();

        private IntPtr controlHandle;
        private Control control;
        private bool isControlAlive;
        private bool switchControl;

        public bool UseDoEvents { get; set; }
        
        public Control Control
        {
            get
            {
                return control;
            }
            set
            {
                if (control == value)
                    return;
                
                if (control != null && !switchControl)
                {
                    isControlAlive = false;
                    control.Disposed -= ControlDisposed;
                    controlHandle = IntPtr.Zero;
                }

                if (value != null && value.IsDisposed)
                {
                    throw new InvalidOperationException("컨트롤이 이미 삭제되었습니다.");
                }

                control = value;
                switchControl = true;
            }
        }

        public static bool IsIdle
        {
            get
            {
                NativeMessage msg;
                return (UnsafeNativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0) == 0);
            }
        }

        public RenderLoop()
        {
        }

        public RenderLoop(Control control)
        {
            Control = control;
        }

        private void ControlDisposed(object sender, EventArgs e)
        {
            isControlAlive = false;
        }

        public void Dispose()
        {
            Control = null;
        }

        public bool NextFrame()
        {
            if (switchControl && control != null)
            {
                controlHandle = control.Handle;
                control.Disposed += ControlDisposed;
                isControlAlive = true;
                switchControl = false;
            }

            if (isControlAlive)
            {
                if (UseDoEvents)
                {
                    Application.DoEvents();
                }
                else
                {
                    var localHandle = controlHandle;
                    if (localHandle != IntPtr.Zero)
                    {
                        NativeMessage msg;

                        while (UnsafeNativeMethods.PeekMessage(out msg, IntPtr.Zero, 0, 0, 0) != 0)
                        {
                            if (UnsafeNativeMethods.GetMessage(out msg, IntPtr.Zero, 0, 0) == -1)
                            {
                                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                                    "Windows 메시지를 처리하는 동안 렌더링 루프에서 오류가 발생했습니다. 오류: {0}",
                                    Marshal.GetLastWin32Error()));
                            }
                            
                            if (msg.msg == 130)
                                isControlAlive = false;

                            var message = new Message()
                            {
                                HWnd = msg.handle,
                                LParam = msg.lParam,
                                Msg = (int)msg.msg,
                                WParam = msg.wParam
                            };

                            if (!Application.FilterMessage(ref message))
                            {
                                UnsafeNativeMethods.TranslateMessage(ref msg);
                                UnsafeNativeMethods.DispatchMessage(ref msg);
                            }
                        }
                    }
                }
            }

            return isControlAlive || switchControl;
        }
        
        public static void Run(ApplicationContext context, RenderCallback renderCallback)
        {
            Run(context.MainForm, renderCallback);
        }

        public static void Run(Control form, RenderCallback renderCallback, bool useDoEvents = false)
        {
            if (form == null || renderCallback == null)
                throw new ArgumentNullException();

            form.Show();

            using (var render = new RenderLoop(form))
            {
                render.UseDoEvents = useDoEvents;

                while (render.NextFrame())
                {
                    renderCallback();
                }
            }
        }
    }
}
