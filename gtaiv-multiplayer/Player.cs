using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using multiplayer_sdk;

namespace gtaiv_multiplayer
{
    public class Player
    {
        public byte id;
        public string nick;
        public Vec3 position, velocity;
        public Quaternion orientation;
        public float heading, speed;
        public ClientConnection connection;
        public int vehicle_model;
        public bool isInVehicle;
        public int pedHealth;
        public int vehicleHealth;

        public Player(string nick, ClientConnection connection)
        {
            this.nick = nick;
            this.connection = connection;
            connection.onUpdateData += onUpdateData;
            connection.onConnect += onConnnect;
        }
        private void onConnnect(string nick)
        {
            this.nick = nick;
        }

        private void onUpdateData(UpdateDataStruct data)
        {
            this.position = new Vec3(data.pos_x, data.pos_y, data.pos_z);
            this.orientation = new Quaternion(data.rot_x, data.rot_y, data.rot_z, data.rot_a);
            this.velocity = new Vec3(data.vel_x, data.vel_y, data.vel_z);
            this.isInVehicle = data.vehicle_model > 0;
            this.vehicle_model = data.vehicle_model;
            this.heading = data.heading;
            this.speed = data.speed;
            this.pedHealth = data.ped_health;
            this.vehicleHealth = data.veh_health;
            //Server.instance.broadcastData(this);
            Console.WriteLine("Updated player " + nick);

        }
    }
}
