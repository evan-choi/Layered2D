using Layered2D.Animations;
using Layered2D.Windows;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Layered2D.Test
{
    [DesignerCategory("code")]
    public class Form1 : LayeredWindow
    {
        SKBitmap profile;

        public Form1() : base()
        {
            this.SuspendLayout();
            this.Width = Properties.Resources.profile.Width;
            this.Height = Properties.Resources.profile.Height;
            this.StartPosition = FormStartPosition.Manual;
            this.Left = 100;
            this.ResumeLayout(false);

            var bmp = Properties.Resources.profile;
            using (var ms = new System.IO.MemoryStream())
            {
                bmp.Save(ms, ImageFormat.Png);

                profile = SKBitmap.Decode(ms.ToArray(), new SKImageInfo(bmp.Width, bmp.Height, SKColorType.Rgba8888));
            }
        }

        public override void OnRender(LayeredContext context)
        {
            base.OnRender(context);
            
            using (var p = new SKPaint()
            {
                Color = SKColors.Blue,
                Style = SKPaintStyle.Stroke,
                StrokeWidth = 1
            })
            {
                context.DrawRect(new SKRect(0, 0, Width - 1, Height - 1), p);
            }

            using (var p = new SKPaint()
            {
                Color = SKColors.Red,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 4
            })
            {
                context.DrawRect(new SKRect(Width - 10, Height - 10, Width, Height), p);
            }

            context.DrawBitmap(profile,
                Width / 2 - profile.Width / 2,
                Height / 2 - profile.Height / 2);
        }


        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (drag)
            {
                var delta = new Size(e.X, e.Y) - (Size)pt;

                this.Size = sz + new SKSize(delta.Width, delta.Height);
            }

            if (isMove)
            {
                var delta = (Size)MousePosition - (Size)mPt;
                this.Location = moPt + delta;
            }
        }

        bool drag = false;
        SKSize sz;
        Point pt;

        bool isMove = false;
        Point mPt;
        Point moPt;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            var pRect = new SKRect(
                Width / 2 - profile.Width / 2,
                Height / 2 - profile.Height / 2,
                Width / 2 + profile.Width / 2,
                Height / 2 + profile.Height / 2);
            
            if (e.X > this.Width - 10 && e.Y > this.Height - 10)
            {
                drag = true;
                sz = this.Size;
                pt = new Point(e.X, e.Y);
            }
            else
            {
                isMove = true;
                mPt = MousePosition;
                moPt = this.Location;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            drag = false;
            isMove = false;
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);

            var sz = new SizeAnimation()
            {
                From = this.Size,
                To = Screen.GetWorkingArea(this).Size.ToSKSize(),
                Easing = EasingTypes.ExpoOut,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            var pt = new PointAnimation()
            {
                From = this.Location.ToSKPoint(),
                To = Screen.GetWorkingArea(this).Location.ToSKPoint(),
                Easing = EasingTypes.ExpoOut,
                Duration = TimeSpan.FromMilliseconds(300)
            };

            sz.ValueChanged += (s, v) =>
            {
                this.SuspendLayout();
                this.Size = v;
            };

            pt.ValueChanged += (s, v) =>
            {
                this.Location = new Point((int)v.X, (int)v.Y);
                this.ResumeLayout();
            };

            sz.Start(true);
            pt.Start(true);
        }
    }
}
