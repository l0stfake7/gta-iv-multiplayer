using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Forms;
using System.Net;
using System.Net.Sockets;
using multiplayer_sdk;

namespace gtaiv_multiplayer_client
{
    public class Client : Script
    {
        Label dlabel;
        TcpClient client;
        //NetworkStream stream;
        string serverName;
        string nick;
        static Client instance;

        PlayerPedController pedController;
        PlayerVehicleController vehicleController;
        Dictionary<byte, UpdateDataStruct> playersdata;
        byte[] buffer;

        enum ClientState
        {
            Invalid,
            Disconnected,
            Disconnecting,
            Initializing,
            Connecting,
            Connected,
            Streaming
        }

        ClientState currentState = ClientState.Initializing;

        public Client()
        {
            playersdata = new Dictionary<byte, UpdateDataStruct>();
            buffer = new byte[512];
            instance = this;
            pedController = new PlayerPedController();
            vehicleController = new PlayerVehicleController();
            currentState = ClientState.Initializing;
            Interval = 40;
            this.KeyDown += new GTA.KeyEventHandler(this.eventOnKeyDown);
            this.Tick += new EventHandler(this.eventOnTick);
            dlabel = new Label();
            dlabel.Text = "IVMulti";
            dlabel.Location = new System.Drawing.Point(10, 10);
            Form form = new Form();
            form.Controls.Add(dlabel);
            form.Transparency = 0.0f;
            form.StartPosition = FormStartPosition.Fixed;
            form.Location = new System.Drawing.Point(10, 10);
            form.BackColor = System.Drawing.Color.Transparent;
            //form.Show();
            //form.KeyDown += new GTA.KeyEventHandler(this.eventOnKeyDown);
            //Game.Mouse.Enabled = false;
           // Player.CanControlCharacter = true;
            //form.Show();
            currentState = ClientState.Disconnected;
            serverName = "Not connected";
            System.IO.File.WriteAllText("multiv-log.txt", "");
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
        private void saveBindPoint(int id)
        {
            if (bindPoints == null) bindPoints = new Dictionary<int, Vector3>();
            if (bindPoints.ContainsKey(id)) bindPoints[id] = getPlayerPed().Position;
            else bindPoints.Add(id, getPlayerPed().Position);
        }
        private void teleportToBindPoint(int id)
        {
            if (bindPoints == null) bindPoints = new Dictionary<int, Vector3>();
            if (bindPoints.ContainsKey(id)) getPlayerPed().Position = bindPoints[id];
        }

        private void eventOnKeyDown(object sender, GTA.KeyEventArgs e)
        {
            foreach (int id in Enumerable.Range((int)System.Windows.Forms.Keys.D0, (int)System.Windows.Forms.Keys.D9))
            {
                if (e.Key == (System.Windows.Forms.Keys)id && e.Alt) saveBindPoint(id - (int)System.Windows.Forms.Keys.D0);
                if (e.Key == (System.Windows.Forms.Keys)id && e.Control) teleportToBindPoint(id - (int)System.Windows.Forms.Keys.D0);
            }

            if (e.Key == System.Windows.Forms.Keys.Enter)
            {
          //      ChatInputForm.show();
            }

            if (e.Key == System.Windows.Forms.Keys.L)
            {
                try
                {
                    currentState = ClientState.Connecting;
                    streamBegin();
                    if (client != null && client.Connected)
                    {
                        client.Close();
                    }
                    client = new TcpClient();
                    INIReader ini = new INIReader(System.IO.File.ReadAllLines("server.ini"));
                    IPAddress address = IPAddress.Parse(ini.getString("ip"));
                    nick = ini.getString("nick");
                    int port = ini.getInt("port");
                    client.Connect(address, port);
                    Client.currentData = new UpdateDataStruct();
                    Client.currentData.pos_x = 0;
                    Client.currentData.pos_y = 0;
                    Client.currentData.pos_z = 0;
                    Client.currentData.rot_x = 0;
                    Client.currentData.rot_y = 0;
                    Client.currentData.rot_z = 0;
                    Client.currentData.rot_a = 0;
                    Client.currentData.heading = 0;
                    Client.currentData.ped_health = 0;
                    Client.currentData.speed = 0;
                    Client.currentData.veh_health = 0;
                    Client.currentData.vehicle_model = 0;
                    Client.currentData.vel_x = 0;
                    Client.currentData.vel_y = 0;
                    Client.currentData.vel_z = 0;
                    //stream = client.GetStream();
                    //stream.Write(buf, 0, buf.Length);

                    buffer = new byte[512];
                    client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
                    World.CurrentDayTime = new TimeSpan(12, 00, 00);
                    World.PedDensity = 0;
                    World.CarDensity = 0;
                     var peds = World.GetPeds(Player.Character.Position, 200.0f);
                     foreach (Ped a in peds) if (a.Exists() && a.isAlive && a != Player.Character) a.Delete();
                     foreach (Vehicle v in World.GetAllVehicles()) if (v.Exists()) v.Delete();
                    log("connected");

                }
                catch (Exception ex)
                {
                    currentState = ClientState.Disconnected;
                    if (client != null && client.Connected)
                    {
                        client.Close();
                    }
                    throw ex;
                }
            }
        }


        private void onReceive(IAsyncResult iar)
        {
            //try
            //{
            client.Client.EndReceive(iar);
            if (iar.IsCompleted)
            {
                if (buffer.Length > 0)
                {
                    switch ((multiplayer_sdk.Commands)BitConverter.ToUInt16(buffer, 0))
                    {
                        case multiplayer_sdk.Commands.UpdateData:
                            {
                                byte playerid = buffer[2];

                                //log(strb.ToString());
                                //log("reading data for received player " + playerid.ToString());
                                multiplayer_sdk.UpdateDataStruct data = multiplayer_sdk.UpdateDataStruct.unserialize(buffer, 3);
                                if (!playersdata.ContainsKey(playerid)) playersdata.Add(playerid, data);
                                else playersdata[playerid] = data;
                                //log("X " + data.pos_x.ToString());
                                //log("Y " + data.pos_y.ToString());

                                //log("Positioned a ped playerid " + playerid.ToString());
                            }
                            break;
                        case multiplayer_sdk.Commands.InfoPlayerName:
                            {
                                byte playerid = buffer[2];
                                if (playersdata.ContainsKey(playerid))
                                {
                                    var list = buffer.ToList();
                                    int nickLength = BitConverter.ToInt32(buffer, 3);
                                    string nick = Encoding.UTF8.GetString(list.Skip(3 + 4).Take(nickLength).ToArray());
                                    playersdata[playerid].nick = nick;
                                }
                            }
                            break;

                    }
                }
            }
            client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
            /*}
            catch (Exception e)
            {
                Console.WriteLine("Failed receive with message " + e.Message);
                //throw e;
            }*/
        }

        /*
         * Structure of packet is as follows
         * 0x00               0x01               0x05
         * [(ushort)COMMAND_ID] [(int)DATA_LENGTH] [(mixed)DATA]
         * 
         */

        private byte[] appendBytes(byte[] byte1, byte[] byte2)
        {
            var list = byte1.ToList();
            list.AddRange(byte2);
            return list.ToArray();
        }

        byte[] tempbuf;

        private void streamBegin()
        {
            tempbuf = new byte[0];
        }

        private void streamFlush()
        {
            client.Client.Send(tempbuf, tempbuf.Length, SocketFlags.None);
            streamBegin();
        }

        private void streamReadString(byte[] buffer)
        {
            //streamWrite(buffer.Length);
            //stream.Write(buffer, 0, buffer.Length);
        }

        private void streamWrite(byte[] buffer)
        {
            tempbuf = appendBytes(tempbuf, buffer);
        }
        private void streamWrite(List<byte> buffer)
        {
            tempbuf = appendBytes(tempbuf, buffer.ToArray());
        }
        private void streamWrite(string buffer)
        {
            byte[] buf = Encoding.UTF8.GetBytes(buffer);
            tempbuf = appendBytes(tempbuf, buf);
        }
        private void streamWrite(UpdateDataStruct buffer)
        {
            byte[] buf = buffer.serialize();
            tempbuf = appendBytes(tempbuf, buf);
        }
        private void streamWrite(Commands command)
        {
            byte[] buf = BitConverter.GetBytes((ushort)command);
            tempbuf = appendBytes(tempbuf, buf);
        }
        private void streamWrite(int integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            tempbuf = appendBytes(tempbuf, buf);
        }

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
                    streamWrite(Commands.UpdateData);
                    streamWrite(data);
                    streamFlush();
                    currentData = data;
                    //log("sent data");
                    // process players
                    foreach (var elem in playersdata)
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
                        }
                        catch
                        {
                            //log("failed positioning");
                        }
                    }
                }
                if (currentState == ClientState.Connecting)
                {
                    streamWrite(Commands.Connect);
                    streamWrite(nick.Length);
                    streamWrite(nick);
                    streamFlush();
                    currentState = ClientState.Connected;
                }
            }
            catch { }
        }

    }
}
