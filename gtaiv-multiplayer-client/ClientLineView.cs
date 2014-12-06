using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;

namespace MIVClient
{
    public class ClientLineView
    {
        public PointF start, end;
        float width;
        public Color color;

        public static List<ClientLineView> LineViewsPool;

        public ClientLineView(PointF start, PointF end, float width, Color color)
        {
            if (LineViewsPool == null) LineViewsPool = new List<ClientLineView>();
            this.start = start;
            this.end = end;
            this.width = width;
            this.color = color;
            LineViewsPool.Add(this);
        }

        public void destroy()
        {
            if (LineViewsPool == null) LineViewsPool = new List<ClientLineView>();
            LineViewsPool.Remove(this);
        }

        public static void renderAll(GTA.Graphics g)
        {
            if (LineViewsPool == null) LineViewsPool = new List<ClientLineView>();
            foreach (ClientLineView view in LineViewsPool)
            {
                g.DrawLine(view.start, view.end, view.width/2.0f, view.color);
                g.DrawLine(view.start, view.end, view.width / 1.4f, System.Drawing.Color.FromArgb(195, view.color));
                g.DrawLine(view.start, view.end, view.width, System.Drawing.Color.FromArgb(80, view.color));
            }
        }
    }
}
