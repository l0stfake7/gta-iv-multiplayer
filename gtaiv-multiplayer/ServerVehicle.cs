using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK.Math;

namespace MIVServer
{
    public class ServerVehicle
    {
        public uint id;
        public string model;
        public Vector3 position, velocity;
        public Quaternion orientation;
        public int health;

        public ServerVehicle(uint id)
        {
            this.id = id;
        }
    }
}
