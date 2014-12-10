using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace MIVClientGUI
{
    public partial class GameLaunchedForm : Form
    {
        public GameLaunchedForm()
        {
            InitializeComponent();
        }

        private void GameLaunchedForm_Load(object sender, EventArgs e)
        {
        }

        private void GameLaunchedForm_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            Process gameProcess = new Process();
            gameProcess.StartInfo = new ProcessStartInfo("LaunchGTAIV.exe");
            gameProcess.Start();
            gameProcess.WaitForExit();
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}