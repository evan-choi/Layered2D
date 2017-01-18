using System;

namespace Layered2D.Animations
{
    internal static class EasingManager
    {
        public static double ApplyEasing(EasingTypes easing, double time, double initial, double change, double duration)
        {
            if (change == 0 || time == 0 || duration == 0) return initial;
            if (time == duration) return initial + change;

            time = Math.Min(time, duration);

            switch (easing)
            {
                default:
                    return change * (time / duration) + initial;

                case EasingTypes.In:
                case EasingTypes.QuadIn:
                    return change * (time /= duration) * time + initial;

                case EasingTypes.Out:
                case EasingTypes.QuadOut:
                    return -change * (time /= duration) * (time - 2) + initial;

                case EasingTypes.QuadInOut:
                    if ((time /= duration / 2) < 1)
                        return change / 2 * time * time + initial;

                    return -change / 2 * ((--time) * (time - 2) - 1) + initial;

                case EasingTypes.CubicIn:
                    return change * (time /= duration) * time * time + initial;

                case EasingTypes.CubicOut:
                    return change * ((time = time / duration - 1) * time * time + 1) + initial;

                case EasingTypes.CubicInOut:
                    if ((time /= duration / 2) < 1)
                        return change / 2 * time * time * time + initial;

                    return change / 2 * ((time -= 2) * time * time + 2) + initial;

                case EasingTypes.QuartIn:
                    return change * (time /= duration) * time * time * time + initial;

                case EasingTypes.QuartOut:
                    return -change * ((time = time / duration - 1) * time * time * time - 1) + initial;

                case EasingTypes.QuartInOut:
                    if ((time /= duration / 2) < 1)
                        return change / 2 * time * time * time * time + initial;

                    return -change / 2 * ((time -= 2) * time * time * time - 2) + initial;

                case EasingTypes.QuintIn:
                    return change * (time /= duration) * time * time * time * time + initial;

                case EasingTypes.QuintOut:
                    return change * ((time = time / duration - 1) * time * time * time * time + 1) + initial;

                case EasingTypes.QuintInOut:
                    if ((time /= duration / 2) < 1)
                        return change / 2 * time * time * time * time * time + initial;

                    return change / 2 * ((time -= 2) * time * time * time * time + 2) + initial;

                case EasingTypes.SineIn:
                    return -change * Math.Cos(time / duration * (Math.PI / 2)) + change + initial;

                case EasingTypes.SineOut:
                    return change * Math.Sin(time / duration * (Math.PI / 2)) + initial;

                case EasingTypes.SineInOut:
                    return -change / 2 * (Math.Cos(Math.PI * time / duration) - 1) + initial;

                case EasingTypes.ExpoIn:
                    return change * Math.Pow(2, 10 * (time / duration - 1)) + initial;

                case EasingTypes.ExpoOut:
                    return (time == duration) ? initial + change : change * (-Math.Pow(2, -10 * time / duration) + 1) + initial;

                case EasingTypes.ExpoInOut:
                    if ((time /= duration / 2) < 1)
                        return change / 2 * Math.Pow(2, 10 * (time - 1)) + initial;

                    return change / 2 * (-Math.Pow(2, -10 * --time) + 2) + initial;

                case EasingTypes.CircIn:
                    return -change * (Math.Sqrt(1 - (time /= duration) * time) - 1) + initial;

                case EasingTypes.CircOu:
                    return change * Math.Sqrt(1 - (time = time / duration - 1) * time) + initial;

                case EasingTypes.CircInOut:
                    if ((time /= duration / 2) < 1)
                        return -change / 2 * (Math.Sqrt(1 - time * time) - 1) + initial;

                    return change / 2 * (Math.Sqrt(1 - (time -= 2) * time) + 1) + initial;

                case EasingTypes.ElasticIn:
                    {
                        if ((time /= duration) == 1) return initial + change;

                        var p = duration * .3;
                        var a = change;
                        var s = 1.70158;

                        if (a < Math.Abs(change)) { a = change; s = p / 4; }
                        else s = p / (2 * Math.PI) * Math.Asin(change / a);
                        return -(a * Math.Pow(2, 10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p)) + initial;
                    }

                case EasingTypes.ElasticOut:
                    {
                        if ((time /= duration) == 1) return initial + change;

                        var p = duration * .3;
                        var a = change;
                        var s = 1.70158;

                        if (a < Math.Abs(change)) { a = change; s = p / 4; }
                        else s = p / (2 * Math.PI) * Math.Asin(change / a);
                        return a * Math.Pow(2, -10 * time) * Math.Sin((time * duration - s) * (2 * Math.PI) / p) + change + initial;
                    }

                case EasingTypes.ElasticHalfOut:
                    {
                        if ((time /= duration) == 1) return initial + change;

                        var p = duration * .3;
                        var a = change;
                        var s = 1.70158;

                        if (a < Math.Abs(change)) { a = change; s = p / 4; }
                        else s = p / (2 * Math.PI) * Math.Asin(change / a);
                        return a * Math.Pow(2, -10 * time) * Math.Sin((0.5f * time * duration - s) * (2 * Math.PI) / p) + change + initial;
                    }

                case EasingTypes.ElasticQuarterOut:
                    {
                        if ((time /= duration) == 1) return initial + change;

                        var p = duration * .3;
                        var a = change;
                        var s = 1.70158;

                        if (a < Math.Abs(change)) { a = change; s = p / 4; }
                        else s = p / (2 * Math.PI) * Math.Asin(change / a);
                        return a * Math.Pow(2, -10 * time) * Math.Sin((0.25f * time * duration - s) * (2 * Math.PI) / p) + change + initial;
                    }

                case EasingTypes.ElasticInOut:
                    {
                        if ((time /= duration / 2) == 2) return initial + change;

                        var p = duration * (.3 * 1.5);
                        var a = change;
                        var s = 1.70158;
                        if (a < Math.Abs(change)) { a = change; s = p / 4; }
                        else s = p / (2 * Math.PI) * Math.Asin(change / a);
                        if (time < 1) return -.5 * (a * Math.Pow(2, 10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p)) + initial;
                        return a * Math.Pow(2, -10 * (time -= 1)) * Math.Sin((time * duration - s) * (2 * Math.PI) / p) * .5 + change + initial;
                    }

                case EasingTypes.BackIn:
                    {
                        var s = 1.70158;
                        return change * (time /= duration) * time * ((s + 1) * time - s) + initial;
                    }

                case EasingTypes.BackOut:
                    {
                        var s = 1.70158;
                        return change * ((time = time / duration - 1) * time * ((s + 1) * time + s) + 1) + initial;
                    }

                case EasingTypes.BackInOut:
                    {
                        var s = 1.70158;
                        if ((time /= duration / 2) < 1) return change / 2 * (time * time * (((s *= (1.525)) + 1) * time - s)) + initial;
                        return change / 2 * ((time -= 2) * time * (((s *= (1.525)) + 1) * time + s) + 2) + initial;
                    }

                case EasingTypes.BounceIn:
                    return change - ApplyEasing(EasingTypes.BounceOut, duration - time, 0, change, duration) + initial;

                case EasingTypes.BounceOut:
                    if ((time /= duration) < (1 / 2.75))
                    {
                        return change * (7.5625 * time * time) + initial;
                    }
                    else if (time < (2 / 2.75))
                    {
                        return change * (7.5625 * (time -= (1.5 / 2.75)) * time + .75) + initial;
                    }
                    else if (time < (2.5 / 2.75))
                    {
                        return change * (7.5625 * (time -= (2.25 / 2.75)) * time + .9375) + initial;
                    }
                    else
                    {
                        return change * (7.5625 * (time -= (2.625 / 2.75)) * time + .984375) + initial;
                    }

                case EasingTypes.BounceInOut:
                    if (time < duration / 2)
                        return ApplyEasing(EasingTypes.BounceIn, time * 2, 0, change, duration) * .5 + initial;

                    return ApplyEasing(EasingTypes.BounceOut, time * 2 - duration, 0, change, duration) * .5 + change * .5 + initial;

                case EasingTypes.SpringIn:
                    {
                        var s = 1.70158;
                        double x = (time / duration);
                        return change * x * x * ((s + 1) * x - s) + initial;
                    }

                case EasingTypes.SpringOut:
                    {
                        var s = 1.70158;
                        double x = (time / duration);
                        return change * ((x - 1) * (x - 1) * ((s + 1) * (x - 1) + s) + 1) + initial;
                    }
            }
        }
    }
}
