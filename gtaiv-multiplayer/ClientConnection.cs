using MIVSDK;
using System;
using System.Net.Sockets;
using System.Data;
using System.Collections.Generic;

namespace MIVServer
{
    public class ClientConnection
    {
        public ServerPlayer player;

        private const int BUFSIZE = 1024 * 100;

        //NetworkStream stream;
        private byte[] buffer;

        private TcpClient connection;

        public ClientConnection(TcpClient client)
        {
            internal_buffer = new List<byte>();
            connection = client;
            client.SendBufferSize = BUFSIZE;
            client.Client.ReceiveBufferSize = BUFSIZE;
            client.Client.Blocking = true;
            client.NoDelay = true;
            client.Client.DontFragment = true;
            client.Client.NoDelay = true;
            client.Client.Blocking = true;
            client.Client.ReceiveBufferSize = BUFSIZE;
            client.Client.SendBufferSize = BUFSIZE;
            buffer = new byte[BUFSIZE];
        }

        public delegate void onConnectDelegate(string nick);

        public event onConnectDelegate onConnect;

        public void startReceiving()
        {
            connection.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
        }

        private List<byte> internal_buffer;
        public void write(byte[] bytes)
        {
            lock (connection)
            {
                internal_buffer.AddRange(bytes);
            }
        }
        public void flush()
        {
            lock (connection)
            {
                var stream = connection.GetStream();
                stream.Write(internal_buffer.ToArray(), 0, internal_buffer.Count);
                stream.Flush();
                internal_buffer = new List<byte>();
            }
        }

        private void onReceive(IAsyncResult iar)
        {
            lock (connection)
            {
                try
                {
                    int count = connection.Client.EndReceive(iar);
                }
                catch
                {
                    Console.WriteLine("Client disconnected");
                    return;
                }


                var bpr = new BinaryPacketReader(buffer);
                while (bpr.canRead())
                {
                    Commands command = bpr.readCommand();
                    if (command == Commands.Invalid) break;
                    switch (command)
                    {
                        case Commands.Disconnect:
                            {
                                if (player != null)
                                {
                                    Server.instance.api.invokeOnPlayerDisconnect(player);
                                }
                            }
                            break;

                        case Commands.Connect:
                            {
                                string nick = bpr.readString();
                                if (onConnect != null) onConnect.Invoke(nick);
                            }
                            break;

                        case Commands.Chat_sendMessage:
                            {
                                string text = bpr.readString();
                                Server.instance.api.invokeOnPlayerSendText(player, text);
                            }
                            break;

                        case Commands.Keys_down:
                            {
                                if (player != null)
                                {
                                    int key = bpr.readInt32();
                                    Server.instance.api.invokeOnPlayerKeyDown(player, (System.Windows.Forms.Keys)key);
                                }
                            }
                            break;

                        case Commands.Keys_up:
                            {
                                if (player != null)
                                {
                                    int key = bpr.readInt32();
                                    Server.instance.api.invokeOnPlayerKeyUp(player, (System.Windows.Forms.Keys)key);
                                }
                            }
                            break;

                        case Commands.NPCDialog_sendResponse:
                            {
                                if (player != null)
                                {
                                    uint key = bpr.readUInt32();
                                    byte answer = buffer[6];
                                    ServerNPCDialog.invokeResponse(player, key, answer);
                                }
                            }
                            break;

                        case Commands.UpdateData:
                            {
                                if (player != null)
                                {
                                    MIVSDK.UpdateDataStruct data = bpr.readUpdateStruct();
                                    player.updateData(data);
                                }
                            }
                            break;
                    }
                }

                connection.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
            }
        }
    }
}