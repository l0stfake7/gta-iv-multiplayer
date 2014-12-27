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
using GTA;

namespace MIVClient
{
    public class ClientUDPTunnel
    {
        UdpClient udp;

        public ClientUDPTunnel(int port)
        {
            udp = new UdpClient(Client.currentIP, port);
            udp.BeginReceive(onReceive, null);
        }

        private void onReceive(IAsyncResult iar)
        {
            try
            {
                IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] datagram = udp.EndReceive(iar, ref endpoint);
                udp.BeginReceive(onReceive, null);
                var bpr = new BinaryPacketReader(datagram, true);
                uint playerid = bpr.readUInt32();
                var data = bpr.readUpdateStruct();
                if (!Client.instance.serverConnection.playersdata.ContainsKey(playerid)) Client.instance.serverConnection.playersdata.Add(playerid, data);
                else Client.instance.serverConnection.playersdata[playerid] = data;
            }
            catch (Exception e)
            {
                Game.Console.Print(e.Message);
            }
        }

        public void broadcastData(UpdateDataStruct data)
        {
            if (Client.instance.BroadcastingPaused) return;
            try
            {
                var bpf = new BinaryPacketFormatter();
                bpf.Add(data);
                var bytes = bpf.getBytes();
                udp.Send(bytes, bytes.Length);
            }
            catch (Exception e) { Game.Log(e.Message); }
        }
    }
}
