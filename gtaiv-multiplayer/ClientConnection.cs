using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace gtaiv_multiplayer
{
    class ClientConnection
    {
        TcpClient connection;
        //NetworkStream stream;
        byte[] buffer;

        public delegate void onUpdateDataDelegate(multiplayer_sdk.UpdateDataStruct data);
        public delegate void onDisconnectDelegate();
        public delegate void onConnectDelegate(string nick);

        public event onUpdateDataDelegate onUpdateData;
        public event onDisconnectDelegate onDisconnect;
        public event onConnectDelegate onConnect;

        public ClientConnection(TcpClient client)
        {
            connection = client;
            buffer = new byte[512];
            //stream = connection.GetStream();
        }

        public void startReceiving()
        {
            connection.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
        }

        public void streamWrite(byte[] buffer)
        {
            streamWrite(buffer.Length);
            connection.Client.Send(buffer, buffer.Length, SocketFlags.None);
        }
        public void streamWrite(List<byte> buffer)
        {
            streamWrite(buffer.Count);
            connection.Client.Send(buffer.ToArray(), buffer.Count, SocketFlags.None);
        }
        public void streamWrite(string buffer)
        {
            byte[] buf = Encoding.UTF8.GetBytes(buffer);
            streamWrite(buf.Length);
            connection.Client.Send(buf, buf.Length, SocketFlags.None);
        }
        public void streamWrite(multiplayer_sdk.UpdateDataStruct buffer)
        {
            byte[] buf = buffer.serialize();
            //streamWrite(buf.Length);
            connection.Client.Send(buf, buf.Length, SocketFlags.None);
        }
        public void streamWrite(multiplayer_sdk.Commands command)
        {
            byte[] buf = BitConverter.GetBytes((ushort)command);
            streamWrite(buf.Length);
            connection.Client.Send(buf, buf.Length, SocketFlags.None);
        }
        public void streamWrite(int integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            connection.Client.Send(buf, buf.Length, SocketFlags.None);
        }
        public void streamWrite(byte b)
        {
            byte[] buf = new byte[1]{b};
            connection.Client.Send(buf, buf.Length, SocketFlags.None);
        }

        /**
         *  zrob petle w funckji tryexecutecommands ktora pobiera z listy buforowanej bajty i zczytuje
         *  stos byte moglby byc
         *  
         */

        private void onReceive(IAsyncResult iar)
        {
            //try
            //{
                int count = connection.Client.EndReceive(iar);
                if (iar.IsCompleted)
                {
                    if (buffer.Length > 0)
                    {
                        switch ((multiplayer_sdk.Commands)BitConverter.ToUInt16(buffer, 0))
                        {
                            case multiplayer_sdk.Commands.Disconnect:
                                {
                                    if(onDisconnect != null) onDisconnect.Invoke();
                                }
                                break;
                            case multiplayer_sdk.Commands.Connect:
                                {
                                    var list = buffer.ToList();
                                    int nickLength = BitConverter.ToInt32(buffer, 2);
                                    if (onConnect != null) onConnect.Invoke(Encoding.UTF8.GetString(list.Skip(2 + 4).Take(nickLength).ToArray()));
                                }
                                break;
                            case multiplayer_sdk.Commands.UpdateData:
                                {
                                    if (count < 2 + 7 * sizeof(float))
                                    {
                                        throw new Exception();
                                    }
                                    multiplayer_sdk.UpdateDataStruct data = multiplayer_sdk.UpdateDataStruct.unserialize(buffer, 2);
                                    if(onUpdateData != null) onUpdateData.Invoke(data);
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
