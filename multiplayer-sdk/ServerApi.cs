// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using SharpDX;
using System.Collections.Generic;
using System.Net;

namespace MIVServer
{
    public class ServerApi
    {
        private Server server;

        public ServerApi(Server instance)
        {
            server = instance;
        }

        public delegate void onPlayerConnectDelegate(EndPoint address, ServerPlayer player);

        public delegate void onPlayerDieDelegate(ServerPlayer player);

        public delegate void onPlayerDisconnectDelegate(ServerPlayer player);

        public delegate void onPlayerEnterVehicleDelegate(ServerPlayer player, ServerVehicle vehicle);

        public delegate void onPlayerExitVehicleDelegate(ServerPlayer player, ServerVehicle vehicle);

        public delegate void onPlayerKeyDownDelegate(ServerPlayer player, System.Windows.Forms.Keys key);

        public delegate void onPlayerKeyUpDelegate(ServerPlayer player, System.Windows.Forms.Keys key);

        public delegate void onPlayerPauseDelegate(ServerPlayer player);

        public delegate void onPlayerResumeDelegate(ServerPlayer player);

        public delegate void onPlayerSendCommandDelegate(ServerPlayer player, string command, string[] param);

        public delegate void onPlayerSendTextDelegate(ServerPlayer player, string text);

        public delegate void onPlayerSpawnDelegate(ServerPlayer player);

        public delegate void onPlayerTakeDamageDelegate(ServerPlayer player, int before, int after, int delta);

        public delegate void onPlayerUpdateDelegate(ServerPlayer player);

        public delegate void onPlayerWriteConsoleDelegate(ServerPlayer player, string text);

        public event onPlayerConnectDelegate onPlayerConnect;

        public event onPlayerDieDelegate onPlayerDie;

        public event onPlayerDisconnectDelegate onPlayerDisconnect;

        public event onPlayerEnterVehicleDelegate onPlayerEnterVehicle;

        public event onPlayerExitVehicleDelegate onPlayerExitVehicle;

        public event onPlayerKeyDownDelegate onPlayerKeyDown;

        public event onPlayerKeyUpDelegate onPlayerKeyUp;

        public event onPlayerPauseDelegate onPlayerPause;

        public event onPlayerResumeDelegate onPlayerResume;

        public event onPlayerSendCommandDelegate onPlayerSendCommand;

        public event onPlayerSendTextDelegate onPlayerSendText;

        public event onPlayerSpawnDelegate onPlayerSpawn;

        public event onPlayerTakeDamageDelegate onPlayerTakeDamage;

        public event onPlayerUpdateDelegate onPlayerUpdate;

        public event onPlayerWriteConsoleDelegate onPlayerWriteConsole;

        public ServerVehicle createVehicle(string model, Vector3 position, Quaternion orientation)
        {
            return server.vehicleController.create(model, position, orientation);
        }

        public ServerVehicle createVehicle(uint model, Vector3 position, Quaternion orientation)
        {
            return server.vehicleController.create(ModelDictionary.getVehicleById(model), position, orientation);
        }

        public List<ServerPlayer> getAllPlayers(byte id)
        {
            return server.playerpool;
        }

        public ServerPlayer getPlayer(byte id)
        {
            return server.getPlayerById(id);
        }

        public void invokeOnPlayerConnect(EndPoint address, ServerPlayer player)
        {
            if (onPlayerConnect != null) onPlayerConnect.Invoke(address, player);
        }

        public void invokeOnPlayerDie(ServerPlayer player)
        {
            if (onPlayerDie != null) onPlayerDie.Invoke(player);
        }

        public void invokeOnPlayerDisconnect(ServerPlayer player)
        {
            if (onPlayerDisconnect != null) onPlayerDisconnect.Invoke(player);
        }

        public void invokeOnPlayerEnterVehicle(ServerPlayer player, ServerVehicle vehicle)
        {
            if (onPlayerEnterVehicle != null) onPlayerEnterVehicle.Invoke(player, vehicle);
        }

        public void invokeOnPlayerExitVehicle(ServerPlayer player, ServerVehicle vehicle)
        {
            if (onPlayerExitVehicle != null) onPlayerExitVehicle.Invoke(player, vehicle);
        }

        public void invokeOnPlayerKeyDown(ServerPlayer player, System.Windows.Forms.Keys key)
        {
            if (onPlayerKeyDown != null) onPlayerKeyDown.Invoke(player, key);
        }

        public void invokeOnPlayerKeyUp(ServerPlayer player, System.Windows.Forms.Keys key)
        {
            if (onPlayerKeyUp != null) onPlayerKeyUp.Invoke(player, key);
        }

        public void invokeOnPlayerPause(ServerPlayer player)
        {
            if (onPlayerPause != null) onPlayerPause.Invoke(player);
        }

        public void invokeOnPlayerResume(ServerPlayer player)
        {
            if (onPlayerResume != null) onPlayerResume.Invoke(player);
        }

        public void invokeOnPlayerSendCommand(ServerPlayer player, string command, string[] param)
        {
            if (onPlayerSendCommand != null) onPlayerSendCommand.Invoke(player, command, param);
        }

        public void invokeOnPlayerSendText(ServerPlayer player, string text)
        {
            if (onPlayerSendText != null) onPlayerSendText.Invoke(player, text);
        }

        public void invokeOnPlayerSpawn(ServerPlayer player)
        {
            if (onPlayerSpawn != null) onPlayerSpawn.Invoke(player);
        }

        public void invokeOnPlayerTakeDamage(ServerPlayer player, int before, int after, int delta)
        {
            if (onPlayerTakeDamage != null) onPlayerTakeDamage.Invoke(player, before, after, delta);
        }

        public void invokeOnPlayerUpdate(ServerPlayer player)
        {
            if (onPlayerUpdate != null) onPlayerUpdate.Invoke(player);
        }

        public void invokeOnPlayerWriteConsole(ServerPlayer player, string text)
        {
            if (onPlayerWriteConsole != null) onPlayerWriteConsole.Invoke(player, text);
        }

        public void writeChat(ServerPlayer player, string text)
        {
            var bpf = new BinaryPacketFormatter(Commands.Chat_writeLine);
            bpf.add(text);
            player.connection.write(bpf.getBytes());
        }

        public void ExecuteJavaScript(ServerPlayer player, string script)
        {
            var bpf = new BinaryPacketFormatter(Commands.Client_JSEval, script);
            player.connection.write(bpf.getBytes());
        }

        public void writeChat(string text)
        {
            server.chat.addLine(text);
        }
    }
}