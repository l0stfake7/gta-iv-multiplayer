using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Forms;
using System.Net;
using System.Net.Sockets;
using MIVSDK;
using System.Drawing;

namespace MIVClient
{
    public class Client : Script
    {
        public TcpClient client;
        public string nick;
        public static Client instance;

        public PlayerPedController pedController;
        public PlayerVehicleController vehicleController;
        public ServerConnection serverConnection;
        public KeyboardHandler keyboardHandler;
        public ChatController chatController;
        public PerFrameRenderer perFrameRenderer;


        public ClientState currentState = ClientState.Initializing;

        public Client()
        {
            instance = this;
            pedController = new PlayerPedController();
            vehicleController = new PlayerVehicleController();
            chatController = new ChatController(this);
            keyboardHandler = new KeyboardHandler(this);
            currentState = ClientState.Initializing;
            Interval = 70;
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
                    Player.Character.CurrentVehicle.RotationQuaternion.W + "f; //" +
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
            GTA.Game.DisplayText(text);
        }

        public Ped getPlayerPed()
        {
            return Player.Character;
        }
        public Player getPlayer()
        {
            return Player;
        }

        Dictionary<int, Vector3> bindPoints;
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
                Player.Character.Health = Player.Character.Health > 100 ? 100 : Player.Character.Health + 8;
                Player.Character.Invincible = true;
                //Game.Console.Print("HP: " + Player.Character.Health + ", Status: " + Enum.GetName(currentState.GetType(), currentState));
                //dlabel.Text = "HP: " + Player.Character.Health + ", Status: " + Enum.GetName(currentState.GetType(), currentState);
                if (currentState == ClientState.Connected)
                {
                    UpdateDataStruct data = new UpdateDataStruct();
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
                        data.veh_health = Player.Character.CurrentVehicle.Health;
                        data.ped_health = Player.Character.Health;
                        data.speed = Player.Character.CurrentVehicle.Speed;
                        data.heading = Player.Character.CurrentVehicle.Heading;
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
                        data.veh_health = 0;
                        data.ped_health = Player.Character.Health;
                        data.speed = 0;
                        data.heading = Player.Character.Heading;
                    }
                    serverConnection.streamWrite(Commands.UpdateData);
                    serverConnection.streamWrite(data);
                    serverConnection.streamFlush();
                    currentData = data;
                    //writeChat("Wrote");
                    //log("sent data");
                    // process players
                    foreach (var elem in serverConnection.playersdata)
                    {
                        try
                        {
                            bool in_vehicle = elem.Value.vehicle_model > 0;
                            //log("TRYING SET positioning");
                            //log(elem.Value.pos_x.ToString());
                            var posnew = new Vector3(elem.Value.pos_x, elem.Value.pos_y, elem.Value.pos_z);
                            Ped ped = pedController.getById(elem.Key, posnew);
                            ped.Invincible = true;
                            ped.WillFlyThroughWindscreen = false;
                            ped.RelationshipGroup = RelationshipGroup.NetworkPlayer_01;

                            if (elem.Value.nick != null && elem.Value.nick.Length > 0)
                            {
                                ped.GiveFakeNetworkName(elem.Value.nick, System.Drawing.Color.Red);
                            }

                            if (!in_vehicle)
                            {
                                vehicleController.destroy(elem.Key);
                                if (ped.isInVehicle())
                                {
                                    ped.CurrentVehicle.PassengersLeaveVehicle(true);
                                }
                                ped.Position = posnew;
                                ped.Heading = elem.Value.rot_y;

                                ped.PreventRagdoll = true;
                                ped.Velocity = new Vector3(elem.Value.vel_x, elem.Value.vel_y, elem.Value.vel_z);
                                ped.Task.ClearAllImmediately();
                                if (ped.Velocity.Length() > 0)
                                {
                                    var animset = new AnimationSet("move_f@bness_b");
                                    if (!ped.Animation.isPlaying(animset, "run"))
                                    {
                                        ped.Animation.Play(animset, "run", 1.0f);
                                    }
                                }

                            }
                            else
                            {
                                Vehicle veh = vehicleController.getById(elem.Key, elem.Value.vehicle_model, posnew);
                                if (!ped.isInVehicle(veh))
                                {
                                    ped.WarpIntoVehicle(veh, VehicleSeat.RightFront);
                                }
                                veh.EngineRunning = true;
                                veh.InteriorLightOn = true;
                                veh.HazardLightsOn = true;
                                veh.RotationQuaternion = new Quaternion(elem.Value.rot_x, elem.Value.rot_y, elem.Value.rot_z, elem.Value.rot_a);
                                veh.Position = new Vector3(elem.Value.pos_x, elem.Value.pos_y, elem.Value.pos_z);
                                veh.Velocity = new Vector3(elem.Value.vel_x, elem.Value.vel_y, elem.Value.vel_z) * 1.4f;
                                veh.Speed = elem.Value.speed;
                                veh.Repair();
                                ped.Task.DrivePointRoute(veh, 999.0f, veh.Position + (veh.Velocity * 10));
                            }
                            //log("GOT IT AND SET positioning");
                            //writeChat("GOT");
                        }
                        catch (Exception e2)
                        {
                            chatController.writeChat(e2.Message);
                            //log("failed positioning");
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
            catch { }
        }

    }
}
