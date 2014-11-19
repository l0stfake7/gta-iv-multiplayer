using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;
using GTA;
using System.Drawing;

namespace MIVClient
{
    public class ChatController
    {
        Client client;

        public Queue<string> chatconsole;
        public string currentTypedText;

        public ChatController(Client client)
        {
            this.client = client;
            currentTypedText = "";
            chatconsole = new Queue<string>();
            writeChat("Console loaded");
            writeChat("OK");
        }

        public void writeChat(string text)
        {
            chatconsole.Enqueue(text);
            while (chatconsole.Count > 8)
            {
                chatconsole.Dequeue();
            }
        }
        


    }
}
