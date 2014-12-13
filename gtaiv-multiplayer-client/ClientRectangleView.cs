// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System.Drawing;

namespace MIVClient
{
    public class ClientRectangleView : DrawBase
    {
        public float width, height;
        public Color color;

        public ClientRectangleView(RectangleF box, Color color)
            : base(box.Location)
        {
            this.width = box.Width;
            this.height = box.Height;
            this.color = color;
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

        protected override void render(GTA.Graphics g)
        {
            g.DrawRectangle(Box, color);
        }
    }
}