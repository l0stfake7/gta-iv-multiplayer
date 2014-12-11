using System.Drawing;

namespace MIVClient
{
    public class ClientLineView : DrawBase
    {
        public Color color;
        public PointF start, end;
        private float width;

        public ClientLineView(PointF start, PointF end, float width, Color color)
            : base(start)
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