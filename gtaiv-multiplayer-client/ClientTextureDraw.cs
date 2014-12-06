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
    public class ClientTextureDraw
    {

        public RectangleF box;
        Texture texture;

        public static List<ClientTextureDraw> TextureViewsPool;

        public ClientTextureDraw(RectangleF box, byte[] texture)
        {
            if (TextureViewsPool == null) TextureViewsPool = new List<ClientTextureDraw>();
            this.box = box;
            this.texture = new Texture(texture);
            TextureViewsPool.Add(this);
        }
        public ClientTextureDraw(RectangleF box, string file)
        {
            if (TextureViewsPool == null) TextureViewsPool = new List<ClientTextureDraw>();
            this.box = box;
            this.texture = new Texture(System.IO.File.ReadAllBytes(file));
            TextureViewsPool.Add(this);
        }
        public ClientTextureDraw(RectangleF box, Uri url)
        {
            if (TextureViewsPool == null) TextureViewsPool = new List<ClientTextureDraw>();
            this.box = box;
            try
            {
                var resp = WebRequest.Create(url).GetResponse();
                var stream = resp.GetResponseStream();
                byte[] buffer = new byte[resp.ContentLength];
                stream.Read(buffer, 0, buffer.Length);
                this.texture = new Texture(buffer);
                TextureViewsPool.Add(this);
            }
            catch { }
        }
        public void destroy()
        {
            if (TextureViewsPool == null) TextureViewsPool = new List<ClientTextureDraw>();
            else TextureViewsPool.Remove(this);
        }

        public static void renderAll(GTA.Graphics g)
        {
            if (TextureViewsPool == null) TextureViewsPool = new List<ClientTextureDraw>();
            foreach (ClientTextureDraw view in TextureViewsPool)
            {
                g.DrawSprite(view.texture, view.box);
            }
        }
    }
}
