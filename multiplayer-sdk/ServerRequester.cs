using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;
using MIVSDK.Math;

namespace MIVServer
{
    class Request
    {
        Commands action;
        Action<object[]> onComplete;
        static Dictionary<uint, Request> requestPool;
        private static uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!requestPool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }
        public Request(ClientConnection connection, Commands action, Action<object[]> onComplete)
        {
            if (requestPool == null) requestPool = new Dictionary<uint, Request>();
            this.action = action;
            this.onComplete = onComplete;
            uint id = findLowestFreeId();
            requestPool.Add(id, this);
            var bpf = new BinaryPacketFormatter(action);
            bpf.add(id);
            connection.write(bpf.getBytes());
        }

        public static void dispatch(uint id, params object[] objects)
        {
            requestPool[id].onComplete.Invoke(objects);
            requestPool.Remove(id);
        }
    }
    public class ServerRequester
    {
        ServerPlayer player;
        public ServerRequester(ServerPlayer player)
        {
            this.player = player;
        }

        public void getSelectedPlayer(Action<ServerPlayer> onComplete)
        {
            var req = new Request(player.connection, Commands.Request_getSelectedPlayer, (o) =>
            {
                onComplete.Invoke(Server.instance.api.getPlayer((byte)o[0]));
            });
        }
        public void getCameraPosition(Action<Vector3> onComplete)
        {
            var req = new Request(player.connection, Commands.Request_getCameraPosition, (o) =>
            {
                onComplete.Invoke((Vector3)o[0]);
            });
        }

    }
}
