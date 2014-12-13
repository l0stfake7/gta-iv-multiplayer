// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Windows.Forms;

namespace MIVClientGUI
{
    public partial class AddServerForm : Form
    {
        public AddServerForm()
        {
            InitializeComponent();
        }

        private void AddServerForm_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Net.IPAddress o;
            if (textBox2.TextLength > 0 && textBox3.TextLength > 0 && char.IsNumber(textBox3.Text, 0) && System.Net.IPAddress.TryParse(textBox2.Text, out o))
            {
                System.IO.File.AppendAllLines("servers.list", new string[1]{
                    textBox2.Text + ":" + textBox3.Text
                });
                ServerBrowser.refreshStatic();
                this.Close();
            }
        }
    }
}