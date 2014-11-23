using MIVSDK;
using MIVSDK.Math;
using System;

namespace MIVServer
{
    public class ServerPlayer
    {
        public byte id;
        public string nick;
        public ClientConnection connection;
        public UpdateDataStruct data;
        private float gravity;
        private TimeSpan gametime;

        public ServerPlayer(string nick, ClientConnection connection)
        {
            this.nick = nick;
            this.connection = connection;
            data = UpdateDataStruct.Zero;
            connection.onUpdateData += onUpdateData;
            connection.onConnect += onConnnect;
            connection.onChatSendMessage += connection_onChatSendMessage;
        }

        private void connection_onChatSendMessage(string line)
        {
            Server.instance.api.invokeOnPlayerSendText(this, line);
        }

        private void onConnnect(string nick)
        {
            this.nick = nick;
        }

        private void onUpdateData(UpdateDataStruct data)
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

        public Vector3 Position
        {
            get { return data.getPositionVector(); }
            set
            {
                data.pos_x = value.X;
                data.pos_y = value.Y;
                data.pos_z = value.Z;
                connection.streamWrite(Commands.Player_setPosition);
                connection.streamWrite(BitConverter.GetBytes(value.X));
                connection.streamWrite(BitConverter.GetBytes(value.Y));
                connection.streamWrite(BitConverter.GetBytes(value.Z));
                connection.streamFlush();
            }
        }

        public Vector3 Velocity
        {
            get { return data.getVelocityVector(); }
            set
            {
                data.vel_x = value.X;
                data.vel_y = value.Y;
                data.vel_z = value.Z;
                connection.streamWrite(Commands.Player_setVelocity);
                connection.streamWrite(BitConverter.GetBytes(value.X));
                connection.streamWrite(BitConverter.GetBytes(value.Y));
                connection.streamWrite(BitConverter.GetBytes(value.Z));
                connection.streamFlush();
            }
        }

        public float Heading
        {
            get { return data.heading; }
            set
            {
                data.heading = value;
                connection.streamWrite(Commands.Player_setHeading);
                connection.streamWrite(BitConverter.GetBytes(value));
                connection.streamFlush();
            }
        }

        public float Gravity
        {
            get { return this.gravity; }
            set
            {
                gravity = value;
                connection.streamWrite(Commands.Player_setGravity);
                connection.streamWrite(BitConverter.GetBytes(value));
                connection.streamFlush();
            }
        }

        public TimeSpan GameTime
        {
            get { return this.gametime; }
            set
            {
                gametime = value;
                connection.streamWrite(Commands.Player_setHeading);
                connection.streamWrite(BitConverter.GetBytes(value.Seconds));
                connection.streamFlush();
            }
        }
    }
}