// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using SharpDX;
using System;

namespace MIVServer
{
    public class ServerPlayer
    {
        public PlayerCamera Camera;
        public ClientConnection connection;
        public UpdateDataStruct data;
        public uint id;
        public object metadata;
        public ServerRequester requester;
        private string currentPedText = "";
        private bool freezed = false;
        private TimeSpan gametime;
        private float gravity;
        private string nick, model;
        private uint virtualWorld;
        public bool isDead = false;
        private WeatherType weather = 0;
        public ServerUDPTunnel udpTunnel;

        public ServerPlayer(uint id, string nick, ClientConnection connection)
        {
            this.id = id;
            this.nick = nick;
            virtualWorld = 0;
            model = "F_Y_NURSE";
            this.connection = connection;
            requester = new ServerRequester(this);
            data = UpdateDataStruct.Zero;
            isDead = true;
            Camera = new PlayerCamera(this);
            udpTunnel = new ServerUDPTunnel(this);
        }

        public enum WeatherType
        {
            ExtraSunny = 0,
            Sunny = 1,
            SunnyAndWindy = 2,
            Cloudy = 3,
            Raining = 4,
            Drizzle = 5,
            Foggy = 6,
            ThunderStorm = 7,
            ExtraSunny2 = 8,
            SunnyAndWindy2 = 9,
        }

        public string CurrentPedText
        {
            get
            {
                return currentPedText;
            }
            set
            {
                currentPedText = value;
                var bpf = new BinaryPacketFormatter(Commands.Global_setPlayerPedText, id, value);
                Server.instance.InvokeParallelForEachPlayer((single) =>
                {
                    if (single.id != this.id)
                    {
                        single.connection.write(bpf.getBytes());
                    }
                });
            }
        }

        public bool Freezed
        {
            get
            {
                return freezed;
            }
            set
            {
                freezed = value;
                var bpf = new BinaryPacketFormatter(value ? Commands.Player_freeze : Commands.Player_unfreeze);
                connection.write(bpf.getBytes());
            }
        }

        public TimeSpan GameTime
        {
            get
            {
                return this.gametime;
            }
            set
            {
                gametime = value;
                var bpf = new BinaryPacketFormatter(Commands.Game_setGameTime);
                bpf.Add((Int64)value.Ticks);
                connection.write(bpf.getBytes());
            }
        }

        public float Gravity
        {
            get
            {
                return this.gravity;
            }
            set
            {
                gravity = value;
                var bpf = new BinaryPacketFormatter(Commands.Game_setGravity);
                bpf.Add(value);
                connection.write(bpf.getBytes());
            }
        }

        public float Heading
        {
            get
            {
                return data.heading;
            }
            set
            {
                //data.heading = value;
                var bpf = new BinaryPacketFormatter(Commands.Player_setHeading);
                bpf.Add(value);
                connection.write(bpf.getBytes());
            }
        }

        public string Model
        {
            get
            {
                return this.model;
            }
            set
            {
                model = value;
                var bpf = new BinaryPacketFormatter(Commands.Player_setModel);
                bpf.Add(ModelDictionary.getPedModelByName(this.model));
                connection.write(bpf.getBytes());
                Server.instance.broadcastPlayerModel(this);
            }
        }

        public string Nick
        {
            get
            {
                return this.nick;
            }
            set
            {
                nick = value;
                Server.instance.broadcastPlayerName(this);
            }
        }

        public Vector3 Position
        {
            get
            {
                return data.getPositionVector();
            }
            set
            {
                //data.pos_x = value.X;
                //data.pos_y = value.Y;
                //data.pos_z = value.Z;
                var bpf = new BinaryPacketFormatter(Commands.Player_setPosition);
                bpf.Add(value);
                connection.write(bpf.getBytes());
            }
        }

        public Vector3 Velocity
        {
            get
            {
                return data.getVelocityVector();
            }
            set
            {
                //data.vel_x = value.X;
                //data.vel_y = value.Y;
                //data.vel_z = value.Z;
                var bpf = new BinaryPacketFormatter(Commands.Player_setVelocity);
                bpf.Add(value);
                connection.write(bpf.getBytes());
            }
        }

        public uint VirtualWorld
        {
            get
            {
                return virtualWorld;
            }
            set
            {
                virtualWorld = value;
                var bpf = new BinaryPacketFormatter(Commands.Client_setVirtualWorld, value);
                connection.write(bpf.getBytes());
                var bpf2 = new BinaryPacketFormatter(Commands.Player_setVirtualWorld, id, value);
                Server.instance.InvokeParallelForEachPlayer((single) =>
                {
                    if (single.id != this.id)
                    {
                        single.connection.write(bpf2.getBytes());
                    }
                });
            }
        }

        public WeatherType Weather
        {
            get
            {
                return this.weather;
            }
            set
            {
                weather = value;
                var bpf = new BinaryPacketFormatter(Commands.Game_setWeather);
                bpf.Add(new byte[1] { (byte)value });
                connection.write(bpf.getBytes());
            }
        }

        private int _ping = 0;
        public int Ping
        {
            get
            {
                updatePing();
                return _ping;
            }
            set
            {
                _ping = value;
            }
        }

        private void updatePing()
        {
            var bpf = new BinaryPacketFormatter(Commands.Client_ping);
            bpf.Add(DateTime.Now.Ticks);
            connection.write(bpf.getBytes());
        }

        public void GiveWeapon(Enums.Weapon weapon, int ammo)
        {
            int w = (int)weapon;
            var bpf = new BinaryPacketFormatter(Commands.Player_giveWeapon, w, ammo);
            connection.write(bpf.getBytes());
        }

        public void updateData(UpdateDataStruct data)
        {
            this.data = data;
            if (isDead && data.ped_health > 0)
            {
                isDead = false;
                Server.instance.api.invokeOnPlayerSpawn(this);
            }
            if (!isDead && data.ped_health <= 0)
            {
                isDead = true;
                Server.instance.api.invokeOnPlayerDie(this);
            }


            if (data.vehicle_id > 0)
            {
                Server.instance.vehicleController.vehicles[data.vehicle_id].position = data.getPositionVector();
                Server.instance.vehicleController.vehicles[data.vehicle_id].orientation = data.getOrientationQuaternion();
                Server.instance.vehicleController.vehicles[data.vehicle_id].velocity = data.getVelocityVector();
                Server.instance.vehicleController.vehicles[data.vehicle_id].health = data.vehicle_health;
            }
            Server.instance.api.invokeOnPlayerUpdate(this);
            //Server.instance.broadcastData(this);
            //Console.WriteLine("Updated Player " + nick);
        }
    }
}