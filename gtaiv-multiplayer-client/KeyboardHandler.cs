using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;
using GTA;
using System.Drawing;
using System.Net;
using System.Net.Sockets;

namespace MIVClient
{
    public class KeyboardHandler
    {
        Client client;
        public bool inKeyboardTypingMode;
        GTA.KeyboardLayoutUS keyboardUS;

        public KeyboardHandler(Client client)
        {
            this.client = client;
            keyboardUS = new KeyboardLayoutUS();
            inKeyboardTypingMode = false;
            client.KeyDown += new GTA.KeyEventHandler(this.eventOnKeyDown);
        }


        private void eventOnKeyDown(object sender, GTA.KeyEventArgs e)
        {

            if (!inKeyboardTypingMode && e.Key == System.Windows.Forms.Keys.T)
            {
                inKeyboardTypingMode = true;
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
                    inKeyboardTypingMode = false;
                    client.getPlayer().CanControlCharacter = true;
                }
                else if (e.Key == System.Windows.Forms.Keys.Escape)
                {
                    client.chatController.currentTypedText = "";
                    inKeyboardTypingMode = false;
                }
                else if (e.Key == System.Windows.Forms.Keys.Back)
                {
                    client.chatController.currentTypedText = client.chatController.currentTypedText.Length > 1 ?
                        client.chatController.currentTypedText.Substring(0, client.chatController.currentTypedText.Length) :
                        client.chatController.currentTypedText;
                }
                else
                {
                    client.chatController.currentTypedText += keyboardUS.ParseKey((int)e.Key, e.Shift, e.Control, e.Alt);
                }
                return;
            }

            foreach (int id in Enumerable.Range((int)System.Windows.Forms.Keys.D0, (int)System.Windows.Forms.Keys.D9))
            {
                if (e.Key == (System.Windows.Forms.Keys)id && e.Alt) client.saveBindPoint(id - (int)System.Windows.Forms.Keys.D0);
                if (e.Key == (System.Windows.Forms.Keys)id && e.Control) client.teleportToBindPoint(id - (int)System.Windows.Forms.Keys.D0);
            }

            if (e.Key == System.Windows.Forms.Keys.Enter)
            {
                //      ChatInputForm.show();
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
                    var peds = World.GetPeds(client.getPlayerPed().Position, 200.0f);
                    foreach (Ped a in peds) if (a.Exists() && a.isAlive && a != client.getPlayerPed()) a.Delete();
                    foreach (Vehicle v in World.GetAllVehicles()) if (v.Exists()) v.Delete();

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
