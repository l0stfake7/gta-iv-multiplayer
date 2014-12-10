using SharpDX;
using System;

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
            length = BitConverter.ToInt32(buffer, position);
            position += sizeof(Int32);
        }

        public bool canRead()
        {
            return position < length;
        }

        public Byte readByte(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck)
            {
                var t = readNextType();
                if (t != BinaryPacketFormatter.Types.Raw && t != BinaryPacketFormatter.Types.Byte) throw new Exception("Desynchronization Byte");
            }
            var data = buffer[position];
            position++;
            return data;
        }

        public Commands readCommand(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.Command) throw new Exception("Desynchronization Commands");
            var data = BitConverter.ToInt16(buffer, position);
            position += sizeof(Int16);
            return (Commands)data;
        }

        public Double readDouble(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.Double) throw new Exception("Desynchronization Double");
            var data = BitConverter.ToDouble(buffer, position);
            position += sizeof(Double);
            return data;
        }

        public Int16 readInt16(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.Int16) throw new Exception("Desynchronization Int16");
            var data = BitConverter.ToInt16(buffer, position);
            position += sizeof(Int16);
            return data;
        }

        public Int32 readInt32(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.Int32) throw new Exception("Desynchronization Int32");
            var data = BitConverter.ToInt32(buffer, position);
            position += sizeof(Int32);
            return data;
        }

        public Int64 readInt64(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.Int64) throw new Exception("Desynchronization Int64");
            var data = BitConverter.ToInt64(buffer, position);
            position += sizeof(Int64);
            return data;
        }

        public BinaryPacketFormatter.Types readNextType()
        {
            var data = buffer[position];
            position++;
            return (BinaryPacketFormatter.Types)data;
        }

        public Quaternion readQuaternion(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.Quaternion) throw new Exception("Desynchronization Quaternion");
            return new Quaternion(readSingle(), readSingle(), readSingle(), readSingle());
        }

        public Single readSingle(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.Single) throw new Exception("Desynchronization Single");
            var data = BitConverter.ToSingle(buffer, position);
            position += sizeof(Single);
            return data;
        }

        public String readString(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.String) throw new Exception("Desynchronization String");
            int tmpint = 0;
            var data = Serializers.unserialize_string(buffer, position, out tmpint);
            position += tmpint;
            return data;
        }

        public UInt16 readUInt16(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.UInt16) throw new Exception("Desynchronization UInt16");
            var data = BitConverter.ToUInt16(buffer, position);
            position += sizeof(UInt16);
            return data;
        }

        public UInt32 readUInt32(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.UInt32) throw new Exception("Desynchronization UInt32");
            var data = BitConverter.ToUInt32(buffer, position);
            position += sizeof(UInt32);
            return data;
        }

        public UInt64 readUInt64(bool skipTypeCheck = false)
        {
            if (!skipTypeCheck && readNextType() != BinaryPacketFormatter.Types.UInt64) throw new Exception("Desynchronization UInt64");
            var data = BitConverter.ToUInt64(buffer, position);
            position += sizeof(UInt64);
            return data;
        }

        public UpdateDataStruct readUpdateStruct()
        {
            if (readNextType() != BinaryPacketFormatter.Types.UpdateDataStruct) throw new Exception("Desynchronization UpdateDataStruct");
            UpdateDataStruct output = new UpdateDataStruct();
            output.timestamp = readInt64(true);
            output.pos_x = readSingle(true);
            output.pos_y = readSingle(true);
            output.pos_z = readSingle(true);

            output.rot_x = readSingle(true);
            output.rot_y = readSingle(true);
            output.rot_z = readSingle(true);
            output.rot_a = readSingle(true);

            output.vel_x = readSingle(true);
            output.vel_y = readSingle(true);
            output.vel_z = readSingle(true);

            output.heading = readSingle(true);

            output.vehicle_model = readInt32(true);
            output.ped_health = readInt32(true);
            output.vehicle_health = readInt32(true);
            output.weapon = readInt32(true);
            output.vehicle_id = readUInt32(true);

            output.state = (PlayerState)readByte(true);
            output.vstate = (VehicleState)readByte(true);

            return output;
        }

        public Vector3 readVector3()
        {
            if (readNextType() != BinaryPacketFormatter.Types.Vector3) throw new Exception("Desynchronization Vector3");
            return new Vector3(readSingle(), readSingle(), readSingle());
        }

        public Vector4 readVector4()
        {
            if (readNextType() != BinaryPacketFormatter.Types.Vector4) throw new Exception("Desynchronization Vector4");
            return new Vector4(readSingle(), readSingle(), readSingle(), readSingle());
        }
    }
}