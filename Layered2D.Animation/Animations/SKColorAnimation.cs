using System;
using SkiaSharp;

namespace Layered2D.Animations
{
    public class SKColorAnimation : Animation<SKColor>
    {
        public SKColorAnimation() : base()
        { 
        }

        internal override SKColor OnApply(TimeSpan duration)
        {
            double fromValue = this.From.ToInt();
            double toValue = this.To.ToInt();

            uint animateValue = (uint)EasingManager.ApplyEasing(
                this.Easing,
                duration.TotalMilliseconds,
                fromValue,
                fromValue - toValue,
                this.Duration.TotalMilliseconds);
            
            return new SKColor(animateValue);
        }
    }
}
