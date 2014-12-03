using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Net;
using System.Net.Sockets;
using MIVSDK;

namespace MIVClientGUI
{
    public partial class ServerBrowser : Form
    {
        public ServerBrowser()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        class ServerInfo
        {
            public string IP;
            public short Port;
            public short GamePort;
        }

        private List<ServerInfo> loadFromFile()
        {
            var servers = new List<ServerInfo>();
            string[] lines = File.ReadAllLines("servers.list");
            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    try
                    {
                        string[] split = line.Split(':');
                        servers.Add(new ServerInfo()
                        {
                            IP = split[0],
                            Port = short.Parse(split[1])
                        });
                    }
                    catch { }
                }
            }
            return servers;
        }

        private void ServerBrowser_Load(object sender, EventArgs e)
        {
            if (!File.Exists("servers.list"))
            {
                File.WriteAllText("servers.list", "");
            }
            var servers = loadFromFile();
            foreach (ServerInfo server in servers)
            {
                new Task(new Action(delegate
                {
                    try
                    {
                        var request = HttpWebRequest.CreateHttp("http://" + server.IP + ":" + server.Port.ToString() + "/get_server_data");
                        var response = (HttpWebResponse)request.GetResponse();
                        string ini = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        INIReader reader = new INIReader(ini.Split('\n'));
                        response.Close();
                        ListViewItem item = new ListViewItem(reader.getString("name") + " (" + server.IP + ":" + server.Port.ToString() + " " + reader.getString("players") + "/" + reader.getString("max_players") + ")");
                        server.GamePort = (short)reader.getInt("game_port");
                        item.Tag = server;
                        listView1.Items.Add(item);
                    }
                    catch
                    {
                        ListViewItem item = new ListViewItem("Offline (" + server.IP + ":" + server.Port.ToString() + ")");
                        listView1.Items.Add(item);
                    }
                })).Start();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AddServerForm().ShowDialog();
        }
    }
}
