using SkiaSharp;
using System;

namespace Layered2D.Animations
{
    public class SizeAnimation : Animation<SKSize>
    {
        public SizeAnimation() : base()
        {
        }

        internal override SKSize OnApply(TimeSpan duration)
        {
            double width = EasingManager.ApplyEasing(
                this.Easing,
                duration.TotalMilliseconds,
                this.From.Width,
                this.To.Width - this.From.Width,
                this.Duration.TotalMilliseconds);

            double height = EasingManager.ApplyEasing(
                this.Easing,
                duration.TotalMilliseconds,
                this.From.Height,
                this.To.Height - this.From.Height,
                this.Duration.TotalMilliseconds);

            return new SKSize((float)width, (float)height);
        }
    }
}
