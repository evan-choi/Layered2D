using Layered2D.Collections;
using System;
using System.Collections.Generic;
using System.Timers;
using thr = System.Threading;

namespace Layered2D.Animations
{
    internal class Ticker : Ticker<Ticker>
    {
    }

    internal class Ticker<T> : ITicker where T : ITicker
    {
        public static T Default;

        bool isDisposed = false;

        Timer timer;
        LazyList<IAnimatable> animatables;

        thr.AutoResetEvent are;

        public Ticker()
        {
            are = new thr.AutoResetEvent(true);
            animatables = new LazyList<IAnimatable>();

            timer = new Timer()
            {
                Interval = 15
            };
            
            timer.Elapsed += Timer_Elapsed;
        }

        public static void Init()
        {
            if (Default == null)
            {
                Default = Activator.CreateInstance<T>();
            }
        }

        public void Add(IAnimatable animatable)
        {
            animatables.LazyAdd(animatable);
            timer.Start();
        }

        public void Remove(IAnimatable animatable)
        {
            animatables.LazyRemove(animatable);

            if (animatables.CountWithLazy == 0)
                timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            are?.WaitOne();
            OnElapsed();
            are?.Set();
        }

        protected virtual void OnElapsed()
        {
            if (isDisposed)
                return;

            animatables.Apply();
            foreach (IAnimatable a in animatables)
            {
                a.Apply();
            }
        }

        public void Dispose()
        {
            isDisposed = true;

            timer?.Close();
            timer?.Stop();

            timer?.Dispose();
            timer = null;
            
            animatables?.Dispose();
            animatables = null;

            are?.Dispose();
            are = null;
        }
    }
}
