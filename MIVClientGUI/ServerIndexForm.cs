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
            loadConfiguration();
        }

        public static void refreshStatic()
        {
            instance.refreshList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveConfiguration();
            ServerInfo server = listView1.SelectedItems[0].Tag as ServerInfo;
            if (server != null && server.GamePort > 1)
            {
                string ini = "timestamp=" + System.Diagnostics.Stopwatch.GetTimestamp().ToString() + "\r\n";
                ini += "ip=" + server.IP + "\r\n";
                ini += "port=" + server.GamePort + "\r\n";
                ini += "nickname=" + textBox1.Text + "\r\n";
                File.WriteAllText("_serverinit.ini", ini);
                new GameLaunchedForm().ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new AddServerForm().ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.SelectedItems[0].Remove();
            System.IO.File.Delete("servers.list");
            foreach (ListViewItem server in listView1.Items)
            {
                var info = (ServerInfo)server.Tag;
                System.IO.File.AppendAllLines("servers.list", new string[1]{
                    info.IP + ":" + info.Port
                });
            }
            refreshList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            refreshList();
        }

        private ListViewItem createListItem(string servername, string ip, string ports, string playercount)
        {
            ListViewItem item = new ListViewItem(new string[4]{
                servername, ip, ports, playercount
            });
            //item.Font = new System.Drawing.Font(new FontFamily("Segoe UI"), 16.0f, FontStyle.Regular);
            return item;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

        private void loadConfiguration()
        {
            if (System.IO.File.Exists("miv_client_config.ini"))
            {
                var ini = new INIReader(System.IO.File.ReadAllLines("miv_client_config.ini"));
                textBox1.Text = ini.getString("nickname");
            }
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

        private void refreshList()
        {
            if (!File.Exists("servers.list"))
            {
                File.WriteAllText("servers.list", "");
            }
            listView1.Items.Clear();
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
                        server.GamePort = reader.getInt16("game_port");
                        ListViewItem item = createListItem(reader.getString("name"), server.IP, server.Port.ToString() + "." + server.GamePort.ToString(), reader.getString("players") + "/" + reader.getString("max_players"));
                        item.Tag = server;
                        listView1.Items.Add(item);
                    }
                    catch
                    {
                        ListViewItem item = createListItem("Offline", server.IP, server.Port.ToString() + "." + server.GamePort.ToString(), "-");
                        listView1.Items.Add(item);
                    }
                })).Start();
            }
        }

        private void runGameWithoutClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileSystemOverlay.prepareForSP();
            Process gameProcess = new Process();
            gameProcess.StartInfo = new ProcessStartInfo("LaunchGTAIV.exe");
            gameProcess.Start();
        }

        private void saveConfiguration()
        {
            string ini = "nickname=" + textBox1.Text;
            System.IO.File.WriteAllText("miv_client_config.ini", ini);
        }

        private void ServerBrowser_Load(object sender, EventArgs e)
        {

            refreshList();
            FileSystemOverlay.prepareForSP();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            saveConfiguration();
        }

        class ServerInfo
        {
            public short GamePort;
            public string IP;
            public short Port;
        }
    }
}
