using GTA;
using System;
using System.Collections.Generic;
using System.Linq;

//using MIVSDK;

namespace MIVClient
{
    public class StreamedVehicle
    {
        private VehicleStreamer streamer;
        public uint id;
        public string model;
        public Vector3 position;
        public Quaternion orientation;
        public bool streamedIn;
        public Vehicle gameReference;

        public StreamedVehicle(VehicleStreamer streamer, uint id, string model, Vector3 position, Quaternion orientation)
        {
            this.streamer = streamer;
            this.model = model;
            this.id = id;
            this.position = position;
            this.orientation = orientation;
            streamedIn = false;
            streamer.add(this);
        }

        public void delete()
        {
            streamer.delete(this);
        }
    }

    public class VehicleStreamer
    {
        private Client client;
        public List<StreamedVehicle> vehicles;

        public VehicleStreamer(Client client)
        {
            this.client = client;
            vehicles = new List<StreamedVehicle>();
        }

        public void add(StreamedVehicle vehicle)
        {
            vehicles.Add(vehicle);
        }

        public void delete(uint id)
        {
            vehicles.Remove(vehicles.First(a => a.id == id));
        }

        public void delete(StreamedVehicle vehicle)
        {
            vehicles.Remove(vehicle);
        }

        public void update()
        {
            Vector3 playerPos = client.getPlayerPed().Position;
            foreach (StreamedVehicle vehicle in vehicles)
            {
                try
                {
                    float distance = playerPos.DistanceTo(vehicle.position);
                    //client.chatController.writeChat(playerPos.X.ToString() + " " + playerPos.Y.ToString() + " " + playerPos.Z.ToString() + " ");
                    if (distance < 300.0f)
                    {
                        if (!vehicle.streamedIn || vehicle.gameReference == null || !vehicle.gameReference.Exists())
                        {
                            vehicle.gameReference = World.CreateVehicle(new Model(vehicle.model), vehicle.position);
                            vehicle.gameReference.RotationQuaternion = vehicle.orientation;
                            client.prepareVehicle(vehicle);
                            vehicle.streamedIn = true;
                        }
                    }
                    else if (vehicle.streamedIn)
                    {
                        vehicle.gameReference.Delete();
                        vehicle.gameReference = null;
                        vehicle.streamedIn = false;
                    }
                }
                catch (Exception e)
                {
                    client.chatController.writeChat(e.Message);
                }
            }
            //var peds = World.GetPeds(client.getPlayerPed().Position, 200.0f);
            //foreach (Ped a in peds) if (a.Exists() && a.isAlive && a != client.getPlayerPed()) a.Delete();
            foreach (Vehicle v in World.GetAllVehicles()) if (v.Exists() && vehicles.Count(a => a.gameReference != null && a.gameReference == v) == 0) v.Delete();
        }
    }
}