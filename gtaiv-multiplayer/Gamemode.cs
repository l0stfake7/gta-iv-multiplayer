using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gtaiv_multiplayer
{
    abstract class Gamemode
    {
        private ServerApi api;

        public Gamemode(ServerApi api)
        {
            this.api = api;
        }
    }
}
