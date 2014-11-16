using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace gtaiv_multiplayer
{
    class GamemodeManager
    {
        ServerApi api;
        public Gamemode current;
        public GamemodeManager(ServerApi api)
        {
            this.api = api;
        }

        public void loadFromFile(string file)
        {
            Assembly asm = Assembly.LoadFile(file);
            List<Type> types = asm.GetTypes().ToList();
            Gamemode gamemode = 
                (Gamemode)types.First(a => a.BaseType == typeof(Gamemode))
                .GetConstructor(new Type[1] { typeof(ServerApi) })
                .Invoke(new object[1] { api });
            loadFromObject(gamemode);
        }

        public void loadFromObject(Gamemode gamemode)
        {
            current = gamemode;
        }
    }
}
