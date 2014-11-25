using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIVServer
{
    public class ServerNPCDialog
    {
        public string caption;
        public string text;
        public uint id;
        public List<string> responses;

        static Dictionary<uint, ServerNPCDialog> pool;

        private uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!pool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }


        public delegate void onPlayerAnswerDialogDelegate(ServerPlayer player, byte choose);

        public event onPlayerAnswerDialogDelegate onPlayerAnswerDialog;

        public ServerNPCDialog(string caption, string text)
        {
            if (pool == null) pool = new Dictionary<uint, ServerNPCDialog>();
            this.caption = caption;
            this.text = text;
            this.id = findLowestFreeId();
            responses = new List<string>();

        }

        public static void invokeResponse(ServerPlayer player, uint id, byte answer)
        {
            if(pool.ContainsKey(id) && pool[id].onPlayerAnswerDialog != null) pool[id].onPlayerAnswerDialog.Invoke(player, answer);
        }

        public void addResponse(string text)
        {
            responses.Add(text);
        }

        public void show(ServerPlayer player)
        {
            lock (player.connection.queue)
            {
                player.connection.streamWrite(MIVSDK.Commands.NPCDialog_show);
                player.connection.streamWrite(BitConverter.GetBytes(id));
                player.connection.streamWrite(MIVSDK.Serializers.serialize(caption + "\x01" + text + "\x01" + String.Join("\x01", responses.ToArray())));
                player.connection.streamFlush();
            }
        }

    }
}
