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

namespace Setup
{
    public partial class Form1 : Form
    {
        public static Form1 instance;
        public Form1()
        {
            instance = this;
            InitializeComponent();
        }

        public static void writeLog(string text)
        {
            instance.textBox2.AppendText(text);
            instance.textBox2.ScrollToCaret();
        }
        public static void writeLogLine(string text)
        {
            instance.textBox2.AppendText(text + "\r\n");
            instance.textBox2.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs ev)
        {
            try
            {
                writeLogLine("Starting installation...");
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                writeLogLine("Checking path...");
                if (textBox1.Text.Length == 0) throw new ArgumentException("Selected install path is empty");
                string path = Directory.Exists(textBox1.Text) ? textBox1.Text : Path.GetDirectoryName(textBox1.Text);
                writeLogLine("Checking " + path);
                if (!Directory.Exists(path)) throw new ArgumentException("Selected install path does not exist");
                if (!File.Exists(path + "\\LaunchGTAIV.exe") || !File.Exists(path + "\\GTAIV.exe")) throw new InvalidDataException("Selected path does not seem to contain valid GTA IV installation");
                writeLogLine("Path seems valid!");
                writeLogLine("Starting download...");
                WebClient client = new WebClient();
                string tmp7zfile = Path.GetTempFileName() + ".exe";
                string tmparchfile = Path.GetTempFileName() + ".7z";
                writeLogLine("Downloading 7z unpacker...");
                Application.DoEvents();
                client.DownloadFile("http://gta.vdgtech.eu/install_7z.exe", tmp7zfile);
                writeLogLine("Downloading release archive...");
                Application.DoEvents();
                client.DownloadFile("http://gta.vdgtech.eu/install_release_current.7z", tmparchfile);

                writeLogLine("Unpacking release");
                Application.DoEvents();
                Process unpacker = new Process();
                unpacker.StartInfo = new ProcessStartInfo(tmp7zfile, "x -y -o\"" + path + "\" \"" + tmparchfile + "\"");
                unpacker.StartInfo.UseShellExecute = false;
                unpacker.StartInfo.CreateNoWindow = true;
                unpacker.StartInfo.RedirectStandardOutput = true;
                unpacker.StartInfo.RedirectStandardError = true;
                unpacker.Start();
                var stdout = unpacker.StandardOutput;
                while (!stdout.EndOfStream)
                {
                    char[] block = new char[32];
                    int count = stdout.ReadBlock(block, 0, 32);

                    string line = String.Join<char>("", block);
                    writeLog(line);
                    Application.DoEvents();
                }
                writeLogLine("Finished!");
                var result = MessageBox.Show("Installation has finished. Would you like to run MIV client now?", "Success and triumph!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(path + "\\MIVClientGUI.exe")
                    {
                        WorkingDirectory = path
                    });
                }
                Application.Exit();
            }
            catch (Exception e)
            {
                writeLogLine("FATAL ERROR: " + e.GetType().ToString() + ": " + e.Message);
                writeLogLine("Installation stopped due to fatal error.");
            }
            finally
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileOk += (o, ev) =>
            {
                if (!ev.Cancel && openFileDialog1.FileName.Length > 0)
                {
                    textBox1.Text = Path.GetDirectoryName(openFileDialog1.FileName);
                }
            };
            openFileDialog1.ShowDialog();
        }
    }
}
