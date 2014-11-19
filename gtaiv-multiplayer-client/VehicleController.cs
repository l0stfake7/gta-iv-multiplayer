using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace MIVClient
{
    public class VehicleInfo
    {
        public uint id;
        public Vehicle vehicle;
    }
    public class VehicleController
    {
        Dictionary<uint, Vehicle> vehicles;

        public VehicleController()
        {
            vehicles = new Dictionary<uint, Vehicle>();
        }

        private uint findLowestFreeId()
        {
            for (uint i = 0; i < uint.MaxValue; i++)
            {
                if (!vehicles.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }

        public Vehicle getById(uint id, int model, Vector3 position)
        {
            if (!vehicles.ContainsKey(id))
            {
                throw new Exception("Not found");
            }
            return vehicles[id];
        }

        public VehicleInfo create(int model, Vector3 position, Quaternion orientation, Vector3 velocity)
        {
            uint vid = findLowestFreeId();
            Vehicle veh = World.CreateVehicle(new Model(model), position);
            veh.RotationQuaternion = orientation;
            veh.Velocity = velocity;
            vehicles.Add(vid, veh);
            return new VehicleInfo() { id = vid, vehicle = veh };
        }

        public void destroy(byte id)
        {
            if (vehicles.ContainsKey(id))
            {
                vehicles[id].Delete();
                vehicles.Remove(id);
            }
        }
    }
}
