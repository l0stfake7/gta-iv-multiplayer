using SharpDX;
using System;
using MIVSDK;
using System.Collections.Generic;

namespace MIVServer
{
    public class ServerNPC
    {
        private float heading;
        public uint id;
        private uint model, currentVehicleId;
        private bool immortal;
        private string name;
        private Vector3 position;

        private static Dictionary<uint, ServerNPC> pool;

        public float Heading
        {
            get
            {
                return heading;
            }
            set
            {
                heading = value;
                var bpf = new BinaryPacketFormatter(Commands.NPC_setHeading);
                bpf.add(id);
                bpf.add(heading);
                broadcastEvent(bpf);
            }
        }
        public Vector3 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                var bpf = new BinaryPacketFormatter(Commands.NPC_setPosition);
                bpf.add(id);
                bpf.add(position);
                broadcastEvent(bpf);
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                var bpf = new BinaryPacketFormatter(Commands.NPC_setName);
                bpf.add(id);
                bpf.add(name);
                broadcastEvent(bpf);
            }
        }
        public uint Model
        {
            get
            {
                return model;
            }
            set
            {
                model = value;
                var bpf = new BinaryPacketFormatter(Commands.NPC_setModel);
                bpf.add(id);
                bpf.add(model);
                broadcastEvent(bpf);
            }
        }

        public bool Immortal
        {
            get
            {
                return immortal;
            }
            set
            {
                immortal = value;
                var bpf = new BinaryPacketFormatter(Commands.NPC_setImmortal);
                bpf.add(id);
                bpf.add(new byte[1] { immortal ? (byte)1 : (byte)0 });
                broadcastEvent(bpf);
            }
        }

        public void RunTo(Vector3 position)
        {
            this.position = position;
            var bpf = new BinaryPacketFormatter(Commands.NPC_runTo);
            bpf.add(id);
            bpf.add(position);
            broadcastEvent(bpf);
        }
        public void WalkTo(Vector3 position)
        {
            this.position = position;
            var bpf = new BinaryPacketFormatter(Commands.NPC_walkTo);
            bpf.add(id);
            bpf.add(position);
            broadcastEvent(bpf);
        }
        public void EnterVehicle(ServerVehicle vehicle)
        {
            currentVehicleId = vehicle.id;
            var bpf = new BinaryPacketFormatter(Commands.NPC_walkTo);
            bpf.add(id);
            bpf.add(vehicle.id);
            broadcastEvent(bpf);
        }
        public void LeaveVehicle()
        {
            currentVehicleId = 0;
            var bpf = new BinaryPacketFormatter(Commands.NPC_leaveVehicle);
            bpf.add(id);
            broadcastEvent(bpf);
        }

        public void DriveTo(Vector3 position)
        {
            this.position = position;
            var bpf = new BinaryPacketFormatter(Commands.NPC_driveTo);
            bpf.add(id);
            bpf.add(position);
            broadcastEvent(bpf);
        }

        public ServerNPC(string name, uint model, Vector3 position, float heading)
        {
            if (NPCPool == null) NPCPool = new Dictionary<uint, ServerNPC>();
            this.name = name;
            this.model = model;
            this.position = position;
            this.heading = heading;
            currentVehicleId = 0;
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

        private void broadcastEvent(BinaryPacketFormatter bpf)
        {
            if (Server.instance.playerpool != null)
            {
                for(int i=0;i<Server.instance.playerpool.Count;i++)
                {
                    Server.instance.playerpool[i].connection.write(bpf.getBytes());
                }
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