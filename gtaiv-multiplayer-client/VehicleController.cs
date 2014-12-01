using GTA;
using System;
using System.Collections.Generic;

namespace MIVClient
{
    public class VehicleController
    {
        public VehicleStreamer streamer;
        public Dictionary<uint, StreamedVehicle> vehicles;

        public VehicleController(VehicleStreamer streamer)
        {
            vehicles = new Dictionary<uint, StreamedVehicle>();
            this.streamer = streamer;
        }

        public StreamedVehicle create(uint vid, string model, Vector3 position, Quaternion orientation, Vector3 velocity)
        {
            var v = new StreamedVehicle(streamer, vid, model, position, orientation);
            this.vehicles.Add(vid, v);
            return v;
        }

        public void destroy(byte id)
        {
            if (vehicles.ContainsKey(id))
            {
                vehicles[id].delete();
                vehicles.Remove(id);
            }
        }

        public StreamedVehicle getById(uint id)
        {
            if (!vehicles.ContainsKey(id))
            {
                throw new Exception("Not found");
            }
            return vehicles[id];
        }

        public StreamedVehicle getByVehicle(Vehicle vehicle)
        {
            foreach (var pair in streamer.vehicles)
            {
                if (pair.gameReference != null && pair.gameReference == vehicle) return pair;
            }
            return null;
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

    public class VehicleInfo
    {
        public uint id;
        public Vehicle vehicle;
    }
}