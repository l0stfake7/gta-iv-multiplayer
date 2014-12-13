// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using MIVSDK;
using MIVServer;
using SharpDX;
using System;
using System.Linq;

namespace EmptyGamemode
{
    public class EmptyGamemode : Gamemode
    {
        public EmptyGamemode(ServerApi a)
            : base(a)
        {
            Console.WriteLine("Empty gamemode started");
            api.onPlayerConnect += api_onPlayerConnect;
            api.onPlayerDisconnect += api_onPlayerDisconnect;
            api.onPlayerDie += api_onPlayerDie;
            api.onPlayerEnterVehicle += api_onPlayerEnterVehicle;
            api.onPlayerExitVehicle += api_onPlayerExitVehicle;
            api.onPlayerKeyDown += api_onPlayerKeyDown;
            api.onPlayerKeyUp += api_onPlayerKeyUp;
            api.onPlayerPause += api_onPlayerPause;
            api.onPlayerResume += api_onPlayerResume;
            api.onPlayerSendCommand += api_onPlayerSendCommand;
            api.onPlayerSendText += api_onPlayerSendText;
            api.onPlayerSpawn += api_onPlayerSpawn;
            api.onPlayerTakeDamage += api_onPlayerTakeDamage;
            api.onPlayerUpdate += api_onPlayerUpdate;
        }

        void api_onPlayerWriteConsole(ServerPlayer player, string text)
        {
           
        }

        void api_onPlayerUpdate(ServerPlayer player)
        {
           
        }

        void api_onPlayerTakeDamage(ServerPlayer player, int before, int after, int delta)
        {
           
        }

        void api_onPlayerSpawn(ServerPlayer player)
        {
           
        }

        void api_onPlayerSendText(ServerPlayer player, string text)
        {
            api.writeChat(player.Nick + "(" + player.id + "): " + text);
        }

        void api_onPlayerSendCommand(ServerPlayer player, string command, string[] param)
        {
           
        }

        void api_onPlayerResume(ServerPlayer player)
        {
           
        }

        void api_onPlayerPause(ServerPlayer player)
        {
           
        }

        void api_onPlayerKeyUp(ServerPlayer player, System.Windows.Forms.Keys key)
        {
           
        }

        void api_onPlayerKeyDown(ServerPlayer player, System.Windows.Forms.Keys key)
        {
           
        }

        void api_onPlayerExitVehicle(ServerPlayer player, ServerVehicle vehicle)
        {
           
        }

        void api_onPlayerEnterVehicle(ServerPlayer player, ServerVehicle vehicle)
        {
           
        }

        void api_onPlayerDie(ServerPlayer player)
        {
           
        }

        void api_onPlayerDisconnect(ServerPlayer player)
        {
           
        }

        void api_onPlayerConnect(System.Net.EndPoint address, ServerPlayer player)
        {
           
        }
    }
}
