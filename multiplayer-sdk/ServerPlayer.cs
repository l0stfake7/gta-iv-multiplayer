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
        private WeatherType weather = 0;

        public ServerPlayer(string nick, ClientConnection connection)
        {
            this.nick = nick;
            virtualWorld = 0;
            model = "F_Y_NURSE";
            this.connection = connection;
            requester = new ServerRequester(this);
            data = UpdateDataStruct.Zero;
            Camera = new PlayerCamera(this);
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
                foreach (var single in Server.instance.playerpool)
                {
                    if (single.id != this.id)
                    {
                        single.connection.write(bpf.getBytes());
                    }
                }
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
                bpf.add((Int64)value.Ticks);
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
                bpf.add(value);
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
                data.heading = value;
                var bpf = new BinaryPacketFormatter(Commands.Player_setHeading);
                bpf.add(value);
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
                bpf.add(ModelDictionary.getPedModelByName(this.model));
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
                data.pos_x = value.X;
                data.pos_y = value.Y;
                data.pos_z = value.Z;
                var bpf = new BinaryPacketFormatter(Commands.Player_setPosition);
                bpf.add(value);
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
                data.vel_x = value.X;
                data.vel_y = value.Y;
                data.vel_z = value.Z;
                var bpf = new BinaryPacketFormatter(Commands.Player_setVelocity);
                bpf.add(value);
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
                var bpf = new BinaryPacketFormatter(Commands.Player_setVirtualWorld);
                bpf.add(ModelDictionary.getPedModelByName(this.model));
                connection.write(bpf.getBytes());
                Server.instance.broadcastPlayerModel(this);
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
                bpf.add(new byte[1] { (byte)value });
                connection.write(bpf.getBytes());
            }
        }

        public void updateData(UpdateDataStruct data)
        {
            this.data = data;

            if (data.vehicle_id > 0)
            {
                Server.instance.vehicleController.vehicles[data.vehicle_id].position = data.getPositionVector();
                Server.instance.vehicleController.vehicles[data.vehicle_id].orientation = data.getOrientationQuaternion();
                Server.instance.vehicleController.vehicles[data.vehicle_id].velocity = data.getVelocityVector();
            }
            Server.instance.api.invokeOnPlayerUpdate(this);
            //Server.instance.broadcastData(this);
            //Console.WriteLine("Updated player " + nick);
        }
    }
}