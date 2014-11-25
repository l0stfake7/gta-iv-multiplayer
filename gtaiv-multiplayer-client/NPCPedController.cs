using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace MIVClient
{
    public class NPCPedController
    {
        Dictionary<uint, StreamedPed> peds;
        public PedStreamer streamer;

        public NPCPedController()
        {
            peds = new Dictionary<uint, StreamedPed>();
            streamer = new PedStreamer(Client.getInstance());
        }

        public StreamedPed getById(uint id, string model, string name, float heading, Vector3 position)
        {
            if (!peds.ContainsKey(id))
            {
                //if (peds.ContainsKey(id)) peds.Remove(id);
                peds.Add(id, new StreamedPed(streamer, model, name, position, heading));
                Client.log("Created npc instance");
            }
            return peds[id];
        }

        public void destroy(uint id)
        {
            if (peds.ContainsKey(id))
            {
                peds[id].delete();
                peds.Remove(id);
            }
        }
    }
}
