// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MIVServer
{
    internal class HTTPServer
    {
        private TcpListener http_listener;

        public HTTPServer()
        {
            http_listener = new TcpListener(IPAddress.Any, Server.instance.config.getInt("http_port"));
            http_listener.Start();
            http_listener.BeginAcceptTcpClient(onConnect, null);
        }

        private string createResponse(string rawline)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(GET )(/)([^ ]+)");
            var result = regex.Match(rawline);
            if (result.Success && result.Groups.Count == 4)
            {
                string command = result.Groups[3].Value;
                string headers = "HTTP/1.1 200 OK\r\n" +
                "Content-Type: text/plain\r\n" +
                "Accept-Ranges: bytes\r\n" +
                "Vary: Accept-Encoding\r\n";
                if (command == "get_server_data")
                {
                    string response = "name=" + Server.instance.config.getString("server_name") + "\nmax_players=" + Server.instance.config.getString("max_players") + "\ngame_port=" + Server.instance.config.getString("game_port") + "\nplayers=" +
                        Server.instance.playerpool.Count;

                    return headers + "Content-Length: " + response.Length + "\r\n\r\n" + response + "\r\n\r\n";
                }
            }
            else
            {
                string headers = "HTTP/1.1 200 OK\r\n" +
                "Content-Type: text/HTML\r\n" +
                "Accept-Ranges: bytes\r\n" +
                "Vary: Accept-Encoding\r\n" +
                "refresh:5;url=http://gta.vdgtech.eu\r\n\r\nThat command is invalid. Redirecting to gta.vdgtech.eu in 5 seconds.\r\n\r\n";
                return headers;
            }
            return null;
        }

        private void onConnect(IAsyncResult iar)
        {
            var client = http_listener.EndAcceptTcpClient(iar);
            try
            {
                using (var streamReader = new StreamReader(client.GetStream()))
                {
                    string request = streamReader.ReadLine();
                    if (request != null)
                    {
                        string response = createResponse(request);
                        if (response != null)
                        {
                            byte[] buf = Encoding.UTF8.GetBytes(response);
                            streamReader.BaseStream.Write(buf, 0, buf.Length);
                        }
                    }
                }
            }
            catch { }
            http_listener.BeginAcceptTcpClient(onConnect, null);
        }
    }
}