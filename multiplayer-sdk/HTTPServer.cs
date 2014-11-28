using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace MIVServer
{
    class HTTPServer
    {
        TcpListener http_listener;

        public HTTPServer()
        {
            http_listener = new TcpListener(IPAddress.Any, Server.instance.config.getInt("http_port"));
            http_listener.Start();
            http_listener.BeginAcceptTcpClient(onConnect, null);
        }

        void onConnect(IAsyncResult iar)
        {
            var client = http_listener.EndAcceptTcpClient(iar);
            var streamReader = new StreamReader(client.GetStream());
            string request = streamReader.ReadLine();
            string response = createResponse(request);
            if (response != null)
            {
                byte[] buf = Encoding.UTF8.GetBytes(response);
                streamReader.BaseStream.Write(buf, 0, buf.Length);
            }
            streamReader.Close();
            Console.WriteLine(request);
            http_listener.BeginAcceptTcpClient(onConnect, null);
        }

        string createResponse(string rawline)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(GET )(/)([^ ]+)");
            var result = regex.Match(rawline);
            if (result.Success && result.Groups.Count == 4)
            {
                string command = result.Groups[3].Value;
                if (command == "get_server_data")
                {
                    return "{name:\"" + Server.instance.config.getString("server_name") + "\",max_players:" + Server.instance.config.getString("max_players") + ",players:" +
                        Server.instance.playerpool.Count + "}";
                }
            }
            return null;
        }

    }
}
