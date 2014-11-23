using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MIVSDK.Math;

namespace MIVServer
{
    public class ServerApi
    {
        Server server;
        public ServerApi(Server instance)
        {
            server = instance;
        }

        public delegate void onPlayerConnectDelegate(EndPoint address, ServerPlayer player);
        public event onPlayerConnectDelegate onPlayerConnect;
        public void invokeOnPlayerConnect(EndPoint address, ServerPlayer player)
        {
            if (onPlayerConnect != null) onPlayerConnect.Invoke(address, player);
        }

        public delegate void onPlayerDisconnectDelegate(ServerPlayer player);
        public event onPlayerDisconnectDelegate onPlayerDisconnect;
        public void invokeOnPlayerDisconnect(ServerPlayer player)
        {
            if (onPlayerDisconnect != null) onPlayerDisconnect.Invoke(player);
        }


        public delegate void onPlayerUpdateDelegate(ServerPlayer player);
        public event onPlayerUpdateDelegate onPlayerUpdate;
        public void invokeOnPlayerUpdate(ServerPlayer player)
        {
            if (onPlayerUpdate != null) onPlayerUpdate.Invoke(player);
        }

        public delegate void onPlayerEnterVehicleDelegate(ServerPlayer player, ServerVehicle vehicle);
        public event onPlayerEnterVehicleDelegate onPlayerEnterVehicle;
        public void invokeOnPlayerEnterVehicle(ServerPlayer player, ServerVehicle vehicle)
        {
            if (onPlayerEnterVehicle != null) onPlayerEnterVehicle.Invoke(player, vehicle);
        }

        public delegate void onPlayerExitVehicleDelegate(ServerPlayer player, ServerVehicle vehicle);
        public event onPlayerExitVehicleDelegate onPlayerExitVehicle;
        public void invokeOnPlayerExitVehicle(ServerPlayer player, ServerVehicle vehicle)
        {
            if (onPlayerExitVehicle != null) onPlayerExitVehicle.Invoke(player, vehicle);
        }



        public delegate void onPlayerSendTextDelegate(ServerPlayer player, string text);
        public event onPlayerSendTextDelegate onPlayerSendText;
        public void invokeOnPlayerSendText(ServerPlayer player, string text)
        {
            if (onPlayerSendText != null) onPlayerSendText.Invoke(player, text);
        }


        public delegate void onPlayerWriteConsoleDelegate(ServerPlayer player, string text);
        public event onPlayerWriteConsoleDelegate onPlayerWriteConsole;
        public void invokeOnPlayerWriteConsole(ServerPlayer player, string text)
        {
            if (onPlayerWriteConsole != null) onPlayerWriteConsole.Invoke(player, text);
        }


        public delegate void onPlayerSpawnDelegate(ServerPlayer player);
        public event onPlayerSpawnDelegate onPlayerSpawn;
        public void invokeOnPlayerSpawn(ServerPlayer player)
        {
            if (onPlayerSpawn != null) onPlayerSpawn.Invoke(player);
        }


        public delegate void onPlayerDieDelegate(ServerPlayer player);
        public event onPlayerDieDelegate onPlayerDie;
        public void invokeOnPlayerDie(ServerPlayer player)
        {
            if (onPlayerDie != null) onPlayerDie.Invoke(player);
        }


        public delegate void onPlayerPauseDelegate(ServerPlayer player);
        public event onPlayerPauseDelegate onPlayerPause;
        public void invokeOnPlayerPause(ServerPlayer player)
        {
            if (onPlayerPause != null) onPlayerPause.Invoke(player);
        }


        public delegate void onPlayerResumeDelegate(ServerPlayer player);
        public event onPlayerResumeDelegate onPlayerResume;
        public void invokeOnPlayerResume(ServerPlayer player)
        {
            if (onPlayerResume != null) onPlayerResume.Invoke(player);
        }


        public delegate void onPlayerTakeDamageDelegate(ServerPlayer player, int before, int after, int delta);
        public event onPlayerTakeDamageDelegate onPlayerTakeDamage;
        public void invokeOnPlayerTakeDamage(ServerPlayer player, int before, int after, int delta)
        {
            if (onPlayerTakeDamage != null) onPlayerTakeDamage.Invoke(player, before, after, delta);
        }


        public ServerPlayer getPlayer(byte id)
        {
            return server.playerpool[id];
        }

        public ServerPlayer getAllPlayers(byte id)
        {
            return server.playerpool[id];
        }

        public ServerVehicleInfo createVehicle(string model, Vector3 position, Quaternion orientation, Vector3 velocity)
        {
            return server.vehicleController.create(model, position, orientation, velocity);
        }
    }
}
