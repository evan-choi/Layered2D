using System.Threading;

namespace Layered2D.Animations
{
    internal class DispatcherTicker : Ticker<DispatcherTicker>
    {
        SynchronizationContext context;

        public DispatcherTicker()
        {
            context = SynchronizationContext.Current;
        }
        
        protected override void OnElapsed()
        {
            context.Send(new SendOrPostCallback(o =>
            {
                base.OnElapsed();
            }), null);
        }
    }
}