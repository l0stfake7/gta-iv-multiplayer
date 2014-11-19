using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;

namespace MIVServer
{
    public class ServerPlayer
    {
        public byte id;
        public string nick;
        public Vector3 position, velocity;
        public Quaternion orientation;
        public float heading, speed;
        public ClientConnection connection;
        public int vehicle_model;
        public bool isInVehicle;
        public int pedHealth;
        public int vehicleHealth;

        public ServerPlayer(string nick, ClientConnection connection)
        {
            this.nick = nick;
            this.connection = connection;
            position = Vector3.Zero;
            velocity = Vector3.Zero;
            orientation = Quaternion.Zero;
            heading = 0;
            speed = 0;
            vehicle_model = 0;
            isInVehicle = false;
            pedHealth = 100;
            vehicleHealth = 100;
            connection.onUpdateData += onUpdateData;
            connection.onConnect += onConnnect;
            connection.onChatSendMessage += connection_onChatSendMessage;
        }

        void connection_onChatSendMessage(string line)
        {
            Server.instance.chat.addLine(nick + ": " + line);
        }
        private void onConnnect(string nick)
        {
            this.nick = nick;
        }

        private void onUpdateData(UpdateDataStruct data)
        {
            this.position = new Vector3(data.pos_x, data.pos_y, data.pos_z);
            this.orientation = new Quaternion(data.rot_x, data.rot_y, data.rot_z, data.rot_a);
            this.velocity = new Vector3(data.vel_x, data.vel_y, data.vel_z);
            this.isInVehicle = data.vehicle_model > 0;
            this.vehicle_model = data.vehicle_model;
            this.heading = data.heading;
            this.speed = data.speed;
            this.pedHealth = data.ped_health;
            this.vehicleHealth = data.veh_health;
            //Server.instance.broadcastData(this);
            Console.WriteLine("Updated player " + nick);

        }

        public void teleport(Vector3 pos)
        {
            position = pos;
            connection.streamWrite(Commands.Player_setPosition);
            connection.streamWrite(BitConverter.GetBytes(pos.X));
            connection.streamWrite(BitConverter.GetBytes(pos.Y));
            connection.streamWrite(BitConverter.GetBytes(pos.Z));
            connection.streamFlush();
        }
    }
}
