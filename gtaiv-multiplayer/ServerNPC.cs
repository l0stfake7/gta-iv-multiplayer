using MIVSDK.Math;
using System;
using System.Collections.Generic;

namespace MIVServer
{
    public class ServerNPC
    {
        public ServerNPCDialog dialog;
        public float heading;
        public uint id, model;
        public string name;
        public Vector3 position;

        private static Dictionary<uint, ServerNPC> pool;

        public ServerNPC(string name, uint model, Vector3 position, float heading, ServerNPCDialog dialog)
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

        private uint findLowestFreeId()
        {
            for (uint i = 1; i < uint.MaxValue; i++)
            {
                if (!NPCPool.ContainsKey(i)) return i;
            }
            throw new Exception("No free ids");
        }
    }
}