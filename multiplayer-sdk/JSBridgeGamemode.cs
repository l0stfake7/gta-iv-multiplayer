using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK;

namespace MIVServer
{
    class JSBridgeGamemode : Gamemode
    {
        JSEngine engine;
        public JSBridgeGamemode(ServerApi a)
            : base(a)
        {
            engine = new JSEngine();
        }

        public void Load(string file)
        {
            string script = System.IO.File.ReadAllText(file);
            engine.Engine.Execute("var MIVSDK = importNamespace('MIVSDK');var MIVServer = importNamespace('MIVServer');var SharpDX = importNamespace('SharpDX');");
            engine.Engine.SetValue("API", api);
            engine.Engine.SetValue("JSBridge", this);
            engine.Engine.Execute(script);
        }
    }
}
