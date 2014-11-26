using GTA;
using System.Collections.Generic;

namespace MIVClient
{
    public class NPCPedController
    {
        public PedStreamer streamer;

        private Dictionary<uint, StreamedPed> peds;

        public NPCPedController()
        {
            peds = new Dictionary<uint, StreamedPed>();
            streamer = new PedStreamer(Client.getInstance());
        }

        public void destroy(uint id)
        {
            if (peds.ContainsKey(id))
            {
                peds[id].delete();
                peds.Remove(id);
            }
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
    }
}