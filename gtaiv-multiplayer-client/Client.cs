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
            buffer = new byte[512];
            instance = this;
            pedController = new PlayerPedController();
            currentState = ClientState.Initializing;
            Interval = 250;
            this.KeyDown += new GTA.KeyEventHandler(this.eventOnKeyDown);
            this.Tick += new EventHandler(this.eventOnTick);
            dlabel = new Label();
            dlabel.Text = "IVMulti";
            dlabel.Location = new System.Drawing.Point(10, 10);
            Form form = new Form();
            form.Controls.Add(dlabel);
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

        private void eventOnKeyDown(object sender, GTA.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Forms.Keys.L)
            {
                try
                {
                    currentState = ClientState.Connecting;
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
                    //stream = client.GetStream();
                    //stream.Write(buf, 0, buf.Length);

                    buffer = new byte[512];
                    client.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
                    log("connected");
                    //World.CurrentDayTime = new TimeSpan(12, 00, 00);
                    //World.PedDensity = 0;
                    //World.CarDensity = 0;
                    var peds = World.GetPeds(Player.Character.Position, 200.0f);
                    foreach (Ped a in peds) if (a.Exists() && a.isAlive && a != Player.Character) a.Delete();
                    //foreach (Vehicle v in World.GetAllVehicles()) if (v.Exists()) v.Delete();
                    
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
                                    try
                                    {
                                        byte playerid = buffer[2];
                                        multiplayer_sdk.UpdateDataStruct data = multiplayer_sdk.UpdateDataStruct.unserialize(buffer, 3);
                                        Ped ped = pedController.getById(playerid);
                                        ped.Position = new Vector3((float)data.pos_x, (float)data.pos_y, (float)data.pos_z);
                                        ped.Heading = (float)data.rot_y;
                                    }
                                    catch { }
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

        private void streamReadString(byte[] buffer)
        {
            //streamWrite(buffer.Length);
            //stream.Write(buffer, 0, buffer.Length);
        }

        private void streamWrite(byte[] buffer)
        {
            streamWrite(buffer.Length);
            client.Client.Send(buffer, buffer.Length, SocketFlags.None);
        }
        private void streamWrite(List<byte> buffer)
        {
            streamWrite(buffer.Count);
            client.Client.Send(buffer.ToArray(), buffer.Count, SocketFlags.None);
        }
        private void streamWrite(string buffer)
        {
            byte[] buf = Encoding.UTF8.GetBytes(buffer);
            streamWrite(buf.Length);
            client.Client.Send(buf, buf.Length, SocketFlags.None);
        }
        private void streamWrite(UpdateDataStruct buffer)
        {
            byte[] buf = buffer.serialize();
            streamWrite(buf.Length);
            client.Client.Send(buf, buf.Length, SocketFlags.None);
        }
        private void streamWrite(Commands command)
        {
            byte[] buf = BitConverter.GetBytes((ushort)command);
            streamWrite(buf.Length);
            client.Client.Send(buf, buf.Length, SocketFlags.None);
        }
        private void streamWrite(int integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            client.Client.Send(buf, buf.Length, SocketFlags.None);
        }

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
                    Vector3 pos = Player.Character.Position;
                    data.pos_x = pos.X;
                    data.pos_y = pos.Y;
                    data.pos_z = pos.Z;
                    log("my X is " + data.pos_x.ToString());
                    if (Player.Character.isInVehicle())
                    {
                        Quaternion quat = Player.Character.CurrentVehicle.RotationQuaternion;
                        data.rot_x = quat.X;
                        data.rot_y = quat.Y;
                        data.rot_z = quat.Z;
                        data.rot_a = quat.W;
                    }
                    else
                    {
                        data.rot_y = Player.Character.Heading;

                        data.rot_x = Player.Character.Heading;
                        data.rot_z = Player.Character.Heading;
                        data.rot_a = Player.Character.Heading;
                    }
                    streamWrite(Commands.UpdateData);
                    streamWrite(data);
                }
                if (currentState == ClientState.Connecting)
                {
                    streamWrite(Commands.Connect);
                    byte[] buf = Encoding.UTF8.GetBytes(nick);
                    streamWrite(buf.Length);
                    client.Client.Send(buf, buf.Length, SocketFlags.None);
                    currentState = ClientState.Connected;
                }
            } catch {}
        }

    }
}
