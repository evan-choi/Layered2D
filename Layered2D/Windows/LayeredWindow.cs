using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using SkiaSharp;

using Layered2D.Interop;
using WS = Layered2D.Interop.UnsafeNativeMethods.WindowStyles;
using WindowLong = Layered2D.Interop.UnsafeNativeMethods.WindowLongFlags;

namespace Layered2D.Windows
{
    [DesignerCategory("code")]
    public class LayeredWindow : Form
    {
        #region [ Property ]
        private bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
        }

        public int NativeWidth
        {
            get { return base.Width; }
            set { base.Width = value; }
        }

        public int NativeHeight
        {
            get { return base.Height; }
            set { base.Height = value; }
        }

        public Size NativeSize
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        public new int Width
        {
            get { return (int)resolution.Width; }
            set
            {
                resolution.Width = value;

                if (isLoaded)
                    SetResolutionCore();
            }
        }

        public new int Height
        {
            get
            {
                return (int)resolution.Height;
            }
            set
            {
                resolution.Height = value;
            }
        }

        public new SKSize Size
        {
            get
            {
                return resolution;
            }
            set
            {
                resolution = value;

                if (isLoaded)
                    SetResolutionCore();
            }
        }

        private bool hitVisible = true;
        public bool HitVisible
        {
            get
            {
                return hitVisible;
            }
            set
            {
                hitVisible = value;

                if (IsLoaded)
                    UpdateHitestVisible();
            }
        }
        #endregion

        int winStyle;
        int winExStyle;

        LayeredBuffer buffer;
        LayeredContext context;

        SKSize resolution = new SKSize(-1, -1);
       
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                
                cp.ExStyle |= (int)WS.EX_LAYERED;

                return cp;
            }
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();

            this.FormBorderStyle = FormBorderStyle.None;

            winStyle = UnsafeNativeMethods.GetWindowLong(this.Handle, (int)WindowLong.GWL_STYLE);
            winExStyle = UnsafeNativeMethods.GetWindowLong(this.Handle, (int)WindowLong.GWL_EXSTYLE);
            
            UpdateHitestVisible();

            // Init client size
            if (this.Width == -1)
                this.Width = this.NativeWidth;

            if (this.Height == -1)
                this.Height = this.NativeHeight;

            // Init buffer
            buffer = new LayeredBuffer(
                this.Width,
                this.Height,
                SKColorType.Bgra8888,
                SKAlphaType.Premul);

            context = new LayeredContext(this.Handle, buffer);
            context.targetPosition = new UnsafeNativeMethods.RawPoint(this.Left, this.Top);

            // ready
            isLoaded = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Render();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            if (IsLoaded)
                context.targetPosition = new UnsafeNativeMethods.RawPoint(this.Left, this.Top);
        }

        private void UpdateHitestVisible()
        {
            int dwLong = winExStyle;

            if (!this.HitVisible)
                dwLong |= (int)WS.EX_TRANSPARENT;

            UnsafeNativeMethods.SetWindowLong(
                this.Handle,
                (int)WindowLong.GWL_EXSTYLE,
                dwLong);
        }

        public void SetResolution(SKSize size)
        {
            this.Size = size;
            SetResolutionCore();
        }

        private void SetResolutionCore()
        {
            if (isLoaded)
            {
                this.SuspendLayout();

                buffer.Resize(Width, Height);

                context.Dispose();
                context = new LayeredContext(this.Handle, buffer);

                this.Render();

                this.NativeWidth = this.Width;
                this.NativeHeight = this.Height;

                this.ResumeLayout(false);
            }
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