using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVSDK.Math;

namespace MIVSDK
{
    public class Serializers
    {
        public static byte[] serialize(string str)
        {
            var buffer = Encoding.UTF8.GetBytes(str).ToList();
            buffer.InsertRange(0, BitConverter.GetBytes(buffer.Count));
            return buffer.ToArray();
        }
        public static string unserialize_string(byte[] b, int o)
        {
            return Encoding.UTF8.GetString(b, o + 4, BitConverter.ToInt32(b, o));
        }
        public static string unserialize_string(byte[] b, int o, out int length)
        {
            int count = BitConverter.ToInt32(b, o);
            length = count + 4;
            return Encoding.UTF8.GetString(b, o + 4, count);
        }

        public static byte[] serialize(Vector3 vec)
        {
            List<byte> output = new List<byte>();
            output.AddRange(BitConverter.GetBytes(vec.X));
            output.AddRange(BitConverter.GetBytes(vec.Y));
            output.AddRange(BitConverter.GetBytes(vec.Z));
            return output.ToArray();
        }
    }
}
