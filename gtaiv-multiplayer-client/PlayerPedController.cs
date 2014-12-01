using GTA;
using System.Linq;
using System.Collections.Generic;

namespace MIVClient
{
    public class PlayerPedController
    {
        public PedStreamer streamer;

        private Dictionary<byte, StreamedPed> peds;

        public PlayerPedController(PedStreamer streamer)
        {
            peds = new Dictionary<byte, StreamedPed>();
            this.streamer = streamer;
        }

        public void destroy(byte id)
        {
            if (peds.ContainsKey(id))
            {
                peds[id].delete();
                peds.Remove(id);
            }
        }

        public byte getPlayerIdByPed(Ped ped)
        {
            var elem = peds.Where(a => a.Value.streamedIn && a.Value.gameReference.Exists() && a.Value.gameReference == ped);
            return elem == null || elem.Count() == 0 ? (byte)255 : elem.First().Key;
        }

        public StreamedPed getById(byte id, string nick, Vector3 position)
        {
            if (!peds.ContainsKey(id))
            {
                //if (peds.ContainsKey(id)) peds.Remove(id);
                var ped = new StreamedPed(streamer, "F_Y_HOOKER_01", nick, position, 0.0f);
                peds.Add(id, ped);
                ped.last_game_health = 100;
                Client.log("Created ped instance");
            }
            return peds[id];
        }
    }
}