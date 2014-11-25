using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MIVServer
{
    public class TextView
    {
        public uint id;
        public string text;
        public float size;
        public Point position;
        ServerPlayer player;
        
        private static Dictionary<uint, TextView> textviewspool
        {
            get
            {
                if (textviewspool == null) textviewspool = new Dictionary<uint, TextView>();
                return textviewspool;
            }
            set
            {
                textviewspool = value;
            }
        }

        private uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!textviewspool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }

        public TextView(ServerPlayer player, string text, float fontsize, Point position)
        {
            if (textviewspool == null) textviewspool = new Dictionary<uint, TextView>();
            this.text = text;
            this.size = fontsize;
            this.position = position;
            this.player = player;
            id = findLowestFreeId();
            textviewspool.Add(id, this);
            player.connection.streamWrite(MIVSDK.Commands.TextView_create);
            player.connection.streamWrite(BitConverter.GetBytes(id));
            player.connection.streamWrite(BitConverter.GetBytes(size));
            player.connection.streamWrite(BitConverter.GetBytes(position.X));
            player.connection.streamWrite(BitConverter.GetBytes(position.Y));
            player.connection.streamWrite(MIVSDK.Serializers.serialize(text));
            player.connection.streamFlush();
        }

        public void destroy()
        {
            textviewspool.Remove(id);
            player.connection.streamWrite(MIVSDK.Commands.TextView_destroy);
            player.connection.streamWrite(BitConverter.GetBytes(id));
            player.connection.streamFlush();
        }

        public void update()
        {
            player.connection.streamWrite(MIVSDK.Commands.TextView_update);
            player.connection.streamWrite(BitConverter.GetBytes(id));
            player.connection.streamWrite(BitConverter.GetBytes(size));
            player.connection.streamWrite(BitConverter.GetBytes(position.X));
            player.connection.streamWrite(BitConverter.GetBytes(position.Y));
            player.connection.streamWrite(MIVSDK.Serializers.serialize(text));
            player.connection.streamFlush();
        }
    }
}
