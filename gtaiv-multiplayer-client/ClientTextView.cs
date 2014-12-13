// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using GTA;
using System.Drawing;

namespace MIVClient
{
    public class ClientTextView : DrawBase
    {
        public TextAlignment alignment;

        public Color color;

        public GTA.Font font;

        public string text;

        public float width, height;

        public DrawType type;

        public ClientTextView(RectangleF textbox, TextAlignment alignment, string text, GTA.Font font, Color color)
            : base(textbox.Location)
        {
            this.width = textbox.Width;
            this.height = textbox.Height;
            this.alignment = alignment;
            this.text = text;
            this.font = font;
            this.color = color;
            type = DrawType.Rectangle;
        }

        public ClientTextView(PointF point, string text, GTA.Font font, Color color)
            : base(point)
        {
            this.text = text;
            this.font = font;
            this.color = color;
            type = DrawType.Point;
        }

        public RectangleF Box
        {
            get
            {
                return new RectangleF(position.X, position.Y, width, height);
            }
            set
            {
                position = value.Location;
                width = value.Width;
                height = value.Height;
            }
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
                g.DrawText(text, new RectangleF(position.X, position.Y, width, height), alignment, color, font);
            }
            else
            {
                g.DrawText(text, position.X, position.Y, color, font);
            }
        }
    }
}