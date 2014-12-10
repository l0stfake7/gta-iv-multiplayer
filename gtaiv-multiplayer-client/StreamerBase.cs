using GTA;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MIVClient
{
    public abstract class StreamedObjectBase
    {
        public bool StreamedIn;
        public uint VirtualWorld;
        protected StreamerBase streamer;

        public StreamedObjectBase(StreamerBase streamer)
        {
            this.streamer = streamer;
            streamer.Add(this);
            StreamedIn = false;
            VirtualWorld = 0;
        }

        public void Delete()
        {
            streamer.Delete(this);
        }

        public abstract Vector3 GetPosition();

        public abstract bool IsStreamedIn();

        public abstract bool NeedRestream();

        public abstract void StreamIn();

        public abstract void StreamOut();
    }

    public abstract class StreamerBase
    {
        public List<StreamedObjectBase> instances;
        protected Client client;
        protected float streamDistance;

        public StreamerBase(Client client, float streamDistance)
        {
            instances = new List<StreamedObjectBase>();
            this.client = client;
            this.streamDistance = streamDistance;
        }

        public void Add(StreamedObjectBase instance)
        {
            instances.Add(instance);
        }

        public void Delete(StreamedObjectBase instance)
        {
            instances.Remove(instance);
        }

        public List<StreamedObjectBase> GetAllStreamed()
        {
            return instances.Where(a => a.IsStreamedIn()).ToList();
        }

        public void Update()
        {
            Vector3 playerPos = Game.CurrentCamera.Position;
            foreach (var instance in instances)
            {
                try
                {
                    float distance = playerPos.DistanceTo(instance.GetPosition());
                    if (distance < streamDistance && instance.VirtualWorld == client.CurrentVirtualWorld)
                    {
                        if (!instance.IsStreamedIn())
                        {
                            instance.StreamIn();
                            instance.StreamedIn = true;
                        }
                        else
                        {
                            if (instance.NeedRestream())
                            {
                                instance.StreamOut();
                                instance.StreamedIn = false;
                            }
                        }
                    }
                    else if (instance.StreamedIn)
                    {
                        instance.StreamOut();
                        instance.StreamedIn = false;
                    }
                }
                catch (Exception e)
                {
                    Game.Console.Print(e.Message + " " + e.StackTrace);
                    Game.Log(e.Message + " " + e.StackTrace);
                }
            }
        }

        public abstract void UpdateGfx();

        public abstract void UpdateNormalTick();

        public abstract void UpdateSlow();
    }
}