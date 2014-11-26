using MIVSDK;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace MIVServer
{
    public class Server
    {
        public static Server instance;
        public ServerApi api;
        public ServerChat chat;
        public Dictionary<byte, ServerPlayer> playerpool;
        public ServerVehicleController vehicleController;

        private GamemodeManager gamemodeManager;
        private TcpListener server;

        public Server(int port)
        {
            chat = new ServerChat();
            instance = this;
            vehicleController = new ServerVehicleController();
            api = new ServerApi(this);
            gamemodeManager = new GamemodeManager(api);
            gamemodeManager.loadFromFile("DeathmatchGamemode.dll");
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            server.BeginAcceptTcpClient(onIncomingConnection, null);
            playerpool = new Dictionary<byte, ServerPlayer>();
            Timer timer = new Timer();
            timer.Elapsed += onBroadcastTimer;
            timer.Interval = 80;
            timer.Enabled = true;
            timer.Start();
            Console.WriteLine("Started server on port " + port.ToString());
        }

        public void broadcastData(ServerPlayer player)
        {
            if (player.data.client_has_been_set) return;
            else player.data.client_has_been_set = true;
            try
            {
                if (player.data != null)
                {
                    foreach (var single in playerpool)
                    {
                        if (single.Value.id != player.id)
                        {
                            var bpf = new BinaryPacketFormatter(Commands.UpdateData);
                            bpf.add(new byte[1] { (byte)player.id });
                            bpf.add(player.data);
                            single.Value.connection.write(bpf.getBytes());
                            //Console.WriteLine("Streaming to player " + single.Value.nick);
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public void broadcastNPCsToPlayer(ServerPlayer player)
        {
            foreach (var pair in ServerNPC.NPCPool)
            {
                var bpf = new BinaryPacketFormatter(Commands.NPC_create);
                bpf.add(pair.Value.id);
                bpf.add(pair.Value.position);
                bpf.add(pair.Value.heading);
                bpf.add(pair.Value.model);
                bpf.add(pair.Value.name);
                player.connection.write(bpf.getBytes());
            }
        }

        public void broadcastVehiclesToPlayer(ServerPlayer player)
        {
            foreach (var pair in vehicleController.vehicles)
            {
                var bpf = new BinaryPacketFormatter(Commands.Vehicle_create);
                bpf.add(pair.Value.id);
                bpf.add(pair.Value.position);
                bpf.add(pair.Value.orientation);
                bpf.add(pair.Value.velocity);
                bpf.add(pair.Value.model);
                player.connection.write(bpf.getBytes());
            }
        }

        private byte findLowestFreeId()
        {
            for (byte i = 0; i < byte.MaxValue; i++)
            {
                if (!playerpool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }

        private void onBroadcastTimer(object sender, ElapsedEventArgs e)
        {
            string currentMessage;
            while ((currentMessage = chat.dequeue()) != null)
            {
                foreach (ServerPlayer player in playerpool.Values)
                {
                    var bpf = new BinaryPacketFormatter(Commands.Chat_writeLine);
                    bpf.add(currentMessage);
                    player.connection.write(bpf.getBytes());
                }
            }

            foreach (ServerPlayer player in playerpool.Values)
            {
                broadcastData(player);
                player.connection.flush();
            }
        }

        private void onIncomingConnection(IAsyncResult iar)
        {
            Console.WriteLine("Connecting");
            TcpClient client = server.EndAcceptTcpClient(iar);

            server.BeginAcceptTcpClient(onIncomingConnection, null);

            ClientConnection connection = new ClientConnection(client);

            connection.onConnect += delegate(string nick)
            {
                Console.WriteLine("Connect from " + nick);
                ServerPlayer player = new ServerPlayer(nick, connection);
                connection.player = player;
                player.id = findLowestFreeId();
                player.nick = nick;
                playerpool.Add(player.id, player);
                broadcastVehiclesToPlayer(player);
                broadcastNPCsToPlayer(player);

                api.invokeOnPlayerConnect(client.Client.RemoteEndPoint, player);

                connection.flush();
            };

            connection.startReceiving();
        }
    }
}