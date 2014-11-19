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
        Dictionary<byte, Ped> peds;

        public PlayerPedController()
        {
            peds = new Dictionary<byte, Ped>();
        }

        public Ped getById(byte id, Vector3 position)
        {
            if (!peds.ContainsKey(id))
            {
                //if (peds.ContainsKey(id)) peds.Remove(id);
                peds.Add(id, World.CreatePed(position, Gender.Male));
                Client.log("Created ped instance");
            }
            if (!Game.Exists(peds[id]) || !peds[id].isAliveAndWell)
            {
                if (Game.Exists(peds[id]))
                {
                    peds[id].Delete();
                }
                peds[id] = World.CreatePed(position, Gender.Male);
            }
            return peds[id];
        }

        public void destroy(byte id)
        {
            if (peds == null)
            {
                peds = new Dictionary<byte, Ped>();
            }
            if (peds.ContainsKey(id))
            {
                peds[id].Delete();
                peds.Remove(id);
            }
        }
    }
}
