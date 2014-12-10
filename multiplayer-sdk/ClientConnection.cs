using MIVSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MIVServer
{
    public class ClientConnection
    {
        public ServerPlayer player;

        private const int BUFSIZE = 1024 * 100;

        //NetworkStream stream;
        private byte[] buffer;

        private TcpClient connection;

        private List<byte> internal_buffer;

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

        public void flush()
        {
            lock (connection)
            {
                try
                {
                    var stream = connection.GetStream();
                    internal_buffer.InsertRange(0, BitConverter.GetBytes((int)(internal_buffer.Count + 4)));
                    stream.Write(internal_buffer.ToArray(), 0, internal_buffer.Count);
                    stream.Flush();
                    internal_buffer = new List<byte>();
                }
                catch
                {
                    if (player != null)
                    {
                        Server.instance.api.invokeOnPlayerDisconnect(player);

                        player.data = null;
                        Server.instance.playerpool.Remove(player);
                        player = null;
                    }
                }
            }
        }

        public void startReceiving()
        {
            connection.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
        }

        public void write(byte[] bytes)
        {
            lock (connection)
            {
                internal_buffer.AddRange(bytes);
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

                        case Commands.InternalClient_requestSpawn:
                            {
                                if (player != null)
                                {
                                    Server.instance.api.invokeOnPlayerDie(player);
                                    Server.instance.api.invokeOnPlayerSpawn(player);
                                    var bpf = new BinaryPacketFormatter(Commands.InternalClient_finishSpawn);
                                    player.connection.write(bpf.getBytes());
                                }
                            }
                            break;

                        case Commands.Request_getSelectedPlayer:
                            {
                                if (player != null)
                                {
                                    uint requestid = bpr.readUInt32();
                                    uint playerid = bpr.readUInt32();
                                    Request.dispatch(requestid, playerid);
                                }
                            }
                            break;

                        case Commands.Request_getCameraPosition:
                            {
                                if (player != null)
                                {
                                    uint requestid = bpr.readUInt32();
                                    var vect = bpr.readVector3();
                                    Request.dispatch(requestid, vect);
                                }
                            }
                            break;

                        case Commands.Request_isObjectVisible:
                            {
                                if (player != null)
                                {
                                    uint requestid = bpr.readUInt32();
                                    var vect = bpr.readByte() == 1;
                                    Request.dispatch(requestid, vect);
                                }
                            }
                            break;

                        case Commands.Request_worldToScreen:
                            {
                                if (player != null)
                                {
                                    uint requestid = bpr.readUInt32();
                                    var x = bpr.readSingle();
                                    var y = bpr.readSingle();
                                    Request.dispatch(requestid, new SharpDX.Vector2(x, y));
                                }
                            }
                            break;

                        case Commands.Chat_sendMessage:
                            {
                                string text = bpr.readString();
                                if (text.StartsWith("/"))
                                {
                                    List<string> split = text.Split(' ').ToList();
                                    Server.instance.api.invokeOnPlayerSendCommand(player, split.First().Substring(1), split.Skip(1).ToArray());
                                }
                                else
                                {
                                    Server.instance.api.invokeOnPlayerSendText(player, text);
                                }
                            }
                            break;

                        case Commands.Player_damage:
                            {
                                if (player != null)
                                {
                                    uint playerid = bpr.readUInt32();
                                    var bpf = new BinaryPacketFormatter(Commands.Player_setHealth);
                                    bpf.add(Server.instance.getPlayerById(playerid).data.ped_health - 10);
                                    Server.instance.getPlayerById(playerid).connection.write(bpf.getBytes());
                                }
                            }
                            break;

                        case Commands.Vehicle_damage:
                            {
                                if (player != null)
                                {
                                    uint playerid = bpr.readUInt32();
                                    uint vehicleid = bpr.readUInt32();
                                    var bpf = new BinaryPacketFormatter(Commands.Player_setVehicleHealth);
                                    bpf.add(Server.instance.vehicleController.getById(vehicleid).health - 20);
                                    Server.instance.getPlayerById(playerid).connection.write(bpf.getBytes());
                                }
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
                                    if (player.data.ped_health > data.ped_health)
                                    {
                                        Server.instance.api.invokeOnPlayerTakeDamage(player, player.data.ped_health, data.ped_health, player.data.ped_health - data.ped_health);
                                    }
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