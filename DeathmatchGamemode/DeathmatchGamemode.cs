using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MIVServer;
using MIVSDK.Math;

namespace DeathmatchGamemode
{
    public class DeathmatchGamemode : Gamemode
    {

        private Vector4[] spawns = new Vector4[]
        {
            new Vector4(2191.345f, 615.6419f, 5.629018f, 108.7571f),
            /*new Vector4(2408.963f, 262.1476f, 5.812733f, 84.8248f),
            new Vector4(2420.187f, 214.0973f, 5.812845f, 196.8374f),
            new Vector4(2344.3f, 120.6696f, 5.812847f, 45.75858f),
            new Vector4(2257.409f, 96.68719f, 5.812839f, 356.7675f),
            new Vector4(2235.377f, 136.2301f, 5.902038f, 346.8188f)*/
        };

        public DeathmatchGamemode(ServerApi a)
            : base(a)
        {
            Console.WriteLine("*****************************");
            Console.WriteLine("*  MultiIV Test Deathmatch  *");
            Console.WriteLine("*****************************");
            api.onPlayerConnect += api_onPlayerConnect;
            api.createVehicle("SUPERGT", new Vector3(2365.749f, 604.4031f, 30.812778f), new Quaternion(0.0003340448f, -0.000308407f, -0.005634441f, 0.999984f), Vector3.Zero);//1
            api.createVehicle("SABREGT", new Vector3(2365.749f, 604.4031f, 30.812778f), new Quaternion(-5.144991E-05f, 0.0004049888f, 0.7113973f, -0.7027899f), Vector3.Zero);//2
            api.createVehicle("INFERNUS", new Vector3(2384.331f, 183.4532f, 15.231522f), new Quaternion(5.031423E-05f, -1.052421E-05f, -0.2579773f, 0.966151f), Vector3.Zero);//3
            api.createVehicle("INFERNUS", new Vector3(2192.73f, 627.2288f, 5.332971f), new Quaternion(0.0002791698f, -5.590584E-06f, 0.7807003f, 0.6249056f), Vector3.Zero);//4
            api.createVehicle("INFERNUS", new Vector3(2181.76f, 615.1063f, 5.525773f), new Quaternion(0.02005787f, -0.01567191f, 0.9466311f, 0.3213124f), Vector3.Zero);//5
            api.createVehicle("NRG900", new Vector3(2538.492f, 268.823f, 15.201221f), new Quaternion(0.01614731f, -0.02719297f, 0.7664362f, 0.6415414f), Vector3.Zero);//6
            api.createVehicle("INFERNUS", new Vector3(2473.699f, 223.2305f, 15.201333f), new Quaternion(-0.01775569f, -0.02556662f, -0.2633584f, 0.9641957f), Vector3.Zero);//7
            api.createVehicle("INFERNUS", new Vector3(2388.855f, 257.8469f, 15.201088f), new Quaternion(0.01293025f, -0.02881934f, 0.6902145f, 0.7229151f), Vector3.Zero);//8
            api.createVehicle("NRG900", new Vector3(2424.969f, 289.9648f, 15.390142f), new Quaternion(-0.001980515f, 0.04101723f, -0.2329017f, 0.9716329f), Vector3.Zero);//9
            api.createVehicle("INFERNUS", new Vector3(2443.699f, 367.3226f, 15.383784f), new Quaternion(-0.008690395f, 0.04274701f, -0.03868492f, 0.9982989f), Vector3.Zero);//10
            api.createVehicle("ANNIHILATOR", new Vector3(2443.363f, 416.7742f, 15.383949f), new Quaternion(-0.01375107f, 0.04257208f, 0.07150503f, 0.9964364f), Vector3.Zero);//11
            api.createVehicle("INFERNUS", new Vector3(2394.754f, 522.0314f, 15.438295f), new Quaternion(-0.0004053938f, 0.006344253f, 0.6920787f, 0.721794f), Vector3.Zero);//12
            api.createVehicle("ANNIHILATOR", new Vector3(2361.649f, 593.551f, 15.432491f), new Quaternion(-0.01194778f, 0.005154964f, 0.8922257f, 0.4514024f), Vector3.Zero);//13
            api.createVehicle("NRG900", new Vector3(2290.812f, 662.457f, 15.437609f), new Quaternion(-0.004348053f, 0.003731936f, 0.5506685f, 0.8347044f), Vector3.Zero);//14
            api.createVehicle("INFERNUS", new Vector3(2213.896f, 719.2641f, 15.458335f), new Quaternion(-0.002295473f, 0.001867346f, 0.3097835f, 0.9508025f), Vector3.Zero);//15
            api.createVehicle("ANNIHILATOR", new Vector3(2231.183f, 736.4599f, 15.459599f), new Quaternion(-0.003429666f, -0.002095187f, 0.9954517f, 0.09518221f), Vector3.Zero);//16
            api.createVehicle("ANNIHILATOR", new Vector3(2323.937f, 794.5594f, 15.998639f), new Quaternion(-0.001141903f, 0.002282516f, -0.2242321f, 0.9745324f), Vector3.Zero);//17
            api.createVehicle("ANNIHILATOR", new Vector3(2257.443f, 721.6019f, 15.438978f), new Quaternion(0.0003481103f, -0.003353917f, 0.8838975f, -0.4676686f), Vector3.Zero);//18
            api.createVehicle("INFERNUS", new Vector3(2464.762f, 589.6474f, 15.437915f), new Quaternion(-0.001692423f, -0.002709477f, 0.9179366f, -0.3967145f), Vector3.Zero);//10
            api.createVehicle("INFERNUS", new Vector3(2604.48f, 414.0821f, 5.438591f), new Quaternion(-0.002986294f, -0.002253091f, 0.9838318f, -0.1790559f), Vector3.Zero);//20

        }

        void api_onPlayerConnect(System.Net.EndPoint address, ServerPlayer player)
        {
            int random = new Random().Next(spawns.Length);
            player.Position = new Vector3(spawns[random].X, spawns[random].Y, spawns[random].Z);
        }
    }
}
