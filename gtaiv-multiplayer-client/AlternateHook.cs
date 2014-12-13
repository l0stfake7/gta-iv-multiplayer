// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using GTA;
using MIVSDK;
using System;
using System.Linq;

namespace MIVClient
{
    public class AlternateHook
    {
        public static void call(AlternateHookRequest.PedCommands command, params object[] param)
        {
            GTA.Native.Function.Call(Enum.GetName(command.GetType(), command), param.Select(a => new GTA.Native.Parameter(a)).ToArray());
        }

        public static void call(AlternateHookRequest.PlayerCommands command, params object[] param)
        {
            GTA.Native.Function.Call(Enum.GetName(command.GetType(), command), param.Select(a => new GTA.Native.Parameter(a)).ToArray());
        }

        public static void call(AlternateHookRequest.VehiclesCommands command, params object[] param)
        {
            GTA.Native.Function.Call(Enum.GetName(command.GetType(), command), param.Select(a => new GTA.Native.Parameter(a)).ToArray());
        }

        public static void call(AlternateHookRequest.TaskCommands command, params object[] param)
        {
            GTA.Native.Function.Call(Enum.GetName(command.GetType(), command), param.Select(a => new GTA.Native.Parameter(a)).ToArray());
        }

        public static void call(AlternateHookRequest.OtherCommands command, params object[] param)
        {
            GTA.Native.Function.Call(Enum.GetName(command.GetType(), command), param.Select(a => new GTA.Native.Parameter(a)).ToArray());
        }

        // START_CHAR_FIRE
        public static void setCharFire(Ped ped)
        {
            GTA.Native.Function.Call("START_CHAR_FIRE", new GTA.Native.Parameter(ped));
        }
    }
}