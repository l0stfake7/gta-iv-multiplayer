using GTA;
using System;
using System.Collections.Generic;
using System.Linq;

//using MIVSDK;

namespace MIVClient
{
    public class StreamedVehicle
    {
        public Vehicle gameReference;
        public uint id;
        public string model;
        public Quaternion orientation;
        public Vector3 position;
        public bool streamedIn;

        private VehicleStreamer streamer;

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
        public List<StreamedVehicle> vehicles;

        private Client client;

        public VehicleStreamer(Client client)
        {
            this.client = client;
            vehicles = new List<StreamedVehicle>();
        }
        public void updateGfx()
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

        public void updateSlow()
        {
            World.CarDensity = 0.0f;
            
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
                    if (distance < 70.0f && Game.CurrentCamera.isSphereVisible(vehicle.position, 1.0f))
                    {
                        if (!vehicle.streamedIn || vehicle.gameReference == null || !vehicle.gameReference.Exists())
                        {
                            var model = new Model(vehicle.model);
                            if (model.isValid)
                            {
                                vehicle.gameReference = World.CreateVehicle(new Model(vehicle.model), vehicle.position);
                                vehicle.gameReference.RotationQuaternion = vehicle.orientation;
                                client.prepareVehicle(vehicle);
                                vehicle.streamedIn = true;
                            }
                            else
                            {
                                vehicle.delete();
                                break;
                            }
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
                    client.chatController.writeDebug("VStreamer" + e.Message);
                }
            }
            //var peds = World.GetPeds(client.getPlayerPed().Position, 200.0f);
            //foreach (Ped a in peds) if (a.Exists() && a.isAlive && a != client.getPlayerPed()) a.Delete();
        }
    }
}