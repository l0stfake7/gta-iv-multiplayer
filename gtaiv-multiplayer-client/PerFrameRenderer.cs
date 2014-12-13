// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using GTA;
using System;
using System.Text;

namespace MIVClient
{
    public sealed class PerFrameRenderer : IDisposable
    {
        public static float test_x, test_y, test_z;
        private Client client;
        private GTA.Font font;
        private GTA.Font font_consolas;
        private GTA.Font font_small;

        public PerFrameRenderer(Client client)
        {
            this.client = client;
            font = new GTA.Font("Segoe UI", widthToPx(20.0f), FontScaling.Pixel);
            font_small = new GTA.Font("Segoe UI", widthToPx(10.0f), FontScaling.Pixel);
            font_consolas = new GTA.Font("Consolas", widthToPx(20.0f), FontScaling.Pixel);
            client.PerFrameDrawing += Client_PerFrameDrawing;
        }

        public void Dispose()
        {
            font.Dispose();
            GC.SuppressFinalize(this);
        }

        private void Client_PerFrameDrawing(object sender, GraphicsEventArgs e)
        {
            e.Graphics.Scaling = FontScaling.Pixel;
            float yoffset = heightToPx(15);
            for (int i = 0; i < client.chatController.chatconsole.Count; i++)
            {
                e.Graphics.DrawText(client.chatController.chatconsole.ToArray()[i], widthToPx(10.0f), yoffset, font);
                yoffset += heightToPx(24);
            }
            /*
            yoffset = 100;
            for (int i = 0; i < client.chatController.debugconsole.Count; i++)
            {
                e.Graphics.DrawText(client.chatController.debugconsole.ToArray()[i], new RectangleF(0, yoffset, Game.Resolution.Width, 40), TextAlignment.Right, font);
                yoffset += 18;
            }*/

            if (client.keyboardHandler.inKeyboardTypingMode)
            {
                int cpos = client.keyboardHandler.cursorpos;
                string prefix = client.nick + ": ";
                string currenttext = prefix + client.chatController.currentTypedText;

                e.Graphics.DrawText(currenttext, widthToPx(10.0f), yoffset, font_consolas);
                if (DateTime.Now.Millisecond < 500)
                {
                    StringBuilder cstr = new StringBuilder();
                    cstr.Append(' ', cpos + prefix.Length);
                    cstr.Append('|');
                    e.Graphics.DrawText(cstr.ToString(), widthToPx(10.0f), yoffset, font_consolas);
                }
            }
            DrawBase.renderAll(e.Graphics);
        }

        private float heightToPx(float normalized)
        {
            return Game.Resolution.Height * (normalized / 900.0f);
        }

        private float widthToPx(float normalized)
        {
            return Game.Resolution.Width * (normalized / 1600.0f);
        }
    }
}