// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Threading.Tasks;

namespace MIVServer
{
    public class Server
    {
        public static Server instance;
        public ServerApi api;
        public ServerChat chat;
        public INIReader config;
        public List<ServerPlayer> playerpool;
        public ServerVehicleController vehicleController;

        private GamemodeManager gamemodeManager;
        private HTTPServer http_server;
        private TcpListener server;
        public int UDPStartPort;

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
            UDPStartPort = config.getInt("udp_start_port");
            Timer timer_slow = new Timer();
            timer_slow.Elapsed += timer_slow_Elapsed;
            timer_slow.Interval = config.getInt("slow_interval");
            timer_slow.Enabled = true;
            timer_slow.Start();
            http_server = new HTTPServer();
            Console.WriteLine("Started game server on port " + config.getInt("game_port").ToString());
            Console.WriteLine("Started http server on port " + config.getInt("http_port").ToString());
        }

        public void broadcastData(ServerPlayer player)
        {
            if (player.data.client_has_been_set) return;
            else player.data.client_has_been_set = true;
            try
            {
                if (player.data != null)
                {
                    InvokeParallelForEachPlayer((single) =>
                    {
                        if (single.id != player.id)
                        {
                            var bpf = new BinaryPacketFormatter(Commands.UpdateData);
                            bpf.Add(player.id);
                            bpf.Add(player.data);
                            single.connection.write(bpf.getBytes());
                            //Console.WriteLine("Streaming to Player " + single.Value.nick);
                        }
                    });
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

        public void broadcastNPCsToPlayer(ServerPlayer player)
        {
            foreach (var pair in ServerNPC.NPCPool)
            {
                var bpf = new BinaryPacketFormatter(Commands.NPC_create);
                bpf.Add(pair.Value.id);
                bpf.Add(pair.Value.Position);
                bpf.Add(pair.Value.Heading);
                bpf.Add(pair.Value.Model);
                bpf.Add(pair.Value.Name);
                player.connection.write(bpf.getBytes());
            }
        }

        public void broadcastPlayerModel(ServerPlayer player)
        {
            InvokeParallelForEachPlayer((single) =>
            {
                if (single != player)
                {
                    var bpf = new BinaryPacketFormatter(Commands.Global_setPlayerModel);
                    bpf.Add(player.id);
                    bpf.Add(ModelDictionary.getPedModelByName(player.Model));
                    single.connection.write(bpf.getBytes());
                }
            });
        }

        public void broadcastPlayerName(ServerPlayer player)
        {
            InvokeParallelForEachPlayer((single) =>
            {
                if (single != player)
                {
                    var bpf = new BinaryPacketFormatter(Commands.Global_setPlayerName);
                    bpf.Add(player.id);
                    bpf.Add(player.Nick);
                    single.connection.write(bpf.getBytes());
                }
            });
        }

        public void broadcastVehiclesToPlayer(ServerPlayer player)
        {
            foreach (var pair in vehicleController.vehicles)
            {
                var bpf = new BinaryPacketFormatter(Commands.Vehicle_create);
                bpf.Add(pair.Value.id);
                bpf.Add(pair.Value.position);
                bpf.Add(pair.Value.orientation);
                bpf.Add(ModelDictionary.getVehicleByName(pair.Value.model));
                player.connection.write(bpf.getBytes());
            }
        }

        public ServerPlayer getPlayerById(uint id)
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

        public void updateNPCsToPlayer(ServerPlayer player)
        {
            foreach (var pair in ServerNPC.NPCPool)
            {
                var bpf = new BinaryPacketFormatter(Commands.NPC_update);
                bpf.Add(pair.Value.id);
                bpf.Add(pair.Value.Position);
                bpf.Add(pair.Value.Heading);
                bpf.Add(pair.Value.Model);
                bpf.Add(pair.Value.Name);
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
                string message = currentMessage;
                InvokeParallelForEachPlayer((player) =>
                {
                    var bpf = new BinaryPacketFormatter(Commands.Chat_writeLine);
                    bpf.Add(message);
                    player.connection.write(bpf.getBytes());
                });
            }

            InvokeParallelForEachPlayer((player) =>
            {
                player.connection.flush();
                player.udpTunnel.broadcastPlayers();
            });
        }

        public void InvokeParallelForEachPlayer(Action<ServerPlayer> action)
        {
            if (playerpool == null || playerpool.Count == 0) return;
            for (int i = 0; i < playerpool.Count; i++)
            {
                /*
                ServerPlayer Player = playerpool[i];
                new Task(() =>
                {
                    lock (Player)
                    {
                        action.Invoke(Player);
                    }
                }).Start();
                */
                action.Invoke(playerpool[i]);
            }
        }

        private void onIncomingConnection(IAsyncResult iar)
        {
            Console.WriteLine("Connecting");
            TcpClient client = server.EndAcceptTcpClient(iar);

            server.BeginAcceptTcpClient(onIncomingConnection, null);

            ClientConnection connection = new ClientConnection(client);

            connection.OnConnect += delegate(string nick)
            {
                if (playerpool.Count >= config.getInt("max_players"))
                {
                    Console.WriteLine("Connection from " + nick + " rejected due to Player limit");
                    client.Close();
                    return;
                }
                Console.WriteLine("Connect from " + nick);
                uint id = findLowestFreeId();
                ServerPlayer player = new ServerPlayer(id, nick, connection);
                connection.SetPlayer(player);
                InvokeParallelForEachPlayer((p) =>
                {
                    player.connection.write(
                        new BinaryPacketFormatter(Commands.Global_createPlayer, p.id, ModelDictionary.getPedModelByName(p.Model), p.Nick)
                        .getBytes());
                    p.connection.write(
                        new BinaryPacketFormatter(Commands.Global_createPlayer, player.id, ModelDictionary.getPedModelByName("M_Y_SWAT"), nick)
                        .getBytes());
                });
                player.Nick = nick;
                player.Model = "M_Y_SWAT";
                playerpool.Add(player);
                broadcastVehiclesToPlayer(player);
                broadcastNPCsToPlayer(player);
                //broadcastPlayerName(Player);
                //broadcastPlayerModel(Player);

                api.invokeOnPlayerConnect(client.Client.RemoteEndPoint, player);
                api.invokeOnPlayerSpawn(player);

                var starter = new BinaryPacketFormatter(Commands.Client_resumeBroadcast);
                connection.write(starter.getBytes());
                connection.write(new BinaryPacketFormatter(Commands.Client_enableUDPTunnel, player.udpTunnel.getPort()).getBytes());

                connection.flush();
            };

            connection.startReceiving();
        }

        private void timer_slow_Elapsed(object sender, ElapsedEventArgs e)
        {
            InvokeParallelForEachPlayer((player) =>
            {
                updateNPCsToPlayer(player);
            });
        }
    }
}