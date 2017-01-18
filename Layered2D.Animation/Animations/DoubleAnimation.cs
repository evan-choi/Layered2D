using System;

namespace Layered2D.Animations
{
    public class Animation : Animation<double>
    {
        public Animation() : base()
        {
        }

        internal override double OnApply(TimeSpan duration)
        {
            return EasingManager.ApplyEasing(
                this.Easing,
                duration.TotalMilliseconds,
                this.From,
                this.To - this.From,
                this.Duration.TotalMilliseconds);
        }
    }
}
