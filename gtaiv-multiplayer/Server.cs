using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MIVSDK;
using System.Timers;

namespace MIVServer
{
    public class Server
    {
        TcpListener server;
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
            timer.Interval = 60;
            timer.Enabled = true;
            timer.Start();
            Console.WriteLine("Started server on port " + port.ToString());
        }

        void onBroadcastTimer(object sender, ElapsedEventArgs e)
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
            try
            {
                if (player.position != null)
                {
                    broadcastNick(player);
                    UpdateDataStruct data = new UpdateDataStruct();
                    data.pos_x = player.position.X;
                    data.pos_y = player.position.Y;
                    data.pos_z = player.position.Z;
                    data.rot_x = player.orientation.X;
                    data.rot_y = player.orientation.Y;
                    data.rot_z = player.orientation.Z;
                    data.rot_a = player.orientation.W;
                    data.heading = player.heading;
                    data.ped_health = player.pedHealth;
                    data.speed = player.speed;
                    data.veh_health = player.vehicleHealth;
                    data.vehicle_model = player.vehicle_model;
                    data.vel_x = player.velocity.X;
                    data.vel_y = player.velocity.Y;
                    data.vel_z = player.velocity.Z;
                    foreach (var single in playerpool)
                    {
                        //if (single.Value.id != player.id)
                        {
                            single.Value.connection.streamWrite(Commands.UpdateData);
                            single.Value.connection.streamWrite(new byte[1] { (byte)player.id });
                            single.Value.connection.streamWrite(data);
                            single.Value.connection.streamFlush();
                            Console.WriteLine("Streaming to player " + single.Value.nick);
                        }
                    }
                }
            } catch (Exception e){ Console.WriteLine(e); }
        }
        public void broadcastNick(ServerPlayer player)
        {
            if (player.position != null)
            {
                foreach (var single in playerpool)
                {
                    if (single.Value.id != player.id)
                    {
                        single.Value.connection.streamWrite(Commands.InfoPlayerName);
                        single.Value.connection.streamWrite(new byte[1] { (byte)player.id });
                        single.Value.connection.streamWrite(player.nick.Length);
                        single.Value.connection.streamWrite(player.nick);
                        single.Value.connection.streamFlush();
                        //Console.WriteLine("Streaming nick TO " + single.Value.nick);
                    }
                }
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
                player.id = findLowestFreeId();
                player.nick = nick;
                playerpool.Add(player.id, player);
                api.invokeOnPlayerConnect(client.Client.RemoteEndPoint, player);
            };

            connection.startReceiving();
            
        }

    }
}
