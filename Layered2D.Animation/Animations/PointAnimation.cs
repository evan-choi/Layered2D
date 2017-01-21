using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Layered2D.Animations
{
    public class PointAnimation : Animation<SKPoint>
    {
        public PointAnimation() : base()
        {
        }

        internal override SKPoint OnApply(TimeSpan duration)
        {
            double x = EasingManager.ApplyEasing(
                this.Easing,
                duration.TotalMilliseconds,
                this.From.X,
                this.To.X - this.From.X,
                this.Duration.TotalMilliseconds);

            double y     = EasingManager.ApplyEasing(
                this.Easing,
                duration.TotalMilliseconds,
                this.From.Y,
                this.To.Y - this.From.Y,
                this.Duration.TotalMilliseconds);

            return new SKPoint((float)x, (float)y);
        }
    }
}
