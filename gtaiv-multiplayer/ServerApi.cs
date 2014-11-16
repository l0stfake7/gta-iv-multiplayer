using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace gtaiv_multiplayer
{
    public class ServerApi
    {
        Server server;
        public ServerApi(Server instance)
        {
            server = instance;
        }

        public delegate void onPlayerConnectDelegate(IPEndPoint address, Player player);
        public event onPlayerConnectDelegate onPlayerConnect;
        public void invokeOnPlayerConnect(IPEndPoint address, Player player)
        {
            if (onPlayerConnect != null) onPlayerConnect.Invoke(address, player);
        }

        public delegate void onPlayerDisconnectDelegate(Player player);
        public event onPlayerDisconnectDelegate onPlayerDisconnect;
        public void invokeOnPlayerDisconnect(Player player)
        {
            if (onPlayerDisconnect != null) onPlayerDisconnect.Invoke(player);
        }

        public Player getPlayer(byte id)
        {
            return server.playerpool[id];
        }
    }
}
