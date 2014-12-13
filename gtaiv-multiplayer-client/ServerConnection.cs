// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using GTA;
using MIVSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MIVClient
{
    public class ServerConnection
    {
        public Dictionary<uint, UpdateDataStruct> playersdata;

        private const int BUFSIZE = 1024 * 1024 * 4;
        private byte[] buffer;
        private Client client;

        private List<byte> internal_buffer;

        public ServerConnection(Client client)
        {
            internal_buffer = new List<byte>();
            this.client = client;
            buffer = new byte[BUFSIZE];
            playersdata = new Dictionary<uint, UpdateDataStruct>();
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

        public void write(byte[] bytes)
        {
            lock (internal_buffer)
            {
                internal_buffer.AddRange(bytes);
            }
        }

        private GTA.Vector3 fromSharpDX(SharpDX.Vector3 v)
        {
            return new GTA.Vector3(v.X, v.Y, v.Z);
        }

        private GTA.Quaternion fromSharpDX(SharpDX.Quaternion v)
        {
            return new GTA.Quaternion(v.X, v.Y, v.Z, v.W);
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
                        try
                        {
                            switch (command)
                            {
                                case Commands.UpdateData:
                                    {
                                        uint playerid = bpr.readUInt32();
                                        MIVSDK.UpdateDataStruct data = bpr.readUpdateStruct();
                                        if (!playersdata.ContainsKey(playerid)) playersdata.Add(playerid, data);
                                        else playersdata[playerid] = data;
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


                                case Commands.Player_setVirtualWorld:
                                    {
                                        uint id = bpr.readUInt32();
                                        uint vworld = bpr.readUInt32();
                                        if (client.pedController.Exists(id))
                                        {
                                            var instance = client.pedController.GetInstance(id);
                                            instance.VirtualWorld = vworld;
                                        }
                                    }
                                    break;

                                case Commands.Game_setGameTime:
                                    {
                                        var g = bpr.readInt64();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            World.CurrentDayTime = new TimeSpan(g);
                                        }));
                                    }
                                    break;

                                case Commands.Game_setWeather:
                                    {
                                        var g = bpr.readByte();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            World.Weather = (Weather)(int)g;
                                            AlternateHook.call(AlternateHookRequest.OtherCommands.FORCE_WEATHER_NOW, (int)g);
                                        }));
                                    }
                                    break;

                                case Commands.Game_fadeScreenIn:
                                    {
                                        var data = bpr.readInt32();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            Game.FadeScreenIn(data);
                                        }));
                                    }
                                    break;

                                case Commands.Game_fadeScreenOut:
                                    {
                                        var data = bpr.readInt32();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            Game.FadeScreenOut(data);
                                        }));
                                    }
                                    break;

                                case Commands.Game_showLoadingScreen:
                                    {
                                        var data = bpr.readInt32();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            AlternateHook.call(AlternateHookRequest.OtherCommands.FORCE_LOADING_SCREEN);
                                        }));
                                    }
                                    break;

                                case Commands.Game_hideLoadingScreen:
                                    {
                                        var data = bpr.readInt32();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            AlternateHook.call(AlternateHookRequest.OtherCommands.DONT_DISPLAY_LOADING_ON_FADE_THIS_FRAME);
                                            Game.FadeScreenIn(1);
                                        }));
                                    }
                                    break;

                                case Commands.Game_setGravity:
                                    {
                                        var data = bpr.readSingle();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            client.getPlayerPed().GravityMultiplier = data;
                                        }));
                                    }
                                    break;

                                case Commands.Client_setVirtualWorld:
                                    {
                                        client.CurrentVirtualWorld = bpr.readUInt32();
                                    }
                                    break;

                                case Commands.Client_pauseBroadcast:
                                    {
                                        client.BroadcastingPaused = true;
                                    }
                                    break;

                                case Commands.Client_resumeBroadcast:
                                    {
                                        client.BroadcastingPaused = false;
                                    }
                                    break;

                                case Commands.Client_JSEval:
                                    {
                                        string script = bpr.readString();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            client.jsEngine.Execute(script);
                                        }));
                                    }
                                    break;

                                case Commands.Vehicle_removePeds:
                                    {
                                        uint id = bpr.readUInt32();
                                        uint vworld = bpr.readUInt32();
                                        if (client.vehicleController.Exists(id))
                                        {
                                            var instance = client.vehicleController.GetInstance(id);
                                            if (instance.StreamedIn)
                                            {
                                                instance.gameReference.EveryoneLeaveVehicle();
                                            }
                                        }
                                    }
                                    break;

                                case Commands.Vehicle_repaint:
                                    {
                                        uint id = bpr.readUInt32();
                                        ushort color = bpr.readUInt16();
                                        if (client.vehicleController.Exists(id))
                                        {
                                            var instance = client.vehicleController.GetInstance(id);
                                            if (instance.StreamedIn)
                                            {
                                                instance.gameReference.Color = (ColorIndex)color;
                                            }
                                        }
                                    }
                                    break;

                                case Commands.Vehicle_repair:
                                    {
                                        uint id = bpr.readUInt32();
                                        if (client.vehicleController.Exists(id))
                                        {
                                            var instance = client.vehicleController.GetInstance(id);
                                            //instance.health = 100;
                                            if (instance.StreamedIn)
                                            {
                                                instance.gameReference.Repair();
                                            }
                                        }
                                    }
                                    break;

                                case Commands.Vehicle_setModel:
                                    {
                                        uint id = bpr.readUInt32();
                                        string model = MIVSDK.ModelDictionary.getVehicleById(bpr.readUInt32());
                                        if (client.vehicleController.Exists(id))
                                        {
                                            var instance = client.vehicleController.GetInstance(id);
                                            instance.model = model;
                                            if (instance.StreamedIn)
                                            {
                                                instance.gameReference.Delete();
                                            }
                                        }
                                    }
                                    break;

                                case Commands.Vehicle_setOrientation:
                                    {
                                        uint id = bpr.readUInt32();
                                        Quaternion quat = fromSharpDX(bpr.readQuaternion());
                                        if (client.vehicleController.Exists(id))
                                        {
                                            var instance = client.vehicleController.GetInstance(id);
                                            instance.orientation = quat;
                                            if (instance.StreamedIn)
                                            {
                                                instance.gameReference.RotationQuaternion = quat;
                                            }
                                        }
                                    }
                                    break;

                                case Commands.Vehicle_setPosition:
                                    {
                                        uint id = bpr.readUInt32();
                                        Vector3 position = fromSharpDX(bpr.readVector3());
                                        if (client.vehicleController.Exists(id))
                                        {
                                            var instance = client.vehicleController.GetInstance(id);
                                            instance.position = position;
                                            if (instance.StreamedIn)
                                            {
                                                instance.gameReference.Position = position;
                                            }
                                        }
                                    }
                                    break;

                                case Commands.Vehicle_setVelocity:
                                    {
                                        uint id = bpr.readUInt32();
                                        Vector3 velocity = fromSharpDX(bpr.readVector3());
                                        if (client.vehicleController.Exists(id))
                                        {
                                            var instance = client.vehicleController.GetInstance(id);
                                            if (instance.StreamedIn)
                                            {
                                                instance.gameReference.Velocity = velocity;
                                            }
                                        }
                                    }
                                    break;

                                case Commands.Vehicle_setVirtualWorld:
                                    {
                                        uint id = bpr.readUInt32();
                                        uint vworld = bpr.readUInt32();
                                        if (client.vehicleController.Exists(id))
                                        {
                                            var instance = client.vehicleController.GetInstance(id);
                                            instance.VirtualWorld = vworld;
                                        }
                                    }
                                    break;

                                case Commands.NPC_setVirtualWorld:
                                    {
                                        uint id = bpr.readUInt32();
                                        uint vworld = bpr.readUInt32();
                                        if (client.npcPedController.Exists(id))
                                        {
                                            var instance = client.npcPedController.GetInstance(id);
                                            instance.VirtualWorld = vworld;
                                        }
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

                                case Commands.Player_setVehicleHealth:
                                    {
                                        int h = bpr.readInt32();
                                        //client.chatController.writeChat("setting healtcz " + h.ToString());
                                        client.enqueueAction(new Action(delegate
                                        {
                                            if (client.getPlayerPed().isInVehicle())
                                            {
                                                if (h <= 0)
                                                {
                                                    client.getPlayerPed().CurrentVehicle.Explode();
                                                }
                                                else
                                                {
                                                    client.getPlayerPed().CurrentVehicle.Health = h;
                                                }
                                            }
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
                                        Vector3 vec = fromSharpDX(bpr.readVector3());
                                        //client.chatController.writeChat("OasK");
                                        client.enqueueAction(new Action(delegate
                                        {
                                            client.getPlayer().TeleportTo(vec);
                                        }));
                                    }
                                    break;

                                case Commands.Player_setVelocity:
                                    {
                                        Vector3 vec = fromSharpDX(bpr.readVector3());
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
                                case Commands.Player_giveWeapon:
                                    {
                                        Weapon weapon = (Weapon)bpr.readInt32();
                                        int ammo = bpr.readInt32();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            client.getPlayerPed().Weapons.FromType(weapon).Ammo = ammo;
                                        }));
                                    }
                                    break;

                                case Commands.Global_setPlayerPedText:
                                    {
                                        uint playerid = bpr.readUInt32();
                                        string text = bpr.readString();
                                        if (client.pedController.Exists(playerid))
                                        {
                                            StreamedPed ped = client.pedController.GetInstance(playerid);
                                            ped.CurrentChatMessage = text;
                                        }
                                    }
                                    break;

                                case Commands.Global_setPlayerModel:
                                    {
                                        uint playerid = bpr.readUInt32();
                                        string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());
                                        if (client.playerModels.ContainsKey(playerid))
                                            client.playerModels[playerid] = model;
                                        else
                                            client.playerModels.Add(playerid, model);
                                        if (client.pedController.Exists(playerid))
                                        {
                                            var player = client.pedController.GetInstance(playerid);
                                            if (player.IsStreamedIn()) player.gameReference.Delete();
                                        }
                                    }
                                    break;

                                case Commands.Global_setPlayerName:
                                    {
                                        uint playerid = bpr.readUInt32();
                                        string name = bpr.readString();
                                        if (client.playerNames.ContainsKey(playerid))
                                            client.playerNames[playerid] = name;
                                        else
                                            client.playerNames.Add(playerid, name);
                                        if (client.pedController.Exists(playerid))
                                        {
                                            var player = client.pedController.GetInstance(playerid);
                                            if (player.IsStreamedIn()) player.gameReference.Delete();
                                        }
                                    }
                                    break;

                                case Commands.Global_createPlayer:
                                    {
                                        uint playerid = bpr.readUInt32();
                                        string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());
                                        string name = bpr.readString();

                                        if (client.playerNames.ContainsKey(playerid))
                                            client.playerNames[playerid] = name;
                                        else
                                            client.playerNames.Add(playerid, name);

                                        if (client.playerModels.ContainsKey(playerid))
                                            client.playerModels[playerid] = model;
                                        else
                                            client.playerModels.Add(playerid, model);

                                        client.pedController.Add(playerid, new StreamedPed(client.pedStreamer, model, name, Vector3.Zero, 0, (BlipColor)(playerid % 13)));
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
                                                bpf.add(client.pedController.dict.Count(a => a.Value.IsStreamedIn() && a.Value.gameReference == selectedPed) > 0 ? client.pedController.dict.First(a => a.Value.IsStreamedIn() && a.Value.gameReference == selectedPed).Key : 0);
                                            }
                                            else
                                            {
                                                bpf.add(0);
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
                                        Vector3 pos = fromSharpDX(bpr.readVector3());
                                        Quaternion rot = fromSharpDX(bpr.readQuaternion());
                                        string model = MIVSDK.ModelDictionary.getVehicleById(bpr.readUInt32());
                                        client.enqueueAction(new Action(delegate
                                        {
                                            client.vehicleController.Add(id, new StreamedVehicle(client.vehicleStreamer, model, pos, rot));
                                        }));
                                    }
                                    break;

                                case Commands.NPC_create:
                                    {
                                        //int count = bpr.readInt32();
                                        uint id = bpr.readUInt32();
                                        Vector3 pos = fromSharpDX(bpr.readVector3());
                                        float heading = bpr.readSingle();
                                        string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());

                                        string str = bpr.readString();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            client.npcPedController.Add(id, new StreamedPed(client.pedStreamer, model, str, pos, heading, BlipColor.Grey));
                                        }));
                                    }
                                    break;

                                case Commands.NPC_update:
                                    {
                                        //int count = bpr.readInt32();
                                        uint id = bpr.readUInt32();
                                        Vector3 pos = fromSharpDX(bpr.readVector3());
                                        float heading = bpr.readSingle();
                                        string model = MIVSDK.ModelDictionary.getPedModelById(bpr.readUInt32());

                                        string str = bpr.readString();
                                        client.enqueueAction(new Action(delegate
                                        {
                                            var ped = client.npcPedController.GetInstance(id);
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

                                        Vector3 pos = fromSharpDX(bpr.readVector3());

                                        client.enqueueAction(new Action(delegate
                                        {
                                            var ped = client.npcPedController.GetInstance(id);
                                            ped.position = pos;
                                            if (ped.IsStreamedIn())
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
                                            var ped = client.npcPedController.GetInstance(id);
                                            ped.heading = heading;
                                            if (ped.IsStreamedIn())
                                            {
                                                ped.gameReference.Heading = heading;
                                            }
                                        }));
                                    }
                                    break;

                                case Commands.NPC_runTo:
                                    {
                                        uint id = bpr.readUInt32();

                                        Vector3 pos = fromSharpDX(bpr.readVector3());

                                        client.enqueueAction(new Action(delegate
                                        {
                                            var ped = client.npcPedController.GetInstance(id);
                                            ped.position = pos;
                                            if (ped.IsStreamedIn())
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

                                        Vector3 pos = fromSharpDX(bpr.readVector3());

                                        client.enqueueAction(new Action(delegate
                                        {
                                            var ped = client.npcPedController.GetInstance(id);
                                            ped.position = pos;
                                            if (ped.IsStreamedIn())
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
                                            var ped = client.npcPedController.GetInstance(id);
                                            ped.vehicle_id = vid;
                                            var veh = client.vehicleController.GetInstance(vid);
                                            if (ped.IsStreamedIn() && veh.IsStreamedIn())
                                            {
                                                ped.gameReference.WarpIntoVehicle(veh.gameReference, VehicleSeat.Driver);
                                            }
                                        }));
                                    }
                                    break;

                                case Commands.NPC_driveTo:
                                    {
                                        uint id = bpr.readUInt32();

                                        Vector3 pos = fromSharpDX(bpr.readVector3());

                                        client.enqueueAction(new Action(delegate
                                        {
                                            var ped = client.npcPedController.GetInstance(id);
                                            if (ped.vehicle_id > 0)
                                            {
                                                var veh = client.vehicleController.GetInstance(ped.vehicle_id);
                                                if (ped.IsStreamedIn() && veh.IsStreamedIn())
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
                                            var ped = client.npcPedController.GetInstance(id);
                                            ped.vehicle_id = 0;
                                            if (ped.IsStreamedIn())
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
                                            var ped = client.npcPedController.GetInstance(id);
                                            ped.model = model;
                                            if (ped.IsStreamedIn())
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
                                            var ped = client.npcPedController.GetInstance(id);
                                            ped.immortal = option == 1;
                                            if (ped.IsStreamedIn())
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
                        catch (Exception e)
                        {
                            Client.log("Failed dispatching command " + command.ToString() + " with " + e.Message + " " + e.StackTrace);
                            client.chatController.writeChat("Failed dispatching command " + command.ToString() + " with " + e.Message + " " + e.StackTrace);
                            client.chatController.writeChat("Disconnected abnormally from server");
                            client.currentState = ClientState.Disconnected;
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