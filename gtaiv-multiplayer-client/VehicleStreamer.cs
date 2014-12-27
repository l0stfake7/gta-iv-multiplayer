// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using GTA;
using System.Linq;

//using MIVSDK;

namespace MIVClient
{
    public class StreamedVehicle : StreamedObjectBase
    {
        public Vehicle gameReference;
        public int last_game_health;
        public string model;
        public Quaternion orientation;
        public Vector3 position;
        public Vector3 spawn_position;
        public Quaternion spawn_orientation;
        private bool will_respawn = false;

        public StreamedVehicle(VehicleStreamer streamer, string model, Vector3 position, Quaternion orientation)
            : base(streamer)
        {
            this.model = model;
            this.position = position;
            spawn_position = position;
            this.orientation = orientation;
            spawn_orientation = orientation;
        }

        public override Vector3 GetPosition()
        {
            return position;
        }

        public override bool IsStreamedIn()
        {
            return StreamedIn && gameReference != null && gameReference.Exists();
        }

        public override bool NeedRestream()
        {
            if (gameReference.Health == 0 && !will_respawn)
            {
                will_respawn = true;
                var timer = new Timer(6000);
                timer.Tick += (o, e) =>
                {
                    position = spawn_position;
                    orientation = spawn_orientation;
                    if (IsStreamedIn()) StreamOut();
                    timer.Stop();
                };
                timer.Start();
            }
            return false;
        }

        public override void StreamIn()
        {
            gameReference = World.CreateVehicle(new Model(model), position);
            gameReference.RotationQuaternion = orientation;
            will_respawn = false;
            Client.instance.prepareVehicle(this);
        }

        public override void StreamOut()
        {
            gameReference.Delete();
            gameReference = null;
        }
    }

    public class VehicleStreamer : StreamerBase
    {
        public VehicleStreamer(Client client, float range)
            : base(client, range)
        {
        }

        public override void UpdateGfx()
        {
        }

        public override void UpdateNormalTick()
        {
            foreach (Vehicle v in World.GetVehicles(client.getPlayerPed().Position, 200.0f))
            {
                if (v.Exists() && instances.Count(a => ((StreamedVehicle)a).gameReference != null && ((StreamedVehicle)a).gameReference == v) == 0)
                {
                    v.Delete();
                }
            }
        }

        public override void UpdateSlow()
        {
            World.CarDensity = 0.0f;
            GTA.Native.Function.Call("DISABLE_CAR_GENERATORS");
            GTA.Native.Function.Call("SET_RANDOM_CAR_DENSITY_MULTIPLIER", 0.0f);
            GTA.Native.Function.Call("SET_PARKED_CAR_DENSITY_MULTIPLIER", 0.0f, 0.0f);
            GTA.Native.Function.Call("SET_REDUCE_VEHICLE_MODEL_BUDGET", 0);
            
        }
    }
}