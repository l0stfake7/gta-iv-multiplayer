// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
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