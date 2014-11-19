using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MIVServer
{
    public class ServerApi
    {
        Server server;
        public ServerApi(Server instance)
        {
            server = instance;
        }

        public delegate void onPlayerConnectDelegate(EndPoint address, ServerPlayer player);
        public event onPlayerConnectDelegate onPlayerConnect;
        public void invokeOnPlayerConnect(EndPoint address, ServerPlayer player)
        {
            if (onPlayerConnect != null) onPlayerConnect.Invoke(address, player);
        }

        public delegate void onPlayerDisconnectDelegate(ServerPlayer player);
        public event onPlayerDisconnectDelegate onPlayerDisconnect;
        public void invokeOnPlayerDisconnect(ServerPlayer player)
        {
            if (onPlayerDisconnect != null) onPlayerDisconnect.Invoke(player);
        }

        public ServerPlayer getPlayer(byte id)
        {
            return server.playerpool[id];
        }

        public ServerVehicleInfo createVehicle(int model, Vector3 position, Quaternion orientation, Vector3 velocity)
        {
            return server.vehicleController.create(model, position, orientation, velocity);
        }
    }
}
