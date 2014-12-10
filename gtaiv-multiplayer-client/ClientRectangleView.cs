using System.Drawing;

namespace MIVClient
{
    public class ClientRectangleView : DrawBase
    {
        public RectangleF box;
        public Color color;

        public ClientRectangleView(RectangleF box, Color color)
            : base()
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