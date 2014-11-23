using System.Collections.Generic;

namespace MIVServer
{
    public class ServerChat
    {
        private Queue<string> broadcastQueue;

        public ServerChat()
        {
            broadcastQueue = new Queue<string>();
        }

        public void addLine(string line)
        {
            broadcastQueue.Enqueue(line);
        }

        public string dequeue()
        {
            if (broadcastQueue.Count == 0) return null;
            return broadcastQueue.Dequeue();
        }

        public void clear()
        {
            broadcastQueue = new Queue<string>();
        }
    }
}