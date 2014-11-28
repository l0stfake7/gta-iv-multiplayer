using GTA;
using MIVSDK;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace MIVClient
{
    public class ServerConnection
    {
        public Dictionary<byte, UpdateDataStruct> playersdata;

        private const int BUFSIZE = 1024 * 100;
        private byte[] buffer;
        private Client client;

        public ServerConnection(Client client)
        {
            internal_buffer = new List<byte>();
            this.client = client;
            buffer = new byte[BUFSIZE];
            playersdata = new Dictionary<byte, UpdateDataStruct>();
            client.client.SendBufferSize = BUFSIZE;
            client.client.Client.ReceiveBufferSize = BUFSIZE;
            client.client.Client.Blocking = true;
            client.client.NoDelay = true;
            client.client.Client.DontFragment = true;
            client.client.Client.NoDelay = true;
            client.client.Client.Blocking = true;
            client.client.Client.ReceiveBufferSize = BUFSIZE;
            client.client.Client.SendBufferSize = BUFSIZE;
            client.client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
        }

        private List<byte> internal_buffer;
        public void write(byte[] bytes)
        {
            lock (internal_buffer)
            {
                internal_buffer.AddRange(bytes);
            }
        }
        public void flush()
        {
            lock (internal_buffer)
            {
                var stream = client.client.GetStream();
                internal_buffer.InsertRange(0, BitConverter.GetBytes((int)(internal_buffer.Count + 4)));
                stream.Write(internal_buffer.ToArray(), 0, internal_buffer.Count);
                stream.Flush();
                internal_buffer = new List<byte>();
            }
        }


        private void onReceive(IAsyncResult iar)
        {
            lock (client)
            {
                try
                {
                    client.client.Client.EndReceive(iar);
                    var bpr = new BinaryPacketReader(buffer);
                    while (bpr.canRead())
                    {
                        Commands command = bpr.readCommand();
                        if (command == Commands.Invalid) break;
                        switch (command)
                        {
                            case Commands.UpdateData:
                                {
                                    byte playerid = bpr.readByte();
                                    MIVSDK.UpdateDataStruct data = bpr.readUpdateStruct();
                                    if (!playersdata.ContainsKey(playerid)) playersdata.Add(playerid, data);
                                    else playersdata[playerid] = data;
                                }
                                break;


                            case Commands.Player_setGravity:
                                {
                                    var g = bpr.readSingle();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayerPed().GravityMultiplier = g;
                                    }));
                                }
                                break;
                            case Commands.Player_setHeading:
                                {
                                    var g = bpr.readSingle();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayerPed().Heading = g;
                                    }));
                                }
                                break;
                            case Commands.Player_setGameTime:
                                {
                                    var g = bpr.readInt64();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        World.CurrentDayTime = new TimeSpan(g);
                                    }));
                                }
                                break;

                            case Commands.Player_setHealth:
                                {
                                    int h = bpr.readInt32();
                                    client.chatController.writeChat("setting healtcz " + h.ToString());
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayerPed().Health = h;
                                    }));
                                }
                                break;

                            case Commands.Chat_writeLine:
                                {
                                    client.chatController.writeChat(bpr.readString());
                                }
                                break;

                            case Commands.Player_setPosition:
                                {
                                    Vector3 vec = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());
                                    client.chatController.writeChat("OasK");
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayerPed().Position = vec;
                                    }));
                                }
                                break;
                            case Commands.Player_setVelocity:
                                {
                                    Vector3 vec = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayerPed().Velocity = vec;
                                    }));
                                }
                                break;

                            case Commands.InternalClient_finishSpawn:
                                {
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.finishSpawn();
                                    }));
                                }
                                break;

                            case Commands.Vehicle_create:
                                {
                                    uint id = bpr.readUInt32();
                                    Vector3 pos = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());
                                    Quaternion rot = new Quaternion(bpr.readSingle(), bpr.readSingle(), bpr.readSingle(), bpr.readSingle());
                                    Vector3 vel = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());
                                    string model = bpr.readString();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.vehicleController.create(id, model, pos, rot, vel);
                                    }));
                                }
                                break;

                            case Commands.NPC_create:
                                {
                                    //int count = bpr.readInt32();
                                    uint id = bpr.readUInt32();
                                    Vector3 pos = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());
                                    float heading = bpr.readSingle();
                                    string model = MIVSDK.ModelDictionary.getById(bpr.readUInt32());

                                    string str = bpr.readString();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.npcPedController.getById(id, model, str, heading, pos);
                                    }));
                                }
                                break;

                            case Commands.NPCDialog_show:
                                {
                                    uint id = bpr.readUInt32();
                                    string captiontext = bpr.readString();
                                    string texttext = bpr.readString();
                                    string str = bpr.readString();
                                    string[] split = str.Split('\x01');
                                    client.enqueueAction(new Action(delegate
                                    {
                                        GTA.Forms.Form form = new GTA.Forms.Form();

                                        GTA.Forms.Label caption = new GTA.Forms.Label();
                                        caption.Location = new System.Drawing.Point(10, 10);
                                        caption.Text = captiontext;

                                        GTA.Forms.Label text = new GTA.Forms.Label();
                                        text.Location = new System.Drawing.Point(10, 40);
                                        text.Text = texttext;

                                        form.Controls.Add(caption);
                                        form.Controls.Add(text);

                                        for (int i = 0; i < split.Length; i++)
                                        {
                                            GTA.Forms.Button button = new GTA.Forms.Button();
                                            button.Location = new System.Drawing.Point(10, 40 + i * 20);
                                            button.Text = split[i];

                                            button.MouseDown += (s, o) =>
                                            {
                                                var bpf = new BinaryPacketFormatter(Commands.NPCDialog_sendResponse);
                                                bpf.add(id);
                                                bpf.add(new byte[1] { (byte)(i - 2) });
                                                write(bpf.getBytes());

                                                form.Close();
                                            };

                                            form.Controls.Add(button);
                                        }
                                        form.Show();
                                    }));
                                }
                                break;
                        }
                    }

                    client.client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
                    //}
                }
                catch (Exception e)
                {
                    Client.log("Failed receive with message " + e.Message + " " + e.StackTrace);
                    client.chatController.writeChat("Failed receive with message " + e.Message + " " + e.StackTrace);
                    client.currentState = ClientState.Disconnected;
                    //throw e;
                }
            }
        }
    }
}