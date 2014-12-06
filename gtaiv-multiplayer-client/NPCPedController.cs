using GTA;
using System.Collections.Generic;

namespace MIVClient
{
    public class NPCPedController
    {
        public PedStreamer streamer;

        private Dictionary<uint, StreamedPed> peds;

        public NPCPedController(PedStreamer streamer)
        {
            peds = new Dictionary<uint, StreamedPed>();
            this.streamer = streamer;
        }

        public void destroy(uint id)
        {
            if (peds.ContainsKey(id))
            {
                peds[id].delete();
                peds.Remove(id);
            }
        }

        public void update()
        {
            foreach (var ped in peds.Values)
            {
                if (ped.streamedIn && ped.gameReference != null && ped.gameReference.Exists())
                {
                    ped.animator.refreshAnimation();
                }
            }
        }

        public StreamedPed getById(uint id, string model, string name, float heading, Vector3 position)
        {
            if (!peds.ContainsKey(id))
            {
                //if (peds.ContainsKey(id)) peds.Remove(id);
                var ped = new StreamedPed(streamer, model, name, position, heading, BlipColor.Grey);
                peds.Add(id, ped);
                ped.last_game_health = 100; // invicible :)
                Client.log("Created npc instance");

            }
            return peds[id];
        }

        public StreamedPed getById(uint id)
        {
            if (!peds.ContainsKey(id)) return null;
            return peds[id];
        }
    }
}