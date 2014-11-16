using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gtaiv_multiplayer
{
    class Program
    {
        static void Main(string[] args)
        {
            multiplayer_sdk.UpdateDataStruct s = new multiplayer_sdk.UpdateDataStruct();
            s.pos_x = 123.0f;
            s.pos_y = 2.1234f;
            s.pos_z = 333.0212f;
            s.rot_a = 121.113f;
            s.rot_x = 999.888f;
            s.rot_y = 1211.1111f;
            s.rot_z = 1.1f;
            byte[] serialized = s.serialize();
            List<byte> test = new List<byte>();
            test.AddRange(new byte[3] { 0, 2, 1 });
            test.AddRange(serialized);
            multiplayer_sdk.UpdateDataStruct abc = multiplayer_sdk.UpdateDataStruct.unserialize(test.ToArray(), 3);
            Console.WriteLine(abc.ToString());
            new Server(9999);
            while (Console.ReadLine() != "c") ;
        }
    }
}
