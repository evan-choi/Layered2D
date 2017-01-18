using System;
using Layered2D.Collections;

namespace Layered2D.Components.Base
{
    public interface IScene
    {
        ILazyList<IScene> RenderScenes { get; }

        void BeginRender(DateTime time);
        void EndRender();
        void Resized();
    }
}