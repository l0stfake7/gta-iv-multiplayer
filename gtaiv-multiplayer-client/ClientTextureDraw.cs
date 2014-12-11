using GTA;
using System;
using System.Drawing;
using System.Net;

namespace MIVClient
{
    public class ClientTextureDraw : DrawBase
    {
        public float width, height;
        private Texture texture;

        public ClientTextureDraw(RectangleF box, byte[] texture)
            : base(box.Location)
        {
            this.width = box.Width;
            this.height = box.Height;
            this.texture = new Texture(texture);
        }

        public ClientTextureDraw(RectangleF box, string file)
            : base(box.Location)
        {
            this.width = box.Width;
            this.height = box.Height;
            this.texture = new Texture(System.IO.File.ReadAllBytes(file));
        }

        public ClientTextureDraw(RectangleF box, Uri url)
            : base(box.Location)
        {
            this.width = box.Width;
            this.height = box.Height;
            try
            {
                var resp = WebRequest.Create(url).GetResponse();
                var stream = resp.GetResponseStream();
                byte[] buffer = new byte[resp.ContentLength];
                stream.Read(buffer, 0, buffer.Length);
                this.texture = new Texture(buffer);
            }
            catch { }
        }

        protected override void render(GTA.Graphics g)
        {
            g.DrawSprite(texture, new RectangleF(position.X, position.Y, width, height));
        }
    }
}