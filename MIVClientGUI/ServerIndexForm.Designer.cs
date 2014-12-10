namespace MIVClientGUI
{
    partial class ServerBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerBrowser));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.clientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runGameWithoutClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conectManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resolutionSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearServerListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMIVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ipColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.portsColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.playerCountColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(922, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // clientToolStripMenuItem
            // 
            this.clientToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runGameWithoutClientToolStripMenuItem,
            this.conectManuallyToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.clientToolStripMenuItem.Name = "clientToolStripMenuItem";
            this.clientToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.clientToolStripMenuItem.Text = "Client";
            // 
            // runGameWithoutClientToolStripMenuItem
            // 
            this.runGameWithoutClientToolStripMenuItem.Name = "runGameWithoutClientToolStripMenuItem";
            this.runGameWithoutClientToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.runGameWithoutClientToolStripMenuItem.Text = "Run game without client";
            this.runGameWithoutClientToolStripMenuItem.Click += new System.EventHandler(this.runGameWithoutClientToolStripMenuItem_Click);
            // 
            // conectManuallyToolStripMenuItem
            // 
            this.conectManuallyToolStripMenuItem.Name = "conectManuallyToolStripMenuItem";
            this.conectManuallyToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.conectManuallyToolStripMenuItem.Text = "Connect manually";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteConsoleToolStripMenuItem,
            this.serverMonitorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // remoteConsoleToolStripMenuItem
            // 
            this.remoteConsoleToolStripMenuItem.Name = "remoteConsoleToolStripMenuItem";
            this.remoteConsoleToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.remoteConsoleToolStripMenuItem.Text = "Remote console";
            // 
            // serverMonitorToolStripMenuItem
            // 
            this.serverMonitorToolStripMenuItem.Name = "serverMonitorToolStripMenuItem";
            this.serverMonitorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.serverMonitorToolStripMenuItem.Text = "Server monitor";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolutionSettingsToolStripMenuItem,
            this.clearServerListToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // resolutionSettingsToolStripMenuItem
            // 
            this.resolutionSettingsToolStripMenuItem.Name = "resolutionSettingsToolStripMenuItem";
            this.resolutionSettingsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.resolutionSettingsToolStripMenuItem.Text = "Resolution settings";
            // 
            // clearServerListToolStripMenuItem
            // 
            this.clearServerListToolStripMenuItem.Name = "clearServerListToolStripMenuItem";
            this.clearServerListToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.clearServerListToolStripMenuItem.Text = "Clear server list";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTutorialToolStripMenuItem,
            this.aboutMIVToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // openTutorialToolStripMenuItem
            // 
            this.openTutorialToolStripMenuItem.Name = "openTutorialToolStripMenuItem";
            this.openTutorialToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.openTutorialToolStripMenuItem.Text = "Open tutorial";
            // 
            // aboutMIVToolStripMenuItem
            // 
            this.aboutMIVToolStripMenuItem.Name = "aboutMIVToolStripMenuItem";
            this.aboutMIVToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.aboutMIVToolStripMenuItem.Text = "About MIV";
            // 
            // listView1
            // 
            this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listView1.AutoArrange = false;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.ipColumn,
            this.portsColumn,
            this.playerCountColumn});
            this.listView1.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(12, 56);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(898, 539);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            this.listView1.Leave += new System.EventHandler(this.listView1_Leave);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Name";
            this.nameColumn.Width = 363;
            // 
            // ipColumn
            // 
            this.ipColumn.Text = "IP Address";
            this.ipColumn.Width = 157;
            // 
            // portsColumn
            // 
            this.portsColumn.Text = "Ports";
            this.portsColumn.Width = 125;
            // 
            // playerCountColumn
            // 
            this.playerCountColumn.Text = "Player count";
            this.playerCountColumn.Width = 178;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(12, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(174, 27);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Remove";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(340, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Nickname:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(404, 29);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(121, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "Player";
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(255, 26);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "Refresh";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // ServerBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 607);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ServerBrowser";
            this.Text = "MIV Server Browser";
            this.Load += new System.EventHandler(this.ServerBrowser_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runGameWithoutClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverMonitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resolutionSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearServerListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTutorialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMIVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conectManuallyToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ColumnHeader nameColumn;
        private System.Windows.Forms.ColumnHeader ipColumn;
        private System.Windows.Forms.ColumnHeader portsColumn;
        private System.Windows.Forms.ColumnHeader playerCountColumn;
    }
}

