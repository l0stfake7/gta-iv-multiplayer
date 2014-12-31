using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;

namespace MIVSDK
{
    class JSEngine
    {
        private Engine engine;
        public Engine Engine
        {
            get{ return engine; }
        }
        public JSEngine()
        {
            engine = new Engine(cfg => cfg.AllowClr(typeof(SharpDX.Vector3).Assembly, typeof(MIVSDK.INIReader).Assembly));
        }
    }
}
