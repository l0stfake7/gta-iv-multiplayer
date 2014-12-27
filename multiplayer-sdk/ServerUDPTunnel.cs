// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MIVSDK;

namespace MIVServer
{
    public sealed class ServerUDPTunnel
    {
        UdpClient udp;
        ServerPlayer player;
        IPEndPoint endpoint;

        public ServerUDPTunnel(ServerPlayer player)
        {
            // enstablish an Connection
            this.player = player;
            udp = new UdpClient(new IPEndPoint(IPAddress.Any, Server.instance.UDPStartPort + (int)player.id));
            udp.BeginReceive(onReceive, null);
        }

        public int getPort()
        {
            return Server.instance.UDPStartPort + (int)player.id;
        }

        private void onReceive(IAsyncResult iar)
        {
            try
            {
                byte[] datagram = udp.EndReceive(iar, ref endpoint);
                udp.BeginReceive(onReceive, null);
                var bpr = new BinaryPacketReader(datagram, true);
                player.updateData(bpr.readUpdateStruct());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void broadcastPlayers()
        {
            try
            {
                if (player.data != null && endpoint != null)
                {
                    Server.instance.InvokeParallelForEachPlayer((single) =>
                    {
                        if (single.id != player.id)
                        {
                            var bpf = new BinaryPacketFormatter();
                            bpf.Add(single.id);
                            bpf.Add(single.data);
                            var bytes = bpf.getBytes();
                            udp.Send(bytes, bytes.Length, endpoint);
                        }
                    });
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }

    }
}
