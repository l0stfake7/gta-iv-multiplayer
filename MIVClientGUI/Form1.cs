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
            public string Name, IP;
            public short Port;
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
                            Name = split[0],
                            IP = split[1],
                            Port = short.Parse(split[2])
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
                listView1.Items.Add(server.Name + " (" + server.IP + ":" + server.Port.ToString() + ")");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AddServerForm().ShowDialog();
        }
    }
}
