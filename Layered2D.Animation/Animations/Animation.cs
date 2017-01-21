using System;

namespace Layered2D.Animations
{
    public abstract class Animation<T> : IAnimatable, IDisposable
    {
        public event EventHandler<T> ValueChanged;
        public event EventHandler Finish;

        public EasingTypes Easing { get; set; } = EasingTypes.None;

        public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(500);
        public T From { get; set; }
        public T To { get; set; }

        public TimeSpan BeginTime { get; set; } = TimeSpan.Zero;

        public bool IsBusy { get; private set; } = false;

        DateTime startTime;
        Func<double, T> easingFunc;
        ITicker ticker;
        
        public Animation()
        {
            Ticker.Init();
            DispatcherTicker.Init();
        }

        public Animation(Func<double, T> easingFunc) : this()
        {
            this.easingFunc = easingFunc;
        }
       
        ~Animation()
        {
            Dispose();
        }

        public virtual void Start(bool dispatch = false)
        {
            if (!IsBusy)
            {
                if (dispatch)
                {
                    ticker = DispatcherTicker.Default;
                }
                else
                {
                    ticker = Ticker.Default;
                }

                startTime = DateTime.Now + BeginTime;
                ticker.Add(this);

                IsBusy = true;
            }
        }

        public virtual void Stop()
        {
            if (IsBusy)
            {
                ticker?.Remove(this);
                IsBusy = false;

                OnFinish();
            }
        }

        public void Apply()
        {
            if (IsBusy)
            {
                var duration = DateTime.Now - startTime;

                if (duration < TimeSpan.Zero)
                    return;

                ValueChanged?.Invoke(this, 
                    easingFunc == null ? OnApply(duration) : ApplyFromFunc(duration));

                if (duration >= this.Duration)
                {
                    Stop();
                }
            }
        }

        internal abstract T OnApply(TimeSpan duration);

        private T ApplyFromFunc(TimeSpan duration)
        {
            double x = duration.TotalMilliseconds / this.Duration.TotalMilliseconds;

            return easingFunc(x);
        }

        protected virtual void OnFinish()
        {
            Finish?.Invoke(this, null);
            GC.Collect();
        }

        public void Dispose()
        {
            if (IsBusy)
                Stop();
            
            ticker = null;

            GC.SuppressFinalize(this);
        }
    }
}
