using GTA;
using System;
using System.Collections.Generic;

namespace MIVClient
{
    public class PlayerVehicleController
    {
        private Dictionary<uint, Vehicle> vehicles;

        public PlayerVehicleController()
        {
            vehicles = new Dictionary<uint, Vehicle>();
        }

        public void destroy(byte id)
        {
            if (vehicles.ContainsKey(id))
            {
                //  vehicles[id].Delete();
                vehicles.Remove(id);
            }
        }

        public Vehicle getById(byte id, int model, Vector3 position)
        {
            if (!vehicles.ContainsKey(id))
            {
                vehicles.Add(findLowestFreeId(), World.CreateVehicle(new Model(model), position));
                Client.log("Created vehicle instance");
            }
            return vehicles[id];
        }

        private uint findLowestFreeId()
        {
            for (uint i = 0; i < uint.MaxValue; i++)
            {
                if (!vehicles.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }
    }
}