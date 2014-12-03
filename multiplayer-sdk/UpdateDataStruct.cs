namespace MIVSDK
{
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
        Player_setHealth,
        Player_damage,

        Vehicle_setPosition,
        Vehicle_create,
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
        NPC_driveTo,
        NPC_runTo,
        NPC_followPlayer,
        NPC_shootAt,
        NPC_setPosition,
        NPC_setHeading,
        NPC_setName,
        NPC_setModel,
        NPC_setWeapon,
        NPC_setImmortal,
        NPC_enterVehicle,
        NPC_leaveVehicle,
        NPC_playAnimation,

        Camera_setPosition,
        Camera_lookAt,
        Camera_sstDirection,
        Camera_setOrientation,
        Camera_setFOV,
        Camera_reset,
        Camera_moveSmooth,

        InternalClient_requestSpawn,
        InternalClient_finishSpawn,

        Request_getSelectedPlayer,
        Request_getCameraPosition,

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
        IsEnteringVehicle = 32,
    }

    public class UpdateDataStruct
    {
        public bool client_has_been_set;
        public string nick;
        public float pos_x, pos_y, pos_z, rot_x, rot_y, rot_z, rot_a, vel_x, vel_y, vel_z, acc_x, acc_y, acc_z, acc_rx, acc_ry, acc_rz, speed, heading;
        public PlayerState state;
        public long timestamp;
        public uint vehicle_id;
        public int vehicle_model, ped_health, vehicle_health, weapon;
        public VehicleState vstate;

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
                    acc_x = 0,
                    acc_y = 0,
                    acc_z = 0,
                    acc_rx = 0,
                    acc_ry = 0,
                    acc_rz = 0,
                    state = PlayerState.None,
                    vstate = VehicleState.None,
                    nick = "",
                    client_has_been_set = false
                };
            }
            set
            {
            }
        }

        public MIVSDK.Math.Quaternion getOrientationQuaternion()
        {
            return new Math.Quaternion(rot_x, rot_y, rot_z, rot_a);
        }

        public MIVSDK.Math.Vector3 getPositionVector()
        {
            return new Math.Vector3(pos_x, pos_y, pos_z);
        }

        public MIVSDK.Math.Vector3 getVelocityVector()
        {
            return new Math.Vector3(vel_x, vel_y, vel_z);
        }
    }
}