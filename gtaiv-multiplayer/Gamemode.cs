using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
