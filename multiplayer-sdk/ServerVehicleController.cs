using MIVSDK.Math;
using System;
using System.Collections.Generic;

namespace MIVServer
{
    public class ServerVehicleController
    {
        public Dictionary<uint, ServerVehicle> vehicles;

        public ServerVehicleController()
        {
            vehicles = new Dictionary<uint, ServerVehicle>();
        }

        public ServerVehicle create(string model, Vector3 position, Quaternion orientation)
        {
            uint vid = findLowestFreeId();
            ServerVehicle veh = new ServerVehicle(vid);
            veh.position = position;
            veh.orientation = orientation;
            veh.model = model;
            veh.velocity = Vector3.Zero;
            vehicles.Add(vid, veh);
            return veh;
        }

        public void destroy(byte id)
        {
            if (vehicles.ContainsKey(id))
            {
                vehicles.Remove(id);
            }
        }

        public ServerVehicle getById(uint id, int model, Vector3 position)
        {
            if (!vehicles.ContainsKey(id))
            {
                throw new Exception("Not found");
            }
            return vehicles[id];
        }

        private uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!vehicles.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }
    }

    public class ServerVehicleInfo
    {
        public uint id;
        public ServerVehicle vehicle;
    }
}