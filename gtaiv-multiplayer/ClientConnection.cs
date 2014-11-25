using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace MIVServer
{
    public class ClientConnection
    {
        private TcpClient connection;

        //NetworkStream stream;
        private byte[] buffer;
        public delegate void onConnectDelegate(string nick);

        public event onConnectDelegate onConnect;

        public ServerPlayer player;

        public Queue<byte[]> queue;
        
        public ClientConnection(TcpClient client)
        {
            queue = new Queue<byte[]>();
            connection = client;
            buffer = new byte[1024 * 1024];
            streamBegin();
            //stream = connection.GetStream();
        }

        public void startReceiving()
        {
            connection.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
        }

        public void streamBroadcastQueue()
        {
            lock (queue)
            {
                if (queue.Count > 0)
                {
                    byte[] arr = queue.Dequeue();
                    connection.Client.Send(arr, arr.Length, SocketFlags.None);
                }
            }
        }

        private byte[] tempbuf;

        public void streamBegin()
        {
            tempbuf = new byte[0];
        }

        public void streamFlush()
        {
            lock (queue)
            {
                byte[] copy = new byte[tempbuf.Length];
                tempbuf.CopyTo(copy, 0);
                queue.Enqueue(copy);
                streamBegin();
            }
        }

        public byte[] appendBytes(byte[] byte1, byte[] byte2)
        {
            var list = byte1.ToList();
            list.AddRange(byte2);
            return list.ToArray();
        }

        public void streamReadString(byte[] buffer)
        {
            //streamWrite(buffer.Length);
            //stream.Write(buffer, 0, buffer.Length);
        }

        public void streamWrite(byte[] buffer)
        {
            tempbuf = appendBytes(tempbuf, buffer);
        }

        public void streamWrite(List<byte> buffer)
        {
            tempbuf = appendBytes(tempbuf, buffer.ToArray());
        }

        public void streamWrite(string buffer)
        {
            byte[] buf = Encoding.UTF8.GetBytes(buffer);
            tempbuf = appendBytes(tempbuf, buf);
        }

        public void streamWrite(MIVSDK.UpdateDataStruct buffer)
        {
            byte[] buf = buffer.serialize();
            tempbuf = appendBytes(tempbuf, buf);
        }

        public void streamWrite(MIVSDK.Commands command)
        {
            byte[] buf = BitConverter.GetBytes((ushort)command);
            tempbuf = appendBytes(tempbuf, buf);
        }

        public void streamWrite(int integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            tempbuf = appendBytes(tempbuf, buf);
        }

        private void onReceive(IAsyncResult iar)
        {
            //try
            //{
            int count = connection.Client.EndReceive(iar);
            if (iar.IsCompleted)
            {
                if (buffer.Length > 0)
                {

                    //foreach (byte by in buffer) Console.Write(by.ToString("X") + " ");
                    switch ((MIVSDK.Commands)BitConverter.ToUInt16(buffer, 0))
                    {
                        case MIVSDK.Commands.Disconnect:
                            {
                                if (player != null)
                                {
                                    Server.instance.api.invokeOnPlayerDisconnect(player);
                                }
                            }
                            break;

                        case MIVSDK.Commands.Connect:
                            {
                                var list = buffer.ToList();
                                int nickLength = BitConverter.ToInt32(buffer, 2);
                                if (onConnect != null) onConnect.Invoke(Encoding.UTF8.GetString(list.Skip(2 + 4).Take(nickLength).ToArray()));
                            }
                            break;

                        case MIVSDK.Commands.Chat_sendMessage:
                            {
                                var list = buffer.ToList();
                                int len = BitConverter.ToInt32(buffer, 2);
                                Server.instance.api.invokeOnPlayerSendText(player, Encoding.UTF8.GetString(list.Skip(2 + 4).Take(len).ToArray()));
                            }
                            break;

                        case MIVSDK.Commands.Keys_down:
                            {
                                if (player != null)
                                {
                                    int key = BitConverter.ToInt32(buffer, 2);
                                    Server.instance.api.invokeOnPlayerKeyDown(player, (System.Windows.Forms.Keys)key);
                                }
                            }
                            break;
                        case MIVSDK.Commands.Keys_up:
                            {
                                if (player != null)
                                {
                                    int key = BitConverter.ToInt32(buffer, 2);
                                    Server.instance.api.invokeOnPlayerKeyUp(player, (System.Windows.Forms.Keys)key);
                                }
                            }
                            break;
                        case MIVSDK.Commands.NPCDialog_sendResponse:
                            {
                                if (player != null)
                                {
                                    uint key = BitConverter.ToUInt32(buffer, 2);
                                    byte answer = buffer[6];
                                    ServerNPCDialog.invokeResponse(player, key, answer);
                                }
                            }
                            break;

                        case MIVSDK.Commands.UpdateData:
                            {
                                if (player != null)
                                {
                                    MIVSDK.UpdateDataStruct data = MIVSDK.UpdateDataStruct.unserialize(buffer, 2);
                                    player.updateData(data);
                                }
                            }
                            break;
                    }
                }
            }
            connection.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
            /*}
            catch (Exception e)
            {
                Console.WriteLine("Failed receive with message " + e.Message);
                throw e;
            }*/
        }
    }
}