using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;

namespace MIVClient
{
    public class ClientLineView : DrawBase
    {
        public PointF start, end;
        float width;
        public Color color;

        public ClientLineView(PointF start, PointF end, float width, Color color)
            : base()
        {
            this.start = start;
            this.end = end;
            this.width = width;
            this.color = color;
        }

        protected override void render(GTA.Graphics g)
        {
            g.DrawLine(start, end, width, color);
        }
    }
}
