using MIVSDK;
using SharpDX;
using System;

namespace MIVServer
{
    public class ServerPlayer
    {
        public ClientConnection connection;
        public UpdateDataStruct data;
        public ServerRequester requester;
        public byte id;
        public string nick;

        private TimeSpan gametime;
        private float gravity;

        public ServerPlayer(string nick, ClientConnection connection)
        {
            this.nick = nick;
            this.connection = connection;
            requester = new ServerRequester(this);
            data = UpdateDataStruct.Zero;
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
                var bpf = new BinaryPacketFormatter(Commands.Player_setGameTime);
                bpf.add(value.Ticks);
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
                var bpf = new BinaryPacketFormatter(Commands.Player_setGravity);
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