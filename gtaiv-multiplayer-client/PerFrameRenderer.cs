using GTA;
using System;
using System.Drawing;
using System.Text;

namespace MIVClient
{
    public class PerFrameRenderer
    {
        private Client client;
        private GTA.Font font;
        private RectangleF chatPosition;
        private Color chatBackground;

        public PerFrameRenderer(Client client)
        {
            this.client = client;
            chatPosition = new RectangleF(10, 10, 400, 230);
            chatBackground = Color.FromArgb(70, 0, 0, 0);
            font = new GTA.Font("Consolas", 24, FontScaling.Pixel);
            client.PerFrameDrawing += Client_PerFrameDrawing;
        }

        private void Client_PerFrameDrawing(object sender, GraphicsEventArgs e)
        {
            e.Graphics.Scaling = FontScaling.Pixel;
            e.Graphics.DrawRectangle(chatPosition, chatBackground);
            int yoffset = 15;
            for (int i = 0; i < client.chatController.chatconsole.Count; i++)
            {
                e.Graphics.DrawText(client.chatController.chatconsole.ToArray()[i], 15, yoffset, font);
                yoffset += 30;
            }

            if (Client.currentData != null)
            {
                StringBuilder debugtext = new StringBuilder();
                debugtext.Append("Debug MIV: Nick:");
                debugtext.Append(Client.currentData.nick);
                debugtext.Append(", IsAiming: ");
                debugtext.Append((Client.currentData.state & MIVSDK.PlayerState.IsAiming) != 0 ? "1" : "0");
                debugtext.Append(", IsCrouching: ");
                debugtext.Append((Client.currentData.state & MIVSDK.PlayerState.IsCrouching) != 0 ? "1" : "0");
                debugtext.Append(", IsJumping: ");
                debugtext.Append((Client.currentData.state & MIVSDK.PlayerState.IsJumping) != 0 ? "1" : "0");
                debugtext.Append(", IsShooting: ");
                debugtext.Append((Client.currentData.state & MIVSDK.PlayerState.IsShooting) != 0 ? "1" : "0");
                e.Graphics.DrawText(debugtext.ToString(), 15, Game.Resolution.Height - 100, font);
            }


            if (client.keyboardHandler.inKeyboardTypingMode)
            {
                int cpos = client.keyboardHandler.cursorpos;
                string prefix = client.nick + ": ";
                string currenttext = prefix + client.chatController.currentTypedText;

                e.Graphics.DrawText(currenttext, 25, 15 + 30 * 8 + 20, font);
                if (DateTime.Now.Millisecond < 500)
                {
                    StringBuilder cstr = new StringBuilder();
                    cstr.Append(' ', cpos + prefix.Length);
                    cstr.Append('|');
                    e.Graphics.DrawText(cstr.ToString(), 25, 15 + 30 * 8 + 20, font);
                }
            }
        }
    }
}