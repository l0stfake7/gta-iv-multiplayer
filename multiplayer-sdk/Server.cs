using MIVSDK;
using System;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Timers;

namespace MIVServer
{
    public class Server
    {
        public static Server instance;
        public ServerApi api;
        public ServerChat chat;
        public List<ServerPlayer> playerpool;
        public ServerVehicleController vehicleController;

        private GamemodeManager gamemodeManager;
        private TcpListener server;
        public INIReader config;
        private HTTPServer http_server;

        public Server()
        {
            config = new INIReader(System.IO.File.ReadAllLines("config.ini"));
            chat = new ServerChat();
            instance = this;
            vehicleController = new ServerVehicleController();
            api = new ServerApi(this);
            gamemodeManager = new GamemodeManager(api);
            gamemodeManager.loadFromFile("gamemodes/" + config.getString("gamemode"));
            server = new TcpListener(IPAddress.Any, config.getInt("game_port"));
            server.Start();
            server.BeginAcceptTcpClient(onIncomingConnection, null);
            playerpool = new List<ServerPlayer>();
            Timer timer = new Timer();
            timer.Elapsed += onBroadcastTimer;
            timer.Interval = config.getInt("broadcast_interval");
            timer.Enabled = true;
            timer.Start();
            Timer timer_slow = new Timer();
            timer_slow.Elapsed += timer_slow_Elapsed;
            timer_slow.Interval = config.getInt("slow_interval");
            timer_slow.Enabled = true;
            timer_slow.Start();
            http_server = new HTTPServer();
            Console.WriteLine("Started game server on port " + config.getInt("game_port").ToString());
            Console.WriteLine("Started http server on port " + config.getInt("http_port").ToString());
        }

        void timer_slow_Elapsed(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < playerpool.Count; i++)
            {
                updateNPCsToPlayer(playerpool[i]);
            }
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
                        if (single.id != player.id)
                        {
                            var bpf = new BinaryPacketFormatter(Commands.UpdateData);
                            bpf.add(new byte[1] { (byte)player.id });
                            bpf.add(player.data);
                            single.connection.write(bpf.getBytes());
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
                bpf.add(pair.Value.Position);
                bpf.add(pair.Value.Heading);
                bpf.add(pair.Value.Model);
                bpf.add(pair.Value.Name);
                player.connection.write(bpf.getBytes());
            }
        }
        public void broadcastPlayerName(ServerPlayer player)
        {
            foreach (var single in playerpool) if (single != player)
                {
                    var bpf = new BinaryPacketFormatter(Commands.Global_setPlayerName);
                    bpf.add(new byte[1] { player.id });
                    bpf.add(player.Nick);
                    single.connection.write(bpf.getBytes());
                }
        }
        public void broadcastPlayerModel(ServerPlayer player)
        {
            foreach (var single in playerpool) if (single != player)
                {
                    var bpf = new BinaryPacketFormatter(Commands.Global_setPlayerModel);
                    bpf.add(new byte[1]{player.id});
                    bpf.add(ModelDictionary.getPedModelByName(player.Model));
                    single.connection.write(bpf.getBytes());
                }
        }
        public void updateNPCsToPlayer(ServerPlayer player)
        {
            foreach (var pair in ServerNPC.NPCPool)
            {
                var bpf = new BinaryPacketFormatter(Commands.NPC_update);
                bpf.add(pair.Value.id);
                bpf.add(pair.Value.Position);
                bpf.add(pair.Value.Heading);
                bpf.add(pair.Value.Model);
                bpf.add(pair.Value.Name);
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
                if (playerpool.Count(a => a.id == i) == 0) return i;
            }
            throw new Exception("No free ids");
        }

        private void onBroadcastTimer(object sender, ElapsedEventArgs e)
        {
            string currentMessage;
            while ((currentMessage = chat.dequeue()) != null)
            {
                foreach (ServerPlayer player in playerpool)
                {
                    var bpf = new BinaryPacketFormatter(Commands.Chat_writeLine);
                    bpf.add(currentMessage);
                    player.connection.write(bpf.getBytes());
                }
            }

            for (int i = 0; i < playerpool.Count; i++)
            {
                broadcastData(playerpool[i]);
                playerpool[i].connection.flush();
            }
        }

        public ServerPlayer getPlayerById(byte id)
        {
            try
            {
                return playerpool.First(a => a.id == id);
            }
            catch
            {
                return null;
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
                if (playerpool.Count >= config.getInt("max_players"))
                {
                    Console.WriteLine("Connection from " + nick + " rejected due to player limit");
                    client.Close();
                    return;
                }
                Console.WriteLine("Connect from " + nick);
                ServerPlayer player = new ServerPlayer(nick, connection);
                connection.player = player;
                player.id = findLowestFreeId();
                player.Nick = nick;
                player.Model = "M_M_CHINATOWN_01";
                playerpool.Add(player);
                broadcastVehiclesToPlayer(player);
                broadcastNPCsToPlayer(player);
                //broadcastPlayerName(player);
                //broadcastPlayerModel(player);

                api.invokeOnPlayerConnect(client.Client.RemoteEndPoint, player);
                api.invokeOnPlayerSpawn(player);

                connection.flush();
            };

            connection.startReceiving();
        }
    }
}