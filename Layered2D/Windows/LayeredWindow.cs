using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

using SkiaSharp;
using SkiaSharp.Views.Desktop;

using WS = Layered2D.Interop.UnsafeNativeMethods.WindowStyles;

namespace Layered2D.Windows
{
    [DesignerCategory("code")]
    public class LayeredWindow : Form
    {
        #region [ Property ]
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

        public new SKPoint Location
        {
            get { return base.Location.ToSKPoint(); }
            set { base.Location = new Point((int)value.X, (int)value.Y); }
        }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get { return isLoaded; }
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
                    UpdateHitVisible();
            }
        }

        int fixedFrame = 60;
        public int FixedFrame
        {
            get
            {
                return fixedFrame;
            }
            set
            {
                fixedFrame = value;
                frameAccurate = 1000d / (value + 0.5d);
            }
        }
        #endregion

        #region [ Resources ]
        int winStyle;
        int winExStyle;

        LayeredBuffer buffer;
        LayeredContext context;

        SKSize resolution = new SKSize(-1, -1);

        double frameAccurate = 1000d / 60;
        DateTime frameTime;
        #endregion

        #region [ Initializer ]

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

            winStyle = this.GetWindowStyle();
            winExStyle = this.GetWindowExtendedStyle();

            UpdateHitVisible();

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
            context.targetPosition = this.Location.ToRawPoint();

            // ready
            isLoaded = true;

            frameTime = DateTime.Now;
        }

        #endregion
        
        #region [ Update ]
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Render();
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            if (IsLoaded)
                context.targetPosition = this.Location.ToRawPoint();
        }

        private void UpdateHitVisible()
        {
            int dwLong = winExStyle;

            if (!this.HitVisible)
                dwLong |= (int)WS.EX_TRANSPARENT;

            this.SetWindowExtendedStyle(dwLong);
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

                var position = context.targetPosition;
                context.Dispose();
                context = new LayeredContext(this.Handle, buffer);
                context.targetPosition = position;
                
                this.Render();

                this.NativeWidth = this.Width;
                this.NativeHeight = this.Height;

                this.ResumeLayout(false);
            }
        }

        #endregion

        #region [ Render ]
        public void Render()
        {
            if (FixedFrame > 0)
            {
                if (frameTime > DateTime.Now)
                    return;

                frameTime = frameTime.Add(TimeSpan.FromMilliseconds(frameAccurate));
            }

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
        #endregion
    }
}