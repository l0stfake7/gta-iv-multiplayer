using MIVSDK.Math;
using MIVServer;
using System;

namespace MIVServer
{
    public abstract class Gamemode
    {
        protected ServerApi api;

        public Gamemode(ServerApi api)
        {
            this.api = api;
        }

    }
}