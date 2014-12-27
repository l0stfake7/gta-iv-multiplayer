// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using SharpDX;
using System;
using System.Collections.Generic;

namespace MIVSDK
{
    public class BinaryPacketFormatter
    {
        private List<byte> bytes;

        public BinaryPacketFormatter()
        {
            this.bytes = new List<byte>();
        }

        public BinaryPacketFormatter(Commands command)
            : this()
        {
            this.Add(command);
        }

        public BinaryPacketFormatter(Commands command, params object[] args)
            : this()
        {
            this.Add(command);
            foreach (var obj in args)
            {
                if (obj is Byte) this.Add((Byte)obj);

                if (obj is Int16) this.Add((Int16)obj);
                if (obj is UInt16) this.Add((UInt16)obj);

                if (obj is Int32) this.Add((Int32)obj);
                if (obj is UInt32) this.Add((UInt32)obj);

                if (obj is Int64) this.Add((Int64)obj);
                if (obj is UInt64) this.Add((UInt64)obj);

                if (obj is byte[]) this.Add((byte[])obj);
                if (obj is List<byte>) this.Add((List<byte>)obj);
                if (obj is string) this.Add((string)obj);
                if (obj is float) this.Add((float)obj);
                if (obj is Vector3) this.Add((Vector3)obj);
                if (obj is Vector4) this.Add((Vector4)obj);
                if (obj is Quaternion) this.Add((Quaternion)obj);
            }
        }

        public enum Types
        {
            Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Raw, Command, String, Single, UpdateDataStruct, Vector2, Vector3, Vector4, Quaternion, Double
        }

        public void Add(byte[] buffer)
        {
            this.bytes.Add((byte)Types.Raw);
            this.bytes.AddRange(buffer);
        }

        public void Add(List<byte> buffer)
        {
            this.bytes.Add((byte)Types.Raw);
            this.bytes.AddRange(buffer);
        }

        public void Add(string buffer)
        {
            this.bytes.Add((byte)Types.String);
            byte[] buf = Serializers.serialize(buffer);
            this.bytes.AddRange(buf);
        }

        public void Add(MIVSDK.Commands command)
        {
            this.bytes.Add((byte)Types.Command);
            byte[] buf = BitConverter.GetBytes((ushort)command);
            this.bytes.AddRange(buf);
        }

        public void Add(int integer)
        {
            this.bytes.Add((byte)Types.Int32);
            byte[] buf = BitConverter.GetBytes(integer);
            this.bytes.AddRange(buf);
        }

        public void Add(uint integer)
        {
            this.bytes.Add((byte)Types.UInt32);
            byte[] buf = BitConverter.GetBytes(integer);
            this.bytes.AddRange(buf);
        }

        public void Add(UInt16 integer)
        {
            this.bytes.Add((byte)Types.UInt16);
            byte[] buf = BitConverter.GetBytes(integer);
            this.bytes.AddRange(buf);
        }

        public void Add(Int16 integer)
        {
            this.bytes.Add((byte)Types.Int16);
            byte[] buf = BitConverter.GetBytes(integer);
            this.bytes.AddRange(buf);
        }

        public void Add(UInt64 integer)
        {
            this.bytes.Add((byte)Types.UInt64);
            byte[] buf = BitConverter.GetBytes(integer);
            this.bytes.AddRange(buf);
        }

        public void Add(Int64 integer)
        {
            this.bytes.Add((byte)Types.Int64);
            byte[] buf = BitConverter.GetBytes(integer);
            this.bytes.AddRange(buf);
        }

        public void Add(float a)
        {
            this.bytes.Add((byte)Types.Single);
            byte[] buf = BitConverter.GetBytes(a);
            this.bytes.AddRange(buf);
        }

        public void Add(UpdateDataStruct data)
        {
            this.bytes.Add((byte)Types.UpdateDataStruct);
            this.bytes.AddRange(BitConverter.GetBytes(System.Diagnostics.Stopwatch.GetTimestamp()));
            this.bytes.AddRange(BitConverter.GetBytes(data.pos_x));
            this.bytes.AddRange(BitConverter.GetBytes(data.pos_y));
            this.bytes.AddRange(BitConverter.GetBytes(data.pos_z));

            this.bytes.AddRange(BitConverter.GetBytes(data.rot_x));
            this.bytes.AddRange(BitConverter.GetBytes(data.rot_y));
            this.bytes.AddRange(BitConverter.GetBytes(data.rot_z));
            this.bytes.AddRange(BitConverter.GetBytes(data.rot_a));

            this.bytes.AddRange(BitConverter.GetBytes(data.vel_x));
            this.bytes.AddRange(BitConverter.GetBytes(data.vel_y));
            this.bytes.AddRange(BitConverter.GetBytes(data.vel_z));
            this.bytes.AddRange(BitConverter.GetBytes(data.camdir_x));
            this.bytes.AddRange(BitConverter.GetBytes(data.camdir_y));
            this.bytes.AddRange(BitConverter.GetBytes(data.camdir_z));

            this.bytes.AddRange(BitConverter.GetBytes(data.heading));

            this.bytes.AddRange(BitConverter.GetBytes(data.vehicle_model));
            this.bytes.AddRange(BitConverter.GetBytes(data.ped_health));
            this.bytes.AddRange(BitConverter.GetBytes(data.vehicle_health));
            this.bytes.AddRange(BitConverter.GetBytes(data.weapon));
            this.bytes.AddRange(BitConverter.GetBytes(data.vehicle_id));
            this.bytes.Add((byte)data.state);
            this.bytes.Add((byte)data.vstate);
        }

        public void Add(Vector3 a)
        {
            this.bytes.Add((byte)Types.Vector3);
            this.Add(a.X);
            this.Add(a.Y);
            this.Add(a.Z);
        }

        public void Add(Vector4 a)
        {
            this.bytes.Add((byte)Types.Vector4);
            this.Add(a.X);
            this.Add(a.Y);
            this.Add(a.Z);
            this.Add(a.W);
        }

        public void Add(Quaternion a)
        {
            this.bytes.Add((byte)Types.Quaternion);
            this.Add(a.X);
            this.Add(a.Y);
            this.Add(a.Z);
            this.Add(a.W);
        }

        public byte[] getBytes()
        {
            return this.bytes.ToArray();
        }
    }
}