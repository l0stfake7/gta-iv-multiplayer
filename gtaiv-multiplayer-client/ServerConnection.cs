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

        private GTA.Vector3 fromSharpDX(SharpDX.Vector3 v)
        {
            return new GTA.Vector3(v.X, v.Y, v.Z);
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

                            case Commands.Player_setModel:
                                {
                                    string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayer().Model = new Model(model);
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
                                    //client.chatController.writeChat("setting healtcz " + h.ToString());
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
                                    //client.chatController.writeChat("OasK");
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayer().TeleportTo(vec);
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
                            case Commands.Camera_setPosition:
                                {
                                    var data = fromSharpDX(bpr.readVector3());
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.cameraController.Position = data;
                                    }));
                                }
                                break;
                            case Commands.Camera_setDirection:
                                {
                                    var data = fromSharpDX(bpr.readVector3());
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.cameraController.Direction = data;
                                    }));
                                }
                                break;

                            case Commands.Camera_setOrientation:
                                {
                                    var data = fromSharpDX(bpr.readVector3());
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.cameraController.Rotation = data;
                                    }));
                                }
                                break;
                            case Commands.Camera_setFOV:
                                {
                                    var data = bpr.readSingle();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.cameraController.FOV = data;
                                    }));
                                }
                                break;

                            case Commands.Camera_lookAt:
                                {
                                    var data = fromSharpDX(bpr.readVector3());
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.cameraController.LookAt(data);
                                    }));
                                }
                                break;
                            case Commands.Camera_reset:
                                {
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.cameraController.Reset();
                                    }));
                                }
                                break;
                            case Commands.Player_freeze:
                                {
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayer().CanControlCharacter = false;
                                    }));
                                }
                                break;
                            case Commands.Player_unfreeze:
                                {
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.getPlayer().CanControlCharacter = true;
                                    }));
                                }
                                break;
                            case Commands.Global_setPlayerModel:
                                {
                                    byte playerid = bpr.readByte();
                                    string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());
                                    if (client.playerModels.ContainsKey(playerid))
                                        client.playerModels[playerid] = model;
                                    else
                                        client.playerModels.Add(playerid, model);
                                    var player = client.pedController.getById(playerid);
                                    if (player.streamedIn && player.gameReference.Exists()) player.gameReference.Delete();
                                }
                                break;
                            case Commands.Global_setPlayerName:
                                {
                                    byte playerid = bpr.readByte();
                                    string name = bpr.readString();
                                    if (client.playerNames.ContainsKey(playerid))
                                        client.playerNames[playerid] = name;
                                    else
                                        client.playerNames.Add(playerid, name);
                                    var player = client.pedController.getById(playerid);
                                    if (player.streamedIn && player.gameReference.Exists()) player.gameReference.Delete();
                                }
                                break;
                            case Commands.Request_getSelectedPlayer:
                                {
                                    uint requestid = bpr.readUInt32();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        var peds = World.GetPeds(client.getPlayerPed().Position, 80.0f);
                                        Ped selectedPed = null;
                                        foreach (var ped in peds)
                                        {
                                            var projected = (Vector2)World.WorldToScreenProject(ped.Position);
                                            if (Math.Abs((projected - new Vector2(Game.Resolution.Width / 2, Game.Resolution.Height / 2)).Length()) < 30.0)
                                            {
                                                selectedPed = ped;
                                                break;
                                            }
                                        }
                                        var bpf = new BinaryPacketFormatter(Commands.Request_getSelectedPlayer);
                                        bpf.add(requestid);
                                        if (selectedPed != null)
                                        {
                                            bpf.add(new byte[1] { client.pedController.getPlayerIdByPed(selectedPed) });
                                        }
                                        else
                                        {
                                            bpf.add(new byte[1] { 255 });
                                        }
                                        client.serverConnection.write(bpf.getBytes());
                                    }));
                                }
                                break;
                            case Commands.Request_getCameraPosition:
                                {
                                    uint requestid = bpr.readUInt32();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        var bpf = new BinaryPacketFormatter(Commands.Request_getCameraPosition);
                                        bpf.add(requestid);
                                        bpf.add(new SharpDX.Vector3(Game.CurrentCamera.Position.X, Game.CurrentCamera.Position.Y, Game.CurrentCamera.Position.Z));
                                        client.serverConnection.write(bpf.getBytes());
                                    }));
                                }
                                break;
                            case Commands.Request_worldToScreen:
                                {
                                    uint requestid = bpr.readUInt32();
                                    var world = bpr.readVector3();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        var bpf = new BinaryPacketFormatter(Commands.Request_worldToScreen);
                                        bpf.add(requestid);
                                        var screen = (Vector2)World.WorldToScreenProject(new Vector3(world.X, world.Y, world.Z));
                                        bpf.add(screen.X);
                                        bpf.add(screen.Y);
                                        client.serverConnection.write(bpf.getBytes());
                                    }));
                                }
                                break;
                            case Commands.Request_isObjectVisible:
                                {
                                    uint requestid = bpr.readUInt32();
                                    var position = bpr.readVector3();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        var bpf = new BinaryPacketFormatter(Commands.Request_isObjectVisible);
                                        bpf.add(requestid);
                                        bpf.add(new byte[1] { (byte)(Game.CurrentCamera.isSphereVisible(new Vector3(position.X, position.Y, position.Z), 1.0f) ? 1 : 0) });
                                        client.serverConnection.write(bpf.getBytes());
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
                                    string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());

                                    string str = bpr.readString();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        client.npcPedController.getById(id, model, str, heading, pos);
                                    }));
                                }
                                break;
                            case Commands.NPC_update:
                                {
                                    //int count = bpr.readInt32();
                                    uint id = bpr.readUInt32();
                                    Vector3 pos = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());
                                    float heading = bpr.readSingle();
                                    string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());

                                    string str = bpr.readString();
                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.position = pos;
                                        ped.heading = heading;
                                        ped.model = model;
                                        ped.networkname = str;
                                    }));
                                }
                                break;
                            case Commands.NPC_setPosition:
                                {
                                    uint id = bpr.readUInt32();

                                    Vector3 pos = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.position = pos;
                                        if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists())
                                        {
                                            ped.gameReference.Position = pos;
                                        }
                                    }));
                                }
                                break;
                            case Commands.NPC_setHeading:
                                {
                                    uint id = bpr.readUInt32();

                                    float heading = bpr.readSingle();

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.heading = heading;
                                        if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists())
                                        {
                                            ped.gameReference.Heading = heading;
                                        }
                                    }));
                                }
                                break;
                            case Commands.NPC_runTo:
                                {
                                    uint id = bpr.readUInt32();

                                    Vector3 pos = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.position = pos;
                                        if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists())
                                        {
                                            ped.animator.refreshAnimationForce();
                                            ped.animator.playAnimation(PedAnimations.RunTo, pos);
                                        }
                                    }));
                                }
                                break;
                            case Commands.NPC_walkTo:
                                {
                                    uint id = bpr.readUInt32();

                                    Vector3 pos = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.position = pos;
                                        if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists())
                                        {
                                            ped.animator.refreshAnimationForce();
                                            ped.animator.playAnimation(PedAnimations.WalkTo, pos);
                                        }
                                    }));
                                }
                                break;
                            case Commands.NPC_enterVehicle:
                                {
                                    uint id = bpr.readUInt32();

                                    uint vid = bpr.readUInt32();

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.vehicle_id = vid;
                                        var veh = client.vehicleController.getById(vid);
                                        if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists() &&
                                            veh.streamedIn && veh.gameReference != null && veh.gameReference.Exists())
                                        {
                                            ped.gameReference.WarpIntoVehicle(veh.gameReference, VehicleSeat.Driver);
                                        }
                                    }));
                                }
                                break;
                            case Commands.NPC_driveTo:
                                {
                                    uint id = bpr.readUInt32();

                                    Vector3 pos = new Vector3(bpr.readSingle(), bpr.readSingle(), bpr.readSingle());

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        if (ped.vehicle_id > 0)
                                        {
                                            var veh = client.vehicleController.getById(ped.vehicle_id);
                                            if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists() &&
                                                veh.streamedIn && veh.gameReference != null && veh.gameReference.Exists())
                                            {
                                                ped.gameReference.Task.DriveTo(veh.gameReference, pos, 999.0f, false, true);
                                            }
                                        }
                                    }));
                                }
                                break;
                            case Commands.NPC_leaveVehicle:
                                {
                                    uint id = bpr.readUInt32();

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.vehicle_id = 0;
                                        if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists())
                                        {
                                            ped.gameReference.LeaveVehicle();
                                        }
                                    }));
                                }
                                break;
                            case Commands.NPC_setModel:
                                {
                                    uint id = bpr.readUInt32();
                                    string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.model = model;
                                        if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists())
                                        {
                                            ped.gameReference.Delete();
                                        }
                                    }));
                                }
                                break;
                            case Commands.NPC_setImmortal:
                                {
                                    uint id = bpr.readUInt32();
                                    byte option = bpr.readByte();

                                    client.enqueueAction(new Action(delegate
                                    {
                                        var ped = client.npcPedController.getById(id);
                                        ped.immortal = option == 1;
                                        if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists())
                                        {
                                            ped.gameReference.Invincible = option == 1;
                                        }
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
                    client.chatController.writeChat("Disconnected abnormally from server");
                    client.currentState = ClientState.Disconnected;
                    //throw e;
                }
            }
        }
    }
}