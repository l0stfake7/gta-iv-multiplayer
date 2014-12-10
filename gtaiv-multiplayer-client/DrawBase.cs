using System.Collections.Generic;

namespace MIVClient
{
    public abstract class DrawBase
    {
        private static List<DrawBase> ViewsPool = new List<DrawBase>();

        protected DrawBase()
        {
            ViewsPool.Add(this);
        }

        public static void renderAll(GTA.Graphics g)
        {
            foreach (var view in ViewsPool)
            {
                view.render(g);
            }
        }

        public void destroy()
        {
            ViewsPool.Remove(this);
        }

        protected abstract void render(GTA.Graphics g);
    }
}