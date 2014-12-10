using GTA;
using System.Drawing;

namespace MIVClient
{
    public class ClientTextView : DrawBase
    {
        public TextAlignment alignment;

        public Color color;

        public GTA.Font font;

        public Point point;

        public string text;

        public RectangleF textbox;

        public DrawType type;

        public ClientTextView(RectangleF textbox, TextAlignment alignment, string text, GTA.Font font, Color color)
            : base()
        {
            this.textbox = textbox;
            this.alignment = alignment;
            this.text = text;
            this.font = font;
            this.color = color;
            type = DrawType.Rectangle;
        }

        public ClientTextView(Point point, string text, GTA.Font font)
            : base()
        {
            this.point = point;
            this.text = text;
            this.font = font;
            type = DrawType.Point;
        }

        public enum DrawType
        {
            Rectangle,
            Point
        }

        protected override void render(GTA.Graphics g)
        {
            if (type == DrawType.Rectangle)
            {
                g.DrawText(text, textbox, alignment, color, font);
            }
            else
            {
                g.DrawText(text, point.X, point.Y, font);
            }
        }
    }
}