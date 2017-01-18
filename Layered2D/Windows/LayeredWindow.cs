using Layered2D.Interop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using static Layered2D.Interop.UnsafeNativeMethods;
using WS = Layered2D.Interop.UnsafeNativeMethods.WindowStyles;

namespace Layered2D.Windows
{
    public class LayeredWindow : Form
    {
        LayeredBuffer buffer;
        LayeredContext context;

        public LayeredWindow()
        {
            this.FormBorderStyle = FormBorderStyle.None;
        }
       
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                cp.ExStyle |= (int)WS.EX_LAYERED;
                cp.ExStyle |= (int)WS.EX_TRANSPARENT;

                return cp;
            }
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            buffer = new LayeredBuffer(
                this.Width,
                this.Height,
                SKColorType.Bgra8888,
                SKAlphaType.Premul);

            context = new LayeredContext(this.Handle, buffer);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Render();
        }

        public void Render()
        {
            context.Clear();

            OnRender(context);
            OnPresent();
        }

        public virtual void OnRender(LayeredContext context)
        {
        }

        private void OnPresent()
        {
            context.Present();
        }
    }
}