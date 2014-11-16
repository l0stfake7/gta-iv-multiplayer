using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gtaiv_multiplayer
{
    class Quaternion
    {
        public float x, y, z, a;
        public Quaternion(float x, float y, float z, float a)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.a = a;
        }
    }
}
