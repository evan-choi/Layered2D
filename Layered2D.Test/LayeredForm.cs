using Layered2D.Windows;
using SkiaSharp;
using System;
using System.Windows.Forms;

namespace Layered2D.Test
{
    class LayeredForm : LayeredWindow
    {
        FPSCounter fps;
        SKPaint p;

        public LayeredForm() : base()
        {
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;

            fps = new FPSCounter();
            p = new SKPaint()
            {
                Color = SKColors.Red,
                Style = SKPaintStyle.Stroke,
                TextSize = 20,
                IsAntialias = true
            };
        }

        public override void OnRender(LayeredContext context)
        {
            base.OnRender(context);
            fps.Update();

            //context.Clear(SKColors.White);

            for (int i = 0; i < 2048; i++)
            {
                context.DrawOval(100 + i, 100 + i, 50, 50, p);
            }

            context.DrawText(fps.FPS.ToString(), 10, 30, p);
        }
    }

    class FPSCounter
    {
        int fpsStep = 0;
        int lastFps = 0;
        DateTime lastDate = DateTime.Now;

        public int FPS
        {
            get
            {
                return lastFps;
            }
        }

        public void Update()
        {
            fpsStep++;

            var delta = DateTime.Now - lastDate;

            if (delta.TotalSeconds >= 1)
            {
                lastDate = DateTime.Now;

                lastFps = fpsStep;
                fpsStep = 0;
            }
        }
    }
}
