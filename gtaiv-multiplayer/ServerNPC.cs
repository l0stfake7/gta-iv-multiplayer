using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;
using MIVSDK.Math;

namespace MIVServer
{
    public class ServerNPC
    {
        static Dictionary<uint, ServerNPC> pool;
        public static Dictionary<uint, ServerNPC> NPCPool
        {
            get
            {
                if (pool == null) pool = new Dictionary<uint, ServerNPC>();
                return pool;
            }
            set
            {
                pool = value;
            }
        }

        public uint id;
        public string name, model;
        public Vector3 position;
        public float heading;
        public ServerNPCDialog dialog;

        private uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!NPCPool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }

        public ServerNPC(string name, string model, Vector3 position, float heading, ServerNPCDialog dialog)
        {
            if (NPCPool == null) NPCPool = new Dictionary<uint, ServerNPC>();
            this.name = name;
            this.model = model;
            this.position = position;
            this.heading = heading;
            this.dialog = dialog;
            id = findLowestFreeId();
            NPCPool.Add(id, this);
            
        }
    }
}
