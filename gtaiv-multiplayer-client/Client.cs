using GTA;
using MIVSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MIVClient
{
    public partial class Client : Script
    {
        public static UpdateDataStruct currentData;
        public static Client instance;
        public bool BroadcastingPaused;
        public CameraController cameraController;
        public ChatController chatController;
        public TcpClient client;
        public ClientState currentState = ClientState.Initializing;
        public uint CurrentVirtualWorld;
        public ClientTextView debugDraw;
        public bool isCurrentlyDead;
        public KeyboardHandler keyboardHandler;
        public string nick;
        public NPCPedController npcPedController;
        public PlayerPedController pedController;
        public PedStreamer pedStreamer;
        public PerFrameRenderer perFrameRenderer;
        public Dictionary<uint, string> playerModels;
        public Dictionary<uint, string> playerNames;
        public PlayerVehicleController playerVehicleController;
        public ServerConnection serverConnection;
        public VehicleController vehicleController;
        public VehicleStreamer vehicleStreamer;

        private Queue<Action> actionQueue;

        private Dictionary<int, GTA.Vector3> bindPoints;

        public Client()
        {
            BindConsoleCommand("reconnect", (o) =>
            {
                if (System.IO.File.Exists("miv_lastserver.ini"))
                {
                    Game.FadeScreenOut(0);
                    string[] lines = System.IO.File.ReadAllLines("miv_lastserver.ini");
                    INIReader reader = new INIReader(lines);
                    initAndConnect(reader.getString("ip"), reader.getInt16("port"), reader.getString("nickname"));
                }
            });
            if (System.IO.File.Exists("_serverinit.ini"))
            {
                Game.FadeScreenOut(0);
                string[] lines = System.IO.File.ReadAllLines("_serverinit.ini");
                INIReader reader = new INIReader(lines);
                Int64 timestamp_saved = reader.getInt64("timestamp");
                Int64 timestamp_now = System.Diagnostics.Stopwatch.GetTimestamp();
                TimeSpan time_delta = new TimeSpan(timestamp_now - timestamp_saved);
                if (time_delta.Minutes < 5)
                {
                    System.IO.File.Delete("_serverinit.ini");
                    System.IO.File.WriteAllLines("miv_lastserver.ini", lines);
                    initAndConnect(reader.getString("ip"), reader.getInt16("port"), reader.getString("nickname"));
                }
            }
            // nope? nothing to do
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

        public void enqueueAction(Action action)
        {
            actionQueue.Enqueue(action);
        }

        public void finishSpawn()
        {
            Game.FadeScreenIn(2000);
            Player.CanControlCharacter = true;
        }

        public Player getPlayer()
        {
            return Player;
        }

        public Ped getPlayerPed()
        {
            return Player.Character;
        }

        public void saveBindPoint(int id)
        {
            if (bindPoints == null) bindPoints = new Dictionary<int, Vector3>();
            if (bindPoints.ContainsKey(id)) bindPoints[id] = getPlayerPed().Position;
            else bindPoints.Add(id, getPlayerPed().Position);
        }

        public void startTimersandBindEvents()
        {
            GTA.Timer gfxupdate = new Timer(1);
            gfxupdate.Tick += gfxupdate_Tick;
            gfxupdate.Start();
            GTA.Timer slow_update = new Timer(600);
            slow_update.Tick += slow_update_Tick;
            slow_update.Start();
            this.Tick += new EventHandler(this.eventOnTick);
            MouseDown += Client_MouseDown;
            MouseUp += Client_MouseUp;
        }

        public void teleportToBindPoint(int id)
        {
            if (bindPoints == null) bindPoints = new Dictionary<int, Vector3>();
            if (bindPoints.ContainsKey(id)) getPlayerPed().Position = bindPoints[id];
        }

        public void updateAllPlayers()
        {
            for (int i = 0; i < serverConnection.playersdata.Keys.Count; i++)
            {
                var elemKey = serverConnection.playersdata.Keys.ToArray()[i];
                var elemValue = serverConnection.playersdata[elemKey];

                if (elemValue.client_has_been_set) continue;
                else elemValue.client_has_been_set = true;

                //if (!pedController.Exists(elemKey))
                //{
                //pedController.Add(new StreamedPed(pedStreamer, "F_Y_NURSE", "-", Vector3.Zero, 0, (BlipColor)(elemKey % 13)));
                //}

                StreamedPed ped = pedController.GetInstance(elemKey);
                ped.model = playerModels.ContainsKey(elemKey) ? playerModels[elemKey] : "F_Y_NURSE";
                if (ped.position == Vector3.Zero)
                {
                    ped.color = (BlipColor)(elemKey % 14);
                    ped.networkname = playerNames.ContainsKey(elemKey) ? playerNames[elemKey] : "-";
                }
                try
                {
                    updateVehicle(elemKey, elemValue, ped);
                }
                catch (Exception ex)
                {
                    log("Failed updating streamed vehicle data for player " + ex.Message);
                }
                try
                {
                    updatePed(elemKey, elemValue, ped);
                }
                catch (Exception ex)
                {
                    log("Failed updating streamed ped data for player " + ex.Message);
                }
            }
        }

        private void Client_MouseDown(object sender, MouseEventArgs e)
        {
            // maybe pass an event
        }

        private void Client_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
            }
        }

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
            if (currentState == ClientState.Connected)
            {
                if (currentData == null) currentData = UpdateDataStruct.Zero;
                if (!BroadcastingPaused)
                {
                    try
                    {
                        UpdateDataStruct data = new UpdateDataStruct();
                        if (Player.Character.isInVehicle() && Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.Driver) == Player.Character)
                        {
                            if (vehicleController.dict.Count(a => a.Value.IsStreamedIn() && a.Value.gameReference == Player.Character.CurrentVehicle) > 0)
                            {
                                Vector3 pos = Player.Character.CurrentVehicle.Position;
                                data.pos_x = pos.X;
                                data.pos_y = pos.Y;
                                data.pos_z = pos.Z;

                                Vector3 currentSpeed = Player.Character.CurrentVehicle.Velocity;
                                float speed = Player.Character.CurrentVehicle.Speed;
                                if (currentData.pos_x != 0)
                                {
                                    float deltax = (pos.X - currentData.pos_x);
                                    float deltay = (pos.Y - currentData.pos_y);
                                    float deltaz = (pos.Z - currentData.pos_z);
                                    data.vel_x = (deltax < 0 ? currentSpeed.X * -1 : currentSpeed.X);
                                    data.vel_y = (deltay < 0 ? currentSpeed.Y * -1 : currentSpeed.Y);
                                    data.vel_z = (deltaz < 0 ? currentSpeed.Z * -1 : currentSpeed.Z);
                                }
                                else
                                {
                                    data.vel_x = currentSpeed.X;
                                    data.vel_y = currentSpeed.Y;
                                    data.vel_z = currentSpeed.Z;
                                }

                                Quaternion quat = Player.Character.CurrentVehicle.RotationQuaternion;
                                data.rot_x = quat.X;
                                data.rot_y = quat.Y;
                                data.rot_z = quat.Z;
                                data.rot_a = quat.W;

                                data.vehicle_model = Player.Character.CurrentVehicle.Model.Hash;
                                data.vehicle_health = Player.Character.CurrentVehicle.Health;
                                var cveh = vehicleController.dict.First(a => a.Value.IsStreamedIn() && a.Value.gameReference == Player.Character.CurrentVehicle);

                                data.vehicle_id = cveh.Key;
                                data.ped_health = Player.Character.Health;
                                data.heading = Player.Character.CurrentVehicle.Heading;

                                cveh.Value.position = pos;
                                cveh.Value.orientation = quat;
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

                            data.rot_x = Player.Character.Direction.X;
                            data.rot_y = Player.Character.Direction.Y;
                            data.rot_z = Player.Character.Direction.Z;
                            data.rot_a = 0;

                            data.vehicle_model = 0;
                            data.vehicle_health = 0;
                            // for passengers:)client.pedController.dict.First(a => a.Value.IsStreamedIn() && a.Value.gameReference == selectedPed)
                            data.vehicle_id = Player.Character.isInVehicle() ? vehicleController.dict.First(a => a.Value.IsStreamedIn() && a.Value.gameReference == Player.Character.CurrentVehicle).Key : 0;
                            data.ped_health = Player.Character.Health;
                            data.heading = Player.Character.Heading;
                            data.weapon = Player.Character.Weapons.CurrentType.GetHashCode();
                            data.state = 0;
                            data.state |= Player.Character.isShooting ? PlayerState.IsShooting : 0;
                            data.state |= Game.isGameKeyPressed(GameKey.Aim) ? PlayerState.IsAiming : 0;
                            data.state |= Game.isGameKeyPressed(GameKey.Crouch) ? PlayerState.IsCrouching : 0;
                            data.state |= Game.isGameKeyPressed(GameKey.Jump) ? PlayerState.IsJumping : 0;
                            data.state |= Game.isGameKeyPressed(GameKey.Attack) ? PlayerState.IsShooting : 0;

                            data.state |= Player.Character.isInVehicle() && Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.RightFront) == Player.Character ? PlayerState.IsPassenger1 : 0;
                            data.state |= Player.Character.isInVehicle() && Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.LeftRear) == Player.Character ? PlayerState.IsPassenger2 : 0;
                            data.state |= Player.Character.isInVehicle() && Player.Character.CurrentVehicle.GetPedOnSeat(VehicleSeat.RightRear) == Player.Character ? PlayerState.IsPassenger3 : 0;
                        }
                        data.vstate = 0;
                        data.vstate |= Game.isGameKeyPressed(GameKey.MoveForward) ? VehicleState.IsAccelerating : 0;
                        data.vstate |= Game.isGameKeyPressed(GameKey.MoveBackward) ? VehicleState.IsBraking : 0;
                        data.vstate |= Game.isGameKeyPressed(GameKey.MoveLeft) ? VehicleState.IsSterringLeft : 0;
                        data.vstate |= Game.isGameKeyPressed(GameKey.MoveRight) ? VehicleState.IsSterringRight : 0;
                        data.vstate |= Game.isGameKeyPressed(GameKey.Sprint) ? VehicleState.IsSprinting : 0;
                        data.vstate |= Player.Character.isGettingIntoAVehicle ? VehicleState.IsEnteringVehicle : 0;
                        data.vstate |= (data.state & PlayerState.IsPassenger1) != 0 || (data.state & PlayerState.IsPassenger2) != 0 || (data.state & PlayerState.IsPassenger3) != 0
                            ? VehicleState.IsAsPassenger : 0;

                        var bpf = new BinaryPacketFormatter(Commands.UpdateData);
                        bpf.add(data);
                        serverConnection.write(bpf.getBytes());

                        if (Player.Character.Health == 0 || Player.Character.isDead || !Player.Character.isAlive)
                        {
                            Player.Character.Die();
                            isCurrentlyDead = true;
                        }

                        if (isCurrentlyDead && !Player.Character.isDead && Player.Character.isAlive && Player.Character.Health > 0)
                        {
                            Game.FadeScreenOut(200);
                            isCurrentlyDead = false;

                            var bpf2 = new BinaryPacketFormatter(Commands.InternalClient_requestSpawn);
                            serverConnection.write(bpf2.getBytes());
                        }

                        currentData = data;
                    }
                    catch (Exception ex)
                    {
                        Game.Log("Failed sending new player data with message " + ex.Message);
                    }
                }
                try
                {
                    serverConnection.flush();
                }
                catch (Exception ex)
                {
                    Game.Log("Failed sending packets " + ex.Message);
                }
                try
                {
                    updateAllPlayers();
                    pedStreamer.Update();
                    vehicleStreamer.Update();
                    pedStreamer.UpdateNormalTick();
                    vehicleStreamer.UpdateNormalTick();
                }
                catch (Exception ex)
                {
                    log("Failed updating streamers and players with message " + ex.Message);
                }
            }

            if (currentState == ClientState.Connecting)
            {
                Game.FadeScreenIn(3000);
                currentState = ClientState.Connected;

                var bpf = new BinaryPacketFormatter(Commands.Connect);
                bpf.add(nick);
                serverConnection.write(bpf.getBytes());

                Player.Model = new Model("F_Y_HOOKER_01");
                Player.NeverGetsTired = true;

                //ClientTextureDraw draw = new ClientTextureDraw(new System.Drawing.RectangleF(20, 20, 400, 400), @"C:\Users\Aerofly\Desktop\4duzy.png");

                //chatController.writeChat("Connected");
            }
        }

        private void gfxupdate_Tick(object sender, EventArgs e)
        {
            //AlternateHook.call(AlternateHook.OtherCommands.HIDE_HUD_AND_RADAR_THIS_FRAME, 1);
            //AlternateHook.call(AlternateHook.OtherCommands.HIDE_HELP_TEXT_THIS_FRAME, 1);
            if (currentState == ClientState.Connected)
            {
                pedStreamer.UpdateGfx();
                vehicleStreamer.UpdateGfx();
            }
        }

        private void initAndConnect(string ip, short port, string nickname)
        {
            BroadcastingPaused = true;
            playerNames = new Dictionary<uint, string>();
            playerModels = new Dictionary<uint, string>();
            isCurrentlyDead = false;
            actionQueue = new Queue<Action>();
            instance = this;

            CurrentVirtualWorld = 0;

            cameraController = new CameraController(this);

            debugDraw = new ClientTextView(new System.Drawing.Point(10, 400), "", new GTA.Font("Segoe UI", 24, FontScaling.Pixel));

            pedStreamer = new PedStreamer(this, 100.0f);
            vehicleStreamer = new VehicleStreamer(this, 100.0f);

            pedController = new PlayerPedController();
            npcPedController = new NPCPedController();
            vehicleController = new VehicleController();
            playerVehicleController = new PlayerVehicleController();
            chatController = new ChatController(this);
            keyboardHandler = new KeyboardHandler(this);
            currentState = ClientState.Initializing;
            Interval = 80;
            //cam = new Camera();
            //cam.Activate();
            currentState = ClientState.Disconnected;
            System.IO.File.WriteAllText("multiv-log.txt", "");
            perFrameRenderer = new PerFrameRenderer(this);

            Player.Character.CurrentRoom = Room.FromString("R_00000000_00000000");

            startTimersandBindEvents();
            try
            {
                if (client != null && client.Connected)
                {
                    client.Close();
                }
                client = new TcpClient();
                IPAddress address = IPAddress.Parse(ip);
                nick = nickname;

                client.Connect(address, port);

                Client.currentData = UpdateDataStruct.Zero;

                serverConnection = new ServerConnection(this);

                World.CurrentDayTime = new TimeSpan(12, 00, 00);
                World.PedDensity = 0;
                World.CarDensity = 0;
                // AlternateHook.call(AlternateHook.OtherCommands.TERMINATE_ALL_SCRIPTS_FOR_NETWORK_GAME);
                AlternateHook.call(AlternateHookRequest.OtherCommands.CLEAR_AREA, 0.0f, 0.0f, 0.0f, 4000.0f, true);
                currentState = ClientState.Connecting;
            }
            catch
            {
                currentState = ClientState.Disconnected;
                if (client != null && client.Connected)
                {
                    client.Close();
                }
                throw;
            }
        }

        private void onMouseMove(float x, float y)
        {
        }

        private void slow_update_Tick(object sender, EventArgs e)
        {
            vehicleStreamer.UpdateSlow();
            pedStreamer.UpdateSlow();

            npcPedController.update();

            GTA.World.UnlockAllIslands();
            GTA.World.LockDayTime();
            Player.WantedLevel = 0;

            //GTA.Light l = new Light(System.Drawing.Color.Red, 5.0f, 10.0f, getPlayerPed().Position);
            Game.WantedMultiplier = 0.0f;
        }
    }
}