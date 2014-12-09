using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;

namespace MIVClient
{
    public class ClientRectangleView : DrawBase
    {
        public RectangleF box;
        public Color color;

        public ClientRectangleView(RectangleF box, Color color) : base()
        {
            this.box = box;
            this.color = color;
        }

        protected override void render(GTA.Graphics g)
        {
            g.DrawRectangle(box, color);
        }
    }
}
