// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using SharpDX;
using System;
using System.Collections.Generic;

namespace MIVServer
{
    public class ServerVehicleController
    {
        public Dictionary<uint, ServerVehicle> vehicles;

        public ServerVehicleController()
        {
            vehicles = new Dictionary<uint, ServerVehicle>();
        }

        public ServerVehicle create(string model, Vector3 position, Quaternion orientation)
        {
            uint vid = findLowestFreeId();
            ServerVehicle veh = new ServerVehicle(vid);
            veh.position = position;
            veh.orientation = orientation;
            veh.model = model;
            vehicles.Add(vid, veh);
            Server.instance.InvokeParallelForEachPlayer((player) =>
            {
                var bpf = new BinaryPacketFormatter(Commands.Vehicle_create);
                bpf.add(veh.id);
                bpf.add(veh.position);
                bpf.add(veh.orientation);
                bpf.add(ModelDictionary.getVehicleByName(veh.model));
                player.connection.write(bpf.getBytes());
            });
            return veh;
        }

        public void destroy(byte id)
        {
            if (vehicles.ContainsKey(id))
            {
                vehicles.Remove(id);
            }
        }

        public ServerVehicle getById(uint id)
        {
            if (!vehicles.ContainsKey(id))
            {
                throw new Exception("Not found");
            }
            return vehicles[id];
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
}