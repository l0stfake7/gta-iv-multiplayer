using MIVSDK;
using SharpDX;
using System;
using System.Collections.Generic;

namespace MIVServer
{
    public class ServerRequester
    {
        private ServerPlayer player;

        public ServerRequester(ServerPlayer player)
        {
            this.player = player;
        }

        public void getCameraPosition(Action<Vector3> onComplete)
        {
            var req = new Request(player.connection, Commands.Request_getCameraPosition, (o) =>
            {
                onComplete.Invoke((Vector3)o[0]);
            });
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
    }

    internal class Request
    {
        public BinaryPacketFormatter bpf;
        private static Dictionary<uint, Request> requestPool;
        private Commands action;
        private ClientConnection connection;
        private Action<object[]> onComplete;

        public Request(ClientConnection connection, Commands action, Action<object[]> onComplete)
        {
            InitInstance(connection, action, onComplete);
        }

        public static void dispatch(uint id, params object[] objects)
        {
            requestPool[id].onComplete.Invoke(objects);
            requestPool.Remove(id);
        }

        public void Flush()
        {
            connection.write(bpf.getBytes());
        }

        private static uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!requestPool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }

        private void InitInstance(ClientConnection connection, Commands action, Action<object[]> onComplete)
        {
            if (requestPool == null) requestPool = new Dictionary<uint, Request>();
            this.action = action;
            this.connection = connection;
            this.onComplete = onComplete;
            uint id = findLowestFreeId();
            requestPool.Add(id, this);
            bpf = new BinaryPacketFormatter(action);
            bpf.add(id);
        }
    }
}