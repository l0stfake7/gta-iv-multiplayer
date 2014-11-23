using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace MIVClient
{
    public class PlayerPedController
    {
        Dictionary<byte, StreamedPed> peds;
        public PedStreamer streamer;

        public PlayerPedController()
        {
            peds = new Dictionary<byte, StreamedPed>();
            streamer = new PedStreamer(Client.getInstance());
        }

        public StreamedPed getById(byte id, Vector3 position)
        {
            if (!peds.ContainsKey(id))
            {
                //if (peds.ContainsKey(id)) peds.Remove(id);
                peds.Add(id, new StreamedPed(streamer, id, position, 0.0f));
                Client.log("Created ped instance");
            }
            return peds[id];
        }

        public void destroy(byte id)
        {
            if (peds.ContainsKey(id))
            {
                peds[id].delete();
                peds.Remove(id);
            }
        }
    }
}
