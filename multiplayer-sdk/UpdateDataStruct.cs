using System;
using System.Collections.Generic;

namespace MIVSDK
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

        Chat_clear,
        Chat_writeLine,
        Chat_sendMessage,

        Player_setPosition,
        Player_warpIntoVehicle,
        Player_setHeading,
        Player_setVelocity,
        Player_setGravity,
        Player_setWeather,
        Player_setGameTime,

        Vehicle_setPosition,
        Vehicle_create_multi,
        vehicle_setVelocity,
        vehicle_setOrientation,
        vehicle_removePeds,
        vehicle_repair,
        vehicle_repaint
    }

    public enum PlayerState
    {
        None = 0,
        IsAiming = 1,
        IsShooting = 2,
        IsCrouching = 4,
        IsJumping = 8,
        IsRagdoll = 16
    }
    public enum VehicleState
    {
        None = 0,
        IsAccelerating = 1,
        IsBraking = 2,
        IsSterringLeft = 4,
        IsSterringRight = 8
    }

    public enum ClientState
    {
        Invalid,
        Disconnected,
        Disconnecting,
        Initializing,
        Connecting,
        Connected,
        Streaming
    }

    public class UpdateDataStruct
    {
        public long timestamp;
        public float pos_x, pos_y, pos_z, rot_x, rot_y, rot_z, rot_a, vel_x, vel_y, vel_z, speed, heading;
        public int vehicle_model, ped_health, vehicle_health, weapon;
        public uint vehicle_id;
        public string nick;
        public PlayerState state;
        public VehicleState vstate;
        public bool client_has_been_set;

        public MIVSDK.Math.Vector3 getPositionVector()
        {
            return new Math.Vector3(pos_x, pos_y, pos_z);
        }

        public MIVSDK.Math.Vector3 getVelocityVector()
        {
            return new Math.Vector3(vel_x, vel_y, vel_z);
        }

        public MIVSDK.Math.Quaternion getOrientationQuaternion()
        {
            return new Math.Quaternion(rot_x, rot_y, rot_z, rot_a);
        }

        public byte[] serialize()
        {
            List<byte> output = new List<byte>();
            output.AddRange(BitConverter.GetBytes(System.Diagnostics.Stopwatch.GetTimestamp()));
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
            output.AddRange(BitConverter.GetBytes(vehicle_health));
            output.AddRange(BitConverter.GetBytes(weapon));
            output.AddRange(BitConverter.GetBytes(vehicle_id));
            output.Add((byte)state);
            output.Add((byte)vstate);
            output.AddRange(Serializers.serialize(nick));
            return output.ToArray();
        }

        public static UpdateDataStruct unserialize(byte[] data, int offset)
        {
            UpdateDataStruct output = new UpdateDataStruct();
            output.timestamp = BitConverter.ToInt64(data, offset);
            output.pos_x = BitConverter.ToSingle(data, (offset += 8));
            output.pos_y = BitConverter.ToSingle(data, (offset += 4));
            output.pos_z = BitConverter.ToSingle(data, (offset += 4));

            output.rot_x = BitConverter.ToSingle(data, (offset += 4));
            output.rot_y = BitConverter.ToSingle(data, (offset += 4));
            output.rot_z = BitConverter.ToSingle(data, (offset += 4));
            output.rot_a = BitConverter.ToSingle(data, (offset += 4));

            output.vel_x = BitConverter.ToSingle(data, (offset += 4));
            output.vel_y = BitConverter.ToSingle(data, (offset += 4));
            output.vel_z = BitConverter.ToSingle(data, (offset += 4));

            output.speed = BitConverter.ToSingle(data, (offset += 4));
            output.heading = BitConverter.ToSingle(data, (offset += 4));

            output.vehicle_model = BitConverter.ToInt32(data, (offset += 4));
            output.ped_health = BitConverter.ToInt32(data, (offset += 4));
            output.vehicle_health = BitConverter.ToInt32(data, (offset += 4));
            output.weapon = BitConverter.ToInt32(data, (offset += 4));
            output.vehicle_id = BitConverter.ToUInt32(data, (offset += 4));
            output.state = (PlayerState)data[(offset += 4)];
            output.vstate = (VehicleState)data[(offset += 1)];
            output.nick = Serializers.unserialize_string(data, (offset += 1));
            return output;
        }

        public static UpdateDataStruct Zero
        {
            get
            {
                return new UpdateDataStruct()
                {
                    pos_x = 0,
                    pos_y = 0,
                    pos_z = 0,
                    rot_x = 0,
                    rot_y = 0,
                    rot_z = 0,
                    rot_a = 0,
                    heading = 0,
                    ped_health = 0,
                    speed = 0,
                    vehicle_health = 0,
                    vehicle_id = 0,
                    vehicle_model = 0,
                    vel_x = 0,
                    vel_y = 0,
                    vel_z = 0,
                    state = PlayerState.None,
                    vstate = VehicleState.None,
                    nick = "",
                    client_has_been_set = false
                };
            }
            set { }
        }
    }
}