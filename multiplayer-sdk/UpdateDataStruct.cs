using System;
using System.Collections.Generic;
using System.IO;

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
        Vehicle_setVelocity,
        Vehicle_setOrientation,
        Vehicle_removePeds,
        Vehicle_repair,
        Vehicle_repaint,

        TextView_create,
        TextView_destroy,
        TextView_update,

        NPCDialog_show,
        NPCDialog_hide,
        NPCDialog_sendResponse,

        NPC_create,
        NPC_destroy,
        NPC_update,
        NPC_walkTo,
        NPC_setPosition,
        NPC_enterVehicle,
        NPC_leaveVehicle,
        NPC_playAnimation,

        Keys_down,
        Keys_up,
    }

    public enum PlayerState
    {
        None = 0,
        IsAiming = 1,
        IsShooting = 2,
        IsCrouching = 4,
        IsJumping = 8,
        IsRagdoll = 16,
        IsPassenger1 = 32,
        IsPassenger2 = 64,
        IsPassenger3 = 128,
    }
    public enum VehicleState
    {
        None = 0,
        IsAccelerating = 1,
        IsBraking = 2,
        IsSterringLeft = 4,
        IsSterringRight = 8,
        IsAsPassenger = 16,
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
            var mstream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(mstream);
            UpdateDataStruct output = new UpdateDataStruct();
            output.timestamp = reader.ReadInt64();
            output.pos_x = reader.ReadSingle();
            output.pos_y = reader.ReadSingle();
            output.pos_z = reader.ReadSingle();

            output.rot_x = reader.ReadSingle();
            output.rot_y = reader.ReadSingle();
            output.rot_z = reader.ReadSingle();
            output.rot_a = reader.ReadSingle();

            output.vel_x = reader.ReadSingle();
            output.vel_y = reader.ReadSingle();
            output.vel_z = reader.ReadSingle();

            output.speed = reader.ReadSingle();
            output.heading = reader.ReadSingle();

            output.vehicle_model = reader.ReadInt32();
            output.ped_health = reader.ReadInt32();
            output.vehicle_health = reader.ReadInt32();
            output.weapon = reader.ReadInt32();
            output.vehicle_id = reader.ReadUInt32();

            output.state = (PlayerState)reader.ReadByte();
            output.vstate = (VehicleState)reader.ReadByte();

            output.nick = Serializers.unserialize_string(data, (int)mstream.Position);

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