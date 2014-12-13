// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
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