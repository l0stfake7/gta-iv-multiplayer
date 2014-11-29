using MIVSDK.Math;
using MIVServer;
using System;
using System.Timers;

namespace MIVServer
{
    public abstract class Gamemode
    {
        protected ServerApi api;

        public Gamemode(ServerApi api)
        {
            this.api = api;
        }

        public delegate void ActionDelegate();

        protected Timer after(int timeout, ActionDelegate action)
        {
            var timer = new Timer();
            timer.Elapsed += delegate
            {
                action.Invoke();
                timer.Stop();
            };
            timer.Interval = timeout;
            timer.Start();
            return timer;
        }

        protected Timer setTimer(int interval, ActionDelegate action)
        {
            var timer = new Timer();
            timer.Elapsed += delegate
            {
                action.Invoke();
            };
            timer.Interval = interval;
            timer.Start();
            return timer;
        }

    }
}