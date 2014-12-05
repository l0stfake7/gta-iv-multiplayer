using SharpDX;

namespace MIVServer
{
    public class ServerVehicle
    {
        public int health;
        public uint id;
        public string model;
        public Quaternion orientation;
        public Vector3 position, velocity;

        public ServerVehicle(uint id)
        {
            this.id = id;
        }
    }
}