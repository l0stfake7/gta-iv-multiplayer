using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;

namespace MIVClient
{
    public class ClientTextView
    {
        public enum DrawType{
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

        public static List<ClientTextView> TextViewsPool;

        public ClientTextView(RectangleF textbox, TextAlignment alignment, string text, GTA.Font font, Color color)
        {
            if (TextViewsPool == null) TextViewsPool = new List<ClientTextView>();
            this.textbox = textbox;
            this.alignment = alignment;
            this.text = text;
            this.font = font;
            this.color = color;
            type = DrawType.Rectangle;
            TextViewsPool.Add(this);
        }
        public ClientTextView(Point point, string text, GTA.Font font)
        {
            if (TextViewsPool == null) TextViewsPool = new List<ClientTextView>();
            this.point = point;
            this.text = text;
            this.font = font;
            type = DrawType.Point;
            TextViewsPool.Add(this);
        }

        public void destroy()
        {
            if (TextViewsPool == null) TextViewsPool = new List<ClientTextView>();
            TextViewsPool.Remove(this);
        }

        public static void renderAll(GTA.Graphics g)
        {
            if (TextViewsPool == null) TextViewsPool = new List<ClientTextView>();
            foreach (ClientTextView view in TextViewsPool)
            {
                if (view.type == DrawType.Rectangle)
                {
                    g.DrawText(view.text, view.textbox, view.alignment, view.color, view.font);
                }
                else
                {
                    g.DrawText(view.text, view.point.X, view.point.Y, view.font);
                }
            }
        }
    }
}
