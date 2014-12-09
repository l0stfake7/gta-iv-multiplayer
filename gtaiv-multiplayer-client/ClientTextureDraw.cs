using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using System.Web;
using GTA;

namespace MIVClient
{
    public class ClientTextureDraw : DrawBase
    {

        public RectangleF box;
        Texture texture;

        public ClientTextureDraw(RectangleF box, byte[] texture)
            : base()
        {
            this.box = box;
            this.texture = new Texture(texture);
        }
        public ClientTextureDraw(RectangleF box, string file)
            : base()
        {
            this.box = box;
            this.texture = new Texture(System.IO.File.ReadAllBytes(file));
        }
        public ClientTextureDraw(RectangleF box, Uri url)
            : base()
        {
            this.box = box;
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
            g.DrawSprite(texture, box);
        }
    }
}
