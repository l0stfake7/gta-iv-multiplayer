using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multiplayer_sdk
{
    public enum Commands
    {
        Invalid,
        Connect,
        Disconnect,
        UpdateData,
        PostChatMessage,
        ServerInfo,
        GetServerName,

    }

    public struct UpdateDataStruct
    {
        public float pos_x, pos_y, pos_z, rot_x, rot_y, rot_z, rot_a;

        public byte[] serialize()
        {
            List<byte> output = new List<byte>();
            output.AddRange(BitConverter.GetBytes(pos_x));
            output.AddRange(BitConverter.GetBytes(pos_y));
            output.AddRange(BitConverter.GetBytes(pos_z));

            output.AddRange(BitConverter.GetBytes(rot_x));
            output.AddRange(BitConverter.GetBytes(rot_y));
            output.AddRange(BitConverter.GetBytes(rot_z));
            output.AddRange(BitConverter.GetBytes(rot_a));
            return output.ToArray();
        }

        public static UpdateDataStruct unserialize(byte[] data, int offset)
        {
            UpdateDataStruct output = new UpdateDataStruct();
            output.pos_x = BitConverter.ToSingle(data, 0 + offset);
            output.pos_y = BitConverter.ToSingle(data, 4 + offset);
            output.pos_z = BitConverter.ToSingle(data, 8 + offset);

            output.rot_x = BitConverter.ToSingle(data, 12 + offset);
            output.rot_y = BitConverter.ToSingle(data, 16 + offset);
            output.rot_z = BitConverter.ToSingle(data, 20 + offset);
            output.rot_a = BitConverter.ToSingle(data, 24 + offset);
            return output;
        }

    }
}
