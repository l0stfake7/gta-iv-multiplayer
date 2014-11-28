using System;
using System.Collections.Generic;

namespace MIVSDK
{
    public class BinaryPacketFormatter
    {
        private List<byte> bytes;

        public BinaryPacketFormatter()
        {
            bytes = new List<byte>();
        }

        public BinaryPacketFormatter(Commands command)
            : this()
        {
            add(command);
        }

        public void add(byte[] buffer)
        {
            bytes.AddRange(buffer);
        }

        public void add(List<byte> buffer)
        {
            bytes.AddRange(buffer);
        }

        public void add(string buffer)
        {
            byte[] buf = Serializers.serialize(buffer);
            bytes.AddRange(buf);
        }

        public void add(MIVSDK.Commands command)
        {
            byte[] buf = BitConverter.GetBytes((ushort)command);
            bytes.AddRange(buf);
        }

        public void add(int integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(uint integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(UInt16 integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(Int16 integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(float a)
        {
            byte[] buf = BitConverter.GetBytes(a);
            bytes.AddRange(buf);
        }

        public void add(UpdateDataStruct data)
        {
            bytes.AddRange(BitConverter.GetBytes(System.Diagnostics.Stopwatch.GetTimestamp()));
            bytes.AddRange(BitConverter.GetBytes(data.pos_x));
            bytes.AddRange(BitConverter.GetBytes(data.pos_y));
            bytes.AddRange(BitConverter.GetBytes(data.pos_z));

            bytes.AddRange(BitConverter.GetBytes(data.rot_x));
            bytes.AddRange(BitConverter.GetBytes(data.rot_y));
            bytes.AddRange(BitConverter.GetBytes(data.rot_z));
            bytes.AddRange(BitConverter.GetBytes(data.rot_a));

            bytes.AddRange(BitConverter.GetBytes(data.vel_x));
            bytes.AddRange(BitConverter.GetBytes(data.vel_y));
            bytes.AddRange(BitConverter.GetBytes(data.vel_z));

            bytes.AddRange(BitConverter.GetBytes(data.acc_x));
            bytes.AddRange(BitConverter.GetBytes(data.acc_y));
            bytes.AddRange(BitConverter.GetBytes(data.acc_z));

            bytes.AddRange(BitConverter.GetBytes(data.acc_rx));
            bytes.AddRange(BitConverter.GetBytes(data.acc_ry));
            bytes.AddRange(BitConverter.GetBytes(data.acc_rz));

            bytes.AddRange(BitConverter.GetBytes(data.speed));
            bytes.AddRange(BitConverter.GetBytes(data.heading));

            bytes.AddRange(BitConverter.GetBytes(data.vehicle_model));
            bytes.AddRange(BitConverter.GetBytes(data.ped_health));
            bytes.AddRange(BitConverter.GetBytes(data.vehicle_health));
            bytes.AddRange(BitConverter.GetBytes(data.weapon));
            bytes.AddRange(BitConverter.GetBytes(data.vehicle_id));
            bytes.Add((byte)data.state);
            bytes.Add((byte)data.vstate);
            bytes.AddRange(Serializers.serialize(data.nick));
        }

        public void add(Math.Vector3 a)
        {
            add(a.X);
            add(a.Y);
            add(a.Z);
        }

        public void add(Math.Vector4 a)
        {
            add(a.X);
            add(a.Y);
            add(a.Z);
            add(a.W);
        }

        public void add(Math.Quaternion a)
        {
            add(a.X);
            add(a.Y);
            add(a.Z);
            add(a.W);
        }

        public byte[] getBytes()
        {
            return bytes.ToArray();
        }
    }
}