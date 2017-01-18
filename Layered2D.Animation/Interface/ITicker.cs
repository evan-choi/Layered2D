using System;

namespace Layered2D.Animations
{
    internal interface ITicker : IDisposable
    {
        void Add(IAnimatable animatable);
        void Remove(IAnimatable animatable);
    }
}
