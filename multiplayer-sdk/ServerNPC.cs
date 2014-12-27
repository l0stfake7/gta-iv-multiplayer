// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using SharpDX;
using System;
using System.Collections.Generic;

namespace MIVServer
{
    public class ServerNPC
    {
        public uint id;
        private static Dictionary<uint, ServerNPC> pool;
        private float heading;
        private bool immortal;
        private uint model, currentVehicleId;
        private string name;
        private Vector3 position;

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
                bpf.Add(id);
                bpf.Add(heading);
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
                bpf.Add(id);
                bpf.Add(new byte[1] { immortal ? (byte)1 : (byte)0 });
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
                bpf.Add(id);
                bpf.Add(model);
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
                bpf.Add(id);
                bpf.Add(name);
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
                bpf.Add(id);
                bpf.Add(position);
                broadcastEvent(bpf);
            }
        }

        public void DriveTo(Vector3 position)
        {
            this.position = position;
            var bpf = new BinaryPacketFormatter(Commands.NPC_driveTo);
            bpf.Add(id);
            bpf.Add(position);
            broadcastEvent(bpf);
        }

        public void EnterVehicle(ServerVehicle vehicle)
        {
            currentVehicleId = vehicle.id;
            var bpf = new BinaryPacketFormatter(Commands.NPC_walkTo);
            bpf.Add(id);
            bpf.Add(vehicle.id);
            broadcastEvent(bpf);
        }

        public void LeaveVehicle()
        {
            currentVehicleId = 0;
            var bpf = new BinaryPacketFormatter(Commands.NPC_leaveVehicle);
            bpf.Add(id);
            broadcastEvent(bpf);
        }

        public void RunTo(Vector3 position)
        {
            this.position = position;
            var bpf = new BinaryPacketFormatter(Commands.NPC_runTo);
            bpf.Add(id);
            bpf.Add(position);
            broadcastEvent(bpf);
        }

        public void WalkTo(Vector3 position)
        {
            this.position = position;
            var bpf = new BinaryPacketFormatter(Commands.NPC_walkTo);
            bpf.Add(id);
            bpf.Add(position);
            broadcastEvent(bpf);
        }

        private void broadcastEvent(BinaryPacketFormatter bpf)
        {
            if (Server.instance.playerpool != null)
            {
                Server.instance.InvokeParallelForEachPlayer((player) =>
                {
                    player.connection.write(bpf.getBytes());
                });
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