using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace gtaiv_multiplayer_client
{
    class PlayerVehicleController
    {
        Dictionary<byte, Vehicle> vehicles;

        public PlayerVehicleController()
        {
            vehicles = new Dictionary<byte, Vehicle>();
        }

        public Vehicle getById(byte id, int model, Vector3 position)
        {
            if (!vehicles.ContainsKey(id))
            {
                vehicles.Add(id, World.CreateVehicle(new Model(model), position));
                Client.log("Created vehicle instance");
            }
            return vehicles[id];
        }

        public void destroy(byte id)
        {
            if (vehicles.ContainsKey(id))
            {
              //  vehicles[id].Delete();
                vehicles.Remove(id);
            }
        }
    }
}
