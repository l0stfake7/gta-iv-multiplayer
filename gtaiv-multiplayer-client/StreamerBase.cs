using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace MIVClient
{
    public abstract class StreamedObjectBase
    {
        protected StreamerBase streamer;
        public bool StreamedIn;
        public uint VirtualWorld;

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

        public abstract void StreamIn();
        public abstract void StreamOut();
        public abstract bool IsStreamedIn();
        public abstract bool NeedRestream();
        public abstract Vector3 GetPosition();
    }
    public abstract class StreamerBase
    {

        protected Client client;
        public List<StreamedObjectBase> instances;
        protected float streamDistance;
        public StreamerBase(Client client, float streamDistance)
        {
            instances = new List<StreamedObjectBase>();
            this.client = client;
            this.streamDistance = streamDistance;
        }
        public abstract void UpdateGfx();
        public abstract void UpdateNormalTick();
        public abstract void UpdateSlow();

        public void Add(StreamedObjectBase instance)
        {
            instances.Add(instance);
        }
        public List<StreamedObjectBase> GetAllStreamed()
        {
            return instances.Where(a => a.IsStreamedIn()).ToList();
        }

        public void Delete(StreamedObjectBase instance)
        {
            instances.Remove(instance);
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
    }
}
