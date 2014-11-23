using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
//using MIVSDK;

namespace MIVClient
{
    public class StreamedPed
    {
        PedStreamer streamer;
        public uint id;
        public Vector3 position;
        public float heading;
        public bool streamedIn;
        public Ped gameReference;
        public PedAnimationManager animator;

        public StreamedPed(PedStreamer streamer, uint id, Vector3 position, float heading)
        {
            this.streamer = streamer;
            this.id = id;
            this.position = position;
            this.heading = heading;
            streamedIn = false;
            streamer.add(this);
            animator = new PedAnimationManager(this);
        }

        public void delete()
        {
            streamer.delete(this);
        }

    }

    public class PedStreamer
    {
        Client client;
        public List<StreamedPed> peds;

        public PedStreamer(Client client)
        {
            this.client = client;
            peds = new List<StreamedPed>();
        }

        public void add(StreamedPed ped)
        {
            peds.Add(ped);
        }

        public void delete(uint id)
        {
            peds.Remove(peds.First(a => a.id == id));
        }

        public void delete(StreamedPed ped)
        {
            peds.Remove(ped);
        }

        public void update()
        {
            Vector3 playerPos = client.getPlayerPed().Position;
            foreach (StreamedPed ped in this.peds)
            {
                try
                {
                    float distance = playerPos.DistanceTo(ped.position);
                    //client.chatController.writeChat(playerPos.X.ToString() + " " + playerPos.Y.ToString() + " " + playerPos.Z.ToString() + " ");
                    if (distance < 30.0f)
                    {
                        if (!ped.streamedIn || ped.gameReference == null || !ped.gameReference.Exists())
                        {
                            ped.gameReference = World.CreatePed("M_Y_BOUNCER_02", ped.position, RelationshipGroup.Player);
                            ped.streamedIn = true;
                            ped.gameReference.Heading = ped.heading;
                        }
                    }
                    else if (ped.streamedIn)
                    {
                        ped.gameReference.Delete();
                        ped.gameReference = null;
                        ped.streamedIn = false;
                    }
                }
                catch (Exception e)
                {
                    client.chatController.writeChat("PEDSTREAMER: " + e.Message);
                }
            }/*
            var pedss = World.GetPeds(client.getPlayerPed().Position, 200.0f);
            foreach (Ped a in pedss) if (a.Exists() && a.isAlive && a != client.getPlayerPed() && peds.Count(ax => ax.gameReference != null && ax.gameReference == a) == 0) a.Delete();*/
        }
    }
}
