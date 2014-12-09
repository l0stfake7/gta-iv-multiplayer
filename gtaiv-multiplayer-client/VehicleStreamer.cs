using GTA;
using System;
using System.Collections.Generic;
using System.Linq;

//using MIVSDK;

namespace MIVClient
{
    public class StreamedVehicle : StreamedObjectBase
    {
        public Vehicle gameReference;
        public string model;
        public Quaternion orientation;
        public Vector3 position;

        public StreamedVehicle(VehicleStreamer streamer, string model, Vector3 position, Quaternion orientation) : base(streamer)
        {
            this.model = model;
            this.position = position;
            this.orientation = orientation;
        }
        public override void StreamIn()
        {
            gameReference = World.CreateVehicle(new Model(model), position);
            gameReference.RotationQuaternion = orientation;
            Client.instance.prepareVehicle(this);

        }
        public override void StreamOut()
        {
            gameReference.Delete();
            gameReference = null;
        }
        public override bool IsStreamedIn()
        {
            return StreamedIn && gameReference != null && gameReference.Exists();
        }
        public override bool NeedRestream()
        {
            return !gameReference.isAlive || gameReference.Health == 0;
        }
        public override Vector3 GetPosition()
        {
            return position;
        }
    }

    public class VehicleStreamer : StreamerBase
    {
        public List<StreamedVehicle> vehicles;

        public VehicleStreamer(Client client, float range) : base(client, range)
        {
        }
        public override void UpdateGfx()
        {

        }

        public override void UpdateSlow()
        {
            World.CarDensity = 0.0f;
            AlternateHook.call(MIVSDK.AlternateHookRequest.VehiclesCommands.DISABLE_CAR_GENERATORS);
            AlternateHook.call(MIVSDK.AlternateHookRequest.VehiclesCommands.DISABLE_CAR_GENERATORS_WITH_HELI);
            AlternateHook.call(MIVSDK.AlternateHookRequest.VehiclesCommands.SET_CAR_GENERATORS_ACTIVE_IN_AREA, 0);
        }

        public override void UpdateNormalTick()
        {
            
            foreach (Vehicle v in World.GetVehicles(client.getPlayerPed().Position, 200.0f))
            {
                if (v.Exists() && vehicles.Count(a => a.gameReference != null && a.gameReference == v) == 0)
                {
                    v.NoLongerNeeded();
                    v.Delete();
                    //v.Delete();
                }
            }
        }
    }
}