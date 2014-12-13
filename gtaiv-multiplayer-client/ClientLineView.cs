// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
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