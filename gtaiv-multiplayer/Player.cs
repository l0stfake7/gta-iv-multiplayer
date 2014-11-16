using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using multiplayer_sdk;

namespace gtaiv_multiplayer
{
    class Player
    {
        public byte id;
        public string nick;
        public Vec3 position;
        public Quaternion orientation;
        public ClientConnection connection;

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
            //Server.instance.broadcastData(this);
            Console.WriteLine("Updated player " + nick);
        }
    }
}
