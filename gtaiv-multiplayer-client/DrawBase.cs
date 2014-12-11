using System.Collections.Generic;

namespace MIVClient
{
    public abstract class DrawBase
    {
        private static List<DrawBase> ViewsPool = new List<DrawBase>();
        protected InterpolatorBase currentInterpolatorX;
        protected InterpolatorBase currentInterpolatorY;
        protected System.Drawing.PointF position;


        protected DrawBase(System.Drawing.PointF position)
        {
            this.position = position;
            ViewsPool.Add(this);
        }

        public static void renderAll(GTA.Graphics g)
        {
            foreach (var view in ViewsPool)
            {
                if (view.currentInterpolatorX != null && view.currentInterpolatorX.HasStarted) // those conditonals actually make sense
                {
                    if (view.currentInterpolatorX.HasEnded && !view.currentInterpolatorY.HasStarted)
                    {
                        view.currentInterpolatorX = null;
                    }
                    else
                    {
                        view.position.X = view.currentInterpolatorX.Current;
                    }
                }
                if (view.currentInterpolatorY != null && view.currentInterpolatorY.HasStarted)
                {
                    if (view.currentInterpolatorY.HasEnded && !view.currentInterpolatorY.HasStarted)
                    {
                        view.currentInterpolatorY = null;
                    }
                    else
                    {
                        view.position.Y = view.currentInterpolatorY.Current;
                    }
                } 
                view.render(g);
            }
        }

        public void interpolateX(InterpolatorBase interpolator)
        {
            interpolator.Start();
            this.currentInterpolatorX = interpolator;
        }

        public void interpolateY(InterpolatorBase interpolator)
        {
            interpolator.Start();
            this.currentInterpolatorY = interpolator;
        }

        public void interpolate(InterpolatorBase x, InterpolatorBase y)
        {
            this.currentInterpolatorX = x;
            this.currentInterpolatorY = y;
            x.Start();
            y.Start();
        }

        public void destroy()
        {
            ViewsPool.Remove(this);
        }

        protected abstract void render(GTA.Graphics g);
    }
}