using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;
using GTA;
using System.Drawing;

namespace MIVClient
{
    public class PerFrameRenderer
    {
        Client client;
        GTA.Font font;
        RectangleF chatPosition;
        Color chatBackground;
        public PerFrameRenderer(Client client)
        {
            this.client = client;
            chatPosition = new RectangleF(10, 10, 400, 200);
            chatBackground = Color.FromArgb(70, 0, 0, 0);
            font = new GTA.Font("Segoe UI", 24, FontScaling.Pixel);
            client.PerFrameDrawing += Client_PerFrameDrawing;
        }

        void Client_PerFrameDrawing(object sender, GraphicsEventArgs e)
        {
            e.Graphics.Scaling = FontScaling.Pixel;
            e.Graphics.DrawRectangle(chatPosition, chatBackground);
            int yoffset = 15;
            foreach (string text in client.chatController.chatconsole)
            {
                e.Graphics.DrawText(text, 15, yoffset, font);
                yoffset += 30;
            }
            if (client.keyboardHandler.inKeyboardTypingMode)
            {
                string currenttext = client.nick + ": " + client.chatController.currentTypedText;
                if (DateTime.Now.Millisecond < 500) currenttext += "|";
                e.Graphics.DrawText(currenttext, 25, 15 + 30 * 8 + 20, font);
            }

        }
    }
}
