using System;
using SharpDX;

namespace MIVSDK
{
    public class BinaryPacketReader
    {
        private byte[] buffer;
        private int position, length;

        public BinaryPacketReader(byte[] buffer)
        {
            this.buffer = buffer;
            position = 0;
            length = readInt32();
        }

        public bool canRead()
        {
            return position < length;
        }

        public Byte readByte()
        {
            var data = buffer[position];
            position++;
            return data;
        }

        public Commands readCommand()
        {
            return (Commands)readUInt16();
        }

        public Double readDouble()

        {
            var data = BitConverter.ToDouble(buffer, position);
            position += sizeof(Double);
            return data;
        }

        public Int16 readInt16()
        {
            var data = BitConverter.ToInt16(buffer, position);
            position += sizeof(Int16);
            return data;
        }

        public Int32 readInt32()
        {
            var data = BitConverter.ToInt32(buffer, position);
            position += sizeof(Int32);
            return data;
        }

        public Int64 readInt64()
        {
            var data = BitConverter.ToInt64(buffer, position);
            position += sizeof(Int64);
            return data;
        }

        public Quaternion readQuaternion()
        {
            return new Quaternion(readSingle(), readSingle(), readSingle(), readSingle());
        }

        public Single readSingle()
        {
            var data = BitConverter.ToSingle(buffer, position);
            position += sizeof(Single);
            return data;
        }

        public String readString()
        {
            int tmpint = 0;
            var data = Serializers.unserialize_string(buffer, position, out tmpint);
            position += tmpint;
            return data;
        }

        public UInt16 readUInt16()
        {
            var data = BitConverter.ToUInt16(buffer, position);
            position += sizeof(UInt16);
            return data;
        }

        public UInt32 readUInt32()
        {
            var data = BitConverter.ToUInt32(buffer, position);
            position += sizeof(UInt32);
            return data;
        }

        public UInt64 readUInt64()
        {
            var data = BitConverter.ToUInt64(buffer, position);
            position += sizeof(UInt64);
            return data;
        }

        public UpdateDataStruct readUpdateStruct()
        {
            UpdateDataStruct output = new UpdateDataStruct();
            output.timestamp = readInt64();
            output.pos_x = readSingle();
            output.pos_y = readSingle();
            output.pos_z = readSingle();

            output.rot_x = readSingle();
            output.rot_y = readSingle();
            output.rot_z = readSingle();
            output.rot_a = readSingle();

            output.vel_x = readSingle();
            output.vel_y = readSingle();
            output.vel_z = readSingle();

            output.heading = readSingle();

            output.vehicle_model = readInt32();
            output.ped_health = readInt32();
            output.vehicle_health = readInt32();
            output.weapon = readInt32();
            output.vehicle_id = readUInt32();

            output.state = (PlayerState)readByte();
            output.vstate = (VehicleState)readByte();
            
            return output;
        }

        public Vector3 readVector3()
        {
            return new Vector3(readSingle(), readSingle(), readSingle());
        }

        public Vector4 readVector4()
        {
            return new Vector4(readSingle(), readSingle(), readSingle(), readSingle());
        }
    }
}