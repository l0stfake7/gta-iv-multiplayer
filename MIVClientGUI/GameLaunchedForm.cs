using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace MIVClientGUI
{
    public partial class GameLaunchedForm : Form
    {
        public GameLaunchedForm()
        {

            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

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
    }
}
