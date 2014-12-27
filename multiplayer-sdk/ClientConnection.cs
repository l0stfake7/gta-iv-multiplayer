// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MIVServer
{
    public class ClientConnection
    {
        private ServerPlayer Player;

        private const int BUFSIZE = 1024 * 1024 * 4;

        private byte[] Buffer;

        private TcpClient Connection;

        private List<byte> InternalBuffer;

        public ClientConnection(TcpClient client)
        {
            InternalBuffer = new List<byte>();
            this.Connection = client;
            client.SendBufferSize = BUFSIZE;
            client.Client.ReceiveBufferSize = BUFSIZE;
            client.Client.Blocking = true;
            client.NoDelay = true;
            client.Client.DontFragment = true;
            client.Client.NoDelay = true;
            client.Client.Blocking = true;
            client.Client.ReceiveBufferSize = BUFSIZE;
            client.Client.SendBufferSize = BUFSIZE;
            Buffer = new byte[BUFSIZE];
        }

        public delegate void OnConnectDelegate(string nick);

        public event OnConnectDelegate OnConnect;

        public void SetPlayer(ServerPlayer player)
        {
            this.Player = player;
        }

        public void flush()
        {
            lock (this.Connection)
            {
                try
                {
                    var stream = this.Connection.GetStream();
                    this.InternalBuffer.InsertRange(0, BitConverter.GetBytes((int)(InternalBuffer.Count + 4)));
                    stream.Write(this.InternalBuffer.ToArray(), 0, this.InternalBuffer.Count);
                    stream.Flush();
                    InternalBuffer = new List<byte>();
                }
                catch
                {
                    if (Player != null)
                    {
                        Server.instance.api.invokeOnPlayerDisconnect(Player);

                        Player.data = null;
                        Server.instance.playerpool.Remove(Player);
                        Player = null;
                    }
                }
            }
        }

        public void startReceiving()
        {
            Connection.Client.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, onReceive, null);
        }

        public void write(byte[] bytes)
        {
            lock (Connection)
            {
                InternalBuffer.AddRange(bytes);
            }
        }

        private void onReceive(IAsyncResult iar)
        {
            lock (Connection)
            {
                try
                {
                    int count = Connection.Client.EndReceive(iar);
                }
                catch
                {
                    Console.WriteLine("Client disconnected");
                    return;
                }

                var bpr = new BinaryPacketReader(Buffer);
                while (bpr.canRead())
                {
                    Commands command = bpr.readCommand();
                    if (command == Commands.Invalid) break;
                    switch (command)
                    {
                        case Commands.Disconnect:
                            {
                                if (Player != null)
                                {
                                    Server.instance.api.invokeOnPlayerDisconnect(Player);
                                }
                            }
                            break;

                        case Commands.Connect:
                            {
                                string nick = bpr.readString();
                                if (OnConnect != null) OnConnect.Invoke(nick);
                            }
                            break;

                        case Commands.InternalClient_requestSpawn:
                            {
                                if (Player != null)
                                {
                                    Server.instance.api.invokeOnPlayerDie(Player);
                                    Server.instance.api.invokeOnPlayerSpawn(Player);
                                    var bpf = new BinaryPacketFormatter(Commands.InternalClient_finishSpawn);
                                    Player.connection.write(bpf.getBytes());
                                }
                            }
                            break;
                        case Commands.Client_ping:
                            {
                                Int64 timestamp = bpr.readInt64();
                                Int64 current = DateTime.Now.Ticks;
                                Player.Ping = (int)((current - timestamp) /  10000);
                            }
                            break;

                        case Commands.Request_getSelectedPlayer:
                            {
                                if (Player != null)
                                {
                                    uint requestid = bpr.readUInt32();
                                    uint playerid = bpr.readUInt32();
                                    Request.dispatch(requestid, playerid);
                                }
                            }
                            break;

                        case Commands.Request_getCameraPosition:
                            {
                                if (Player != null)
                                {
                                    uint requestid = bpr.readUInt32();
                                    var vect = bpr.readVector3();
                                    Request.dispatch(requestid, vect);
                                }
                            }
                            break;

                        case Commands.Request_isObjectVisible:
                            {
                                if (Player != null)
                                {
                                    uint requestid = bpr.readUInt32();
                                    var vect = bpr.readByte() == 1;
                                    Request.dispatch(requestid, vect);
                                }
                            }
                            break;

                        case Commands.Request_worldToScreen:
                            {
                                if (Player != null)
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
                                    Server.instance.api.invokeOnPlayerSendCommand(Player, split.First().Substring(1), split.Skip(1).ToArray());
                                }
                                else
                                {
                                    Server.instance.api.invokeOnPlayerSendText(Player, text);
                                }
                            }
                            break;

                        case Commands.Player_damage:
                            {
                                if (Player != null)
                                {
                                    uint playerid = bpr.readUInt32();
                                    var bpf = new BinaryPacketFormatter(Commands.Player_setHealth);
                                    int newvalue = Server.instance.getPlayerById(playerid).data.ped_health - 10;
                                    bpf.Add(newvalue);
                                    var damaged_player = Server.instance.getPlayerById(playerid);
                                    damaged_player.connection.write(bpf.getBytes());
                                    if (newvalue <= 0 && !Player.isDead)
                                    {
                                        Player.isDead = true;
                                        Server.instance.api.invokeOnPlayerDie(damaged_player, Player, (Enums.Weapon)Player.data.weapon);
                                    }
                                }
                            }
                            break;

                        case Commands.Vehicle_damage:
                            {
                                if (Player != null)
                                {
                                    uint playerid = bpr.readUInt32();
                                    uint vehicleid = bpr.readUInt32();
                                    int delta = bpr.readInt32();
                                    var bpf = new BinaryPacketFormatter(Commands.Player_setVehicleHealth);
                                    int newvalue = Server.instance.vehicleController.getById(vehicleid).health - delta;
                                    bpf.Add(newvalue);
                                    Server.instance.getPlayerById(playerid).connection.write(bpf.getBytes());
                                }
                            }
                            break;

                        case Commands.Keys_down:
                            {
                                if (Player != null)
                                {
                                    int key = bpr.readInt32();
                                    Server.instance.api.invokeOnPlayerKeyDown(Player, (System.Windows.Forms.Keys)key);
                                }
                            }
                            break;

                        case Commands.Keys_up:
                            {
                                if (Player != null)
                                {
                                    int key = bpr.readInt32();
                                    Server.instance.api.invokeOnPlayerKeyUp(Player, (System.Windows.Forms.Keys)key);
                                }
                            }
                            break;

                        case Commands.NPCDialog_sendResponse:
                            {
                                if (Player != null)
                                {
                                    uint key = bpr.readUInt32();
                                    byte answer = Buffer[6];
                                    ServerNPCDialog.invokeResponse(Player, key, answer);
                                }
                            }
                            break;

                        case Commands.UpdateData:
                            {
                                if (Player != null)
                                {
                                    MIVSDK.UpdateDataStruct data = bpr.readUpdateStruct();
                                    if (Player.data.ped_health > data.ped_health)
                                    {
                                        Server.instance.api.invokeOnPlayerTakeDamage(Player, Player.data.ped_health, data.ped_health, Player.data.ped_health - data.ped_health);
                                    }
                                    Player.updateData(data);
                                }
                            }
                            break;
                    }
                }

                Connection.Client.BeginReceive(Buffer, 0, Buffer.Length, SocketFlags.None, onReceive, null);
            }
        }
    }
}