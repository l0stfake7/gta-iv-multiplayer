using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Forms;
using System.Drawing;

namespace MIVClient
{
    class ChatInputForm : Form
    {
        public static bool isActive = false;
        Textbox textbox;
        public ChatInputForm()
        {
            isActive = true;
            this.Location = new Point(10, 300);
            this.StartPosition = FormStartPosition.Fixed;
            this.TitleSize = 1;
            this.TitleBackColor = Color.Transparent;
            textbox = new Textbox();
            this.Controls.Add(textbox);
            this.KeyDown += ChatInputForm_KeyDown;
        }

        void ChatInputForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Forms.Keys.Enter)
            {

                this.Close();
                isActive = false;
            }
        }

        public static void show()
        {
            if (!isActive)
            {
                var i = new ChatInputForm();
                i.Show();
            }
            
        }
    }
}
