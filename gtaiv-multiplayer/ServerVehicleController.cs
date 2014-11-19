using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MIVServer
{
    public class ServerVehicleInfo
    {
        public uint id;
        public ServerVehicle vehicle;
    }
    public class ServerVehicleController
    {
        Dictionary<uint, ServerVehicle> vehicles;

        public ServerVehicleController()
        {
            vehicles = new Dictionary<uint, ServerVehicle>();
        }

        private uint findLowestFreeId()
        {
            for (uint i = 0; i < uint.MaxValue; i++)
            {
                if (!vehicles.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }

        public ServerVehicle getById(uint id, int model, Vector3 position)
        {
            if (!vehicles.ContainsKey(id))
            {
                throw new Exception("Not found");
            }
            return vehicles[id];
        }

        public ServerVehicleInfo create(int model, Vector3 position, Quaternion orientation, Vector3 velocity)
        {
            uint vid = findLowestFreeId();
            ServerVehicle veh = new ServerVehicle(vid);
            veh.orientation = orientation;
            veh.velocity = velocity;
            vehicles.Add(vid, veh);
            return new ServerVehicleInfo() { id = vid, vehicle = veh };
        }

        public void destroy(byte id)
        {
            if (vehicles.ContainsKey(id))
            {
                vehicles.Remove(id);
            }
        }
    }
}
