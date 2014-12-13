// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIVClient
{
    public abstract class InterpolatorBase
    {
        protected float start, end;
        protected int duration;
        private DateTime startTime;
        private bool started;
        private float scale;
        public delegate void InterpolationFinishedDelegate();
        public event InterpolationFinishedDelegate InterpolationFinished;
        private bool invert;
        public InterpolatorBase(float start, float end, int duration)
        {
            if (start > end)
            {
                invert = true;
                this.end = start;
                this.start = end;
            }
            else
            {
                this.start = start;
                this.end = end;
                invert = false;
            }

            this.duration = duration;
            scale = 1.0f;
        }

        protected abstract float GetCurrentValue(float step);

        public float Current
        {
            get
            {
                if (!started) return invert ? end : start;
                var duration = DateTime.Now - startTime;
                float result = GetCurrentValue(((float)duration.TotalMilliseconds / (float)this.duration) * scale);
                if (result < start) return invert ? end : start;
                if (result > end)
                {
                    started = false;
                    if (InterpolationFinished != null) InterpolationFinished.Invoke();
                    return invert ? start : end;
                }
                return invert ? end - (result - start) : result;
            }
        }
        public bool HasEnded
        {
            get
            {
                if (!started) return true;
                var duration = DateTime.Now - startTime;
                float step = ((float)duration.TotalMilliseconds / (float)this.duration) * scale;
                float result = GetCurrentValue(step);
                if (result < start) return false;
                if (step >= 1.0f)
                {
                    started = false;
                    if (InterpolationFinished != null) InterpolationFinished.Invoke();
                    return true;
                }
                return false;
            }
        }

        public bool HasStarted
        {
            get
            {
                return started;
            }
        }

        public void OnInterpolationFinished(InterpolationFinishedDelegate action)
        {
            InterpolationFinished += action;
        }

        public void Start()
        {
            startTime = DateTime.Now;
            started = true;
        }
        public void Stop()
        {
            started = false;
        }
        public void Rewind()
        {
            started = false;
        }
        public void SetSpeedScale(float scale)
        {
            this.scale = scale;
        }
    }

    public class LinearInterpolator : InterpolatorBase
    {
        public LinearInterpolator(float start, float end, int duration) : base(start, end, duration) { }

        protected override float GetCurrentValue(float step)
        {
            return ((end - start) * step) + start;
        }
    }
    public class EaseInOutInterpolator : InterpolatorBase
    {
        public EaseInOutInterpolator(float start, float end, int duration) : base(start, end, duration) { }

        private float transform(float step)
        {
            if (step < 0.5f)
            {
                return 2.0f * (step * step);
            }
            else
            {
                step -= 0.5f;
                return 2.0f * step * (1.0f - step) + 0.5f;
            }
        }

        protected override float GetCurrentValue(float step)
        {
            return ((end - start) * transform(step)) + start;
        }
    }
}
