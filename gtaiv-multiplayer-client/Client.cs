using GTA;
using MIVSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace MIVClient
{
    public partial class Client : Script
    {
        public TcpClient client;
        public string nick;
        public static Client instance;

        public PlayerPedController pedController;
        public PlayerVehicleController playerVehicleController;
        public VehicleController vehicleController;
        public ServerConnection serverConnection;
        public KeyboardHandler keyboardHandler;
        public ChatController chatController;
        public PerFrameRenderer perFrameRenderer;

        private Queue<Action> actionQueue;

        public void enqueueAction(Action action)
        {
            actionQueue.Enqueue(action);
        }

        public ClientState currentState = ClientState.Initializing;

        public Client()
        {
            actionQueue = new Queue<Action>();
            instance = this;
            pedController = new PlayerPedController();
            vehicleController = new VehicleController();
            playerVehicleController = new PlayerVehicleController();
            chatController = new ChatController(this);
            keyboardHandler = new KeyboardHandler(this);
            currentState = ClientState.Initializing;
            Interval = 40;
            this.Tick += new EventHandler(this.eventOnTick);

            currentState = ClientState.Disconnected;
            System.IO.File.WriteAllText("multiv-log.txt", "");
            BindConsoleCommand("savepos", Client_ScriptCommand);
            BindConsoleCommand("tp2wp", delegate(ParameterCollection Parameters)
            {
                Blip wp = GTA.Game.GetWaypoint();
                if (wp != null)
                {
                    var pos = wp.Position;
                    Player.TeleportTo(pos.X, pos.Y);
                }
            });
            perFrameRenderer = new PerFrameRenderer(this);
        }

        private void Client_ScriptCommand(ParameterCollection Parameters)
        {
            if (Player.Character.isInVehicle())
            {
                System.IO.File.AppendAllText("saved.txt",
                    "pos = " + Player.Character.CurrentVehicle.Position.X + "f, " +
                    Player.Character.CurrentVehicle.Position.Y + "f, " +
                    Player.Character.CurrentVehicle.Position.Z + "f; quaternion = " +
                    Player.Character.CurrentVehicle.RotationQuaternion.X + "f, " +
                    Player.Character.CurrentVehicle.RotationQuaternion.Y + "f, " +
                    Player.Character.CurrentVehicle.RotationQuaternion.Z + "f, " +
                    Player.Character.CurrentVehicle.RotationQuaternion.W + "f; model = " +
                    Player.Character.CurrentVehicle.Model.GetHashCode() + "; //" +
                    (Parameters.Count > 0 ? Parameters[0].ToString() : "") + "\r\n");
            }
            else
            {
                System.IO.File.AppendAllText("saved.txt",
                    "pos = " + Player.Character.Position.X + "f, " +
                    Player.Character.Position.Y + "f, " +
                    Player.Character.Position.Z + "f; heading = " + Player.Character.Heading + "f; //" +
                    (Parameters.Count > 0 ? Parameters[0].ToString() : "") + "\r\n");
            }
            log("Saved");
        }

        public static Client getInstance()
        {
            return instance;
        }

        public static void log(string text)
        {
            System.IO.File.AppendAllText("multiv-log.txt", text + "\r\n");
            //GTA.Game.DisplayText(text);
        }

        public Ped getPlayerPed()
        {
            return Player.Character;
        }

        public Player getPlayer()
        {
            return Player;
        }

        private Dictionary<int, GTA.Vector3> bindPoints;

        public void saveBindPoint(int id)
        {
            if (bindPoints == null) bindPoints = new Dictionary<int, Vector3>();
            if (bindPoints.ContainsKey(id)) bindPoints[id] = getPlayerPed().Position;
            else bindPoints.Add(id, getPlayerPed().Position);
        }

        public void teleportToBindPoint(int id)
        {
            if (bindPoints == null) bindPoints = new Dictionary<int, Vector3>();
            if (bindPoints.ContainsKey(id)) getPlayerPed().Position = bindPoints[id];
        }

        /*
         * Structure of packet is as follows
         * 0x00               0x01               0x05
         * [(ushort)COMMAND_ID] [(int)DATA_LENGTH] [(mixed)DATA]
         *
         */

        public static UpdateDataStruct currentData;

        private void eventOnTick(object sender, EventArgs e)
        {
            try
            {
                while (actionQueue.Count > 0)
                {
                    actionQueue.Dequeue().Invoke();
                }
            }
            catch (Exception ex)
            {
                log("Failed executing action queue with message " + ex.Message);
            }
            try
            {
                Player.Character.Health = Player.Character.Health > 100 ? 100 : Player.Character.Health + 8;
                Player.Character.Invincible = true;
            }
            catch (Exception ex)
            {
                log("Failed setting player health " + ex.Message);
            }
            if (currentState == ClientState.Connected)
            {
                try
                {
                    Player.WantedLevel = 0;
                    UpdateDataStruct data = new UpdateDataStruct();
                    data.nick = nick;
                    //log("my X is " + data.pos_x.ToString());
                    if (Player.Character.isInVehicle())
                    {
                        Vector3 pos = Player.Character.CurrentVehicle.Position;
                        data.pos_x = pos.X;
                        data.pos_y = pos.Y;
                        data.pos_z = pos.Z;

                        Vector3 vel = Player.Character.CurrentVehicle.Velocity;
                        data.vel_x = vel.X;
                        data.vel_y = vel.Y;
                        data.vel_z = vel.Z;

                        Quaternion quat = Player.Character.CurrentVehicle.RotationQuaternion;
                        data.rot_x = quat.X;
                        data.rot_y = quat.Y;
                        data.rot_z = quat.Z;
                        data.rot_a = quat.W;

                        data.vehicle_model = Player.Character.CurrentVehicle.Model.Hash;
                        data.vehicle_health = Player.Character.CurrentVehicle.Health;
                        data.vehicle_id = vehicleController.getByVehicle(Player.Character.CurrentVehicle).id;
                        data.ped_health = Player.Character.Health;
                        data.speed = Player.Character.CurrentVehicle.Speed;
                        data.heading = Player.Character.CurrentVehicle.Heading;
                        if (vehicleController.streamer.vehicles.Count(a => a.gameReference != null && a.gameReference == Player.Character.CurrentVehicle) > 0)
                        {
                            var cveh = vehicleController.streamer.vehicles.First(a => a.gameReference != null && a.gameReference == Player.Character.CurrentVehicle);
                            cveh.position = pos;
                            cveh.orientation = quat;
                        }
                        data.state = 0;
                    }
                    else
                    {
                        Vector3 pos = Player.Character.Position;
                        data.pos_x = pos.X;
                        data.pos_y = pos.Y;
                        data.pos_z = pos.Z;

                        Vector3 vel = Player.Character.Velocity;
                        data.vel_x = vel.X;
                        data.vel_y = vel.Y;
                        data.vel_z = vel.Z;

                        data.rot_x = 0;
                        data.rot_y = 0;
                        data.rot_z = 0;
                        data.rot_a = 0;

                        data.vehicle_model = 0;
                        data.vehicle_health = 0;
                        data.vehicle_id = 0;
                        data.ped_health = Player.Character.Health;
                        data.speed = 0;
                        data.heading = Player.Character.Heading;
                        data.weapon = Player.Character.Weapons.CurrentType.GetHashCode();
                        data.state = 0;
                        data.state |= Player.Character.isShooting ? PlayerState.IsShooting : 0;
                        data.state |= Game.isGameKeyPressed(GameKey.Aim) ? PlayerState.IsAiming : 0;
                        data.state |= Game.isGameKeyPressed(GameKey.Crouch) ? PlayerState.IsCrouching : 0;
                        data.state |= Game.isGameKeyPressed(GameKey.Jump) ? PlayerState.IsJumping : 0;
                        data.state |= Game.isGameKeyPressed(GameKey.Attack) ? PlayerState.IsShooting : 0;
                    }
                    data.vstate = 0;
                    data.vstate |= Game.isGameKeyPressed(GameKey.MoveForward) ? VehicleState.IsAccelerating : 0;
                    data.vstate |= Game.isGameKeyPressed(GameKey.MoveBackward) ? VehicleState.IsBraking : 0;
                    data.vstate |= Game.isGameKeyPressed(GameKey.MoveLeft) ? VehicleState.IsSterringLeft : 0;
                    data.vstate |= Game.isGameKeyPressed(GameKey.MoveRight) ? VehicleState.IsSterringRight : 0;
                    serverConnection.streamWrite(Commands.UpdateData);
                    serverConnection.streamWrite(data);
                    serverConnection.streamFlush();
                    currentData = data;
                }
                catch (Exception ex)
                {
                    log("Failed sending new player data with message " + ex.Message);
                }
                try
                {
                    vehicleController.streamer.update();
                    pedController.streamer.update();
                }
                catch (Exception ex)
                {
                    log("Failed updating streamers with message " + ex.Message);
                }
                //writeChat("Wrote");
                //log("sent data");
                // process players
                for (int i = 0; i < serverConnection.playersdata.Keys.Count; i++)
                {
                    var elemKey = serverConnection.playersdata.Keys.ToArray()[i];
                    var elemValue = serverConnection.playersdata[elemKey];

                    if (elemValue.client_has_been_set) continue;
                    else elemValue.client_has_been_set = true;

                    //bool in_vehicle = elemValue.vehicle_id > 0;
                    var posnew = new Vector3(elemValue.pos_x, elemValue.pos_y, elemValue.pos_z - 1.0f);
                    StreamedPed ped = pedController.getById(elemKey, posnew);
                    try
                    {
                        updateVehicle(elemValue, ped);
                    }
                    catch (Exception ex)
                    {
                        log("Failed updating streamed vehicle data for player " + ex.Message);
                    }
                    try
                    {
                        updatePed(elemValue, ped);
                    }
                    catch (Exception ex)
                    {
                        log("Failed updating streamed ped data for player " + ex.Message);
                    }
                }
            }
            if (currentState == ClientState.Connecting)
            {
                serverConnection.streamWrite(Commands.Connect);
                serverConnection.streamWrite(nick.Length);
                serverConnection.streamWrite(nick);
                serverConnection.streamFlush();
                currentState = ClientState.Connected;
                chatController.writeChat("Connected");
            }
        }
    }
}