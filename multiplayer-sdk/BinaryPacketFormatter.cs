using System;
using System.Collections.Generic;
using SharpDX;

namespace MIVSDK
{
    public class BinaryPacketFormatter
    {
        private List<byte> bytes;
        public enum Types
        {
            Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Raw, Command, String, Single, UpdateDataStruct, Vector2, Vector3, Vector4, Quaternion, Double
        }

        public BinaryPacketFormatter()
        {
            bytes = new List<byte>();
        }

        public BinaryPacketFormatter(Commands command)
            : this()
        {
            add(command);
        }
        public BinaryPacketFormatter(Commands command, params object[] args)
            : this()
        {
            add(command);
            foreach (var obj in args)
            {
                if (obj is Byte) add((Byte)obj);

                if (obj is Int16) add((Int16)obj);
                if (obj is UInt16) add((UInt16)obj);

                if (obj is Int32) add((Int32)obj);
                if (obj is UInt32) add((UInt32)obj);

                if (obj is Int64) add((Int64)obj);
                if (obj is UInt64) add((UInt64)obj);

                if (obj is byte[]) add((byte[])obj);
                if (obj is List<byte>) add((List<byte>)obj);
                if (obj is string) add((string)obj);
                if (obj is float) add((float)obj);
                if (obj is Vector3) add((Vector3)obj);
                if (obj is Vector4) add((Vector4)obj);
                if (obj is Quaternion) add((Quaternion)obj);
            }
        }

        public void add(byte[] buffer)
        {
            bytes.Add((byte)Types.Raw);
            bytes.AddRange(buffer);
        }

        public void add(List<byte> buffer)
        {
            bytes.Add((byte)Types.Raw);
            bytes.AddRange(buffer);
        }

        public void add(string buffer)
        {
            bytes.Add((byte)Types.String);
            byte[] buf = Serializers.serialize(buffer);
            bytes.AddRange(buf);
        }

        public void add(MIVSDK.Commands command)
        {
            bytes.Add((byte)Types.Command);
            byte[] buf = BitConverter.GetBytes((ushort)command);
            bytes.AddRange(buf);
        }

        public void add(int integer)
        {
            bytes.Add((byte)Types.Int32);
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(uint integer)
        {
            bytes.Add((byte)Types.UInt32);
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(UInt16 integer)
        {
            bytes.Add((byte)Types.UInt16);
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(Int16 integer)
        {
            bytes.Add((byte)Types.Int16);
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }
        public void add(UInt64 integer)
        {
            bytes.Add((byte)Types.UInt64);
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(Int64 integer)
        {
            bytes.Add((byte)Types.Int64);
            byte[] buf = BitConverter.GetBytes(integer);
            bytes.AddRange(buf);
        }

        public void add(float a)
        {
            bytes.Add((byte)Types.Single);
            byte[] buf = BitConverter.GetBytes(a);
            bytes.AddRange(buf);
        }

        public void add(UpdateDataStruct data)
        {
            bytes.Add((byte)Types.UpdateDataStruct);
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

            bytes.AddRange(BitConverter.GetBytes(data.heading));

            bytes.AddRange(BitConverter.GetBytes(data.vehicle_model));
            bytes.AddRange(BitConverter.GetBytes(data.ped_health));
            bytes.AddRange(BitConverter.GetBytes(data.vehicle_health));
            bytes.AddRange(BitConverter.GetBytes(data.weapon));
            bytes.AddRange(BitConverter.GetBytes(data.vehicle_id));
            bytes.Add((byte)data.state);
            bytes.Add((byte)data.vstate);
        }

        public void add(Vector3 a)
        {
            bytes.Add((byte)Types.Vector3);
            add(a.X);
            add(a.Y);
            add(a.Z);
        }

        public void add(Vector4 a)
        {
            bytes.Add((byte)Types.Vector4);
            add(a.X);
            add(a.Y);
            add(a.Z);
            add(a.W);
        }

        public void add(Quaternion a)
        {
            bytes.Add((byte)Types.Quaternion);
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