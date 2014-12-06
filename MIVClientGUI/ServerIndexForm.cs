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
using System.Diagnostics;
using MIVSDK;

namespace MIVClientGUI
{
    public partial class ServerBrowser : Form
    {
        private static ServerBrowser instance;
        public ServerBrowser()
        {
            instance = this;
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

            refreshList();
        }

        public static void refreshStatic()
        {
            instance.refreshList();
        }

        private void refreshList()
        {
            if (!File.Exists("servers.list"))
            {
                File.WriteAllText("servers.list", "");
            }
            listView1.Clear();
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
                        server.GamePort = (short)reader.getInt32("game_port");
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

        private void saveConfiguration()
        {
            string ini = "nickname=" + textBox1.Text;
            System.IO.File.WriteAllText("miv_client_config.ini", ini);
        }

        private void loadConfiguration()
        {
            if (System.IO.File.Exists("miv_client_config.ini"))
            {
                var ini = new INIReader(System.IO.File.ReadAllLines("miv_client_config.ini"));
                textBox1.Text = ini.getString("nickname");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ServerInfo server = listView1.SelectedItems[0].Tag as ServerInfo;
            if (server != null && server.GamePort > 1)
            {
                string ini = "timestamp=" + System.Diagnostics.Stopwatch.GetTimestamp().ToString() + "\r\n";
                ini += "ip=" + server.IP + "\r\n";
                ini += "port=" + server.GamePort + "\r\n";
                ini += "nickname=" + textBox1.Text + "\r\n";
                File.WriteAllText("_serverinit.ini", ini);
                Process gameProcess = new Process();
                gameProcess.StartInfo = new ProcessStartInfo("LaunchGTAIV.exe");
                gameProcess.Start();
                gameProcess.WaitForExit();
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            saveConfiguration();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            refreshList();
        }

        private void listView1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                button1.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void listView1_Leave(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                button1.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button3.Enabled = false;
            }
        }
    }
}
