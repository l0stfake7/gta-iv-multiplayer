using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using multiplayer_sdk;
using System.Timers;

namespace gtaiv_multiplayer
{
    class Server
    {
        TcpListener server;
        Dictionary<byte, Player> playerpool;
        public static Server instance;


        public Server(int port)
        {
            instance = this;
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            server.BeginAcceptTcpClient(onIncomingConnection, null);
            playerpool = new Dictionary<byte, Player>();
            Timer timer = new Timer();
            timer.Elapsed += onBroadcastTimer;
            timer.Interval = 80;
            timer.Enabled = true;
            timer.Start();
            Console.WriteLine("Started server on port " + port.ToString());
        }

        void onBroadcastTimer(object sender, ElapsedEventArgs e)
        {
            foreach (Player player in playerpool.Values)
            {
                broadcastData(player);
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

        public void broadcastData(Player player)
        {
            if (player.position != null)
            {
                UpdateDataStruct data = new UpdateDataStruct();
                data.pos_x = player.position.x;
                data.pos_y = player.position.y;
                data.pos_z = player.position.z;
                data.rot_x = player.orientation.x;
                data.rot_y = player.orientation.y;
                data.rot_z = player.orientation.z;
                data.rot_a = player.orientation.a;
                data.heading = player.heading;
                data.ped_health = player.pedHealth;
                data.speed = player.speed;
                data.veh_health = player.vehicleHealth;
                data.vehicle_model = player.vehicle_model;
                data.vel_x = player.velocity.x;
                data.vel_y = player.velocity.y;
                data.vel_z = player.velocity.z;
                foreach (var single in playerpool)
                {
                    if (single.Value.id != player.id)
                    {
                        single.Value.connection.streamWrite(Commands.UpdateData);
                        single.Value.connection.streamWrite(new byte[1] { (byte)player.id });
                        single.Value.connection.streamWrite(data);
                        single.Value.connection.streamFlush();
                        Console.WriteLine("Streaming to player " + single.Value.nick);
                    }
                }
            }
        }
        public void broadcastNick(Player player)
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
                        Console.WriteLine("Streaming nick TO " + single.Value.nick);
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
                Player player = new Player(nick, connection);
                player.id = findLowestFreeId();
                player.nick = nick;
                playerpool.Add(player.id, player);
                broadcastNick(player);
            };

            connection.startReceiving();
            
        }

    }
}
