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
            timer.Interval = 1000;
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
                data.rot_x = 0.0f;
                data.rot_y = player.orientation.y;
                data.rot_z = 0.0f;
                data.rot_a = 0.0f;
                foreach (var single in playerpool)
                {
                    if (single.Value.id != player.id)
                    {
                        single.Value.connection.streamWrite(Commands.UpdateData);
                        single.Value.connection.streamWrite((byte)player.id);
                        single.Value.connection.streamWrite(data);
                        Console.WriteLine("Streaming to player " + single.Value.nick);
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
            };

            connection.startReceiving();
            
        }

    }
}
