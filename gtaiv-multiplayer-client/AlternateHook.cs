using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using MIVSDK;

namespace MIVClient
{
    public class AlternateHook
    {
        // START_CHAR_FIRE
        public static void setCharFire(Ped ped)
        {
            GTA.Native.Function.Call("START_CHAR_FIRE", new GTA.Native.Parameter(ped));
        }

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
    }
}
