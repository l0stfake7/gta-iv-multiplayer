using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;
using SharpDX;

namespace MIVServer
{
    class Request
    {
        Commands action;
        Action<object[]> onComplete;
        static Dictionary<uint, Request> requestPool;
        public BinaryPacketFormatter bpf;
        private ClientConnection connection;
        private static uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!requestPool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }
        private void InitInstance(ClientConnection connection, Commands action, Action<object[]> onComplete){
            
            if (requestPool == null) requestPool = new Dictionary<uint, Request>();
            this.action = action;
            this.connection = connection;
            this.onComplete = onComplete;
            uint id = findLowestFreeId();
            requestPool.Add(id, this);
            bpf = new BinaryPacketFormatter(action);
            bpf.add(id);
        }
        public Request(ClientConnection connection, Commands action, Action<object[]> onComplete)
        {
            InitInstance(connection, action, onComplete);
        }

        public void Flush(){

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
        public void isObjectVisible(Vector3 position, Action<bool> onComplete)
        {
            var req = new Request(player.connection, Commands.Request_isObjectVisible, (o) =>
            {
                onComplete.Invoke((bool)o[0]);
            });
            req.bpf.add(position);
            req.Flush();
        }
        public void worldToScreenProject(Vector3 position, Action<Vector2> onComplete)
        {
            var req = new Request(player.connection, Commands.Request_worldToScreen, (o) =>
            {
                onComplete.Invoke((Vector2)o[0]);
            });
            req.bpf.add(position);
            req.Flush();
        }

        public void getSelectedPlayer(Action<ServerPlayer> onComplete)
        {
            var req = new Request(player.connection, Commands.Request_getSelectedPlayer, (o) =>
            {
                onComplete.Invoke(Server.instance.api.getPlayer((byte)o[0]));
            });
            req.Flush();
        }
        public void getCameraPosition(Action<Vector3> onComplete)
        {
            var req = new Request(player.connection, Commands.Request_getCameraPosition, (o) =>
            {
                onComplete.Invoke((Vector3)o[0]);
            });
            req.Flush();
        }

    }
}
