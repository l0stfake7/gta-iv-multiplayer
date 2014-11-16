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
        InfoPlayerName,

    }

    public struct UpdateDataStruct
    {
        public float pos_x, pos_y, pos_z, rot_x, rot_y, rot_z, rot_a, vel_x, vel_y, vel_z, speed, heading;
        public int vehicle_model, ped_health, veh_health;
        public string nick;

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

            output.AddRange(BitConverter.GetBytes(vel_x));
            output.AddRange(BitConverter.GetBytes(vel_y));
            output.AddRange(BitConverter.GetBytes(vel_z));

            output.AddRange(BitConverter.GetBytes(speed));
            output.AddRange(BitConverter.GetBytes(heading));

            output.AddRange(BitConverter.GetBytes(vehicle_model));
            output.AddRange(BitConverter.GetBytes(ped_health));
            output.AddRange(BitConverter.GetBytes(veh_health));
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

            output.vel_x = BitConverter.ToSingle(data, 28 + offset);
            output.vel_y = BitConverter.ToSingle(data, 32 + offset);
            output.vel_z = BitConverter.ToSingle(data, 36 + offset);

            output.speed = BitConverter.ToSingle(data, 40 + offset);
            output.heading = BitConverter.ToSingle(data, 44 + offset);

            output.vehicle_model = BitConverter.ToInt32(data, 48 + offset);
            output.ped_health = BitConverter.ToInt32(data, 52 + offset);
            output.veh_health = BitConverter.ToInt32(data, 56 + offset);

            return output;
        }

    }
}
