using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;

namespace MIVClient
{
    public class ClientRectangleView
    {
        public RectangleF box;
        public Color color;

        public static List<ClientRectangleView> RectangleViewsPool;

        public ClientRectangleView(RectangleF box, Color color)
        {
            if (RectangleViewsPool == null) RectangleViewsPool = new List<ClientRectangleView>();
            this.box = box;
            this.color = color;
            RectangleViewsPool.Add(this);
        }

        public void destroy()
        {
            if (RectangleViewsPool == null) RectangleViewsPool = new List<ClientRectangleView>();
            RectangleViewsPool.Remove(this);
        }

        public static void renderAll(GTA.Graphics g)
        {
            if (RectangleViewsPool == null) RectangleViewsPool = new List<ClientRectangleView>();
            foreach (ClientRectangleView view in RectangleViewsPool)
            {
                g.DrawRectangle(view.box, view.color);
            }
        }
    }
}
