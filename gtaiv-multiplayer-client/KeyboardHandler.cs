using GTA;
using MIVSDK;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MIVClient
{
    public class KeyboardHandler
    {
        private Client client;
        public bool inKeyboardTypingMode;
        private GTA.KeyboardLayoutUS keyboardUS;

        public KeyboardHandler(Client client)
        {
            this.client = client;
            keyboardUS = new KeyboardLayoutUS();
            inKeyboardTypingMode = false;
            client.KeyDown += new GTA.KeyEventHandler(this.eventOnKeyDown);
        }

        private float gamescale;
        public int cursorpos = 0;

        private void eventOnKeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (!inKeyboardTypingMode && e.Key == System.Windows.Forms.Keys.T)
            {
                inKeyboardTypingMode = true;
                cursorpos = 0;
                client.getPlayer().CanControlCharacter = false;
            }
            else if (inKeyboardTypingMode)
            {
                if (e.Key == System.Windows.Forms.Keys.Enter)
                {
                    if (client.chatController.currentTypedText.Length > 0)
                    {
                        //writeChat(currentTypedText);
                        client.serverConnection.streamWrite(Commands.Chat_sendMessage);
                        client.serverConnection.streamWrite(client.chatController.currentTypedText.Length);
                        client.serverConnection.streamWrite(client.chatController.currentTypedText);
                        client.serverConnection.streamFlush();
                    }
                    client.chatController.currentTypedText = "";
                    cursorpos = 0;
                    inKeyboardTypingMode = false;
                    client.getPlayer().CanControlCharacter = true;
                }
                else if (e.Key == System.Windows.Forms.Keys.Escape)
                {
                    client.chatController.currentTypedText = "";
                    cursorpos = 0;
                    inKeyboardTypingMode = false;
                }
                else if (e.Key == System.Windows.Forms.Keys.Left)
                {
                    cursorpos = cursorpos > 0 ? cursorpos - 1 : cursorpos;
                }
                else if (e.Key == System.Windows.Forms.Keys.Right)
                {
                    cursorpos = cursorpos >= client.chatController.currentTypedText.Length ? client.chatController.currentTypedText.Length : cursorpos + 1;
                }
                else if (e.Key == System.Windows.Forms.Keys.Back)
                {
                    string leftcut = cursorpos > 0 ? client.chatController.currentTypedText.Substring(0, cursorpos - 1) : client.chatController.currentTypedText;
                    string rightcut = client.chatController.currentTypedText.Substring(cursorpos, client.chatController.currentTypedText.Length - cursorpos);
                    client.chatController.currentTypedText = leftcut + rightcut;
                    cursorpos = cursorpos > 0 ? cursorpos - 1 : cursorpos;
                }
                else
                {
                    string leftcut = client.chatController.currentTypedText.Substring(0, cursorpos);

                    string rightcut =
                        cursorpos >= client.chatController.currentTypedText.Length ?
                        "" :
                        client.chatController.currentTypedText.Substring(cursorpos, client.chatController.currentTypedText.Length - cursorpos);
                    string newstr = keyboardUS.ParseKey((int)e.Key, e.Shift, e.Control, e.Alt);
                    client.chatController.currentTypedText = leftcut + newstr + rightcut;
                    cursorpos += newstr.Length;
                }
                return;
            }

            foreach (int id in Enumerable.Range((int)System.Windows.Forms.Keys.D0, (int)System.Windows.Forms.Keys.D9))
            {
                if (e.Key == (System.Windows.Forms.Keys)id && e.Alt) client.saveBindPoint(id - (int)System.Windows.Forms.Keys.D0);
                if (e.Key == (System.Windows.Forms.Keys)id && e.Control) client.teleportToBindPoint(id - (int)System.Windows.Forms.Keys.D0);
            }

            if (gamescale == null) gamescale = 1.0f;
            if (e.Key == System.Windows.Forms.Keys.Add)
            {
                gamescale *= 1.3f;
                Game.TimeScale = gamescale;
            }
            if (e.Key == System.Windows.Forms.Keys.Subtract)
            {
                gamescale *= 0.7f;
                Game.TimeScale = gamescale;
            }
            if (e.Key == System.Windows.Forms.Keys.Multiply)
            {
                gamescale = 1.0f;
                Game.TimeScale = gamescale;
            }

            if (e.Key == System.Windows.Forms.Keys.Insert)
            {
                if (client.getPlayerPed().isInVehicle())
                {
                    client.getPlayerPed().CurrentVehicle.Velocity *= 2.0f;
                }
                else
                {
                    client.getPlayerPed().Velocity *= 2.0f;
                }
            }

            if (e.Key == System.Windows.Forms.Keys.L)
            {
                try
                {
                    client.currentState = ClientState.Connecting;
                    if (client.client != null && client.client.Connected)
                    {
                        client.client.Close();
                    }
                    client.client = new TcpClient();
                    INIReader ini = new INIReader(System.IO.File.ReadAllLines("server.ini"));
                    IPAddress address = IPAddress.Parse(ini.getString("ip"));
                    client.nick = ini.getString("nick");
                    int port = ini.getInt("port");

                    client.client.Connect(address, port);

                    Client.currentData = UpdateDataStruct.Zero;

                    client.serverConnection = new ServerConnection(client);

                    World.CurrentDayTime = new TimeSpan(12, 00, 00);
                    World.PedDensity = 0;
                    World.CarDensity = 0;
                }
                catch (Exception ex)
                {
                    client.currentState = ClientState.Disconnected;
                    if (client.client != null && client.client.Connected)
                    {
                        client.client.Close();
                    }
                    throw ex;
                }
            }
        }
    }
}