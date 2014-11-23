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
        private TcpListener server;
        public Dictionary<byte, ServerPlayer> playerpool;
        public static Server instance;
        public ServerApi api;
        public ServerVehicleController vehicleController;
        private GamemodeManager gamemodeManager;
        public ServerChat chat;

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

        private void onBroadcastTimer(object sender, ElapsedEventArgs e)
        {
            foreach (ServerPlayer player in playerpool.Values)
            {
                broadcastData(player);
            }
            string currentMessage;
            while ((currentMessage = chat.dequeue()) != null)
            {
                foreach (ServerPlayer player in playerpool.Values)
                {
                    player.connection.streamWrite(Commands.Chat_writeLine);
                    player.connection.streamWrite(currentMessage.Length);
                    player.connection.streamWrite(currentMessage);
                    player.connection.streamFlush();
                }
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
                            single.Value.connection.streamWrite(Commands.UpdateData);
                            single.Value.connection.streamWrite(new byte[1] { (byte)player.id });
                            single.Value.connection.streamWrite(player.data);
                            single.Value.connection.streamFlush();
                            //Console.WriteLine("Streaming to player " + single.Value.nick);
                        }
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public void broadcastVehiclesToPlayer(ServerPlayer player)
        {
            player.connection.streamWrite(Commands.Vehicle_create_multi);
            player.connection.streamWrite(BitConverter.GetBytes(vehicleController.vehicles.Count));
            foreach (var pair in vehicleController.vehicles)
            {
                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.id));

                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.position.X));
                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.position.Y));
                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.position.Z));

                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.orientation.X));
                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.orientation.Y));
                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.orientation.Z));
                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.orientation.W));

                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.velocity.X));
                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.velocity.Y));
                player.connection.streamWrite(BitConverter.GetBytes(pair.Value.velocity.Z));
                player.connection.streamWrite(Serializers.serialize(pair.Value.model));
                //Console.WriteLine("sent vehicle " + pair.Value.id);
            }
            player.connection.streamFlush();
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
                player.id = findLowestFreeId();
                player.nick = nick;
                playerpool.Add(player.id, player);
                broadcastVehiclesToPlayer(player);
                api.invokeOnPlayerConnect(client.Client.RemoteEndPoint, player);
            };

            connection.startReceiving();
        }
    }
}