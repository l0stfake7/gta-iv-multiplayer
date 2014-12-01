using MIVSDK.Math;
using MIVServer;
using System;

namespace DeathmatchGamemode
{
    public class DeathmatchGamemode : Gamemode
    {
        private ServerNPCDialog dialog;

        private ServerNPC npc;

        private Vector4[] spawns = new Vector4[]
        {
            new Vector4(-229.4026f, 261.9114f, 14.862f, 359.3771f),
            //new Vector4(-242.1259f, 277.121f, 14.78422f, 203.8875f),
            //new Vector4(-219.1516f, 277.0148f, 14.79722f, 196.8374f)
            /*new Vector4(2344.3f, 120.6696f, 5.812847f, 45.75858f),
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
            api.onPlayerDisconnect += api_onPlayerDisconnect;
            api.onPlayerSendText += api_onPlayerSendText;
            api.onPlayerSendCommand += api_onPlayerSendCommand;
            api.onPlayerSpawn += api_onPlayerSpawn;
            api.createVehicle("SUPERGT", new Vector3(2365.749f, 604.4031f, 30.812778f), new Quaternion(0.0003340448f, -0.000308407f, -0.005634441f, 0.999984f));//1
            api.createVehicle("SABREGT", new Vector3(2365.749f, 604.4031f, 30.812778f), new Quaternion(-5.144991E-05f, 0.0004049888f, 0.7113973f, -0.7027899f));//2
            api.createVehicle("INFERNUS", new Vector3(2384.331f, 183.4532f, 15.231522f), new Quaternion(5.031423E-05f, -1.052421E-05f, -0.2579773f, 0.966151f));//3
            api.createVehicle("PMP600", new Vector3(2192.73f, 627.2288f, 5.332971f), new Quaternion(0.0002791698f, -5.590584E-06f, 0.7807003f, 0.6249056f));//4
            api.createVehicle("COMET", new Vector3(2181.76f, 615.1063f, 5.525773f), new Quaternion(0.02005787f, -0.01567191f, 0.9466311f, 0.3213124f));//5
            api.createVehicle("NRG900", new Vector3(2538.492f, 268.823f, 15.201221f), new Quaternion(0.01614731f, -0.02719297f, 0.7664362f, 0.6415414f));//6
            api.createVehicle("PMP600", new Vector3(2473.699f, 223.2305f, 15.201333f), new Quaternion(-0.01775569f, -0.02556662f, -0.2633584f, 0.9641957f));//7
            api.createVehicle("SABREGT", new Vector3(2388.855f, 257.8469f, 15.201088f), new Quaternion(0.01293025f, -0.02881934f, 0.6902145f, 0.7229151f));//8
            api.createVehicle("NRG900", new Vector3(2424.969f, 289.9648f, 15.390142f), new Quaternion(-0.001980515f, 0.04101723f, -0.2329017f, 0.9716329f));//9
            api.createVehicle("BURRITO2", new Vector3(2443.699f, 367.3226f, 15.383784f), new Quaternion(-0.008690395f, 0.04274701f, -0.03868492f, 0.9982989f));//10
            api.createVehicle("ANNIHILATOR", new Vector3(2443.363f, 416.7742f, 15.383949f), new Quaternion(-0.01375107f, 0.04257208f, 0.07150503f, 0.9964364f));//11
            api.createVehicle("BURRITO2", new Vector3(2394.754f, 522.0314f, 15.438295f), new Quaternion(-0.0004053938f, 0.006344253f, 0.6920787f, 0.721794f));//12
            api.createVehicle("ANNIHILATOR", new Vector3(2361.649f, 593.551f, 15.432491f), new Quaternion(-0.01194778f, 0.005154964f, 0.8922257f, 0.4514024f));//13
            api.createVehicle("SABREGT", new Vector3(2290.812f, 662.457f, 15.437609f), new Quaternion(-0.004348053f, 0.003731936f, 0.5506685f, 0.8347044f));//14
            api.createVehicle("INFERNUS", new Vector3(2213.896f, 719.2641f, 15.458335f), new Quaternion(-0.002295473f, 0.001867346f, 0.3097835f, 0.9508025f));//15
            api.createVehicle("TRASH", new Vector3(2231.183f, 736.4599f, 15.459599f), new Quaternion(-0.003429666f, -0.002095187f, 0.9954517f, 0.09518221f));//16
            api.createVehicle("ANNIHILATOR", new Vector3(2323.937f, 794.5594f, 15.998639f), new Quaternion(-0.001141903f, 0.002282516f, -0.2242321f, 0.9745324f));//17
            api.createVehicle("TRASH", new Vector3(2257.443f, 721.6019f, 15.438978f), new Quaternion(0.0003481103f, -0.003353917f, 0.8838975f, -0.4676686f));//18
            api.createVehicle("INFERNUS", new Vector3(2464.762f, 589.6474f, 15.437915f), new Quaternion(-0.001692423f, -0.002709477f, 0.9179366f, -0.3967145f));//10
            api.createVehicle("TRASH", new Vector3(2604.48f, 414.0821f, 5.438591f), new Quaternion(-0.002986294f, -0.002253091f, 0.9838318f, -0.1790559f));//20
            api.createVehicle("COMET", new Vector3(-222.2813f, 308.7827f, 14.25351f), new Quaternion(0.005902957f, -0.0244328f, 0.1292002f, 0.9912999f));
            api.createVehicle("TURISMO", new Vector3(-215.838f, 232.841f, 14.49461f), new Quaternion(0.04665223f, -0.001668674f, 0.9988528f, -0.01066844f)); //turismo
            api.createVehicle("BUS", new Vector3(-213.8333f, 211.2882f, 14.5193f), new Quaternion(-0.02278385f, -0.002269422f, 0.9997158f, 0.006635446f)); //bus
            api.createVehicle("BOBCAT", new Vector3(-213.7859f, 222.0432f, 14.51102f), new Quaternion(-0.03546162f, 0.001662043f, 0.9993044f, 0.0114226f)); //bobcat
            api.createVehicle("BUS", new Vector3(-190.178f, 234.4099f, 14.44768f), new Quaternion(-0.001852133f, 0.03537843f, 0.1309137f, 0.9907606f)); //bus
            api.createVehicle("DUKES", new Vector3(-177.4015f, 250.5323f, 14.50622f), new Quaternion(0.020323f, 0.02114783f, -0.7005287f, 0.7130212f)); //limo
            api.createVehicle("COMET", new Vector3(-156.8637f, 250.3792f, 14.50686f), new Quaternion(-0.01927846f, -0.02319385f, 0.7236615f, -0.6894958f)); //comet
            api.createVehicle("POLICE2", new Vector3(-147.4345f, 250.4484f, 14.50243f), new Quaternion(0.01940863f, 0.02000833f, -0.6933619f, 0.7200502f)); //police
            api.createVehicle("TAXI", new Vector3(-142.6815f, 272.3999f, 14.49533f), new Quaternion(-0.01609346f, 0.01616452f, 0.7106057f, 0.7032207f)); //taxi
            api.createVehicle("TAXI2", new Vector3(-152.8583f, 272.2471f, 14.5034f), new Quaternion(-0.01622381f, 0.02089827f, 0.6986172f, 0.7150064f)); //taxi
            api.createVehicle("TAXI", new Vector3(-163.0444f, 272.3791f, 14.51627f), new Quaternion(-0.02475159f, 0.02206312f, 0.7136199f, 0.699748f)); //taxi
            api.createVehicle("POLICE", new Vector3(-173.0317f, 272.3221f, 14.43763f), new Quaternion(-0.02928617f, 0.01854296f, 0.7071857f, 0.7061777f)); //oplice
            api.createVehicle("BUS", new Vector3(-193.0686f, 293.6498f, 14.57422f), new Quaternion(-0.005227661f, 0.03427088f, -0.01627137f, 0.9992664f)); //bus
            api.createVehicle("PRIMO", new Vector3(-193.3126f, 333.744f, 14.53526f), new Quaternion(-0.001298098f, 0.03176659f, -0.02627786f, 0.999149f)); //primo
            api.createVehicle("ROMEO", new Vector3(-230.3364f, 340.1446f, 14.50708f), new Quaternion(-0.03258187f, -0.006839427f, 0.9927344f, -0.115629f)); //romeo
            api.createVehicle("POLICE2", new Vector3(-254.6183f, 272.2597f, 14.5031f), new Quaternion(-0.02169987f, 0.02011596f, 0.7134325f, 0.700099f)); //romeo
            api.createVehicle("PRIMO", new Vector3(-246.4241f, 272.4527f, 14.48144f), new Quaternion(-0.01935347f, 0.01869651f, 0.7128517f, 0.7007983f)); //primo
            api.createVehicle("TAXI2", new Vector3(-226.1842f, 272.3907f, 14.49781f), new Quaternion(-0.0246615f, 0.01908576f, 0.7055625f, 0.7079613f)); //taxi
            api.createVehicle("POLICE", new Vector3(-240.1565f, 272.2839f, 14.50288f), new Quaternion(-0.0212904f, 0.01782359f, 0.7199863f, 0.6934326f)); //police
            api.createVehicle("SABREGT", new Vector3(-218.9045f, 295.5639f, 14.50088f), new Quaternion(0.003567336f, -0.03650447f, 0.1216747f, 0.9918922f)); //sabre
            api.createVehicle("INFERNUS", new Vector3(-229.1061f, 340.6898f, 14.59078f), new Quaternion(-0.03452284f, -0.009315806f, 0.9967159f, -0.07265526f)); //infernus
            api.createVehicle("SUPERGT", new Vector3(-192.2986f, 318.2284f, 14.56394f), new Quaternion(-0.003200853f, -0.04295082f, 0.002740919f, 0.9990683f)); //super gt
            api.createVehicle("TAXI", new Vector3(-192.0485f, 299.793f, 14.53912f), new Quaternion(-0.001895015f, -0.02245895f, -0.004931871f, 0.9997338f)); //taxi
            api.createVehicle("POLICE2", new Vector3(-192.5934f, 288.5875f, 14.51985f), new Quaternion(0.002819478f, 0.04960814f, -0.01887278f, 0.9985865f)); //police
            api.createVehicle("PMP600", new Vector3(-214.3087f, 236.0003f, 14.48661f), new Quaternion(-0.02997677f, -0.004240569f, 0.9993124f, 0.02140519f)); //sabre gt
            api.createVehicle("SABREGT", new Vector3(-190.662f, 238.0278f, 14.52205f), new Quaternion(0.005922097f, -0.03470945f, 0.1292048f, 0.9909926f)); //sabre gt
            api.createVehicle("BUS", new Vector3(-179.097f, 193.3891f, 14.47749f), new Quaternion(0.003143757f, -0.01116914f, 0.9981435f, -0.05979124f)); //bus
            api.createVehicle("PMP600", new Vector3(-168.2918f, 151.6172f, 14.45264f), new Quaternion(0.02342152f, 0.0009504845f, 0.9943504f, -0.1035266f)); //bobcat
            api.createVehicle("ROMEO", new Vector3(-214.6939f, 160.3058f, 14.44315f), new Quaternion(-0.001716826f, -0.03475021f, -0.01936596f, 0.999207f)); //romeo
            api.createVehicle("PRIMO", new Vector3(-215.0857f, 193.1165f, 14.4923f), new Quaternion(0.001982026f, 0.02319973f, 0.01776342f, 0.999571f)); //primo
            api.createVehicle("POLMAV", new Vector3(-215.3695f, 223.9587f, 14.4941f), new Quaternion(-0.003962317f, 0.0218101f, 0.003715113f, 0.9997474f)); //limo
            api.createVehicle("NRG900", new Vector3(-276.691f, 284.2321f, 14.51862f), new Quaternion(0.001020271f, 0.03293191f, 0.0185688f, 0.9992846f)); //nrg
            api.createVehicle("NRG900", new Vector3(-269.4363f, 231.6854f, 13.85983f), new Quaternion(0.006640278f, -0.005883073f, 0.9999465f, 0.005312838f)); //nrg
            api.createVehicle("TRASH", new Vector3(-259.2492f, 233.8205f, 13.87821f), new Quaternion(-0.004233141f, -0.01031113f, -0.02754607f, 0.9995584f)); //truck
            api.createVehicle("NRG900", new Vector3(-231.8375f, 183.8126f, 14.44518f), new Quaternion(0.00354668f, -0.006330743f, 0.7044f, 0.7097662f)); //nrg
            api.createVehicle("NRG900", new Vector3(-104.2222f, 239.5194f, 14.49237f), new Quaternion(0.01910741f, -0.006123073f, 0.999511f, 0.02398209f)); //nrg
            api.createVehicle("NRG900", new Vector3(-93.77892f, 284.0671f, 14.50623f), new Quaternion(-0.005234673f, -0.02496774f, -0.03796142f, 0.9989535f)); //nrg
            api.createVehicle("NRG900", new Vector3(-235.6815f, 314.0493f, 14.54362f), new Quaternion(0.004766549f, -0.007501219f, 0.7268152f, 0.6867756f)); //nrg
            api.createVehicle("TRASH", new Vector3(-295.1207f, 242.3266f, 14.59556f), new Quaternion(-0.006421932f, -0.01044879f, -0.3519753f, 0.9359289f)); //truck
            api.createVehicle("ANNIHILATOR", new Vector3(-48.01921f, 256.5985f, 14.4919f), new Quaternion(-0.0002179013f, -0.0001522939f, 0.7066892f, 0.7075241f)); //anihilator
            api.createVehicle("ANNIHILATOR", new Vector3(-50.16575f, 268.3227f, 14.49202f), new Quaternion(-7.295316E-05f, -0.0004549232f, 0.7083731f, -0.7058381f)); //anihilators

            dialog = new ServerNPCDialog("Helo", "what?");

            dialog.addResponse("nuthin");

            npc = new ServerNPC("Idiot", MIVSDK.ModelDictionary.getPedModelByName("F_Y_BANK_01"), new Vector3(-242.1259f, 277.121f, 14.78422f), 1.0f, dialog);

            npc = new ServerNPC("Idiot", MIVSDK.ModelDictionary.getPedModelByName("F_Y_STRIPPERC01"), new Vector3(-219.1516f, 277.0148f, 14.79722f), 1.0f, dialog);

            npc = new ServerNPC("Idiot", MIVSDK.ModelDictionary.getPedModelByName("F_Y_DOCTOR_01"), new Vector3(-219.1516f, 271.0148f, 14.79722f), 1.0f, dialog);


            dialog.onPlayerAnswerDialog += (player, key) =>
            {
                Server.instance.api.writeChat(player, "ok i say then sir");
            };

            api.onPlayerKeyDown += api_onPlayerKeyDown;
        }

        void api_onPlayerSpawn(ServerPlayer player)
        {
            int random = new Random().Next(spawns.Length);
            player.Position = new Vector3(spawns[random].X, spawns[random].Y, spawns[random].Z);
            player.Heading = spawns[random].W;
        }

        void api_onPlayerDisconnect(ServerPlayer player)
        {
            api.writeChat("Player " + player.nick + " disconnected");
        }

        void api_onPlayerSendCommand(ServerPlayer player, string command, string[] param)
        {
            if (command == "tpto")
            {
                if (param.Length != 1 || !Char.IsNumber(param[0], 0))
                {
                    api.writeChat(player, "Usage: /tpto player_id");
                    return;
                }
                byte id = 0;
                bool result = byte.TryParse(param[0], out id);
                if (result)
                {
                    player.Position = api.getPlayer(id).Position;
                }
                return;
            }
            if (command == "help")
            {
                Console.WriteLine("abc");
                api.writeChat(player, "help");
                api.writeChat(player, "help2");
                return;
            }
            if (command == "cam")
            {
                player.requester.getCameraPosition((vec) =>
                {
                    api.writeChat(vec.X.ToString() + " " + vec.Y.ToString() + " " + vec.Z.ToString());
                });
                return;
            }
            if (command == "tep")
            {
                player.Position = new Vector3(2468.039f, 147.9008f, 5.838196f);
            }
        }

        private void api_onPlayerConnect(System.Net.EndPoint address, ServerPlayer player)
        {
            api.writeChat("Player " + player.nick + " connected");
            api.writeChat(player, "Hello " + player.nick);
        }

        private void api_onPlayerKeyDown(ServerPlayer player, System.Windows.Forms.Keys key)
        {
            if (key == System.Windows.Forms.Keys.O)
            {
                if (npc.position.distanceTo(player.Position) < 5.0f)
                {
                    dialog.show(player);
                }
            }
        }

        private void api_onPlayerSendText(ServerPlayer player, string text)
        {
            api.writeChat(player.nick + "(" + player.id + "): " + text);
            Console.WriteLine("# " + player.nick + " [" + player.id + "]: " + text);
        }
    }
}