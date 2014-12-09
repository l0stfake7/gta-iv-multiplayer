using SharpDX;
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
        Player_setModel,
        Player_setVelocity,
        Player_setHealth,
        Player_damage,
        Player_freeze,
        Player_unfreeze,
        Player_setVirtualWorld,

        Global_setPlayerName,
        Global_setPlayerModel,
        Global_setPlayerPedText,
        Global_createPlayer,
        Global_removePlayer,

        Vehicle_setPosition,
        Vehicle_create,
        Vehicle_setModel,
        Vehicle_setVelocity,
        Vehicle_setOrientation,
        Vehicle_removePeds,
        Vehicle_repair,
        Vehicle_repaint,
        Vehicle_setVirtualWorld,

        TextView_create,
        TextView_destroy,
        TextView_update,

        TextureView_create,
        TextureView_destroy,
        TextureView_update,

        LineView_create,
        LineView_destroy,
        LineView_update,

        RectangleView_create,
        RectangleView_destroy,
        RectangleView_update,

        Client_setVirtualWorld,
        Client_setGfxInterval,
        Client_setBrodcastInterval,
        Client_setSlowInterval,
        Client_pauseBroadcast,
        Client_resumeBroadcast,
        Client_clearChat,
        Client_consoleWrite,

        Game_fadeScreenIn,
        Game_fadeScreenOut,
        Game_showLoadingScreen,
        Game_hideLoadingScreen,
        Game_setGravity,
        Game_setWeather,
        Game_setGameTime,

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
        NPC_setVirtualWorld,

        Camera_setPosition,
        Camera_getPosition,

        Camera_setDirection,
        Camera_getDirection,

        Camera_setOrientation,
        Camera_getOrientation,

        Camera_setFOV,
        Camera_getFOV,

        Camera_lookAt,
        Camera_reset,
        Camera_moveSmooth,

        InternalClient_requestSpawn,
        InternalClient_finishSpawn,
        
        Request_getSelectedPlayer,
        Request_getCameraPosition,
        Request_getCameraDirection,
        Request_isObjectVisible,
        Request_worldToScreen,

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
        public float pos_x, pos_y, pos_z, rot_x, rot_y, rot_z, rot_a, vel_x, vel_y, vel_z, heading;
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
                    vehicle_health = 0,
                    vehicle_id = 0,
                    vehicle_model = 0,
                    vel_x = 0,
                    vel_y = 0,
                    vel_z = 0,
                    state = PlayerState.None,
                    vstate = VehicleState.None,
                    client_has_been_set = false
                };
            }
            set
            {
            }
        }

        public Quaternion getOrientationQuaternion()
        {
            return new Quaternion(rot_x, rot_y, rot_z, rot_a);
        }

        public Vector3 getPositionVector()
        {
            return new Vector3(pos_x, pos_y, pos_z);
        }

        public Vector3 getVelocityVector()
        {
            return new Vector3(vel_x, vel_y, vel_z);
        }
    }
}