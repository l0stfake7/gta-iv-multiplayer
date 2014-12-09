using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;

namespace MIVClient
{
    public class ClientTextView : DrawBase
    {
        public enum DrawType
        {
            Rectangle,
            Point
        }
        public DrawType type;
        public RectangleF textbox;
        public Point point;
        public TextAlignment alignment;
        public string text;
        public GTA.Font font;
        public Color color;

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
