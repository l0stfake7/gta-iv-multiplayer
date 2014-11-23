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