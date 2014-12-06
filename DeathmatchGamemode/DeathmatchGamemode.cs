using SharpDX;
using MIVServer;
using MIVSDK;
using System;
using System.Linq;

namespace DeathmatchGamemode
{
    public class DeathmatchGamemode : Gamemode
    {

        private class PlayerData
        {
            public bool inSkinSelectionMode = false;
            public uint currentModelIndex = 0;
        }

        private Vector3 skinCameraPos = new Vector3(-19.4158f, 599.838f, 214.9283f);
        private Vector3 skinPedPos = new Vector3(-25.26545f, 603.2829f, 212.9283f);

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
            // session date: Thursday, December 4, 2014
            // session date: Thursday, December 4, 2014

            #region vehicles

            api.createVehicle(0x4020325C, new Vector3(-455.6746f, 1473.081f, 18.38642f), new Quaternion(0.01949715f, -0.01123321f, -0.003363066f, 0.9997412f)); //
            api.createVehicle(0xDD3BD501, new Vector3(-419.015f, 1363.867f, 16.21517f), new Quaternion(-0.01049074f, -0.00793629f, -0.6964357f, 0.7174987f)); //
            api.createVehicle(0x2B26F456, new Vector3(-427.7136f, 1422.072f, 14.22857f), new Quaternion(0.017751f, 0.04233353f, 0.7106156f, 0.7020814f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-450.4042f, 1475.543f, 18.32446f), new Quaternion(0.01654042f, 0.02677742f, 0.001133326f, 0.9995039f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-461.1624f, 1422.017f, 15.07972f), new Quaternion(-0.01264003f, 0.01377441f, 0.6978475f, 0.7160025f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-414.743f, 1422.679f, 12.61326f), new Quaternion(0.02840845f, 0.0636142f, 0.7051259f, 0.7056513f)); //
            api.createVehicle(0xDD3BD501, new Vector3(-450.1914f, 1401.522f, 14.9451f), new Quaternion(-0.009531916f, 0.03070211f, 0.003888393f, 0.9994756f)); //
            api.createVehicle(0x4020325C, new Vector3(-456.5518f, 1465.855f, 17.86194f), new Quaternion(0.03929293f, -0.02081504f, -0.002872198f, 0.9990068f)); //
            api.createVehicle(0x2B26F456, new Vector3(-455.9571f, 1480.137f, 18.43574f), new Quaternion(0.00719512f, -0.01909843f, -0.004335247f, 0.9997823f)); //
            api.createVehicle(0xDD3BD501, new Vector3(-451.435f, 1481.818f, 18.37416f), new Quaternion(-0.003727992f, 0.01422108f, 0.001632323f, 0.9998906f)); //
            api.createVehicle(0xDD3BD501, new Vector3(-451.0665f, 1468.406f, 17.84684f), new Quaternion(0.03086632f, 0.018427f, 0.002198264f, 0.9993512f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-431.6533f, 1416.156f, 14.59489f), new Quaternion(0.03488899f, -0.0005202398f, 0.7029593f, 0.7103736f)); //
            api.createVehicle(0x4020325C, new Vector3(-450.6173f, 1391.765f, 15.17949f), new Quaternion(-0.0139762f, 0.02446515f, 0.0007675764f, 0.9996027f)); //
            api.createVehicle(0xDD3BD501, new Vector3(-403.5644f, 1421.593f, 11.83f), new Quaternion(-0.006523213f, 0.01246016f, 0.7163674f, 0.6975815f)); //
            api.createVehicle(0x4020325C, new Vector3(-410.1617f, 1416.225f, 12.33805f), new Quaternion(0.02398131f, 0.01094856f, 0.7091243f, 0.7045904f)); //
            api.createVehicle(0x705A3E41, new Vector3(-392.216f, 1416.703f, 12.04814f), new Quaternion(0.006880081f, 0.006284247f, 0.7030606f, 0.711069f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-450.5667f, 1382.015f, 15.62852f), new Quaternion(-0.07877923f, 0.02258645f, -0.002930325f, 0.9966319f)); //
            api.createVehicle(0x9F05F101, new Vector3(-390.8204f, 1422.084f, 12.19474f), new Quaternion(0.01043477f, 0.008494115f, 0.7045771f, 0.7094999f)); //
            api.createVehicle(0x2B26F456, new Vector3(-450.6154f, 1373.944f, 16.65843f), new Quaternion(-0.08072501f, -0.0004660829f, -0.02007671f, 0.9965341f)); //
            api.createVehicle(0x2B26F456, new Vector3(-450.9185f, 1363.071f, 17.09429f), new Quaternion(0.01837721f, -0.003183984f, 0.002765385f, 0.9998222f)); //
            api.createVehicle(0x2B26F456, new Vector3(-439.7057f, 1421.546f, 14.99641f), new Quaternion(-0.002208749f, 0.02052474f, 0.6990367f, 0.7147877f)); //
            // session date: Thursday, December 4, 2014
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x4020325C, new Vector3(-526.874f, 1424.024f, 15.10749f), new Quaternion(-0.01872399f, 0.01794788f, 0.6917421f, 0.7216787f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x45D56ADA, new Vector3(-411.529f, 1322.882f, 16.67972f), new Quaternion(-0.01318194f, 0.01752388f, -0.7038385f, 0.7100215f)); //
            api.createVehicle(0x45D56ADA, new Vector3(-434.4043f, 1324.91f, 17.07065f), new Quaternion(0.00317203f, -0.01017654f, 0.008859174f, 0.9999039f)); //
            api.createVehicle(0xDD3BD501, new Vector3(-510.049f, 1336.483f, 16.86543f), new Quaternion(0.0182118f, 0.01786626f, -0.6996049f, 0.7140744f)); //
            api.createVehicle(0x9F05F101, new Vector3(-470.1107f, 1339.21f, 17.38569f), new Quaternion(0.009321272f, 0.006926433f, -0.7047764f, 0.7093346f)); //
            api.createVehicle(0x705A3E41, new Vector3(-451.2696f, 1315.414f, 17.87201f), new Quaternion(-0.04121688f, 0.00722856f, -0.001057369f, 0.9991236f)); //
            api.createVehicle(0x45D56ADA, new Vector3(-436.7464f, 1309.41f, 17.18413f), new Quaternion(0.002371244f, 0.001182224f, 0.9684471f, 0.2492049f)); //
            api.createVehicle(0x705A3E41, new Vector3(-450.632f, 1304.694f, 18.51781f), new Quaternion(-0.04140744f, -1.011193E-06f, -4.11853E-05f, 0.9991424f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-498.8588f, 1344.368f, 17.05631f), new Quaternion(-0.0169715f, -0.01299927f, -0.7049351f, 0.7089496f)); //
            api.createVehicle(0x9F05F101, new Vector3(-450.9787f, 1324.077f, 17.50583f), new Quaternion(-0.008825083f, 0.02131835f, 0.001057305f, 0.9997332f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-451.5659f, 1293.337f, 18.99623f), new Quaternion(-0.02664101f, 2.403564E-06f, 6.124284E-05f, 0.9996451f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(-463.4789f, 1343.576f, 16.97848f), new Quaternion(0.004087608f, 0.0179743f, 0.7089069f, -0.7050611f)); //
            api.createVehicle(0x2B26F456, new Vector3(-451.2108f, 1284.636f, 19.23579f), new Quaternion(-0.01164455f, -0.002075195f, -0.001281761f, 0.9999292f)); //
            api.createVehicle(0x705A3E41, new Vector3(-474.6147f, 1344.713f, 17.19801f), new Quaternion(0.02299001f, 0.01424939f, 0.7085879f, -0.7051039f)); //
            api.createVehicle(0x705A3E41, new Vector3(-480.5341f, 1339.276f, 17.24834f), new Quaternion(0.002196792f, 0.01642669f, -0.7049038f, 0.7091092f)); //
            api.createVehicle(0x45D56ADA, new Vector3(-430.1096f, 1305.375f, 17.19075f), new Quaternion(0.0001918519f, 0.001304709f, 0.9999812f, -0.00598033f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xDD3BD501, new Vector3(-553.5922f, 1302.049f, 16.69743f), new Quaternion(-0.001290195f, 0.001263419f, -0.2146526f, 0.9766888f)); //
            api.createVehicle(0x9F05F101, new Vector3(-549.9125f, 1312.505f, 17.29152f), new Quaternion(0.0139815f, 0.01147719f, -0.1377069f, 0.9903078f)); //
            api.createVehicle(0x705A3E41, new Vector3(-546.5997f, 1383.235f, 15.96197f), new Quaternion(0.003385925f, 0.0174862f, 0.9995807f, -0.02283207f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(-556.3f, 1343.858f, 16.97096f), new Quaternion(-0.02433403f, 0.02492619f, 0.639182f, 0.7682661f)); //
            api.createVehicle(0x705A3E41, new Vector3(-552.3223f, 1392.518f, 15.47154f), new Quaternion(0.0001477292f, 0.006220593f, 0.9999806f, -0.0003374063f)); //
            api.createVehicle(0x4020325C, new Vector3(-529.81f, 1366.738f, 16.96622f), new Quaternion(6.2277E-05f, 0.0001842863f, 0.7209759f, -0.69296f)); //
            api.createVehicle(0x9F05F101, new Vector3(-552.0369f, 1385.019f, 15.97401f), new Quaternion(0.0007323439f, 0.03573221f, 0.9991397f, -0.02103761f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-546.8504f, 1374.915f, 16.43138f), new Quaternion(0.02249908f, 0.04410105f, 0.9987735f, 0.0005950796f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(-587.63f, 1333.159f, 16.65525f), new Quaternion(0.008360815f, -0.003362945f, -0.1815633f, 0.9833379f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x4020325C, new Vector3(-599.2016f, 1308.768f, 16.51165f), new Quaternion(-0.005687137f, -0.01193035f, -0.3222518f, 0.9465618f)); //
            api.createVehicle(0xC703DB5F, new Vector3(-602.0274f, 1298.755f, 16.27939f), new Quaternion(0.001867923f, 0.001040874f, -0.1856144f, 0.9826204f)); //
            api.createVehicle(0x4020325C, new Vector3(-600.5532f, 1315.487f, 16.52461f), new Quaternion(-0.009558606f, -0.00600747f, 0.9707755f, 0.2397238f)); //
            api.createVehicle(0x4020325C, new Vector3(-597.1768f, 1343.076f, 5.644133f), new Quaternion(0f, 0f, 0.999242f, 0.03892792f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x4020325C, new Vector3(1331.17f, -216.81f, 22.79458f), new Quaternion(0.003609615f, 0.03679212f, 0.7079129f, 0.7053317f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1274.932f, -188.1885f, 26.75959f), new Quaternion(-0.0232921f, -0.06462238f, 0.9976377f, -0.0006892725f)); //
            api.createVehicle(0x4020325C, new Vector3(1275.169f, -171.684f, 27.31551f), new Quaternion(2.476223E-06f, -0.004096225f, 0.9999915f, 0.0004142359f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1274.676f, -197.1887f, 26.07586f), new Quaternion(-0.02324016f, -0.02964046f, 0.9992874f, -0.002455125f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1280.868f, -166.335f, 27.18139f), new Quaternion(0.0005532402f, 0.02216974f, 0.001557639f, 0.9997529f)); //
            api.createVehicle(0x4020325C, new Vector3(1280.156f, -231.8489f, 25.60754f), new Quaternion(-0.005868063f, 0.01243783f, -0.004098888f, 0.9998971f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1275.177f, -164.0663f, 27.19744f), new Quaternion(-0.0002421892f, 0.000560959f, 0.9999998f, -0.0004982953f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1280.182f, -261.3022f, 25.06648f), new Quaternion(0.0433522f, 0.02552465f, 0.002556099f, 0.9987305f)); //
            api.createVehicle(0x4020325C, new Vector3(1251.775f, -223.0545f, 24.83924f), new Quaternion(0.02423944f, 0.002742202f, -0.7045581f, 0.709227f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1275.6f, -280.2559f, 23.44193f), new Quaternion(-0.01662136f, -0.04153021f, 0.9989933f, -0.003373104f)); //
            api.createVehicle(0x4020325C, new Vector3(1274.641f, -268.3007f, 24.61079f), new Quaternion(-0.02393962f, -0.04930567f, 0.9984968f, -0.0001963513f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1275.131f, -242.692f, 25.45211f), new Quaternion(-0.01667267f, -0.002162512f, 0.9997361f, -0.0156545f)); //
            api.createVehicle(0x4020325C, new Vector3(1274.931f, -205.9149f, 25.71831f), new Quaternion(-0.02156612f, -0.03602104f, 0.9990147f, 0.01438427f)); //
            api.createVehicle(0x4020325C, new Vector3(1281.338f, -238.5219f, 25.55692f), new Quaternion(-0.004852255f, 0.02267252f, -0.007113123f, 0.9997059f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1271.88f, -223.0494f, 25.3502f), new Quaternion(0.01885243f, 0.01098501f, -0.7032225f, 0.710635f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x9F05F101, new Vector3(1298.792f, -302.2654f, 22.99019f), new Quaternion(-0.003222435f, 0.01458781f, 0.7098171f, 0.7042276f)); //
            api.createVehicle(0x4020325C, new Vector3(1239.52f, -309.4f, 21.65703f), new Quaternion(0.04752925f, -0.001472441f, -0.7001389f, 0.7124215f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1251.52f, -309.403f, 22.16663f), new Quaternion(-0.02558514f, -0.006333931f, 0.707916f, -0.7058046f)); //
            api.createVehicle(0x4020325C, new Vector3(1277.385f, -334.2386f, 18.6012f), new Quaternion(0.0155792f, -0.0001567107f, 0.9998278f, -0.01007879f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1263.254f, -307.1832f, 22.56404f), new Quaternion(-0.02253604f, -0.01083835f, 0.7271526f, -0.6860204f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1300.193f, -354.0285f, 18.21424f), new Quaternion(6.822934E-05f, 6.362209E-05f, 0.6820335f, 0.7313209f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1243.266f, -306.4014f, 21.8136f), new Quaternion(0.03994202f, -0.02480162f, -0.6895291f, 0.7227303f)); //
            api.createVehicle(0x4020325C, new Vector3(1296.719f, -307.2852f, 22.78057f), new Quaternion(0.01508404f, 0.01227161f, -0.7052051f, 0.7087367f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1282.667f, -307.5007f, 22.43753f), new Quaternion(0.01759732f, 0.007910141f, -0.7063431f, 0.7076066f)); //
            api.createVehicle(0x4020325C, new Vector3(1317.957f, -301.9278f, 23.14455f), new Quaternion(-0.02431805f, 0.003985791f, 0.7125766f, 0.7011614f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x4020325C, new Vector3(1395.011f, -349.4033f, 35.7801f), new Quaternion(0.007739007f, 0.004695257f, 0.4985509f, 0.8668132f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1381.486f, -342.4107f, 36.04941f), new Quaternion(0.01454124f, 0.01032464f, 0.5017815f, 0.8648105f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1383.609f, -322.6009f, 19.72698f), new Quaternion(0.01073219f, -0.002625108f, 0.03004247f, 0.9994875f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1382.329f, -304.6984f, 20.14471f), new Quaternion(0.0151635f, -0.0002660947f, 0.006404967f, 0.9998645f)); //
            api.createVehicle(0x4020325C, new Vector3(1384.82f, -334.8058f, 19.69973f), new Quaternion(0.009815226f, -8.694203E-06f, 0.06644225f, 0.9977421f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1291.646f, -377.4229f, 18.416f), new Quaternion(0.002177408f, 8.770611E-05f, -0.005841086f, 0.9999806f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1385.91f, -346.1757f, 19.32973f), new Quaternion(0.006016817f, 0.0006293662f, 0.09824756f, 0.9951437f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1367.676f, -341.3811f, 36.59006f), new Quaternion(0.0202417f, 0.0001073515f, 0.5267383f, 0.8497864f)); //
            api.createVehicle(0x4020325C, new Vector3(1353.862f, -344.171f, 18.75332f), new Quaternion(0f, 0f, -0.6907972f, 0.7230486f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1354.337f, -369.5323f, 18.35762f), new Quaternion(0.004698366f, 0.005659895f, 0.7603779f, -0.6494392f)); //
            api.createVehicle(0x4020325C, new Vector3(1353.846f, -364.5977f, 18.61907f), new Quaternion(0.007303318f, -0.007273979f, 0.71313f, 0.700956f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1297.83f, -365.0844f, 18.18757f), new Quaternion(-0.02770376f, 0.02662016f, 0.7247115f, 0.6879805f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1388.624f, -361.5028f, 18.94967f), new Quaternion(0.009577684f, 0.001539832f, 0.1112395f, 0.9937463f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1383.628f, -350.3605f, 35.84839f), new Quaternion(0.01518236f, 0.008639993f, 0.4989783f, 0.8664384f)); //
            api.createVehicle(0x4020325C, new Vector3(1377.337f, -291.6154f, 19.89877f), new Quaternion(-0.003801564f, -0.007212395f, 0.9999645f, 0.002112836f)); //
            api.createVehicle(0x4020325C, new Vector3(1362.857f, -307.2719f, 20.43714f), new Quaternion(-0.02252781f, 0.05343973f, -0.704171f, 0.7076581f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1353.027f, -307.5038f, 21.04122f), new Quaternion(-0.01822784f, 0.05109163f, -0.7041723f, 0.707954f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x4020325C, new Vector3(1393.896f, -382.2788f, 18.47338f), new Quaternion(0.004212224f, 0.01226599f, 0.06485257f, 0.9978106f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1392.359f, -375.775f, 18.53975f), new Quaternion(0.01109352f, 0.01970551f, 0.08783845f, 0.995878f)); //
            api.createVehicle(0x4BA4E8DC, new Vector3(1390.328f, -369.4518f, 18.61639f), new Quaternion(0.01227357f, 0.01322964f, 0.1008401f, 0.994739f)); //

            api.createVehicle(0xDD3BD501, new Vector3(1261.683f, 202.1285f, 31.91344f), new Quaternion(-0.01216404f, 0.01930986f, 0.994775f, 0.09950811f)); //
            api.createVehicle(0x779F23AA, new Vector3(1223.032f, 508.7926f, 28.42864f), new Quaternion(-0.007068978f, 0.04160224f, 0.6139446f, 0.7882203f)); //
            api.createVehicle(0x779F23AA, new Vector3(1469.591f, 413.2977f, 18.19483f), new Quaternion(0.02303405f, 0.006636653f, 0.07116486f, 0.9971764f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1445.538f, 376.918f, 17.30123f), new Quaternion(0.002553489f, 0.002997973f, -0.03587502f, 0.9993485f)); //
            api.createVehicle(0x2B26F456, new Vector3(1540.632f, 312.6407f, 12.55528f), new Quaternion(0.03106543f, -0.01604345f, 0.7588025f, -0.6503817f)); //
            api.createVehicle(0x4020325C, new Vector3(1271.639f, 198.7567f, 32.17654f), new Quaternion(0.0007657141f, 0.03542699f, -0.07927998f, 0.9962224f)); //
            api.createVehicle(0x2B26F456, new Vector3(1273.629f, 209.765f, 32.31894f), new Quaternion(0.02484329f, 0.02014381f, -0.1190438f, 0.9923738f)); //
            api.createVehicle(0x4020325C, new Vector3(1389.057f, 261.4211f, 27.20026f), new Quaternion(0.02090507f, 0.05288386f, 0.7512028f, 0.6576174f)); //
            api.createVehicle(0x45D56ADA, new Vector3(1190.138f, 194.6678f, 32.23777f), new Quaternion(-0.0004352215f, 1.339349E-06f, -0.01573104f, 0.9998762f)); //
            api.createVehicle(0x779F23AA, new Vector3(1241.959f, 331.2983f, 20.90311f), new Quaternion(-0.001995076f, -0.001487409f, 0.6881655f, 0.7255494f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1010.398f, 297.9752f, 43.7925f), new Quaternion(0.06370876f, 0.08014531f, 0.7586421f, 0.6434126f)); //
            api.createVehicle(0x779F23AA, new Vector3(1362.513f, 182.1017f, 27.10232f), new Quaternion(0.002404395f, 0.003253399f, 0.7044426f, 0.7097494f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1235.908f, 382.7881f, 21.65616f), new Quaternion(-0.006465713f, -0.02338746f, -0.5759018f, 0.8171588f)); //
            api.createVehicle(0x4020325C, new Vector3(1397.887f, 383.9893f, 21.4464f), new Quaternion(0.05923137f, -0.05922155f, 0.7046229f, -0.7046212f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1531.972f, 313.9851f, 13.23589f), new Quaternion(0.030724f, -0.03032832f, 0.7377823f, -0.6736568f)); //
            api.createVehicle(0x779F23AA, new Vector3(1022.892f, 299.55f, 40.97048f), new Quaternion(0.08597963f, 0.104352f, 0.7261729f, 0.6740854f)); //
            api.createVehicle(0x4020325C, new Vector3(1019.144f, 258.2711f, 48.35819f), new Quaternion(0.007413443f, 0.005334687f, 0.6117652f, 0.7909868f)); //
            api.createVehicle(0x2B26F456, new Vector3(1549.091f, 316.2231f, 11.89467f), new Quaternion(0.03308756f, -0.01527602f, 0.777555f, -0.6277581f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1229.918f, 365.1559f, 21.25584f), new Quaternion(0.01525163f, -0.0002013296f, 0.003617599f, 0.9998771f)); //
            api.createVehicle(0x779F23AA, new Vector3(1026.265f, 256.1588f, 48.07016f), new Quaternion(0.02423444f, 0.01758122f, 0.6137074f, 0.7889657f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1302.49f, 289.9674f, 28.018f), new Quaternion(0.01373309f, 0.01322225f, 0.6862985f, 0.7270702f)); //
            api.createVehicle(0x4020325C, new Vector3(1248.716f, 331.6711f, 20.91424f), new Quaternion(-0.008409448f, -0.007359053f, 0.6668509f, 0.7451075f)); //
            api.createVehicle(0x2B26F456, new Vector3(1516.798f, 319.5202f, 14.31093f), new Quaternion(0.02970564f, -0.009620366f, 0.7203491f, -0.6929086f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1284.065f, 319.3446f, 21.20257f), new Quaternion(-0.005996602f, 0.006448614f, 0.7296202f, -0.6837959f)); //
            api.createVehicle(0x779F23AA, new Vector3(1037.251f, 253.9123f, 47.37612f), new Quaternion(0.0327228f, 0.02423997f, 0.6247988f, 0.7797232f)); //
            api.createVehicle(0x2B26F456, new Vector3(1256.136f, 324.6186f, 20.87739f), new Quaternion(-0.002048927f, 0.02368593f, 0.7645686f, -0.6441035f)); //
            api.createVehicle(0x2B26F456, new Vector3(1425.157f, 224.4209f, 25.75608f), new Quaternion(-0.002120186f, -0.002672951f, -0.008998482f, 0.9999537f)); //
            api.createVehicle(0x779F23AA, new Vector3(1230.539f, 471.9211f, 27.26596f), new Quaternion(0.04248653f, 2.76638E-06f, 0.0001715677f, 0.999097f)); //
            api.createVehicle(0x4020325C, new Vector3(1298.235f, 295.3522f, 28.31992f), new Quaternion(0.003704658f, 0.006029784f, 0.6883071f, 0.7253848f)); //
            api.createVehicle(0x9F05F101, new Vector3(1425.776f, 345.1183f, 18.46995f), new Quaternion(0.005425615f, 0.01271092f, 0.929692f, 0.3680786f)); //
            api.createVehicle(0x2B26F456, new Vector3(1417.269f, 337.0809f, 18.41406f), new Quaternion(0.01087734f, 0.01514188f, 0.8857393f, 0.4638086f)); //
            api.createVehicle(0x9F05F101, new Vector3(1434.006f, 353.963f, 18.18719f), new Quaternion(0.003832484f, 0.01161361f, 0.9680826f, 0.2503327f)); //
            api.createVehicle(0x4020325C, new Vector3(1413.582f, 390.3606f, 18.86953f), new Quaternion(0.05002601f, 0.05007975f, 0.7050511f, 0.7056149f)); //
            api.createVehicle(0x4020325C, new Vector3(1159.482f, 388.1048f, 27.21511f), new Quaternion(0.03535613f, 0.03285095f, 0.7013913f, 0.7111406f)); //
            api.createVehicle(0x2B26F456, new Vector3(1298.818f, 508.8209f, 27.08979f), new Quaternion(-0.008856103f, 0.04276894f, 0.5059966f, 0.8614289f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1424.56f, 233.8956f, 25.50714f), new Quaternion(-0.00369886f, 7.797273E-05f, 0.001418153f, 0.9999921f)); //
            api.createVehicle(0x4020325C, new Vector3(1313.631f, 249.2182f, 36.71484f), new Quaternion(-0.0064808f, 0.002134561f, 0.8026452f, 0.5964178f)); //
            api.createVehicle(0x2B26F456, new Vector3(1422.079f, 390.698f, 17.62689f), new Quaternion(0.03775814f, 0.03437208f, 0.7048221f, 0.7075441f)); //
            api.createVehicle(0x2B26F456, new Vector3(1520.021f, 314.8965f, 14.03893f), new Quaternion(0.0343346f, -0.02250008f, 0.7211575f, -0.6915539f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1410.916f, 334.238f, 18.48587f), new Quaternion(0.00483814f, 0.00835104f, 0.8538676f, 0.5204007f)); //
            api.createVehicle(0x779F23AA, new Vector3(1263.796f, 137.4265f, 34.11816f), new Quaternion(-0.0007805753f, 0.1481768f, 0.9889576f, 0.002431439f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1371.588f, 181.1339f, 26.70093f), new Quaternion(0.009137281f, 0.009111038f, 0.7084369f, 0.7056561f)); //
            api.createVehicle(0x4020325C, new Vector3(1264.493f, 197.3019f, 32.34136f), new Quaternion(-0.004422047f, -0.006936362f, 0.9976982f, 0.0673087f)); //
            api.createVehicle(0x2B26F456, new Vector3(1461.688f, 265.909f, 25.48358f), new Quaternion(-0.00236627f, -0.01532117f, -0.5085892f, 0.8608698f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1136.403f, 250.6086f, 40.16186f), new Quaternion(0.02398374f, 0.02587728f, 0.7502721f, 0.660187f)); //
            api.createVehicle(0x779F23AA, new Vector3(1420.86f, 154.5256f, 26.5421f), new Quaternion(-0.07807698f, -0.01679649f, 0.9757674f, -0.2037152f)); //
            api.createVehicle(0x4020325C, new Vector3(1441.615f, 300.7571f, 8.427133f), new Quaternion(-0.01079772f, 0.01021555f, -0.5657074f, 0.824472f)); //
            api.createVehicle(0x2B26F456, new Vector3(1070.178f, 249.4234f, 44.5732f), new Quaternion(0.03709057f, 0.02887706f, 0.6741235f, 0.7371214f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1297.257f, 334.1321f, 21.26231f), new Quaternion(3.52801E-05f, 0.004098184f, 0.9999915f, -0.0003159872f)); //
            api.createVehicle(0x779F23AA, new Vector3(1041.539f, 258.631f, 47.14282f), new Quaternion(0.03552309f, 0.01789897f, 0.6290089f, 0.7763797f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1277.593f, 320.2571f, 21.21695f), new Quaternion(-0.004050343f, 0.004800966f, 0.7520802f, -0.6590416f)); //
            api.createVehicle(0x4020325C, new Vector3(1053.054f, 257.1184f, 46.14253f), new Quaternion(0.03411411f, 0.02469826f, 0.6488253f, 0.759771f)); //
            api.createVehicle(0x2B26F456, new Vector3(1278.895f, 181.8583f, 32.08387f), new Quaternion(0.02398906f, 0.0203322f, 0.7078146f, 0.705698f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1389.632f, 328.5202f, 18.94074f), new Quaternion(0.007406555f, 0.007278125f, 0.744579f, 0.6674538f)); //
            api.createVehicle(0x779F23AA, new Vector3(1262.601f, 159.0754f, 32.39766f), new Quaternion(-0.0002671897f, 0.002411873f, 0.999997f, 0.0003535654f)); //
            api.createVehicle(0x779F23AA, new Vector3(1442.691f, 262.3233f, 25.7914f), new Quaternion(0.0006432769f, 0.002631739f, 0.7935116f, 0.6085491f)); //
            api.createVehicle(0x2B26F456, new Vector3(1407.09f, 181.5607f, 26.12317f), new Quaternion(0.05352958f, 0.02210207f, 0.7300305f, 0.6809564f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1424.199f, 245.4451f, 25.41589f), new Quaternion(0.002266881f, -0.002014898f, -0.001081797f, 0.9999948f)); //
            api.createVehicle(0x779F23AA, new Vector3(1424.201f, 217.6235f, 26.17253f), new Quaternion(-0.01534388f, 0.0001045349f, 0.01049908f, 0.9998272f)); //
            api.createVehicle(0x4020325C, new Vector3(1458.193f, 305.6542f, 19.20594f), new Quaternion(-0.03661593f, 0.0132956f, -0.5698575f, 0.8208197f)); //
            api.createVehicle(0x779F23AA, new Vector3(1149.117f, 252.5918f, 39.55541f), new Quaternion(0.0306557f, 0.03821937f, 0.7697858f, 0.6364191f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1368.296f, 334.6386f, 19.16081f), new Quaternion(6.110012E-05f, -2.429114E-05f, 0.9999986f, -0.001658504f)); //
            api.createVehicle(0x4020325C, new Vector3(1452.37f, 149.5505f, 23.58221f), new Quaternion(-0.06624448f, 0.06228111f, -0.7012671f, 0.7070764f)); //
            api.createVehicle(0x2B26F456, new Vector3(1389.78f, 390.099f, 22.18886f), new Quaternion(0.03399641f, 0.03135171f, 0.6836547f, 0.728339f)); //
            api.createVehicle(0x779F23AA, new Vector3(1390.486f, 385.1478f, 22.38641f), new Quaternion(-0.03096234f, 0.03476287f, -0.6939087f, 0.7185566f)); //
            api.createVehicle(0x4020325C, new Vector3(1384.708f, 273.2044f, 11.4078f), new Quaternion(-0.05376896f, 0.03558642f, -0.5195057f, 0.8520307f)); //
            api.createVehicle(0x4020325C, new Vector3(1346.17f, 109.4101f, 31.00372f), new Quaternion(0.01464395f, -0.01134312f, 0.7359477f, 0.676785f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1409.987f, 448.0944f, 22.89841f), new Quaternion(-0.02284558f, 0.02158729f, -0.7065486f, 0.7069664f)); //
            api.createVehicle(0x4020325C, new Vector3(1285.38f, 174.2643f, 31.74909f), new Quaternion(0.03790353f, 0.001442837f, 0.7019759f, 0.7111899f)); //
            api.createVehicle(0x9F05F101, new Vector3(1373.829f, 379.7999f, 22.7921f), new Quaternion(0.006239842f, -0.0001749065f, -7.590091E-05f, 0.9999806f)); //
            api.createVehicle(0x2B26F456, new Vector3(1314.365f, 324.6392f, 21.28731f), new Quaternion(0.01293031f, 0.007347793f, 0.705525f, 0.708529f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1368.72f, 341.5477f, 19.01986f), new Quaternion(-0.0001078623f, 0.001756649f, 0.9999985f, -0.000191224f)); //
            api.createVehicle(0x779F23AA, new Vector3(1403.36f, 382.6193f, 20.523f), new Quaternion(0.04570748f, -0.07460538f, 0.7236738f, -0.6845737f)); //
            api.createVehicle(0x4020325C, new Vector3(1412.503f, 349.9323f, 18.66259f), new Quaternion(0.01832693f, -0.01096051f, 0.9997582f, -0.005256262f)); //
            api.createVehicle(0x2B26F456, new Vector3(1396.178f, 329.2295f, 18.87584f), new Quaternion(0.01433733f, 0.01283254f, 0.7695735f, 0.6382683f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1396.632f, 284.9137f, 9.219736f), new Quaternion(0.02959812f, 0.05062426f, 0.8620009f, 0.5035033f)); //
            api.createVehicle(0x779F23AA, new Vector3(1315.64f, 183.8292f, 29.09097f), new Quaternion(-0.06150131f, 0.02942874f, -0.6966295f, 0.7141842f)); //
            api.createVehicle(0x4020325C, new Vector3(1400.241f, 262.1834f, 26.17467f), new Quaternion(0.001614376f, 0.03415503f, 0.7279373f, 0.6847906f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1373.7f, 360.8935f, 21.36888f), new Quaternion(0.07394259f, 2.669246E-06f, -3.533111E-05f, 0.9972625f)); //
            api.createVehicle(0x2B26F456, new Vector3(1307.499f, 296.1815f, 27.80828f), new Quaternion(0.009738591f, 0.009619858f, 0.6816266f, 0.7315722f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1400.51f, 392.2667f, 20.67184f), new Quaternion(0.03945813f, 0.06830481f, 0.7241991f, 0.6850643f)); //
            api.createVehicle(0x779F23AA, new Vector3(1296.02f, 174.3862f, 31.20922f), new Quaternion(0.03971674f, 0.006184755f, 0.6894574f, 0.72321f)); //
            api.createVehicle(0x2B26F456, new Vector3(1272.482f, 221.4002f, 33.13461f), new Quaternion(-0.02319906f, -0.03566229f, 0.9588309f, 0.2807728f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1402.757f, 331.2162f, 18.66588f), new Quaternion(0.006285883f, 0.009786565f, 0.810985f, 0.5849513f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1436.329f, 260.844f, 25.56083f), new Quaternion(0.006458642f, 0.006501322f, 0.7467059f, 0.6650913f)); //
            api.createVehicle(0x779F23AA, new Vector3(1401.645f, 389.8022f, 20.93558f), new Quaternion(0.05881871f, 0.05698734f, 0.7137015f, 0.6956456f)); //
            api.createVehicle(0x4020325C, new Vector3(1422.721f, 257.3694f, 25.74639f), new Quaternion(-0.01901351f, 0.04408057f, 0.2565402f, 0.9653406f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x4020325C, new Vector3(1320.781f, 828.2645f, 24.30318f), new Quaternion(0.02449303f, 0.1194766f, 0.9701109f, 0.2097861f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1336.878f, 824.7803f, 23.23963f), new Quaternion(-0.113757f, 0.01966536f, -0.2155248f, 0.9696503f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1167.678f, 759.3639f, 35.5747f), new Quaternion(-0.01022537f, 0.01014689f, 0.7090214f, -0.7050399f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1156.77f, 759.5086f, 35.35459f), new Quaternion(0.01560724f, -0.01577151f, -0.7027274f, 0.7111132f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1146.562f, 759.3839f, 34.92654f), new Quaternion(-0.01579715f, 0.01549959f, 0.7129005f, -0.7009159f)); //
            api.createVehicle(0x2B26F456, new Vector3(1309.365f, 807.6331f, 28.50157f), new Quaternion(0.0173282f, 0.039065f, 0.9670485f, 0.2509799f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1186.225f, 742.3461f, 35.78223f), new Quaternion(-0.002612456f, 2.552885E-05f, -0.001904645f, 0.9999948f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1185.623f, 731.7031f, 35.79783f), new Quaternion(-0.001830164f, 9.630267E-06f, 0.00184828f, 0.9999967f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1186.231f, 701.0776f, 36.79268f), new Quaternion(-0.01334917f, -0.0006110763f, -0.01210475f, 0.9998375f)); //
            api.createVehicle(0x2B26F456, new Vector3(1384.288f, 920.2015f, 13.56853f), new Quaternion(-0.0008576327f, -0.006761001f, 0.9614971f, 0.2747306f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1378.472f, 910.3107f, 13.3608f), new Quaternion(0.007657391f, 0.0007103159f, 0.9494044f, 0.3139618f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1260.696f, 979.6235f, 13.35075f), new Quaternion(-0.005961131f, 0.005572517f, -0.7007616f, 0.7133489f)); //
            api.createVehicle(0x2B26F456, new Vector3(1397.205f, 937.673f, 13.61044f), new Quaternion(0.002329185f, -0.0006901046f, 0.9338281f, 0.3577141f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1370.659f, 910.4979f, 13.35877f), new Quaternion(0.00172901f, -0.006226918f, 0.9509594f, 0.3092482f)); //
            api.createVehicle(0x2B26F456, new Vector3(1185.797f, 693.0735f, 37.01714f), new Quaternion(-0.008560014f, -0.002152048f, 0.01927467f, 0.9997753f)); //
            api.createVehicle(0x9F05F101, new Vector3(1389.167f, 936.2875f, 13.93171f), new Quaternion(0.005025519f, 0.008463198f, 0.935966f, 0.3519531f)); //
            api.createVehicle(0x4020325C, new Vector3(1430.479f, 970.8681f, 15.06096f), new Quaternion(-0.009828886f, -0.01931686f, 0.8942469f, 0.4470488f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1138.74f, 759.4626f, 34.47303f), new Quaternion(0.01580706f, -0.0160031f, -0.7002396f, 0.7135536f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1247.183f, 979.7259f, 13.3445f), new Quaternion(0.006670482f, -0.006715322f, -0.7054664f, 0.7086802f)); //
            api.createVehicle(0x4020325C, new Vector3(1185.949f, 709.8181f, 36.60877f), new Quaternion(-0.02278971f, 0.0001986178f, 0.006996712f, 0.9997158f)); //
            api.createVehicle(0x2B26F456, new Vector3(1396.232f, 943.4954f, 13.61667f), new Quaternion(-0.001485149f, -0.009562164f, 0.9331247f, 0.3594227f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1404.476f, 952.582f, 13.5494f), new Quaternion(-0.003223586f, -0.007852542f, 0.9329029f, 0.3600281f)); //
            api.createVehicle(0x4020325C, new Vector3(1233.62f, 980.1632f, 13.4894f), new Quaternion(-0.002841876f, 0.002077206f, -0.7017552f, 0.7124098f)); //
            api.createVehicle(0x9F05F101, new Vector3(1256.405f, 675.3495f, 37.44617f), new Quaternion(0.003008299f, -0.0007243214f, -0.1853117f, 0.982675f)); //
            api.createVehicle(0x2B26F456, new Vector3(1326.626f, 817.97f, 25.75183f), new Quaternion(-0.09930781f, 0.01914868f, -0.2147352f, 0.9714217f)); //
            api.createVehicle(0x2B26F456, new Vector3(1406.558f, 651.6899f, 34.51343f), new Quaternion(0.01590971f, 0.007146539f, 0.4175376f, 0.9084922f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1344.903f, 843.1186f, 18.33027f), new Quaternion(-0.1051947f, 0.0278734f, -0.2473364f, 0.962799f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1377.301f, 653.4262f, 35.6269f), new Quaternion(0.03673915f, 0.03620033f, 0.7076755f, 0.7046525f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1253.391f, 666.6846f, 37.40415f), new Quaternion(-0.02965433f, 0.006118678f, -0.1895169f, 0.9814104f)); //
            api.createVehicle(0x4020325C, new Vector3(1292.695f, 644.1343f, 38.44649f), new Quaternion(-0.01258709f, 0.003032774f, -0.47757f, 0.8784983f)); //
            api.createVehicle(0x2B26F456, new Vector3(1283.745f, 636.6331f, 38.27734f), new Quaternion(0.008466458f, -0.006100487f, -0.3725781f, 0.9279422f)); //
            api.createVehicle(0x9F05F101, new Vector3(1304.111f, 648.5474f, 38.60411f), new Quaternion(0.01197447f, -0.001818356f, -0.6550443f, 0.7554934f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1210.67f, 767.1215f, 35.52466f), new Quaternion(-0.01635774f, 0.0158239f, 0.6926093f, 0.7209538f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1421.1f, 962.848f, 14.29998f), new Quaternion(-0.008196884f, -0.01061187f, 0.9138015f, 0.4059398f)); //
            api.createVehicle(0x4020325C, new Vector3(1265.795f, 684.697f, 36.98585f), new Quaternion(-0.02306151f, 0.002580635f, -0.2299556f, 0.9729245f)); //
            api.createVehicle(0x2B26F456, new Vector3(1257.261f, 663.1226f, 37.39868f), new Quaternion(-0.0001871243f, 0.008045737f, -0.1830947f, 0.9830623f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1369.959f, 654.3051f, 36.46339f), new Quaternion(0.04161922f, 0.04228225f, 0.7045317f, 0.7071882f)); //
            api.createVehicle(0x4020325C, new Vector3(1337.56f, 649.8671f, 38.44001f), new Quaternion(-0.01175909f, 0.03251552f, 0.1443543f, 0.9889218f)); //
            api.createVehicle(0x2B26F456, new Vector3(1219.355f, 980.0223f, 13.51574f), new Quaternion(-0.0005737717f, -0.004188408f, -0.6921537f, 0.7217378f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1412.298f, 960.994f, 13.84921f), new Quaternion(-0.01215491f, -0.02951591f, 0.9290186f, 0.3686535f)); //
            api.createVehicle(0x4020325C, new Vector3(1409.363f, 679.3943f, 34.34003f), new Quaternion(-0.03762503f, 2.299678E-05f, -0.0003807157f, 0.9992918f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1392.593f, 653.6711f, 34.62263f), new Quaternion(0.02183487f, 0.02055798f, 0.717995f, 0.695402f)); //
            api.createVehicle(0x2B26F456, new Vector3(1185.873f, 721.0529f, 36.02943f), new Quaternion(-0.01914612f, -0.002192937f, 0.006140967f, 0.9997955f)); //
            api.createVehicle(0x4020325C, new Vector3(1204.22f, 757.4214f, 35.7583f), new Quaternion(0.01624637f, 0.01600237f, -0.7014204f, 0.7123829f)); //
            api.createVehicle(0x2B26F456, new Vector3(1219.72f, 757.2708f, 35.5962f), new Quaternion(0.01559923f, 0.01654779f, -0.6897312f, 0.7237083f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1266.46f, 678.8229f, 36.71198f), new Quaternion(-0.01005426f, 0.02589501f, -0.2765996f, 0.9605837f)); //
            api.createVehicle(0x4020325C, new Vector3(1276.829f, 703.4054f, 36.38097f), new Quaternion(-0.001423427f, 0.02371041f, -0.2796227f, 0.9598161f)); //
            api.createVehicle(0x2B26F456, new Vector3(1258.714f, 714.0806f, 36.22928f), new Quaternion(-0.02162153f, 0.007524744f, 0.9599121f, 0.2793647f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1193.811f, 860.0341f, 35.47533f), new Quaternion(-7.419418E-05f, 9.613147E-05f, 0.9276924f, -0.3733452f)); //
            api.createVehicle(0x779F23AA, new Vector3(1187.672f, 877.3538f, 35.86012f), new Quaternion(0.02178785f, 0.0001055015f, 0.9997607f, 0.001960016f)); //
            api.createVehicle(0x4020325C, new Vector3(1329.308f, 786.4359f, 28.73171f), new Quaternion(0.004582365f, 0.00146628f, 0.5342846f, 0.845291f)); //
            api.createVehicle(0x4020325C, new Vector3(1341.56f, 834.1567f, 20.84403f), new Quaternion(-0.126979f, 0.02574214f, -0.234002f, 0.9635646f)); //
            api.createVehicle(0x4020325C, new Vector3(1173.131f, 815.5454f, 35.70578f), new Quaternion(-3.397787E-05f, 0.0001133443f, 0.3593911f, 0.933187f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1321.9f, 817.5054f, 26.18158f), new Quaternion(0.02387943f, 0.1042629f, 0.9704982f, 0.2160837f)); //
            api.createVehicle(0x779F23AA, new Vector3(1340.614f, 782.1746f, 28.57949f), new Quaternion(0.01313247f, 0.009420768f, 0.6152811f, 0.788142f)); //
            api.createVehicle(0x2B26F456, new Vector3(1324.725f, 813.1099f, 26.87772f), new Quaternion(-0.09855255f, 0.02393989f, -0.21663f, 0.9709715f)); //
            api.createVehicle(0x2B26F456, new Vector3(1175.882f, 804.7305f, 35.54965f), new Quaternion(-0.00120008f, -1.147872E-05f, 0.01471556f, 0.999891f)); //
            api.createVehicle(0x809AA4CB, new Vector3(1346.835f, 901.5129f, 13.91902f), new Quaternion(-0.003589359f, 0.005337278f, 0.7070976f, -0.7070866f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1192.047f, 805.9245f, 35.47522f), new Quaternion(-8.282608E-05f, 0.0001080645f, 0.9253405f, -0.3791369f)); //
            api.createVehicle(0x779F23AA, new Vector3(1198.418f, 811.7545f, 35.7872f), new Quaternion(-2.302329E-05f, -3.729451E-05f, 0.9327199f, -0.3606018f)); //
            api.createVehicle(0x4020325C, new Vector3(1178.954f, 843.2908f, 35.78063f), new Quaternion(-0.02313596f, 4.044689E-05f, 0.9997323f, -0.0003982402f)); //
            api.createVehicle(0x2B26F456, new Vector3(1187.709f, 839.2537f, 35.62423f), new Quaternion(-0.0008338406f, 0.02289965f, 0.01763265f, 0.9995819f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1187.503f, 831.2863f, 35.55918f), new Quaternion(0.0001086727f, 0.02275296f, -0.0002763575f, 0.9997411f)); //
            api.createVehicle(0x779F23AA, new Vector3(1187.795f, 784.757f, 35.85553f), new Quaternion(0.0004138756f, 0.0227953f, 0.01789436f, 0.9995799f)); //
            api.createVehicle(0x4020325C, new Vector3(1178.38f, 831.0061f, 35.75457f), new Quaternion(-0.02275363f, 0.0003292361f, 0.9996263f, -0.01514321f)); //
            api.createVehicle(0x2B26F456, new Vector3(1178.442f, 790.7623f, 35.60011f), new Quaternion(-0.02294071f, -0.001497192f, 0.9996749f, 0.01102197f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1190.158f, 856.1256f, 35.48165f), new Quaternion(0.009388891f, 0.001123388f, 0.9320851f, -0.3621159f)); //
            api.createVehicle(0x779F23AA, new Vector3(1176.244f, 854.624f, 35.79368f), new Quaternion(-0.0009257631f, 0.002074703f, 0.9164934f, -0.4000432f)); //
            api.createVehicle(0x4020325C, new Vector3(1189.629f, 820.7169f, 35.72469f), new Quaternion(0.01770448f, 0.0008066334f, 0.9326109f, -0.3604482f)); //
            api.createVehicle(0x2B26F456, new Vector3(1331.283f, 692.4018f, 35.33908f), new Quaternion(-0.02203611f, 0.04775876f, 0.9985021f, -0.01506759f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1331.013f, 685.228f, 35.95262f), new Quaternion(-0.02135069f, 0.04935592f, 0.9980753f, -0.03088441f)); //
            api.createVehicle(0x779F23AA, new Vector3(1330.961f, 666.9899f, 38.04251f), new Quaternion(-0.02166405f, 0.04915937f, 0.9982876f, -0.02315101f)); //
            api.createVehicle(0x4020325C, new Vector3(1340.545f, 699.8389f, 34.7532f), new Quaternion(-0.04890484f, 0.02207485f, -0.01938375f, 0.9983713f)); //
            api.createVehicle(0x2B26F456, new Vector3(1340.272f, 692.6648f, 35.3095f), new Quaternion(-0.04989471f, 0.02247383f, -0.006554944f, 0.9984801f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1340.525f, 680.006f, 36.45929f), new Quaternion(-0.04911681f, 0.02135531f, -0.02935199f, 0.9981332f)); //
            api.createVehicle(0x779F23AA, new Vector3(1340.38f, 672.832f, 37.47777f), new Quaternion(-0.04883362f, 0.02247211f, -0.009103262f, 0.9985126f)); //
            api.createVehicle(0x4020325C, new Vector3(1402.012f, 720.9011f, 30.51028f), new Quaternion(-0.02179939f, 0.04939704f, 0.9983231f, -0.02087356f)); //
            api.createVehicle(0x2B26F456, new Vector3(1402.132f, 707.3875f, 31.70958f), new Quaternion(-0.02336804f, 0.04814687f, 0.9985043f, 0.01117168f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1401.873f, 700.214f, 32.33802f), new Quaternion(-0.02166085f, 0.04961881f, 0.9982741f, -0.02274876f)); //
            api.createVehicle(0x779F23AA, new Vector3(1411.217f, 717.634f, 30.92522f), new Quaternion(-0.05037976f, 0.02214308f, -0.01306086f, 0.9983992f)); //
            api.createVehicle(0x4020325C, new Vector3(1411.324f, 725.163f, 30.10235f), new Quaternion(-0.04738517f, 0.02291009f, 0.002872615f, 0.9986098f)); //
            api.createVehicle(0x2B26F456, new Vector3(1197.883f, 864.0148f, 35.54994f), new Quaternion(0.000445017f, -0.001160722f, 0.920579f, -0.3905545f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xC703DB5F, new Vector3(1179.654f, 736.2314f, 35.79349f), new Quaternion(0.0007892123f, -0.004957873f, 0.9999112f, -0.01234439f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1176.559f, 757.5746f, 35.6908f), new Quaternion(0.04077138f, 0.01883999f, 0.8989472f, -0.4357483f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1148.224f, 759.3395f, 34.99225f), new Quaternion(-0.02239301f, 0.02265576f, 0.7153156f, -0.6980751f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1186.228f, 743.5989f, 35.77871f), new Quaternion(-0.001888776f, -1.940042E-05f, -0.001492045f, 0.9999971f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1185.613f, 736.8036f, 35.78922f), new Quaternion(-0.004053234f, -3.51773E-06f, 0.001645348f, 0.9999905f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1186.337f, 705.948f, 36.62767f), new Quaternion(-0.01771686f, -0.0005682094f, -0.009198418f, 0.9998006f)); //
            api.createVehicle(0x2B26F456, new Vector3(1185.608f, 697.9879f, 36.85715f), new Quaternion(-0.008758388f, -0.00217501f, 0.01857653f, 0.9997868f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1140.181f, 759.5251f, 34.53656f), new Quaternion(0.01534914f, -0.01582782f, -0.6941879f, 0.719456f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1292.653f, 979.507f, 13.2766f), new Quaternion(0.00446179f, -0.003261874f, 0.7248394f, -0.6888958f)); //
            api.createVehicle(0x4020325C, new Vector3(1185.887f, 715.7612f, 36.38701f), new Quaternion(-0.02527361f, 6.117968E-05f, 0.005329763f, 0.9996664f)); //
            api.createVehicle(0x4020325C, new Vector3(1278.196f, 980.5055f, 13.45792f), new Quaternion(0.008906986f, -0.007969f, 0.7124408f, -0.7016305f)); //
            api.createVehicle(0x9F05F101, new Vector3(1271.006f, 710.2737f, 36.64081f), new Quaternion(0.006857683f, 0.0002512118f, -0.2188789f, 0.9757279f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1265.63f, 697.2763f, 36.56844f), new Quaternion(-0.02087821f, 0.002980886f, -0.1996033f, 0.9796498f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1210.67f, 767.1215f, 35.52466f), new Quaternion(-0.01635774f, 0.0158239f, 0.6926093f, 0.7209538f)); //
            api.createVehicle(0x4020325C, new Vector3(1283.952f, 722.7597f, 36.39954f), new Quaternion(-0.01960186f, 0.008543592f, -0.2614006f, 0.9649935f)); //
            api.createVehicle(0x2B26F456, new Vector3(1272.985f, 703.2265f, 36.37064f), new Quaternion(-0.002664f, -0.01188793f, -0.2415122f, 0.9703213f)); //
            api.createVehicle(0x2B26F456, new Vector3(1266.908f, 980.8371f, 13.31153f), new Quaternion(0.003344547f, -0.008716723f, -0.7033851f, 0.7107478f)); //
            api.createVehicle(0x2B26F456, new Vector3(1185.833f, 726.3361f, 35.82721f), new Quaternion(-0.01150613f, -0.002362586f, 0.002893633f, 0.9999269f)); //
            api.createVehicle(0x4020325C, new Vector3(1165.225f, 758.999f, 35.6644f), new Quaternion(-0.003870243f, 0.002201157f, 0.7188777f, -0.6951224f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1240.534f, 980.6002f, 13.24164f), new Quaternion(-0.001252551f, 0.001305168f, -0.7020742f, 0.7121015f)); //
            api.createVehicle(0x4020325C, new Vector3(1204.22f, 757.4214f, 35.7583f), new Quaternion(0.01624637f, 0.01600237f, -0.7014204f, 0.7123829f)); //
            api.createVehicle(0x2B26F456, new Vector3(1219.72f, 757.2708f, 35.5962f), new Quaternion(0.01559923f, 0.01654779f, -0.6897312f, 0.7237083f)); //
            api.createVehicle(0x4020325C, new Vector3(1260.994f, 674.3707f, 37.2682f), new Quaternion(-0.01299821f, -0.0004323101f, -0.1551751f, 0.9878014f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1266.46f, 678.8229f, 36.71198f), new Quaternion(-0.01005426f, 0.02589501f, -0.2765996f, 0.9605837f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1260.04f, 682.8967f, 36.84853f), new Quaternion(-0.01588982f, 0.003135323f, -0.185936f, 0.9824284f)); //
            api.createVehicle(0x4020325C, new Vector3(1276.829f, 703.4054f, 36.38097f), new Quaternion(-0.001423427f, 0.02371041f, -0.2796227f, 0.9598161f)); //
            api.createVehicle(0x2B26F456, new Vector3(1186.065f, 689.2383f, 37.14186f), new Quaternion(-0.00831519f, -0.002132545f, 0.00692249f, 0.9999393f)); //
            api.createVehicle(0x2B26F456, new Vector3(1258.714f, 714.0806f, 36.22928f), new Quaternion(-0.02162153f, 0.007524744f, 0.9599121f, 0.2793647f)); //
            api.createVehicle(0x4020325C, new Vector3(1173.131f, 815.5454f, 35.70578f), new Quaternion(-3.397787E-05f, 0.0001133443f, 0.3593911f, 0.933187f)); //
            api.createVehicle(0x2B26F456, new Vector3(1175.882f, 804.7305f, 35.54965f), new Quaternion(-0.00120008f, -1.147872E-05f, 0.01471556f, 0.999891f)); //
            api.createVehicle(0x4020325C, new Vector3(1178.954f, 843.2908f, 35.78063f), new Quaternion(-0.02313596f, 4.044689E-05f, 0.9997323f, -0.0003982402f)); //
            api.createVehicle(0x2B26F456, new Vector3(1187.709f, 839.2537f, 35.62423f), new Quaternion(-0.0008338406f, 0.02289965f, 0.01763265f, 0.9995819f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1187.503f, 831.2863f, 35.55918f), new Quaternion(0.0001086727f, 0.02275296f, -0.0002763575f, 0.9997411f)); //
            api.createVehicle(0x779F23AA, new Vector3(1187.795f, 784.757f, 35.85553f), new Quaternion(0.0004138756f, 0.0227953f, 0.01789436f, 0.9995799f)); //
            api.createVehicle(0x4020325C, new Vector3(1178.38f, 831.0061f, 35.75457f), new Quaternion(-0.02275363f, 0.0003292361f, 0.9996263f, -0.01514321f)); //
            api.createVehicle(0x2B26F456, new Vector3(1178.442f, 790.7623f, 35.60011f), new Quaternion(-0.02294071f, -0.001497192f, 0.9996749f, 0.01102197f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xDD3BD501, new Vector3(1368.326f, 879.6794f, 13.37765f), new Quaternion(-0.005669066f, 0.0008484189f, -0.2971378f, 0.9548173f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1165.463f, 758.9667f, 35.55398f), new Quaternion(0.003323124f, -0.003605095f, 0.7107634f, -0.7034141f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1315.535f, 973.9986f, 13.33176f), new Quaternion(-0.002312327f, 0.004181358f, 0.8472152f, -0.5312283f)); //
            api.createVehicle(0x2B26F456, new Vector3(1398.311f, 880.5754f, 15.08075f), new Quaternion(0.01045625f, 0.09562501f, 0.9781949f, -0.1840687f)); //
            api.createVehicle(0x9F05F101, new Vector3(1361.532f, 899.4968f, 13.79281f), new Quaternion(0.00403159f, 0.009225486f, 0.9462377f, 0.3233155f)); //
            api.createVehicle(0x4020325C, new Vector3(1383.268f, 918.8783f, 13.70804f), new Quaternion(-0.004169179f, -0.01853449f, 0.9663707f, 0.2564507f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1151.836f, 759.9291f, 35.04065f), new Quaternion(0.01326646f, -0.01411485f, -0.6976782f, 0.7161493f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1305.665f, 977.5544f, 13.32823f), new Quaternion(-0.001176576f, 0.0003953709f, 0.788492f, -0.6150438f)); //
            api.createVehicle(0x2B26F456, new Vector3(1367.144f, 905.861f, 13.48426f), new Quaternion(0.003232009f, -0.017613f, 0.9457049f, 0.3245327f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1376.725f, 919.3165f, 13.46248f), new Quaternion(0.001968735f, -0.000545506f, 0.9566979f, 0.2910759f)); //
            api.createVehicle(0x4020325C, new Vector3(1295.943f, 979.9832f, 13.4022f), new Quaternion(0.002649044f, -0.003124841f, 0.7278023f, -0.6857747f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1384.387f, 900.0494f, 13.35774f), new Quaternion(0.005373766f, -0.007346738f, -0.4245994f, 0.9053356f)); //
            api.createVehicle(0x4020325C, new Vector3(1219.836f, 979.8549f, 13.66058f), new Quaternion(-0.01283823f, 0.01294747f, -0.7045214f, 0.7094485f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1210.67f, 767.1215f, 35.52466f), new Quaternion(-0.01635774f, 0.0158239f, 0.6926093f, 0.7209538f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1358.148f, 885.7137f, 13.37946f), new Quaternion(-0.00640548f, -0.01607839f, 0.9562196f, 0.2921374f)); //
            api.createVehicle(0x2B26F456, new Vector3(1285.782f, 980.6568f, 13.25075f), new Quaternion(-0.00269541f, 0.006033088f, 0.7125399f, -0.7016005f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1384.979f, 930.5173f, 13.53634f), new Quaternion(-0.002098689f, 0.003181642f, 0.9392455f, 0.3432248f)); //
            api.createVehicle(0x4020325C, new Vector3(1195.311f, 759.8134f, 35.89548f), new Quaternion(-0.008104471f, 0.002762499f, -0.65014f, 0.7597662f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1274.396f, 980.9333f, 13.24696f), new Quaternion(0.01129224f, -0.009891684f, 0.711547f, -0.7024778f)); //
            api.createVehicle(0x4020325C, new Vector3(1204.22f, 757.4214f, 35.7583f), new Quaternion(0.01624637f, 0.01600237f, -0.7014204f, 0.7123829f)); //
            api.createVehicle(0x2B26F456, new Vector3(1219.72f, 757.2708f, 35.5962f), new Quaternion(0.01559923f, 0.01654779f, -0.6897312f, 0.7237083f)); //
            api.createVehicle(0x4020325C, new Vector3(1388.221f, 925.8027f, 13.77563f), new Quaternion(-0.01188987f, -0.01776154f, 0.945652f, 0.3244773f)); //
            api.createVehicle(0x4020325C, new Vector3(1375.081f, 888.5067f, 13.63997f), new Quaternion(-0.01699508f, 0.0008588724f, -0.3365537f, 0.9415104f)); //
            api.createVehicle(0x4020325C, new Vector3(1173.131f, 815.5454f, 35.70578f), new Quaternion(-3.397787E-05f, 0.0001133443f, 0.3593911f, 0.933187f)); //
            api.createVehicle(0x2B26F456, new Vector3(1175.882f, 804.7305f, 35.54965f), new Quaternion(-0.00120008f, -1.147872E-05f, 0.01471556f, 0.999891f)); //
            api.createVehicle(0x809AA4CB, new Vector3(1346.835f, 901.5129f, 13.91902f), new Quaternion(-0.003589359f, 0.005337278f, 0.7070976f, -0.7070866f)); //
            api.createVehicle(0x4020325C, new Vector3(1178.954f, 843.2908f, 35.78063f), new Quaternion(-0.02313596f, 4.044689E-05f, 0.9997323f, -0.0003982402f)); //
            api.createVehicle(0x2B26F456, new Vector3(1187.709f, 839.2537f, 35.62423f), new Quaternion(-0.0008338406f, 0.02289965f, 0.01763265f, 0.9995819f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1187.503f, 831.2863f, 35.55918f), new Quaternion(0.0001086727f, 0.02275296f, -0.0002763575f, 0.9997411f)); //
            api.createVehicle(0x779F23AA, new Vector3(1187.795f, 784.757f, 35.85553f), new Quaternion(0.0004138756f, 0.0227953f, 0.01789436f, 0.9995799f)); //
            api.createVehicle(0x4020325C, new Vector3(1178.38f, 831.0061f, 35.75457f), new Quaternion(-0.02275363f, 0.0003292361f, 0.9996263f, -0.01514321f)); //
            api.createVehicle(0x2B26F456, new Vector3(1178.442f, 790.7623f, 35.60011f), new Quaternion(-0.02294071f, -0.001497192f, 0.9996749f, 0.01102197f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1238.364f, 980.2584f, 13.23852f), new Quaternion(0.003800587f, -0.00386365f, -0.7021422f, 0.712016f)); //
            api.createVehicle(0x2B26F456, new Vector3(1411.433f, 961.1194f, 13.92005f), new Quaternion(-0.04995584f, -0.02143898f, 0.9301142f, 0.3632252f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x2B26F456, new Vector3(1405.573f, 944.9153f, 13.59672f), new Quaternion(0.009796568f, -0.002097939f, 0.9336883f, 0.3579467f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1404.666f, 951.9493f, 13.64836f), new Quaternion(-0.007710502f, -0.01342092f, 0.9333031f, 0.3587559f)); //
            api.createVehicle(0x9F05F101, new Vector3(1421.299f, 963.1965f, 14.70679f), new Quaternion(0.002881367f, 0.006998107f, 0.9143905f, 0.4047626f)); //
            api.createVehicle(0x2B26F456, new Vector3(1422.684f, 971.0895f, 14.64315f), new Quaternion(-0.004410959f, 0.004667552f, 0.9092264f, 0.4162523f)); //
            api.createVehicle(0x809AA4CB, new Vector3(1346.829f, 901.2039f, 13.91945f), new Quaternion(-0.003561842f, 0.005309798f, 0.707095f, -0.7070897f)); //
            api.createVehicle(0x2B26F456, new Vector3(1394.769f, 942.5894f, 13.62289f), new Quaternion(-0.002897071f, -0.01790575f, 0.9332244f, 0.3588361f)); //
            api.createVehicle(0x4020325C, new Vector3(1433.072f, 972.7081f, 15.20582f), new Quaternion(-0.0115487f, -0.02176214f, 0.8947316f, 0.4459243f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1381.993f, 926.5345f, 13.5419f), new Quaternion(0.001194974f, -0.01975378f, 0.9568157f, 0.2900203f)); //
            api.createVehicle(0x4020325C, new Vector3(1387.129f, 924.1807f, 13.76421f), new Quaternion(-0.001251228f, -0.005773958f, 0.9522136f, 0.3053754f)); //
            api.createVehicle(0x2B26F456, new Vector3(1384.121f, 931.0172f, 13.61142f), new Quaternion(-0.01858239f, 0.01058916f, 0.9700689f, 0.241886f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x79FBB0C5, new Vector3(1898.468f, 655.0106f, 21.68057f), new Quaternion(0.003183194f, 0.01748635f, 0.9890164f, 0.1467331f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(1898.706f, 646.2834f, 21.83521f), new Quaternion(0.0003144311f, 0.01969268f, 0.9996017f, -0.02021567f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1725.014f, 632.6572f, 28.6398f), new Quaternion(-0.0006365865f, 0.009437774f, 0.9999093f, 0.009583366f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.428f, 672.0499f, 21.60397f), new Quaternion(-0.000418815f, -0.03701606f, 0.9991925f, -0.015624f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.407f, 667.3268f, 21.29672f), new Quaternion(-0.0003082646f, -0.01762036f, 0.9997098f, -0.01642263f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1699.543f, 541.7711f, 28.5937f), new Quaternion(0.001685776f, -0.001901275f, -0.7025859f, 0.7115945f)); //
            api.createVehicle(0x4020325C, new Vector3(1909.379f, 534.3122f, 18.10461f), new Quaternion(-0.00889038f, -0.009036046f, 0.7123038f, 0.7017569f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.431f, 634.4984f, 21.21623f), new Quaternion(0.0002527816f, -0.02394727f, 0.999641f, 0.01201424f)); //
            api.createVehicle(0x4020325C, new Vector3(1702.567f, 616.8916f, 28.86136f), new Quaternion(-0.008371927f, 0.008508706f, -0.7065f, 0.7076123f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1573.435f, 495.8863f, 28.52788f), new Quaternion(0.01126978f, 0.0008869261f, 0.006509038f, 0.9999149f)); //
            api.createVehicle(0x2B26F456, new Vector3(1904.289f, 530.6441f, 17.92493f), new Quaternion(-0.01425363f, -0.002729729f, 0.006881979f, 0.999871f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.91f, 585.1635f, 20.54999f), new Quaternion(0.0002701712f, -0.002090239f, 0.999982f, -0.005617753f)); //
            api.createVehicle(0x4020325C, new Vector3(1904.621f, 567.3823f, 19.99261f), new Quaternion(0.02547763f, 0.0007418508f, -0.01083197f, 0.9996164f)); //
            api.createVehicle(0x2B26F456, new Vector3(1898.481f, 618.7782f, 20.32176f), new Quaternion(0.00498396f, -0.02125127f, 0.9996942f, 0.01161683f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.382f, 662.3169f, 21.21046f), new Quaternion(-4.783807E-05f, -0.005961147f, 0.9999548f, -0.007404157f)); //
            api.createVehicle(0x4020325C, new Vector3(1568.373f, 651.9913f, 28.23437f), new Quaternion(0.0007512448f, -0.007521288f, 0.9999693f, 0.002068094f)); //
            api.createVehicle(0x2B26F456, new Vector3(1605.568f, 462.4683f, 28.72575f), new Quaternion(0.0006814962f, 0.00285698f, 0.7095161f, -0.7046832f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(1568.632f, 693.3049f, 28.09619f), new Quaternion(-0.02807217f, 0.01057689f, 0.9975274f, 0.06355435f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1568.021f, 676.8118f, 27.96203f), new Quaternion(-3.625554E-05f, 0.0003960804f, 0.9999984f, 0.00177947f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.478f, 514.3026f, 17.9574f), new Quaternion(0.003029129f, 9.773541E-05f, 0.01658386f, 0.9998579f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1579.372f, 463.0138f, 28.5945f), new Quaternion(-0.02390325f, 0.01319709f, 0.7241207f, -0.6891326f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1903.84f, 613.1066f, 20.01533f), new Quaternion(0.02571322f, -0.0001095452f, 0.004423108f, 0.9996596f)); //
            api.createVehicle(0x4020325C, new Vector3(1899.053f, 627.0177f, 21.02661f), new Quaternion(0.006419782f, -0.03704979f, 0.9990893f, 0.02016959f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1760.886f, 737.2642f, 25.33551f), new Quaternion(0.001198598f, -0.002319651f, 0.9998997f, -0.01391915f)); //
            api.createVehicle(0x2B26F456, new Vector3(1681.853f, 728.4691f, 26.29075f), new Quaternion(0.0009634645f, 0.03052825f, 0.648216f, 0.7608436f)); //
            api.createVehicle(0x4020325C, new Vector3(1675.258f, 729.5562f, 26.84738f), new Quaternion(0.009642985f, 0.03787794f, 0.6328997f, 0.7732466f)); //
            api.createVehicle(0x2B26F456, new Vector3(1668.853f, 731.2708f, 27.02701f), new Quaternion(-0.004392378f, 0.02586973f, 0.6631421f, 0.7480335f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1723.854f, 734.7769f, 25.21864f), new Quaternion(-0.02316225f, 0.00230452f, 0.9994925f, 0.02174771f)); //
            api.createVehicle(0x4020325C, new Vector3(1708.199f, 433.0808f, 27.71718f), new Quaternion(-0.0368204f, 0.02803735f, -0.4295736f, 0.9018451f)); //
            api.createVehicle(0x2B26F456, new Vector3(1697.826f, 437.2414f, 28.21311f), new Quaternion(0.02939072f, 0.0068454f, 0.9069241f, 0.4202118f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1668.13f, 472.7304f, 28.60948f), new Quaternion(0.02845543f, -0.01616287f, 0.8370392f, 0.5461634f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.594f, 555.7414f, 19.02458f), new Quaternion(0.04333119f, 0.001060586f, -0.02091972f, 0.9988412f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1573.169f, 492.3116f, 28.49672f), new Quaternion(0.01551966f, -0.0003164273f, -0.03818729f, 0.99915f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.535f, 520.1812f, 18.04479f), new Quaternion(0.001703631f, 0.002332191f, -0.01687599f, 0.9998534f)); //
            api.createVehicle(0x4020325C, new Vector3(1675.096f, 475.9154f, 28.73809f), new Quaternion(-0.01446321f, 0.01233554f, 0.8760008f, 0.481935f)); //
            api.createVehicle(0x2B26F456, new Vector3(1644.344f, 487.3764f, 28.5685f), new Quaternion(-0.02340852f, 0.002059335f, 0.9997119f, 0.004891905f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1643.998f, 494.3929f, 28.44179f), new Quaternion(-0.01635668f, 0.001054534f, 0.9998649f, 0.001260618f)); //
            api.createVehicle(0x4020325C, new Vector3(1617.212f, 511.9188f, 28.94821f), new Quaternion(0.01264549f, 0.01227346f, 0.6958548f, 0.7179663f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1593.871f, 462.6515f, 28.68802f), new Quaternion(-0.01411897f, 0.01343744f, 0.7129323f, -0.7009619f)); //
            api.createVehicle(0x2B26F456, new Vector3(1623.945f, 512.3341f, 28.58756f), new Quaternion(0.01056993f, -0.009960994f, 0.7094861f, -0.7045696f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1594.438f, 511.7607f, 28.56301f), new Quaternion(-0.0001891004f, 0.00169515f, 0.7158242f, -0.6982784f)); //
            api.createVehicle(0x4020325C, new Vector3(1587.625f, 512.2516f, 28.7921f), new Quaternion(-0.0005595467f, 0.0008836025f, 0.729261f, 0.6842348f)); //
            api.createVehicle(0x2B26F456, new Vector3(1602.048f, 513.0189f, 28.76994f), new Quaternion(-0.01419722f, -0.01606578f, 0.705921f, 0.707966f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1607.829f, 548.8681f, 28.52953f), new Quaternion(-0.01735689f, 0.008120537f, 0.6993281f, 0.7145438f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1898.563f, 609.551f, 20.00084f), new Quaternion(0.0004435413f, 0.01678327f, 0.9998327f, 0.00724829f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.503f, 588.0978f, 20.57552f), new Quaternion(-0.004042671f, -5.178616E-05f, -0.021652f, 0.9997574f)); //
            api.createVehicle(0x4020325C, new Vector3(1601.169f, 548.9094f, 28.67675f), new Quaternion(-0.01572869f, 0.0007769378f, 0.6919377f, 0.7217854f)); //
            api.createVehicle(0x2B26F456, new Vector3(1594.148f, 548.9172f, 28.44136f), new Quaternion(-0.01848863f, 0.0135442f, 0.7232929f, 0.6901609f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1592.428f, 539.0435f, 28.35098f), new Quaternion(-0.01543182f, -0.0150702f, 0.7099143f, -0.7039578f)); //
            api.createVehicle(0x4020325C, new Vector3(1603.274f, 531.0704f, 28.90816f), new Quaternion(-0.008090911f, 0.03115012f, 0.9988942f, -0.03427431f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1904.578f, 576.836f, 20.44767f), new Quaternion(0.03593462f, 3.924783E-05f, 0.0006606797f, 0.9993539f)); //
            api.createVehicle(0x2B26F456, new Vector3(1627.547f, 470.7017f, 28.61576f), new Quaternion(-0.01763649f, 0.01536687f, 0.7128772f, 0.7008986f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1669.132f, 462.2936f, 28.58572f), new Quaternion(0.009722583f, 0.02181043f, -0.627747f, 0.7780511f)); //
            api.createVehicle(0x4020325C, new Vector3(1675.418f, 464.9923f, 28.78311f), new Quaternion(0.006869779f, 0.01732299f, -0.5242612f, 0.8513536f)); //
            api.createVehicle(0x2B26F456, new Vector3(1682.084f, 468.7804f, 28.58878f), new Quaternion(0.005476967f, 0.02004525f, -0.4929985f, 0.869782f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1784.813f, 450.3745f, 27.35435f), new Quaternion(0.01745448f, 0.01563575f, -0.6763365f, 0.7362199f)); //
            api.createVehicle(0x4020325C, new Vector3(1777.964f, 449.6568f, 27.54731f), new Quaternion(-0.01681542f, -0.01633267f, 0.7099841f, -0.7038274f)); //
            api.createVehicle(0x2B26F456, new Vector3(1770.972f, 449.3569f, 27.37124f), new Quaternion(0.01683587f, 0.01620981f, -0.6713174f, 0.7408015f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1567.927f, 685.9743f, 27.88346f), new Quaternion(-0.0001571955f, -0.004681121f, 0.9997265f, 0.0229088f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1763.733f, 448.7534f, 27.26125f), new Quaternion(0.01801412f, 0.01511288f, -0.6614245f, 0.7496431f)); //
            api.createVehicle(0x4020325C, new Vector3(1756.742f, 448.5385f, 27.48678f), new Quaternion(0.01568948f, 0.01728927f, -0.685891f, 0.7273296f)); //
            api.createVehicle(0x2B26F456, new Vector3(1736.617f, 434.5209f, 26.92482f), new Quaternion(0.03076716f, 0.006264121f, -0.003449802f, 0.999501f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1873.834f, 562.598f, 28.6042f), new Quaternion(-0.0009449988f, 0.0003106783f, -0.0237023f, 0.9997185f)); //
            api.createVehicle(0x4020325C, new Vector3(1873.798f, 569.811f, 28.81216f), new Quaternion(-8.555243E-05f, 1.820995E-05f, 0.004116457f, 0.9999915f)); //
            api.createVehicle(0x2B26F456, new Vector3(1873.748f, 579.8249f, 28.657f), new Quaternion(-0.001634448f, 0.001640286f, -0.001867535f, 0.9999956f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1873.819f, 614.561f, 28.41866f), new Quaternion(9.328532E-05f, 2.368097E-07f, 0.002555381f, 0.9999967f)); //
            api.createVehicle(0x4020325C, new Vector3(1873.827f, 623.9391f, 28.64836f), new Quaternion(0f, 0f, 0.0103281f, 0.9999467f)); //
            api.createVehicle(0x2B26F456, new Vector3(1600.703f, 577.9228f, 30.15652f), new Quaternion(0.01146078f, -0.01425255f, -0.6781695f, 0.7346779f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1573.28f, 503.4405f, 28.58851f), new Quaternion(0.002977789f, -0.001421246f, 0.002910914f, 0.9999903f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1573.366f, 480.8689f, 28.6402f), new Quaternion(-0.006021571f, 0.000470439f, -0.002139891f, 0.9999795f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1899.394f, 595.2306f, 20.35983f), new Quaternion(0.0004495183f, 0.02323209f, 0.9997105f, -0.006241489f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1904.547f, 549.4739f, 18.61222f), new Quaternion(0.03067208f, -5.195448E-06f, 0.003325782f, 0.9995239f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1591.835f, 582.9266f, 29.48217f), new Quaternion(0.03643724f, -0.02533712f, -0.6988631f, 0.7138773f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1904.716f, 601.4536f, 20.1342f), new Quaternion(-0.007165144f, 4.876782E-05f, 0.0002859223f, 0.9999743f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1898.89f, 568.0964f, 19.9152f), new Quaternion(-0.000253458f, -0.034541f, 0.9994025f, -0.001281172f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1904.745f, 541.4761f, 18.22485f), new Quaternion(0.008501317f, 0.0001319302f, 0.00331957f, 0.9999583f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x79FBB0C5, new Vector3(1898.183f, 651.2725f, 21.759f), new Quaternion(-0.0003990451f, 0.01440696f, 0.9991465f, 0.03870727f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(1898.932f, 639.0318f, 21.76127f), new Quaternion(-3.1703E-05f, -0.006800291f, 0.9998959f, -0.01272585f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.656f, 509.2692f, 17.88731f), new Quaternion(0.01147524f, 0.0002369232f, 0.01636729f, 0.9998002f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.595f, 525.9404f, 18.08583f), new Quaternion(-0.008535274f, -0.001973607f, -0.01236859f, 0.9998851f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1898.401f, 584.6479f, 20.61868f), new Quaternion(-0.0001988712f, -0.007260416f, 0.9999722f, -0.001682315f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.428f, 672.0499f, 21.60397f), new Quaternion(-0.000418815f, -0.03701606f, 0.9991925f, -0.015624f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.43f, 666.6394f, 21.27112f), new Quaternion(-0.0005454015f, -0.01012677f, 0.9998087f, -0.0167267f)); //
            api.createVehicle(0x4020325C, new Vector3(1906.874f, 535.8353f, 18.13813f), new Quaternion(0.01274404f, -0.00475596f, 0.5018296f, 0.8648595f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1898.633f, 611.4006f, 20.04604f), new Quaternion(0.004699458f, -0.01520232f, 0.9998646f, 0.004193644f)); //
            api.createVehicle(0x2B26F456, new Vector3(1898.714f, 605.3383f, 20.04495f), new Quaternion(0.002147994f, 0.01502518f, 0.999819f, -0.01147325f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.194f, 628.3495f, 20.8778f), new Quaternion(0.002962191f, -0.02858647f, 0.9993703f, -0.0208096f)); //
            api.createVehicle(0x2B26F456, new Vector3(1904.361f, 531.9951f, 17.91209f), new Quaternion(0.009290702f, -0.001903355f, -0.01634826f, 0.9998214f)); //
            api.createVehicle(0x4020325C, new Vector3(1898.582f, 622.4279f, 20.72611f), new Quaternion(-0.0003133743f, -0.04051794f, 0.9991623f, -0.005740983f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1899.108f, 550.1921f, 18.55902f), new Quaternion(-8.149246E-05f, -0.04420913f, 0.9990031f, -0.006201874f)); //
            api.createVehicle(0x4020325C, new Vector3(1904.711f, 592.7654f, 20.57162f), new Quaternion(-0.0272547f, 0.0005140579f, 0.004377036f, 0.9996189f)); //
            api.createVehicle(0x2B26F456, new Vector3(1898.289f, 594.3834f, 20.35634f), new Quaternion(0.00187196f, 0.01904805f, 0.9998146f, -0.002146994f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1898.409f, 659.4703f, 21.20182f), new Quaternion(-3.17278E-05f, 0.003897357f, 0.9999785f, -0.005267099f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.448f, 515.2083f, 17.96974f), new Quaternion(0.01081438f, 0.0002389602f, 0.01617275f, 0.9998107f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1903.65f, 654.8865f, 21.28955f), new Quaternion(-0.01291583f, 0.0008216905f, 0.002160355f, 0.9999139f)); //
            api.createVehicle(0x4020325C, new Vector3(1899.286f, 615.5046f, 20.34625f), new Quaternion(0.008558594f, -0.02309658f, 0.9995949f, 0.01426472f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1760.886f, 737.2642f, 25.33551f), new Quaternion(0.001198598f, -0.002319651f, 0.9998997f, -0.01391915f)); //
            api.createVehicle(0x2B26F456, new Vector3(1681.853f, 728.4691f, 26.29075f), new Quaternion(0.0009634645f, 0.03052825f, 0.648216f, 0.7608436f)); //
            api.createVehicle(0x4020325C, new Vector3(1675.258f, 729.5562f, 26.84738f), new Quaternion(0.009642985f, 0.03787794f, 0.6328997f, 0.7732466f)); //
            api.createVehicle(0x2B26F456, new Vector3(1668.853f, 731.2708f, 27.02701f), new Quaternion(-0.004392378f, 0.02586973f, 0.6631421f, 0.7480335f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1723.854f, 734.7769f, 25.21864f), new Quaternion(-0.02316225f, 0.00230452f, 0.9994925f, 0.02174771f)); //
            api.createVehicle(0x4020325C, new Vector3(1708.199f, 433.0808f, 27.71718f), new Quaternion(-0.0368204f, 0.02803735f, -0.4295736f, 0.9018451f)); //
            api.createVehicle(0x2B26F456, new Vector3(1697.826f, 437.2414f, 28.21311f), new Quaternion(0.02939072f, 0.0068454f, 0.9069241f, 0.4202118f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1668.13f, 472.7304f, 28.60948f), new Quaternion(0.02845543f, -0.01616287f, 0.8370392f, 0.5461634f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.933f, 576.0571f, 20.34966f), new Quaternion(0.05475476f, 0.003309972f, -0.0006248715f, 0.9984941f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.534f, 520.1711f, 18.03715f), new Quaternion(0.01298595f, -0.0001865704f, -0.01841837f, 0.999746f)); //
            api.createVehicle(0x4020325C, new Vector3(1675.096f, 475.9154f, 28.73809f), new Quaternion(-0.01446321f, 0.01233554f, 0.8760008f, 0.481935f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1898.611f, 575.9652f, 20.36649f), new Quaternion(-0.0004088747f, -0.0360597f, 0.9993489f, -0.001124354f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1904.648f, 628.2917f, 20.94285f), new Quaternion(0.03850335f, 0.0005369384f, 0.0008000773f, 0.999258f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1904.34f, 614.4779f, 20.16883f), new Quaternion(0.02499266f, -0.0001681148f, 0.00437218f, 0.9996781f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1669.132f, 462.2936f, 28.58572f), new Quaternion(0.009722583f, 0.02181043f, -0.627747f, 0.7780511f)); //
            api.createVehicle(0x4020325C, new Vector3(1675.418f, 464.9923f, 28.78311f), new Quaternion(0.006869779f, 0.01732299f, -0.5242612f, 0.8513536f)); //
            api.createVehicle(0x2B26F456, new Vector3(1682.084f, 468.7804f, 28.58878f), new Quaternion(0.005476967f, 0.02004525f, -0.4929985f, 0.869782f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1784.813f, 450.3745f, 27.35435f), new Quaternion(0.01745448f, 0.01563575f, -0.6763365f, 0.7362199f)); //
            api.createVehicle(0x4020325C, new Vector3(1777.964f, 449.6568f, 27.54731f), new Quaternion(-0.01681542f, -0.01633267f, 0.7099841f, -0.7038274f)); //
            api.createVehicle(0x2B26F456, new Vector3(1770.972f, 449.3569f, 27.37124f), new Quaternion(0.01683587f, 0.01620981f, -0.6713174f, 0.7408015f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1763.733f, 448.7534f, 27.26125f), new Quaternion(0.01801412f, 0.01511288f, -0.6614245f, 0.7496431f)); //
            api.createVehicle(0x4020325C, new Vector3(1756.742f, 448.5385f, 27.48678f), new Quaternion(0.01568948f, 0.01728927f, -0.685891f, 0.7273296f)); //
            api.createVehicle(0x2B26F456, new Vector3(1736.617f, 434.5209f, 26.92482f), new Quaternion(0.03076716f, 0.006264121f, -0.003449802f, 0.999501f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1873.834f, 562.598f, 28.6042f), new Quaternion(-0.0009449988f, 0.0003106783f, -0.0237023f, 0.9997185f)); //
            api.createVehicle(0x4020325C, new Vector3(1873.798f, 569.811f, 28.81216f), new Quaternion(-8.555243E-05f, 1.820995E-05f, 0.004116457f, 0.9999915f)); //
            api.createVehicle(0x2B26F456, new Vector3(1873.748f, 579.8249f, 28.657f), new Quaternion(-0.001634448f, 0.001640286f, -0.001867535f, 0.9999956f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1873.819f, 614.561f, 28.41866f), new Quaternion(9.328532E-05f, 2.368097E-07f, 0.002555381f, 0.9999967f)); //
            api.createVehicle(0x4020325C, new Vector3(1873.827f, 623.9391f, 28.64836f), new Quaternion(0f, 0f, 0.0103281f, 0.9999467f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1899.593f, 565.8884f, 19.77674f), new Quaternion(-0.000282729f, -0.0306046f, 0.9995292f, -0.002155418f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1904.492f, 558.9131f, 19.29291f), new Quaternion(0.04856031f, -2.030619E-05f, 0.00243771f, 0.9988173f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1904.474f, 646.5972f, 21.57297f), new Quaternion(-0.01287753f, -0.0007657983f, 0.00161267f, 0.9999155f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1899.076f, 543.3876f, 18.29801f), new Quaternion(0.0003071188f, -0.02716865f, 0.9996212f, -0.004368719f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1904.723f, 544.1805f, 18.31166f), new Quaternion(0.02931707f, 0.0002837811f, 0.003586121f, 0.9995637f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xC703DB5F, new Vector3(2312.185f, 532.7762f, 5.706959f), new Quaternion(0.005687268f, 0.004837144f, 0.2290509f, 0.9733859f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2320.565f, 530.576f, 5.706227f), new Quaternion(0.001642826f, 0.007500859f, 0.1952597f, 0.9807215f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2324.229f, 519.3237f, 5.711151f), new Quaternion(0.001080787f, 0.004325106f, 0.13338f, 0.991055f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2327.372f, 496.6627f, 5.473542f), new Quaternion(0.003384926f, 0.0002720076f, 0.05976056f, 0.998207f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2322.625f, 488.8508f, 5.470831f), new Quaternion(0.007736317f, -0.0006797047f, 0.007016416f, 0.9999452f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2317.39f, 523.3914f, 5.713795f), new Quaternion(-0.01099123f, 0.005000424f, 0.1694825f, 0.9854593f)); //
            api.createVehicle(0x2B26F456, new Vector3(2322.607f, 483.2803f, 5.547687f), new Quaternion(-0.001199006f, -0.004873734f, -0.01984295f, 0.9997905f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2321.196f, 502.034f, 5.473794f), new Quaternion(0.0002416405f, 0.0009282127f, 0.06656089f, 0.9977819f)); //
            api.createVehicle(0x2B26F456, new Vector3(2034.319f, 489.388f, 17.69384f), new Quaternion(-0.01808836f, -0.002275109f, 0.00242601f, 0.9998308f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(2019.483f, 478.9352f, 17.99938f), new Quaternion(-0.005453826f, 0.005259991f, -0.6991909f, 0.7148952f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2004.887f, 479.0237f, 17.8157f), new Quaternion(-0.001276184f, 0.0005104416f, 0.728378f, -0.6851742f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2015.948f, 698.6859f, 14.77082f), new Quaternion(-0.01465193f, 0.01748902f, 0.9532596f, -0.30129f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2322.701f, 476.601f, 5.582333f), new Quaternion(0.003276158f, -1.401365E-06f, -0.01547328f, 0.9998749f)); //
            api.createVehicle(0x2B26F456, new Vector3(2329.898f, 480.2884f, 5.553486f), new Quaternion(0.00392121f, 0.002403073f, 0.1848735f, 0.9827516f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2328.168f, 490.3899f, 5.473893f), new Quaternion(0.001907257f, -9.005967E-06f, 0.002404116f, 0.9999952f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2048.568f, 670.9637f, 14.75347f), new Quaternion(0.002446449f, 0.02321173f, 0.9922165f, -0.1223174f)); //
            api.createVehicle(0x2B26F456, new Vector3(2044.957f, 665.6334f, 14.98118f), new Quaternion(0.002152999f, 0.02532439f, 0.9922832f, -0.1213597f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2050.768f, 659.3328f, 15.29601f), new Quaternion(-0.0005514884f, 0.02095474f, 0.9926211f, -0.1194321f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2025.118f, 674.9426f, 14.7206f), new Quaternion(0.007473709f, -0.0006329127f, 0.9938096f, -0.1108438f)); //
            api.createVehicle(0x2B26F456, new Vector3(2034.228f, 465.7823f, 17.82391f), new Quaternion(0.006624579f, -0.003466983f, 0.03085379f, 0.9994959f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2029.108f, 479.5862f, 17.63321f), new Quaternion(0.02130631f, 0.0006432809f, -0.5808857f, 0.8137059f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2011.172f, 478.9867f, 17.80041f), new Quaternion(-2.479335E-05f, -0.0001136556f, 0.7204695f, -0.6934867f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2045.324f, 682.684f, 14.1725f), new Quaternion(-0.0006877269f, 0.01761875f, 0.9900523f, -0.1395906f)); //
            api.createVehicle(0x2B26F456, new Vector3(2010.912f, 704.1667f, 14.61898f), new Quaternion(-0.004250257f, 0.01798666f, 0.92993f, -0.3672721f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2323.593f, 503.418f, 5.477561f), new Quaternion(-0.008564432f, -0.0008003942f, 0.07463899f, 0.9971735f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2327.844f, 477.1668f, 5.53839f), new Quaternion(0.007563618f, -0.0004209289f, -0.007844566f, 0.9999405f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2327.295f, 502.7315f, 5.534925f), new Quaternion(0.01603631f, 0.000675806f, 0.04420279f, 0.9988936f)); //
            api.createVehicle(0x2B26F456, new Vector3(2019.779f, 692.5737f, 14.82856f), new Quaternion(0.001493811f, 0.03026051f, 0.9695772f, -0.2429035f)); //
            api.createVehicle(0x2B26F456, new Vector3(2042.664f, 674.7005f, 14.50145f), new Quaternion(0.006778314f, 0.01085466f, 0.9920107f, -0.1255025f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2034.318f, 470.7076f, 17.73052f), new Quaternion(-0.002235048f, -8.534844E-06f, 0.009471674f, 0.9999527f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2034.855f, 456.6019f, 17.89486f), new Quaternion(0.005607266f, -0.0001495379f, 0.02005672f, 0.9997831f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(2322.457f, 467.5099f, 5.868979f), new Quaternion(0.004269803f, -0.0006838962f, -0.007438916f, 0.999963f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2043.905f, 688.0417f, 13.89111f), new Quaternion(0.0008968626f, 0.00256436f, 0.9895193f, -0.144376f)); //
            api.createVehicle(0x2B26F456, new Vector3(2035.198f, 448.1288f, 17.95835f), new Quaternion(0.004253626f, -0.00270091f, 0.01318084f, 0.9999005f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2320.962f, 507.8461f, 5.529976f), new Quaternion(0.02291872f, 0.005062699f, 0.1067329f, 0.9940106f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2031.899f, 705.6766f, 12.78648f), new Quaternion(0.01277652f, 0.02651452f, 0.9520219f, -0.3046113f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2326.714f, 506.868f, 5.5393f), new Quaternion(0.01735889f, 0.00132359f, 0.07625406f, 0.9969364f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2023.516f, 681.3536f, 14.84436f), new Quaternion(-0.0002195903f, -0.01715433f, 0.9952142f, -0.09620038f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(2034.579f, 700.371f, 13.41623f), new Quaternion(-0.01286764f, 0.04092047f, 0.9866274f, -0.1572461f)); //
            api.createVehicle(0x2B26F456, new Vector3(2327.811f, 485.3125f, 5.55259f), new Quaternion(-0.000753349f, -0.01659731f, 0.003354294f, 0.9998563f)); //
            api.createVehicle(0x2B26F456, new Vector3(2273.495f, 523.937f, 5.480648f), new Quaternion(-2.701978E-05f, -0.00121758f, 0.9998873f, 0.01496104f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2273.395f, 517.7428f, 5.40629f), new Quaternion(9.328671E-05f, -4.117175E-07f, -0.004367499f, 0.9999904f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2269.904f, 517.6669f, 5.47456f), new Quaternion(-4.099061E-05f, -9.613118E-07f, 0.02450922f, 0.9996996f)); //
            api.createVehicle(0x4020325C, new Vector3(2224.887f, 475.093f, 5.636481f), new Quaternion(0f, 0f, -0.01556515f, 0.9998789f)); //
            api.createVehicle(0x2B26F456, new Vector3(2267.2f, 474.5355f, 5.481136f), new Quaternion(-5.283362E-06f, -0.001217893f, 0.999995f, -0.002909965f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2263.648f, 474.8308f, 5.406777f), new Quaternion(9.328339E-05f, 5.441665E-07f, 0.00576126f, 0.9999834f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2241.031f, 474.5915f, 5.475048f), new Quaternion(8.818476E-08f, -4.100301E-05f, 0.9999945f, -0.00332753f)); //
            api.createVehicle(0x4020325C, new Vector3(2263.585f, 481.5297f, 5.63624f), new Quaternion(7.207155E-05f, 9.816827E-07f, 0.01372296f, 0.9999058f)); //
            api.createVehicle(0x35ED670B, new Vector3(2249.352f, 711.6138f, 6.063028f), new Quaternion(0f, 0f, 0.8436982f, -0.5368178f)); //
            api.createVehicle(0xCD935EF9, new Vector3(2293.122f, 671.8082f, 5.906947f), new Quaternion(0f, 0f, 0.8436928f, -0.5368263f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(2299.485f, 552.8946f, 5.989349f), new Quaternion(0.01599742f, -0.00597866f, 0.3642664f, 0.9311382f)); //
            api.createVehicle(0x4020325C, new Vector3(2302.221f, 556.936f, 5.831792f), new Quaternion(-0.01018402f, 0.0100787f, 0.3739003f, 0.9273582f)); //
            api.createVehicle(0x2B26F456, new Vector3(2287.452f, 567.9501f, 5.668913f), new Quaternion(-0.002442268f, 0.02402225f, 0.5322102f, 0.8462678f)); //
            api.createVehicle(0x31F0B376, new Vector3(2245.307f, 758.4113f, 5.579461f), new Quaternion(-3.425064E-05f, -1.424049E-05f, 0.3826945f, 0.923875f)); //
            api.createVehicle(0x2B26F456, new Vector3(2260.053f, 481.2297f, 5.480891f), new Quaternion(-6.991005E-06f, -0.001299698f, 0.9999972f, -0.001967289f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2266.912f, 481.0668f, 5.406534f), new Quaternion(-1.153955E-08f, 5.443387E-07f, 0.9997755f, -0.02119083f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2237.906f, 481.6637f, 5.47456f), new Quaternion(-4.099807E-05f, 6.583959E-07f, -0.01482427f, 0.9998901f)); //
            api.createVehicle(0x4020325C, new Vector3(2270.022f, 481.6988f, 5.63624f), new Quaternion(7.207214E-05f, 9.867205E-07f, 0.01372308f, 0.9999058f)); //
            api.createVehicle(0x2B26F456, new Vector3(2254.304f, 481.1843f, 5.480648f), new Quaternion(-0.001217797f, -1.656474E-05f, 0.02090718f, 0.9997807f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2273.605f, 481.4812f, 5.406532f), new Quaternion(0.0001859541f, 5.094768E-06f, 0.02742206f, 0.999624f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2234.662f, 480.941f, 5.47456f), new Quaternion(1.94771E-07f, -4.100298E-05f, 0.9999813f, -0.006110685f)); //
            api.createVehicle(0x4020325C, new Vector3(2234.125f, 459.8965f, 5.636235f), new Quaternion(-7.205437E-05f, 1.880731E-06f, -0.02608028f, 0.9996599f)); //
            api.createVehicle(0x2B26F456, new Vector3(2227.545f, 460.1364f, 5.480891f), new Quaternion(-0.001299106f, 4.013652E-05f, -0.02353228f, 0.9997222f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2241.161f, 460.0664f, 5.406532f), new Quaternion(-4.455661E-06f, 0.0001859753f, 0.9997146f, -0.02388818f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2250.781f, 459.5285f, 5.475048f), new Quaternion(-8.49352E-07f, -4.099428E-05f, 0.999809f, 0.01954529f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xC703DB5F, new Vector3(2283.301f, 563.3279f, 5.718515f), new Quaternion(-0.01879179f, -0.004229839f, 0.5441725f, 0.8387522f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2295.986f, 563.7128f, 5.707251f), new Quaternion(-0.004658438f, 0.01135022f, 0.4590322f, 0.8883349f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2294.721f, 558.0869f, 5.706928f), new Quaternion(-0.005484244f, 0.01542472f, 0.4317147f, 0.9018616f)); //
            api.createVehicle(0x2B26F456, new Vector3(2059.11f, 506.854f, 14.84783f), new Quaternion(-0.01288527f, 0.02247779f, -0.7059342f, 0.7078033f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(2033.56f, 487.6914f, 18.0535f), new Quaternion(0.0005045105f, -0.01216724f, -0.1209171f, 0.9925879f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2008.703f, 478.7616f, 17.77634f), new Quaternion(0.004147975f, -0.00428457f, 0.7301643f, -0.6832456f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2023.34f, 683.749f, 14.83994f), new Quaternion(0.01322568f, 0.02540211f, 0.9879214f, -0.1522867f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2058.563f, 627.1133f, 17.02962f), new Quaternion(0.002178911f, 0.01842411f, 0.9957102f, -0.09064715f)); //
            api.createVehicle(0x2B26F456, new Vector3(2055.715f, 614.9818f, 17.56824f), new Quaternion(0.00447735f, 0.05026801f, 0.9957724f, -0.07674909f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2060.976f, 607.1172f, 17.90619f), new Quaternion(-0.0009536436f, 0.03083601f, 0.9970954f, -0.06963402f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2027.823f, 626.5869f, 14.84394f), new Quaternion(0.0006472694f, 0.03746348f, 0.9992872f, -0.004587273f)); //
            api.createVehicle(0x2B26F456, new Vector3(2034.23f, 465.7829f, 17.82124f), new Quaternion(0.00817502f, -0.002118944f, 0.03093888f, 0.9994856f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2033.518f, 514.2319f, 17.05227f), new Quaternion(0.001361993f, 0.0004583665f, 5.176919E-05f, 0.999999f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2014.083f, 478.8804f, 17.74916f), new Quaternion(-0.0008229527f, 0.0009624172f, 0.7194076f, -0.6945871f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2055.026f, 643.9692f, 16.2055f), new Quaternion(-0.00258518f, 0.03581052f, 0.9933643f, -0.1092634f)); //
            api.createVehicle(0x2B26F456, new Vector3(2014.939f, 698.9731f, 14.76165f), new Quaternion(-0.007029423f, 0.01893131f, 0.9514051f, -0.3072792f)); //
            api.createVehicle(0x2B26F456, new Vector3(2027.708f, 654.6834f, 14.15387f), new Quaternion(0.007520081f, 0.006611761f, 0.9998791f, -0.01189831f)); //
            api.createVehicle(0x2B26F456, new Vector3(2052.3f, 634.4179f, 16.62568f), new Quaternion(0.002244581f, 0.04744297f, 0.9941034f, -0.09748106f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2034.318f, 470.7076f, 17.73052f), new Quaternion(-0.002235048f, -8.534844E-06f, 0.009471674f, 0.9999527f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2034.835f, 459.8124f, 17.8603f), new Quaternion(0.006820838f, -0.0001328915f, 0.001706183f, 0.9999753f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2050.255f, 664.785f, 15.12122f), new Quaternion(-0.00145949f, 0.01688576f, 0.9922248f, -0.1232984f)); //
            api.createVehicle(0x2B26F456, new Vector3(2035.117f, 453.0476f, 17.92837f), new Quaternion(0.005116225f, -0.002476902f, 0.008398664f, 0.9999486f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2039.315f, 683.2956f, 13.94688f), new Quaternion(-0.002960761f, 0.03061609f, 0.9896673f, -0.1400446f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2026.98f, 645.1945f, 14.1062f), new Quaternion(0.002466044f, 0.02352419f, 0.9996877f, -0.008067676f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(2047.223f, 655.6276f, 15.82456f), new Quaternion(-0.00685097f, 0.04203822f, 0.9921208f, -0.1178228f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2032.535f, 705.5942f, 12.75278f), new Quaternion(-0.005029972f, 0.03372824f, 0.9903319f, -0.1344618f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2008.231f, 707.041f, 14.54896f), new Quaternion(-0.005826776f, 0.01237294f, 0.8973339f, -0.4411403f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2039.956f, 699.501f, 13.25974f), new Quaternion(-0.003919862f, 0.02784267f, 0.9849223f, -0.1706965f)); //
            api.createVehicle(0x2B26F456, new Vector3(2028.405f, 718.489f, 12.15715f), new Quaternion(-0.005055896f, 0.04576861f, 0.9837493f, -0.1735416f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2043.683f, 687.9982f, 13.76885f), new Quaternion(-0.005347032f, 0.035282f, 0.9892964f, -0.141489f)); //
            api.createVehicle(0x2B26F456, new Vector3(2038.214f, 705.6194f, 12.91474f), new Quaternion(-0.001842548f, 0.03016502f, 0.9908469f, -0.1315642f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2026.285f, 670.0022f, 14.63368f), new Quaternion(0.0211751f, -0.01238223f, 0.9981374f, -0.05585676f)); //
            api.createVehicle(0xC703DB5F, new Vector3(2033.813f, 718.8766f, 12.26587f), new Quaternion(-0.004259686f, 0.02526945f, 0.9886357f, -0.1481311f)); //
            api.createVehicle(0x2B26F456, new Vector3(2036.026f, 712.8128f, 12.54728f), new Quaternion(-0.003088122f, 0.03250236f, 0.9862205f, -0.1621826f)); //
            api.createVehicle(0x2B26F456, new Vector3(2273.495f, 523.937f, 5.480648f), new Quaternion(-2.701978E-05f, -0.00121758f, 0.9998873f, 0.01496104f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2273.395f, 517.7428f, 5.40629f), new Quaternion(9.328671E-05f, -4.117175E-07f, -0.004367499f, 0.9999904f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2269.904f, 517.6669f, 5.47456f), new Quaternion(-4.099061E-05f, -9.613118E-07f, 0.02450922f, 0.9996996f)); //
            api.createVehicle(0x4020325C, new Vector3(2224.887f, 475.093f, 5.636481f), new Quaternion(0f, 0f, -0.01556515f, 0.9998789f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2024.875f, 478.7059f, 17.5365f), new Quaternion(-0.004996616f, -0.002692861f, 0.7088429f, -0.7053437f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2031.823f, 724.9487f, 11.87337f), new Quaternion(-0.003995286f, 0.02350657f, 0.9853804f, -0.1686924f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2241.031f, 474.5915f, 5.475048f), new Quaternion(8.818476E-08f, -4.100301E-05f, 0.9999945f, -0.00332753f)); //
            api.createVehicle(0x2B26F456, new Vector3(2018.505f, 694.9453f, 14.82478f), new Quaternion(0.0002041622f, 0.008494386f, 0.9635847f, -0.267268f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2011.547f, 704.0104f, 14.51091f), new Quaternion(-0.004148075f, 0.01001656f, 0.9238189f, -0.3826761f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2020.902f, 690.1461f, 14.85917f), new Quaternion(-0.002092257f, 0.009437134f, 0.9791378f, -0.2029668f)); //
            api.createVehicle(0x35ED670B, new Vector3(2249.352f, 711.6138f, 6.063028f), new Quaternion(0f, 0f, 0.8436982f, -0.5368178f)); //
            api.createVehicle(0xCD935EF9, new Vector3(2293.122f, 671.8082f, 5.906947f), new Quaternion(0f, 0f, 0.8436928f, -0.5368263f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(2273.777f, 567.8957f, 5.987669f), new Quaternion(0.01705535f, 0.0005709644f, 0.6002685f, 0.7996165f)); //
            api.createVehicle(0x4020325C, new Vector3(2258.519f, 575.401f, 5.646364f), new Quaternion(-0.01299914f, -0.00603077f, 0.6643219f, 0.7473092f)); //
            api.createVehicle(0x31F0B376, new Vector3(2245.307f, 758.4113f, 5.579461f), new Quaternion(-3.425064E-05f, -1.424049E-05f, 0.3826945f, 0.923875f)); //
            api.createVehicle(0x2B26F456, new Vector3(2260.053f, 481.2297f, 5.480891f), new Quaternion(-6.991005E-06f, -0.001299698f, 0.9999972f, -0.001967289f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2237.906f, 481.6637f, 5.47456f), new Quaternion(-4.099807E-05f, 6.583959E-07f, -0.01482427f, 0.9998901f)); //
            api.createVehicle(0x2B26F456, new Vector3(2254.304f, 481.1843f, 5.480648f), new Quaternion(-0.001217797f, -1.656474E-05f, 0.02090718f, 0.9997807f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2234.662f, 480.941f, 5.47456f), new Quaternion(1.94771E-07f, -4.100298E-05f, 0.9999813f, -0.006110685f)); //
            api.createVehicle(0x4020325C, new Vector3(2234.125f, 459.8965f, 5.636235f), new Quaternion(-7.205437E-05f, 1.880731E-06f, -0.02608028f, 0.9996599f)); //
            api.createVehicle(0x2B26F456, new Vector3(2227.545f, 460.1364f, 5.480891f), new Quaternion(-0.001299106f, 4.013652E-05f, -0.02353228f, 0.9997222f)); //
            api.createVehicle(0xDD3BD501, new Vector3(2241.161f, 460.0664f, 5.406532f), new Quaternion(-4.455661E-06f, 0.0001859753f, 0.9997146f, -0.02388818f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xC703DB5F, new Vector3(1812.2f, 564.8148f, 28.67347f), new Quaternion(0f, 0f, 0f, 1f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1573.344f, 589.9461f, 28.69828f), new Quaternion(0.00884967f, -0.000268513f, -0.005170947f, 0.9999474f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1573.626f, 600.7263f, 28.60672f), new Quaternion(0.001883378f, 0.0002940947f, -0.003270742f, 0.9999928f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1573.3f, 574.4563f, 28.65787f), new Quaternion(-0.007724681f, 0.0008117621f, 0.01029871f, 0.9999167f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1812.434f, 570.9183f, 28.61392f), new Quaternion(0.0009713168f, -2.771995E-06f, -0.02261302f, 0.9997439f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1903.517f, 695.951f, 23.33889f), new Quaternion(0.01716436f, 0.0002175063f, -0.003577403f, 0.9998463f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1573.408f, 624.9195f, 28.54888f), new Quaternion(-0.007078154f, 0.0002125562f, -0.0006850571f, 0.9999747f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1897.906f, 697.2562f, 23.45179f), new Quaternion(-0.0002887944f, -0.01242521f, 0.9999158f, -0.003711277f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1572.99f, 610.3936f, 28.66911f), new Quaternion(-0.001958301f, 0.0001962407f, 0.0003382314f, 0.999998f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1905.392f, 719.7689f, 23.80985f), new Quaternion(0.0001040421f, -0.002928889f, 0.737745f, 0.675073f)); //
            api.createVehicle(0x2B26F456, new Vector3(1903.788f, 662.6545f, 21.23733f), new Quaternion(0.004640482f, 1.945826E-05f, 0.01143519f, 0.9999238f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1573.582f, 582.5483f, 28.58765f), new Quaternion(-0.008535145f, 0.0004302671f, -0.01377168f, 0.9998687f)); //
            api.createVehicle(0x2B26F456, new Vector3(1898.073f, 709.6342f, 23.6674f), new Quaternion(0.01059593f, -0.003230725f, 0.9999387f, -0.0001468857f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1573.325f, 569.9775f, 28.53422f), new Quaternion(9.328467E-05f, 0f, 0f, 1f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1730.326f, 674.8823f, 27.47996f), new Quaternion(-0.0349452f, -0.0005887666f, 0.005298817f, 0.999375f)); //
            api.createVehicle(0x2B26F456, new Vector3(1649.917f, 709.7501f, 27.76781f), new Quaternion(0.01481243f, 0.0012745f, 0.05212665f, 0.9985297f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1903.616f, 667.7167f, 21.37069f), new Quaternion(0.03091202f, 0.001219222f, 0.02256972f, 0.9992665f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1898.258f, 680.9913f, 22.47717f), new Quaternion(-0.001143906f, -0.03743666f, 0.9992816f, -0.005778873f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1853.465f, 720.1141f, 25.39897f), new Quaternion(0.002042688f, 0.002536508f, 0.7062045f, 0.7080003f)); //
            api.createVehicle(0x779B4F2D, new Vector3(2008.791f, 706.5536f, 14.5413f), new Quaternion(-0.010074f, 0.03174054f, 0.9049241f, -0.4242682f)); //
            api.createVehicle(0x2B26F456, new Vector3(1903.607f, 679.0267f, 22.27473f), new Quaternion(0.05587456f, -0.00240076f, 0.01661725f, 0.9982967f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1903.692f, 671.8373f, 21.53764f), new Quaternion(0.03751385f, 0f, 0f, 0.9992961f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1897.6f, 689.9003f, 23.19922f), new Quaternion(0.003749379f, -0.02370518f, 0.9997118f, -0.0003630253f)); //
            api.createVehicle(0x2B26F456, new Vector3(1897.292f, 719.9521f, 23.8548f), new Quaternion(0.00683829f, 0.004637797f, 0.7217572f, 0.6920971f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1731.15f, 666.0121f, 27.703f), new Quaternion(0.0005115495f, 0.0002732675f, -0.00311977f, 0.999995f)); //
            api.createVehicle(0x779B4F2D, new Vector3(1891.943f, 719.9192f, 24.03439f), new Quaternion(0.04813915f, 0.04821839f, 0.6945597f, 0.7162014f)); //
            api.createVehicle(0x2B26F456, new Vector3(1870.604f, 719.6041f, 25.42994f), new Quaternion(0.01038199f, 0.006690416f, 0.7077574f, 0.7063476f)); //
            api.createVehicle(0xDD3BD501, new Vector3(1725.854f, 753.4956f, 24.90977f), new Quaternion(-0.0002918489f, 0.03638281f, 0.9989266f, 0.02866903f)); //
            api.createVehicle(0xC703DB5F, new Vector3(1903.989f, 690.4086f, 23.23587f), new Quaternion(0.02482856f, 0.0004914259f, -0.0104535f, 0.9996369f)); //
            api.createVehicle(0x2B26F456, new Vector3(1903.82f, 702.8088f, 23.60131f), new Quaternion(0.01986887f, -0.003062345f, -0.01808273f, 0.9996344f)); //
            api.createVehicle(0x2B26F456, new Vector3(1901.086f, 716.4925f, 23.82552f), new Quaternion(0.01905636f, -0.003504223f, 0.3269616f, 0.944839f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xC703DB5F, new Vector3(711.8542f, 633.7622f, 27.68678f), new Quaternion(-0.07641193f, -0.07641161f, 0.7029658f, 0.7029663f)); //
            api.createVehicle(0xDD3BD501, new Vector3(555.7955f, 733.4468f, 20.41442f), new Quaternion(-0.006378652f, -0.0007302529f, 0.01515412f, 0.9998646f)); //
            api.createVehicle(0x779B4F2D, new Vector3(544.8675f, 762.2349f, 20.47655f), new Quaternion(-0.0001289837f, 0.00860092f, 0.9999625f, 0.001008686f)); //
            api.createVehicle(0xC703DB5F, new Vector3(709.1329f, 628.4688f, 27.08502f), new Quaternion(-0.06754725f, -0.06637052f, 0.7074391f, 0.7004016f)); //
            api.createVehicle(0x779B4F2D, new Vector3(715.3033f, 628.4851f, 28.36987f), new Quaternion(-0.07578138f, -0.07551198f, 0.7048376f, 0.7012555f)); //
            api.createVehicle(0xDD3BD501, new Vector3(535.6713f, 762.8901f, 20.41207f), new Quaternion(-0.0007218097f, 0.0004380647f, 0.9999f, 0.01411636f)); //
            api.createVehicle(0xDD3BD501, new Vector3(727.4567f, 628.4331f, 30.97246f), new Quaternion(-0.07658843f, -0.07428548f, 0.7124449f, 0.6935694f)); //
            api.createVehicle(0x779B4F2D, new Vector3(545.0895f, 767.7921f, 20.47802f), new Quaternion(-0.0001246099f, 0.008893683f, 0.9999602f, 0.000656714f)); //
            api.createVehicle(0x779B4F2D, new Vector3(705.832f, 633.7939f, 26.31088f), new Quaternion(-0.05401078f, -0.05720238f, 0.6984216f, 0.7113494f)); //
            api.createVehicle(0xDD3BD501, new Vector3(556.2798f, 722.9609f, 20.41119f), new Quaternion(0.001972389f, -0.003709625f, 0.02759412f, 0.9996104f)); //
            api.createVehicle(0x779B4F2D, new Vector3(535.8315f, 768.4927f, 20.47654f), new Quaternion(-0.0009685394f, 0.008640599f, 0.9992278f, 0.03831894f)); //
            api.createVehicle(0xDD3BD501, new Vector3(574.0137f, 712.2162f, 19.85728f), new Quaternion(-0.04537643f, 0.01001274f, -0.2005827f, 0.9785742f)); //
            api.createVehicle(0x779B4F2D, new Vector3(529.7567f, 902.1654f, 13.345f), new Quaternion(0.02907009f, 0.09671567f, 0.9507924f, 0.2929077f)); //
            api.createVehicle(0xDD3BD501, new Vector3(671.4558f, 819.9088f, 2.255436f), new Quaternion(-0.0008563433f, -1.926842E-05f, 0.9918217f, 0.1276282f)); //
            api.createVehicle(0xC703DB5F, new Vector3(606.206f, 629.5026f, 20.22507f), new Quaternion(0.009176007f, -0.01902946f, 0.6550111f, 0.7553239f)); //
            api.createVehicle(0xDD3BD501, new Vector3(540.6735f, 895.175f, 20.41146f), new Quaternion(-3.966034E-07f, 0.005605402f, 0.9999585f, 0.007175667f)); //
            api.createVehicle(0x779B4F2D, new Vector3(540.559f, 879.6392f, 20.47467f), new Quaternion(-0.008781021f, 0.01132498f, 0.9998782f, -0.006184487f)); //
            api.createVehicle(0xDD3BD501, new Vector3(535.9017f, 926.9796f, 20.36178f), new Quaternion(0.003816217f, -0.01799741f, 0.9721119f, -0.2337947f)); //
            api.createVehicle(0x779B4F2D, new Vector3(634.417f, 634.6564f, 20.21317f), new Quaternion(-0.008293599f, -0.0003318746f, 0.6887684f, 0.724934f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.6048f, 872.215f, 20.41108f), new Quaternion(-2.04136E-05f, 0.002946134f, 0.9999497f, 0.009587212f)); //
            api.createVehicle(0xDD3BD501, new Vector3(537.9291f, 922.378f, 20.33284f), new Quaternion(0.0006172006f, 0.003941683f, 0.9787409f, -0.2050614f)); //
            api.createVehicle(0x779B4F2D, new Vector3(633.2719f, 629.9119f, 20.15698f), new Quaternion(-0.009228922f, 0.08276544f, 0.7962509f, 0.5992072f)); //
            api.createVehicle(0x779B4F2D, new Vector3(693.2025f, 634.0241f, 23.93162f), new Quaternion(-0.06022311f, -0.05863591f, 0.7017709f, 0.7074269f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(627.7443f, 629.853f, 20.50869f), new Quaternion(0.01637301f, 0.003677254f, 0.7033202f, 0.7106751f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.3043f, 916.3199f, 20.40855f), new Quaternion(-0.0004326592f, 0.008137873f, 0.9999514f, 0.005559146f)); //
            api.createVehicle(0x779B4F2D, new Vector3(545.7202f, 904.8298f, 20.46967f), new Quaternion(0.0002320345f, 0.02255702f, 0.9997455f, -0.0001847395f)); //
            api.createVehicle(0xC703DB5F, new Vector3(540.1793f, 925.9561f, 20.52033f), new Quaternion(-5.363579E-05f, -0.002100534f, 0.9999201f, 0.01246636f)); //
            api.createVehicle(0xDD3BD501, new Vector3(537.993f, 911.7026f, 10.96954f), new Quaternion(0.03556562f, 0.08247939f, 0.8977041f, 0.4313462f)); //
            api.createVehicle(0x779B4F2D, new Vector3(669.1438f, 634.4908f, 20.90325f), new Quaternion(0.002935574f, 0.005488132f, 0.7066948f, 0.707491f)); //
            api.createVehicle(0xDD3BD501, new Vector3(716.8532f, 633.7262f, 28.67588f), new Quaternion(-0.0761638f, -0.07532246f, 0.7049877f, 0.7010837f)); //
            api.createVehicle(0x779B4F2D, new Vector3(533.2993f, 931.8965f, 20.78967f), new Quaternion(0.007149876f, -0.02363575f, 0.9830669f, -0.1815756f)); //
            api.createVehicle(0xC703DB5F, new Vector3(637.4315f, 628.7018f, 20.27041f), new Quaternion(-0.01507884f, -0.01761922f, 0.687179f, 0.726118f)); //
            api.createVehicle(0xDD3BD501, new Vector3(647.1472f, 629.0105f, 20.20971f), new Quaternion(-0.01556645f, -0.02082543f, 0.6922002f, 0.7212372f)); //
            api.createVehicle(0xC703DB5F, new Vector3(728.8596f, 633.8244f, 31.38629f), new Quaternion(-0.07538578f, -0.07708258f, 0.6950604f, 0.7108209f)); //
            api.createVehicle(0x779B4F2D, new Vector3(545.9218f, 923.9323f, 20.47873f), new Quaternion(-0.0002617796f, 0.005348505f, 0.9998655f, -0.01549945f)); //
            api.createVehicle(0xDD3BD501, new Vector3(663.2546f, 628.7473f, 20.50782f), new Quaternion(-0.02324367f, -0.02349052f, 0.7065075f, 0.7069337f)); //
            api.createVehicle(0x779B4F2D, new Vector3(540.3199f, 933.707f, 20.47647f), new Quaternion(-0.0002941185f, 0.008736026f, 0.9999411f, -0.006444522f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.6526f, 931.0563f, 20.41216f), new Quaternion(-7.818235E-05f, 0.0002319767f, 0.9999764f, 0.006867393f)); //
            api.createVehicle(0x779B4F2D, new Vector3(670.1589f, 627.9926f, 20.94109f), new Quaternion(-0.01016712f, -0.01130749f, 0.70618f, 0.707869f)); //
            api.createVehicle(0xC703DB5F, new Vector3(719.2944f, 628.7861f, 29.31473f), new Quaternion(-0.06797574f, -0.07535953f, 0.6995062f, 0.7073835f)); //
            api.createVehicle(0xDD3BD501, new Vector3(684.1838f, 628.85f, 22.44871f), new Quaternion(-0.04348387f, -0.04328579f, 0.7052673f, 0.7062815f)); //
            api.createVehicle(0x779B4F2D, new Vector3(722.7619f, 633.7764f, 30.03464f), new Quaternion(-0.07605201f, -0.0766765f, 0.6997622f, 0.7061655f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xC703DB5F, new Vector3(710.6117f, 633.7963f, 27.40887f), new Quaternion(-0.06534617f, -0.06872662f, 0.6973383f, 0.7104405f)); //
            api.createVehicle(0xC703DB5F, new Vector3(699.2268f, 628.188f, 25.082f), new Quaternion(-0.05761892f, -0.05441472f, 0.7167863f, 0.6927744f)); //
            api.createVehicle(0x779B4F2D, new Vector3(714.3187f, 628.511f, 28.17171f), new Quaternion(-0.05789197f, -0.06113138f, 0.6982897f, 0.7108467f)); //
            api.createVehicle(0xDD3BD501, new Vector3(725.9414f, 628.3577f, 30.64569f), new Quaternion(-0.07858215f, -0.07536588f, 0.7175686f, 0.6879246f)); //
            api.createVehicle(0x779B4F2D, new Vector3(696.0773f, 634.1308f, 24.45325f), new Quaternion(-0.04771437f, -0.05024152f, 0.6920303f, 0.7185356f)); //
            api.createVehicle(0xC703DB5F, new Vector3(709.2331f, 628.4899f, 27.09764f), new Quaternion(-0.07343223f, -0.07468495f, 0.7025695f, 0.7038651f)); //
            api.createVehicle(0x779B4F2D, new Vector3(526.6323f, 897.0596f, 14.44399f), new Quaternion(0.01212717f, 0.08428775f, 0.9612206f, 0.2623039f)); //
            api.createVehicle(0xDD3BD501, new Vector3(671.4558f, 819.9088f, 2.255436f), new Quaternion(-0.0008563433f, -1.926842E-05f, 0.9918217f, 0.1276282f)); //
            api.createVehicle(0xC703DB5F, new Vector3(588.1719f, 634.2584f, 20.2255f), new Quaternion(0.003374964f, -0.01512709f, 0.5556811f, 0.8312511f)); //
            api.createVehicle(0xDD3BD501, new Vector3(540.5462f, 879.3112f, 20.41133f), new Quaternion(-0.003481444f, 0.002256388f, 0.9999669f, -0.007008375f)); //
            api.createVehicle(0x779B4F2D, new Vector3(540.7671f, 866.5568f, 20.4757f), new Quaternion(-0.0004036443f, 0.01027139f, 0.9999201f, -0.007356511f)); //
            api.createVehicle(0xDD3BD501, new Vector3(535.9017f, 926.9796f, 20.36178f), new Quaternion(0.003816217f, -0.01799741f, 0.9721119f, -0.2337947f)); //
            api.createVehicle(0x779B4F2D, new Vector3(621.2845f, 634.8867f, 20.18491f), new Quaternion(-0.0005568577f, -0.003142474f, 0.7073006f, 0.7069057f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.2453f, 853.6202f, 20.41164f), new Quaternion(-0.0005492126f, 0.002139933f, 0.9999538f, 0.009357244f)); //
            api.createVehicle(0xDD3BD501, new Vector3(537.9291f, 922.378f, 20.33284f), new Quaternion(0.0006172006f, 0.003941683f, 0.9787409f, -0.2050614f)); //
            api.createVehicle(0x779B4F2D, new Vector3(631.8897f, 629.7842f, 20.21399f), new Quaternion(-0.02754418f, 0.02261206f, 0.7356459f, 0.6764283f)); //
            api.createVehicle(0x779B4F2D, new Vector3(674.8763f, 634.0677f, 21.28701f), new Quaternion(-0.04826175f, -0.04810119f, 0.7050399f, 0.7058866f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(622.6194f, 629.9155f, 20.49593f), new Quaternion(0.02626163f, 0.02454643f, 0.7038081f, 0.7094801f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.3282f, 908.6893f, 20.40915f), new Quaternion(-0.0003471223f, 0.007949779f, 0.9999637f, -0.003037062f)); //
            api.createVehicle(0x779B4F2D, new Vector3(545.7208f, 892.4348f, 20.4702f), new Quaternion(4.929863E-05f, 0.01772348f, 0.999843f, 0.0001201038f)); //
            api.createVehicle(0xC703DB5F, new Vector3(540.178f, 925.9019f, 20.51862f), new Quaternion(3.815461E-05f, 0.002691686f, 0.9999179f, 0.01253221f)); //
            api.createVehicle(0xDD3BD501, new Vector3(531.9391f, 905.9365f, 12.49865f), new Quaternion(0.02690426f, 0.08878005f, 0.9253117f, 0.3676853f)); //
            api.createVehicle(0x779B4F2D, new Vector3(647.8464f, 634.5488f, 20.26735f), new Quaternion(0.01195854f, -4.055137E-05f, 0.700296f, 0.7137526f)); //
            api.createVehicle(0xDD3BD501, new Vector3(716.8532f, 633.7262f, 28.67588f), new Quaternion(-0.0761638f, -0.07532246f, 0.7049877f, 0.7010837f)); //
            api.createVehicle(0x779B4F2D, new Vector3(533.2993f, 931.8965f, 20.78967f), new Quaternion(0.007149876f, -0.02363575f, 0.9830669f, -0.1815756f)); //
            api.createVehicle(0xC703DB5F, new Vector3(636.9312f, 628.2427f, 20.26134f), new Quaternion(0.004729701f, -0.01198765f, 0.5428834f, 0.8397092f)); //
            api.createVehicle(0xDD3BD501, new Vector3(639.7944f, 629.4719f, 20.16673f), new Quaternion(-0.01082698f, -0.004095409f, 0.6365654f, 0.7711358f)); //
            api.createVehicle(0xC703DB5F, new Vector3(728.0301f, 633.8506f, 31.20783f), new Quaternion(-0.07530329f, -0.07738069f, 0.693558f, 0.7122633f)); //
            api.createVehicle(0x779B4F2D, new Vector3(545.9686f, 922.4316f, 20.47f), new Quaternion(-0.0004522562f, 0.02220168f, 0.9996329f, -0.01552634f)); //
            api.createVehicle(0xDD3BD501, new Vector3(649.4137f, 628.7714f, 20.22205f), new Quaternion(-0.01270495f, -0.01374982f, 0.7043886f, 0.7095677f)); //
            api.createVehicle(0x779B4F2D, new Vector3(540.3502f, 932.3159f, 20.4767f), new Quaternion(-0.0001657757f, 0.008255421f, 0.9999173f, -0.009869131f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.6273f, 929.6005f, 20.41216f), new Quaternion(-9.632518E-06f, 0.0002299681f, 0.9999655f, 0.008300692f)); //
            api.createVehicle(0x779B4F2D, new Vector3(656.8145f, 627.9999f, 20.3362f), new Quaternion(-0.005334978f, -0.005054692f, 0.7075233f, 0.7066519f)); //
            api.createVehicle(0xC703DB5F, new Vector3(719.28f, 628.7917f, 29.31832f), new Quaternion(-0.07782564f, -0.07759005f, 0.6970928f, 0.7085087f)); //
            api.createVehicle(0xDD3BD501, new Vector3(666.0947f, 628.8619f, 20.68168f), new Quaternion(-0.03671993f, -0.03653532f, 0.7061742f, 0.7061408f)); //
            api.createVehicle(0x779B4F2D, new Vector3(721.8115f, 633.7997f, 29.82789f), new Quaternion(-0.07825219f, -0.07945599f, 0.6959693f, 0.709359f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xC703DB5F, new Vector3(688.4871f, 634.3708f, 23.11408f), new Quaternion(-0.04911257f, -0.04599802f, 0.7027662f, 0.7082314f)); //
            api.createVehicle(0xC703DB5F, new Vector3(669.8564f, 627.8516f, 21.00156f), new Quaternion(-0.02768175f, -0.02792921f, 0.7067419f, 0.7063779f)); //
            api.createVehicle(0x779B4F2D, new Vector3(666.88f, 634.4065f, 20.79919f), new Quaternion(-0.002073809f, -0.000528553f, 0.7067533f, 0.7074569f)); //
            api.createVehicle(0xDD3BD501, new Vector3(539.4584f, 912.623f, 10.65774f), new Quaternion(0.038119f, 0.08104931f, 0.9027383f, 0.4207629f)); //
            api.createVehicle(0x779B4F2D, new Vector3(522.8795f, 887.6503f, 16.12177f), new Quaternion(-0.0001299004f, 0.0967671f, 0.9867147f, 0.1305003f)); //
            api.createVehicle(0xDD3BD501, new Vector3(671.4558f, 819.9088f, 2.255436f), new Quaternion(-0.0008563433f, -1.926842E-05f, 0.9918217f, 0.1276282f)); //
            api.createVehicle(0xC703DB5F, new Vector3(545.769f, 919.8919f, 20.51692f), new Quaternion(-0.0008811351f, 0.01013445f, 0.9998549f, -0.01366203f)); //
            api.createVehicle(0xC703DB5F, new Vector3(570.6394f, 645.6682f, 20.26573f), new Quaternion(0.0161774f, -0.00291465f, 0.3919114f, 0.9198562f)); //
            api.createVehicle(0xDD3BD501, new Vector3(541.0378f, 851.1039f, 20.41763f), new Quaternion(0.0006148476f, -0.00872407f, 0.9999342f, -0.007428515f)); //
            api.createVehicle(0x779B4F2D, new Vector3(541.0333f, 843.5856f, 20.47607f), new Quaternion(0.001049055f, 0.009568126f, 0.9999534f, -0.000759593f)); //
            api.createVehicle(0xDD3BD501, new Vector3(535.9017f, 926.9796f, 20.36178f), new Quaternion(0.003816217f, -0.01799741f, 0.9721119f, -0.2337947f)); //
            api.createVehicle(0x779B4F2D, new Vector3(602.4857f, 636.173f, 20.1759f), new Quaternion(0.01817227f, -0.008721759f, 0.6401411f, 0.7679929f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.077f, 820.2619f, 20.4117f), new Quaternion(-0.0002400686f, 0.001389353f, 0.9999988f, 0.0003895008f)); //
            api.createVehicle(0xDD3BD501, new Vector3(537.9291f, 922.378f, 20.33284f), new Quaternion(0.0006172006f, 0.003941683f, 0.9787409f, -0.2050614f)); //
            api.createVehicle(0x779B4F2D, new Vector3(624.6115f, 629.676f, 20.17278f), new Quaternion(0.01645818f, 0.01588599f, 0.7024539f, 0.7113617f)); //
            api.createVehicle(0x779B4F2D, new Vector3(636.8425f, 634.5778f, 20.21093f), new Quaternion(0.01717712f, 0.005792828f, 0.6867991f, 0.7266212f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(605.3516f, 630.9222f, 20.51071f), new Quaternion(0.009379191f, 0.02013075f, 0.6533076f, 0.7567667f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.5035f, 884.3163f, 20.41103f), new Quaternion(0.001573934f, 0.002779142f, 0.9999943f, -0.001157432f)); //
            api.createVehicle(0x779B4F2D, new Vector3(545.3947f, 866.3127f, 20.47458f), new Quaternion(-0.0002380193f, 0.01310919f, 0.9998643f, 0.009971479f)); //
            api.createVehicle(0xC703DB5F, new Vector3(540.178f, 925.9019f, 20.51862f), new Quaternion(3.815461E-05f, 0.002691686f, 0.9999179f, 0.01253221f)); //
            api.createVehicle(0xDD3BD501, new Vector3(528.0211f, 900.6465f, 13.69648f), new Quaternion(0.02444978f, 0.09898017f, 0.9538882f, 0.2823163f)); //
            api.createVehicle(0x779B4F2D, new Vector3(612.7836f, 635.5349f, 20.18893f), new Quaternion(0.005536784f, -0.02140859f, 0.6813747f, 0.7316006f)); //
            api.createVehicle(0x779B4F2D, new Vector3(533.2993f, 931.8965f, 20.78967f), new Quaternion(0.007149876f, -0.02363575f, 0.9830669f, -0.1815756f)); //
            api.createVehicle(0xC703DB5F, new Vector3(636.4031f, 628.3953f, 20.25394f), new Quaternion(0.004796279f, 0.007133168f, 0.5654776f, 0.8247189f)); //
            api.createVehicle(0xDD3BD501, new Vector3(639.8206f, 629.4839f, 20.16206f), new Quaternion(-0.0101521f, 0.01026162f, 0.6487485f, 0.760866f)); //
            api.createVehicle(0x779B4F2D, new Vector3(546.0904f, 906.6823f, 20.46965f), new Quaternion(-0.0001309771f, 0.02228098f, 0.9997408f, 0.004676109f)); //
            api.createVehicle(0xDD3BD501, new Vector3(646.0833f, 628.8599f, 20.19633f), new Quaternion(-0.00525708f, -0.004967152f, 0.6964243f, 0.7175937f)); //
            api.createVehicle(0x779B4F2D, new Vector3(540.3599f, 931.9227f, 20.4695f), new Quaternion(-0.0001542312f, 0.01425939f, 0.9998409f, -0.01071947f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.5709f, 926.7954f, 20.40996f), new Quaternion(0.0009283634f, 0.001416193f, 0.9999466f, 0.01019307f)); //
            api.createVehicle(0x779B4F2D, new Vector3(651.6101f, 628.0068f, 20.30191f), new Quaternion(-0.009457426f, -0.009035653f, 0.7046747f, 0.7094099f)); //
            api.createVehicle(0x31F0B376, new Vector3(700.5175f, 950.5509f, 55.27115f), new Quaternion(-0.0489912f, -0.1046897f, 0.913192f, 0.3907943f)); //
            api.createVehicle(0xDD3BD501, new Vector3(659.4175f, 628.8595f, 20.31846f), new Quaternion(-0.01111022f, -0.01097147f, 0.7068173f, 0.7072239f)); //
            api.createVehicle(0xDD3BD501, new Vector3(547.4341f, 918.8866f, 8.814619f), new Quaternion(0.0419917f, 0.07830423f, 0.877268f, 0.4717055f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.7f, 932.0203f, 20.36234f), new Quaternion(0f, 9.328467E-05f, 1f, 0f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xDD3BD501, new Vector3(559.4501f, 1012.802f, 20.36234f), new Quaternion(9.328467E-05f, 0f, 0f, 1f)); //
            api.createVehicle(0xC703DB5F, new Vector3(529.6359f, 1029.953f, 2.331861f), new Quaternion(0.001686625f, 0.005546538f, 0.01250334f, 0.999905f)); //
            api.createVehicle(0xC703DB5F, new Vector3(580.1831f, 1028.398f, 23.7816f), new Quaternion(-0.05946601f, -0.01064742f, 0.2016855f, 0.9775855f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(559.4484f, 1000.407f, 20.75773f), new Quaternion(0.0001767684f, -4.82249E-07f, -8.524637E-11f, 0.9999999f)); //
            api.createVehicle(0xC703DB5F, new Vector3(567.4537f, 1009.523f, 10.54495f), new Quaternion(0.05013855f, 0.02808997f, 0.4288217f, 0.9015592f)); //
            api.createVehicle(0xC703DB5F, new Vector3(599.7067f, 1367.901f, 11.06347f), new Quaternion(-0.01309336f, -0.01148375f, 0.6694198f, 0.7426802f)); //
            api.createVehicle(0xDD3BD501, new Vector3(618.1068f, 1365.626f, 11.96311f), new Quaternion(-0.02576257f, -0.02185416f, 0.675871f, 0.7362453f)); //
            api.createVehicle(0xDD3BD501, new Vector3(550.2238f, 1018.318f, 12.76461f), new Quaternion(0.05123908f, 0.04327254f, 0.6135761f, 0.7867824f)); //
            api.createVehicle(0xC703DB5F, new Vector3(584.0308f, 1018.119f, 25.14375f), new Quaternion(-0.06317717f, -0.01094955f, 0.2107658f, 0.9754315f)); //
            api.createVehicle(0xDD3BD501, new Vector3(525.2156f, 1010.893f, 2.22474f), new Quaternion(0.0007879284f, 0.00104255f, -0.19153f, 0.9814859f)); //
            api.createVehicle(0xDD3BD501, new Vector3(535.9017f, 926.9796f, 20.36178f), new Quaternion(0.003816217f, -0.01799741f, 0.9721119f, -0.2337947f)); //
            api.createVehicle(0xDD3BD501, new Vector3(512.8781f, 1358.379f, 11.36559f), new Quaternion(0.002852289f, -0.01111996f, 0.9229199f, -0.3848211f)); //
            api.createVehicle(0xDD3BD501, new Vector3(537.9291f, 922.378f, 20.33284f), new Quaternion(0.0006172006f, 0.003941683f, 0.9787409f, -0.2050614f)); //
            api.createVehicle(0xC703DB5F, new Vector3(559.8778f, 1018.968f, 20.52268f), new Quaternion(-0.005655722f, 0.003492817f, -0.01404787f, 0.9998792f)); //
            api.createVehicle(0xDD3BD501, new Vector3(553.6443f, 1029.211f, 20.41361f), new Quaternion(-0.003981438f, -0.0005053373f, 0.004555519f, 0.9999816f)); //
            api.createVehicle(0xDD3BD501, new Vector3(527.7604f, 1017.555f, 2.221865f), new Quaternion(0.005675047f, 0.001928558f, -0.1499543f, 0.9886748f)); //
            api.createVehicle(0xC703DB5F, new Vector3(540.178f, 925.9019f, 20.51862f), new Quaternion(3.815461E-05f, 0.002691686f, 0.9999179f, 0.01253221f)); //
            api.createVehicle(0xC703DB5F, new Vector3(589.4043f, 1003.28f, 27.21731f), new Quaternion(-0.06408124f, -0.001424055f, 0.1513902f, 0.9863937f)); //
            api.createVehicle(0xDD3BD501, new Vector3(576.7367f, 999.8951f, 9.430889f), new Quaternion(0.03586696f, 0.01608557f, 0.3396985f, 0.9397126f)); //
            api.createVehicle(0x779B4F2D, new Vector3(533.2993f, 931.8965f, 20.78967f), new Quaternion(0.007149876f, -0.02363575f, 0.9830669f, -0.1815756f)); //
            api.createVehicle(0xDD3BD501, new Vector3(502.7895f, 1351.621f, 11.66713f), new Quaternion(-0.009016478f, 0.01962477f, 0.9766474f, -0.2137603f)); //
            api.createVehicle(0xDD3BD501, new Vector3(559.5096f, 1014.602f, 11.45457f), new Quaternion(0.04752598f, 0.02737804f, 0.5345963f, 0.8433259f)); //
            api.createVehicle(0xC703DB5F, new Vector3(554.5441f, 1020.11f, 20.52383f), new Quaternion(-0.005540546f, 0.006078115f, -0.0146912f, 0.9998583f)); //
            api.createVehicle(0xC703DB5F, new Vector3(514.3835f, 1363.614f, 11.53641f), new Quaternion(-0.002720626f, -0.006270865f, 0.9493825f, -0.3140482f)); //
            api.createVehicle(0x779B4F2D, new Vector3(540.3599f, 931.9227f, 20.4695f), new Quaternion(-0.0001542312f, 0.01425939f, 0.9998409f, -0.01071947f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.5371f, 885.7848f, 20.41166f), new Quaternion(0.0008151667f, 0.001490529f, 0.9999961f, -0.002202217f)); //
            api.createVehicle(0xDD3BD501, new Vector3(588.2432f, 1008.034f, 26.46718f), new Quaternion(-0.05985077f, -0.01088326f, 0.1734267f, 0.9829662f)); //
            api.createVehicle(0xDD3BD501, new Vector3(559.4825f, 1005.955f, 20.40949f), new Quaternion(0.007973619f, -0.0001693008f, -0.007722495f, 0.9999384f)); //
            api.createVehicle(0xC703DB5F, new Vector3(553.8752f, 1013.653f, 20.51818f), new Quaternion(-0.005880218f, 0.003015494f, 0.02100989f, 0.9997575f)); //
            api.createVehicle(0xDD3BD501, new Vector3(498.5176f, 1362.369f, 11.38089f), new Quaternion(-0.00199989f, 0.004793485f, 0.9837813f, -0.1792957f)); //
            api.createVehicle(0xDD3BD501, new Vector3(523.4549f, 890.3257f, 15.6012f), new Quaternion(0.0218605f, 0.08169733f, 0.9851164f, 0.1496437f)); //
            api.createVehicle(0xDD3BD501, new Vector3(506.6622f, 1364.431f, 11.49225f), new Quaternion(0.005594172f, -0.003653189f, 0.9220389f, -0.3870397f)); //
            api.createVehicle(0xDD3BD501, new Vector3(546.0618f, 905.3931f, 20.40977f), new Quaternion(-0.0001568176f, 0.004253767f, 0.999982f, 0.004268609f)); //
            api.createVehicle(0xDD3BD501, new Vector3(531.571f, 936.1231f, 21.01812f), new Quaternion(0.007884038f, -0.03607144f, 0.976261f, -0.2134274f)); //
            api.createVehicle(0x32B91AE8, new Vector3(487.7693f, 1286.811f, 2.69058f), new Quaternion(-1.885413E-05f, -7.093334E-05f, 0.9663353f, 0.2572861f)); //
            api.createVehicle(0xC703DB5F, new Vector3(584.7936f, 1388.672f, 10.55948f), new Quaternion(0.01010278f, 0.002202f, -0.006956157f, 0.9999224f)); //
            api.createVehicle(0xDD3BD501, new Vector3(628.6624f, 1364.919f, 12.56785f), new Quaternion(-0.02926234f, -0.02629318f, 0.6556218f, 0.754064f)); //
            api.createVehicle(0xDD3BD501, new Vector3(586.6586f, 1369.786f, 10.37297f), new Quaternion(0.002069851f, -0.03669531f, 0.3487588f, 0.9364917f)); //
            api.createVehicle(0xC703DB5F, new Vector3(608.2138f, 1367.351f, 11.42895f), new Quaternion(-0.02702626f, -0.02422683f, 0.662843f, 0.7478783f)); //
            api.createVehicle(0xDD3BD501, new Vector3(642.2608f, 1362.631f, 13.08681f), new Quaternion(-0.01590135f, -0.009902942f, 0.6390345f, 0.7689501f)); //
            api.createVehicle(0xC703DB5F, new Vector3(592.2186f, 1367.926f, 10.67961f), new Quaternion(-0.01674686f, -0.02768104f, 0.7252412f, 0.6877342f)); //
            api.createVehicle(0xDD3BD501, new Vector3(664.9658f, 1362.039f, 13.97506f), new Quaternion(-0.008601102f, -0.0008806847f, 0.7073001f, 0.7068607f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0xDD3BD501, new Vector3(559.3702f, 1016.761f, 20.40912f), new Quaternion(0.007952997f, 0.0003907869f, 0.01070895f, 0.999911f)); //
            api.createVehicle(0xC703DB5F, new Vector3(525.9106f, 1045.507f, 2.33388f), new Quaternion(-0.002617059f, 0.003752057f, 0.2226529f, 0.9748871f)); //
            api.createVehicle(0xC703DB5F, new Vector3(573.4913f, 1042.275f, 22.06226f), new Quaternion(-0.04935063f, -0.01186674f, 0.2181061f, 0.9746041f)); //
            api.createVehicle(0x79FBB0C5, new Vector3(559.4484f, 1000.407f, 20.75773f), new Quaternion(0.0001767684f, -4.82249E-07f, -8.524637E-11f, 0.9999999f)); //
            api.createVehicle(0xC703DB5F, new Vector3(554.2815f, 1016.825f, 12.30573f), new Quaternion(0.04737047f, 0.04710558f, 0.5607467f, 0.8252879f)); //
            api.createVehicle(0xDD3BD501, new Vector3(554.8626f, 1018.603f, 20.41452f), new Quaternion(-0.003658793f, -0.0006066685f, -0.02358054f, 0.999715f)); //
            api.createVehicle(0xC703DB5F, new Vector3(599.7067f, 1367.901f, 11.06347f), new Quaternion(-0.01309336f, -0.01148375f, 0.6694198f, 0.7426802f)); //
            api.createVehicle(0xDD3BD501, new Vector3(615.6024f, 1365.849f, 11.80555f), new Quaternion(-0.02450641f, -0.02115762f, 0.6740481f, 0.7379776f)); //
            api.createVehicle(0xDD3BD501, new Vector3(639.6196f, 1362.501f, 13.00202f), new Quaternion(-0.02147456f, -0.01135943f, 0.6401345f, 0.7678787f)); //
            api.createVehicle(0xDD3BD501, new Vector3(579.1447f, 996.8831f, 9.23578f), new Quaternion(0.0217572f, 0.006034996f, 0.2775753f, 0.9604385f)); //
            api.createVehicle(0xDD3BD501, new Vector3(526.5751f, 1021.309f, 16.19593f), new Quaternion(0.04961392f, 0.06525384f, 0.7010091f, 0.7084255f)); //
            api.createVehicle(0xDD3BD501, new Vector3(509.0395f, 1362.788f, 11.46225f), new Quaternion(0.003882571f, -0.0007473963f, 0.9091381f, -0.4164759f)); //
            api.createVehicle(0xDD3BD501, new Vector3(553.9622f, 1010.98f, 20.41601f), new Quaternion(-0.007663274f, 7.294029E-05f, 0.004284641f, 0.9999615f)); //
            api.createVehicle(0xC703DB5F, new Vector3(498.4189f, 1361.955f, 11.49037f), new Quaternion(0.0004592764f, 0.004703484f, 0.9901322f, -0.1400567f)); //
            api.createVehicle(0xC703DB5F, new Vector3(578.7357f, 1029.511f, 23.58739f), new Quaternion(-0.05895457f, -0.01054189f, 0.2204378f, 0.9735606f)); //
            api.createVehicle(0xDD3BD501, new Vector3(516.0101f, 1364.205f, 11.37502f), new Quaternion(0.01028618f, -0.01813929f, 0.8947723f, -0.4460356f)); //
            api.createVehicle(0xDD3BD501, new Vector3(567.5023f, 1009.777f, 10.439f), new Quaternion(0.05420859f, 0.03054805f, 0.4328059f, 0.8993372f)); //
            api.createVehicle(0xDD3BD501, new Vector3(526.9748f, 1016.009f, 2.223652f), new Quaternion(0.003050159f, 0.002573022f, -0.1505443f, 0.9885951f)); //
            api.createVehicle(0xDD3BD501, new Vector3(523.6634f, 1347.773f, 11.01983f), new Quaternion(-0.004422846f, -0.01368188f, 0.9163331f, -0.4001586f)); //
            api.createVehicle(0xC703DB5F, new Vector3(560.0655f, 1039.115f, 20.51949f), new Quaternion(0.001963055f, 2.327353E-05f, -0.00355187f, 0.9999918f)); //
            api.createVehicle(0xDD3BD501, new Vector3(553.52f, 1049.732f, 20.39413f), new Quaternion(0.003658844f, -6.063177E-06f, -0.001359332f, 0.9999924f)); //
            api.createVehicle(0xDD3BD501, new Vector3(529.6557f, 1029.354f, 2.223928f), new Quaternion(0.002159555f, 0.00382785f, -0.0263261f, 0.9996437f)); //
            api.createVehicle(0xC703DB5F, new Vector3(588.697f, 1005.286f, 26.93507f), new Quaternion(-0.06172942f, -0.007841328f, 0.165111f, 0.9843101f)); //
            api.createVehicle(0xDD3BD501, new Vector3(573.5109f, 1003.419f, 9.764088f), new Quaternion(0.03255236f, 0.01349988f, 0.366801f, 0.9296317f)); //
            api.createVehicle(0xDD3BD501, new Vector3(513.8953f, 1334.577f, 13.86059f), new Quaternion(-0.02547062f, 0.04559853f, 0.9359707f, -0.3481822f)); //
            api.createVehicle(0xDD3BD501, new Vector3(545.042f, 1019.287f, 13.4923f), new Quaternion(0.05255663f, 0.05157847f, 0.628713f, 0.774143f)); //
            api.createVehicle(0xC703DB5F, new Vector3(554.7114f, 1039.104f, 20.51991f), new Quaternion(0.003760113f, -1.165456E-05f, -0.002934566f, 0.9999886f)); //
            api.createVehicle(0xC703DB5F, new Vector3(523.179f, 1352.254f, 11.22308f), new Quaternion(0.01790285f, -0.02659617f, 0.9644217f, -0.2624177f)); //
            api.createVehicle(0xDD3BD501, new Vector3(583.8022f, 1019.545f, 24.855f), new Quaternion(-0.06405411f, -0.00956846f, 0.1903898f, 0.97957f)); //
            api.createVehicle(0xDD3BD501, new Vector3(559.5123f, 1006.961f, 20.41179f), new Quaternion(0.001098518f, 3.838588E-05f, -0.01158361f, 0.9999323f)); //
            api.createVehicle(0xC703DB5F, new Vector3(553.5802f, 1028.689f, 20.51974f), new Quaternion(0.001919991f, 4.881269E-06f, 0.003342172f, 0.9999925f)); //
            api.createVehicle(0xDD3BD501, new Vector3(505.1807f, 1347.523f, 12.05888f), new Quaternion(-0.02480582f, 0.06802648f, 0.9674674f, -0.2424126f)); //
            api.createVehicle(0xDD3BD501, new Vector3(517.3566f, 1353.818f, 11.21779f), new Quaternion(0.003340738f, -0.01224189f, 0.9224733f, -0.3858524f)); //
            api.createVehicle(0x32B91AE8, new Vector3(487.7693f, 1286.811f, 2.69058f), new Quaternion(-1.885413E-05f, -7.093334E-05f, 0.9663353f, 0.2572861f)); //
            api.createVehicle(0xC703DB5F, new Vector3(584.8658f, 1410.978f, 10.65619f), new Quaternion(-0.007813892f, 7.284759E-06f, -0.0002788754f, 0.9999694f)); //
            api.createVehicle(0xDD3BD501, new Vector3(624.9891f, 1365.412f, 12.35342f), new Quaternion(-0.02092329f, -0.01892972f, 0.6569416f, 0.7534133f)); //
            api.createVehicle(0xDD3BD501, new Vector3(585.3169f, 1385.25f, 10.40893f), new Quaternion(0.008255567f, 0.001454399f, -0.006347059f, 0.9999447f)); //
            api.createVehicle(0xC703DB5F, new Vector3(606.4446f, 1367.555f, 11.33619f), new Quaternion(-0.01519233f, -0.01341207f, 0.6649379f, 0.7466238f)); //
            api.createVehicle(0xDD3BD501, new Vector3(632.9279f, 1364.289f, 12.77623f), new Quaternion(-0.01214469f, -0.007302775f, 0.6546992f, 0.7557566f)); //
            api.createVehicle(0xC703DB5F, new Vector3(584.8864f, 1374.772f, 10.44301f), new Quaternion(0.001488231f, -0.01322791f, 0.09301507f, 0.9955757f)); //
            api.createVehicle(0xDD3BD501, new Vector3(649.1271f, 1361.86f, 13.46298f), new Quaternion(-0.02024034f, -0.03012679f, 0.6843316f, 0.7282671f)); //
            // session date: Thursday, December 4, 2014
            api.createVehicle(0x79FBB0C5, new Vector3(509.6697f, 1369.052f, 11.89071f), new Quaternion(-0.001578687f, 0.009433744f, 0.9185203f, -0.3952584f)); //
            api.createVehicle(0xC703DB5F, new Vector3(505.8539f, 1365.288f, 11.63135f), new Quaternion(0.001934481f, -0.004343366f, 0.9137638f, -0.4062181f)); //
            api.createVehicle(0xC703DB5F, new Vector3(599.7064f, 1367.901f, 11.06365f), new Quaternion(-0.01319821f, -0.01160298f, 0.6694183f, 0.7426778f)); //
            api.createVehicle(0xDD3BD501, new Vector3(612.9161f, 1366.102f, 11.629f), new Quaternion(-0.02435868f, -0.02110387f, 0.6728266f, 0.7390979f)); //
            api.createVehicle(0xDD3BD501, new Vector3(631.7381f, 1363.75f, 12.72833f), new Quaternion(-0.01835482f, -0.01308861f, 0.6564623f, 0.7540219f)); //
            api.createVehicle(0xDD3BD501, new Vector3(527.065f, 1346.332f, 10.92747f), new Quaternion(0.004996428f, -0.01448986f, 0.9029678f, -0.4294347f)); //
            api.createVehicle(0xC703DB5F, new Vector3(524.8707f, 1321.384f, 15.85786f), new Quaternion(-0.02064496f, 0.05080192f, 0.9282491f, -0.3678949f)); //
            api.createVehicle(0xDD3BD501, new Vector3(560.0495f, 1352.278f, 10.37197f), new Quaternion(0.006611809f, 0.003956745f, -0.6167257f, 0.7871407f)); //
            api.createVehicle(0xDD3BD501, new Vector3(585.0034f, 1407.013f, 10.53142f), new Quaternion(-0.008403718f, 0.0001210784f, 0.0004626182f, 0.9999646f)); //
            api.createVehicle(0xDD3BD501, new Vector3(591.3484f, 1335.655f, 8.295257f), new Quaternion(0.009497398f, 0.007894359f, -0.5998602f, 0.8000095f)); //
            api.createVehicle(0xDD3BD501, new Vector3(518.3235f, 1327.975f, 14.7288f), new Quaternion(-0.01535944f, 0.03400123f, 0.9292505f, -0.3675615f)); //
            api.createVehicle(0xDD3BD501, new Vector3(518.4432f, 1353.978f, 11.20492f), new Quaternion(0.002124996f, -0.009684767f, 0.9166238f, -0.3996279f)); //
            api.createVehicle(0xDD3BD501, new Vector3(530.4649f, 1354.704f, 10.73511f), new Quaternion(0.007773749f, -0.01584464f, 0.8262025f, -0.5630965f)); //
            api.createVehicle(0xDD3BD501, new Vector3(649.0151f, 1361.214f, 13.4546f), new Quaternion(-0.01640306f, -0.01980312f, 0.7049342f, 0.7088065f)); //
            api.createVehicle(0xDD3BD501, new Vector3(525.7438f, 1357.197f, 10.93574f), new Quaternion(-0.001030968f, -0.02066947f, 0.8844538f, -0.4661686f)); //
            api.createVehicle(0xDD3BD501, new Vector3(584.4724f, 1379.725f, 10.33254f), new Quaternion(0.01371783f, -0.008259894f, 0.02335643f, 0.999599f)); //
            api.createVehicle(0xC703DB5F, new Vector3(511.668f, 1336.011f, 13.68593f), new Quaternion(-0.0201038f, 0.02221402f, 0.9424703f, -0.3329448f)); //
            api.createVehicle(0xDD3BD501, new Vector3(541.5876f, 1278.309f, 20.36556f), new Quaternion(-0.007080286f, 0.008532722f, 0.9980088f, 0.06209162f)); //
            api.createVehicle(0xDD3BD501, new Vector3(656.2668f, 1361.317f, 13.70584f), new Quaternion(-0.01075382f, -0.01101833f, 0.7440506f, 0.667946f)); //
            api.createVehicle(0xC703DB5F, new Vector3(555.7619f, 1332.647f, 9.477661f), new Quaternion(0.009085056f, -0.02299251f, 0.7553961f, -0.6548019f)); //
            api.createVehicle(0xDD3BD501, new Vector3(663.0302f, 1361.95f, 13.91644f), new Quaternion(-0.01045539f, -0.01149452f, 0.7141804f, 0.6997892f)); //
            api.createVehicle(0xC703DB5F, new Vector3(591.4692f, 1367.819f, 10.62181f), new Quaternion(-0.01520144f, -0.01628242f, 0.7360906f, 0.6765164f)); //
            api.createVehicle(0xDD3BD501, new Vector3(516.9594f, 1360.825f, 11.35112f), new Quaternion(0.00542206f, -0.009903703f, 0.9550926f, -0.2960927f)); //
            api.createVehicle(0xDD3BD501, new Vector3(542.6425f, 1294.642f, 19.62642f), new Quaternion(0.003362626f, 0.06615558f, 0.9924153f, -0.1035559f)); //
            api.createVehicle(0x2FBC4D30, new Vector3(497.8012f, 1604.794f, 31.32489f), new Quaternion(0.04028612f, -0.04018969f, -0.7051145f, 0.7068064f)); //
            api.createVehicle(0xDD3BD501, new Vector3(538.7499f, 1338.053f, 10.32432f), new Quaternion(0.001431017f, -0.02429833f, 0.8424696f, -0.5381939f)); //
            api.createVehicle(0x2FBC4D30, new Vector3(480.4936f, 1604.869f, 29.24557f), new Quaternion(-0.04400962f, 0.04449807f, 0.7096044f, -0.7018152f)); //
            api.createVehicle(0xDD3BD501, new Vector3(502.5063f, 1350.423f, 11.71937f), new Quaternion(-0.01073039f, 0.02705628f, 0.9800959f, -0.1963789f)); //
            api.createVehicle(0xC703DB5F, new Vector3(511.8725f, 1359.815f, 11.50951f), new Quaternion(0.007150183f, -0.009090293f, 0.9190125f, -0.3940587f)); //
            api.createVehicle(0xDD3BD501, new Vector3(498.2426f, 1362.868f, 11.38549f), new Quaternion(-0.0005262471f, 0.006082787f, 0.9875344f, -0.1572856f)); //
            api.createVehicle(0x32B91AE8, new Vector3(487.7693f, 1286.811f, 2.69058f), new Quaternion(-1.885413E-05f, -7.093334E-05f, 0.9663353f, 0.2572861f)); //
            api.createVehicle(0xC703DB5F, new Vector3(584.5574f, 1437.94f, 10.75272f), new Quaternion(-0.0001184504f, -6.72872E-05f, 0.007350571f, 0.999973f)); //
            api.createVehicle(0xDD3BD501, new Vector3(619.1572f, 1366.164f, 12.01961f), new Quaternion(-0.02066803f, -0.01849673f, 0.661588f, 0.7493545f)); //
            api.createVehicle(0xDD3BD501, new Vector3(585.2256f, 1428.308f, 10.60767f), new Quaternion(0.002094737f, 7.634195E-05f, 0.008982517f, 0.9999574f)); //
            api.createVehicle(0xC703DB5F, new Vector3(606.4415f, 1367.555f, 11.33455f), new Quaternion(-0.01585607f, -0.01409818f, 0.6649278f, 0.7466063f)); //
            api.createVehicle(0xDD3BD501, new Vector3(625.2671f, 1365.301f, 12.37019f), new Quaternion(-0.02478107f, -0.02219356f, 0.6594991f, 0.7509689f)); //
            api.createVehicle(0xC703DB5F, new Vector3(584.7513f, 1420.193f, 10.69257f), new Quaternion(-0.003698333f, 1.704882E-05f, 0.003371741f, 0.9999875f)); //
            api.createVehicle(0xDD3BD501, new Vector3(640.4583f, 1363.291f, 13.00825f), new Quaternion(-0.01506393f, -0.006063036f, 0.642159f, 0.7663996f)); //


            api.createVehicle("SUPERGT", new Vector3(2365.749f, 604.4031f, 30.812778f), new Quaternion(0.0003340448f, -0.000308407f, -0.005634441f, 0.999984f));//1
            api.createVehicle("SABREGT", new Vector3(2365.749f, 604.4031f, 30.812778f), new Quaternion(-5.144991E-05f, 0.0004049888f, 0.7113973f, -0.7027899f));//2
            api.createVehicle("TURISMO", new Vector3(2384.331f, 183.4532f, 15.231522f), new Quaternion(5.031423E-05f, -1.052421E-05f, -0.2579773f, 0.966151f));//3
            api.createVehicle("TURISMO", new Vector3(2192.73f, 627.2288f, 5.332971f), new Quaternion(0.0002791698f, -5.590584E-06f, 0.7807003f, 0.6249056f));//4
            api.createVehicle("TURISMO", new Vector3(2181.76f, 615.1063f, 5.525773f), new Quaternion(0.02005787f, -0.01567191f, 0.9466311f, 0.3213124f));//5
            api.createVehicle("TURISMO", new Vector3(2538.492f, 268.823f, 15.201221f), new Quaternion(0.01614731f, -0.02719297f, 0.7664362f, 0.6415414f));//6
            api.createVehicle("PMP600", new Vector3(2473.699f, 223.2305f, 15.201333f), new Quaternion(-0.01775569f, -0.02556662f, -0.2633584f, 0.9641957f));//7
            api.createVehicle("SABREGT", new Vector3(2388.855f, 257.8469f, 15.201088f), new Quaternion(0.01293025f, -0.02881934f, 0.6902145f, 0.7229151f));//8
            api.createVehicle("TURISMO", new Vector3(2424.969f, 289.9648f, 15.390142f), new Quaternion(-0.001980515f, 0.04101723f, -0.2329017f, 0.9716329f));//9
            api.createVehicle("BURRITO2", new Vector3(2443.699f, 367.3226f, 15.383784f), new Quaternion(-0.008690395f, 0.04274701f, -0.03868492f, 0.9982989f));//10
            api.createVehicle("ANNIHILATOR", new Vector3(2443.363f, 416.7742f, 15.383949f), new Quaternion(-0.01375107f, 0.04257208f, 0.07150503f, 0.9964364f));//11
            api.createVehicle("BURRITO2", new Vector3(2394.754f, 522.0314f, 15.438295f), new Quaternion(-0.0004053938f, 0.006344253f, 0.6920787f, 0.721794f));//12
            api.createVehicle("ANNIHILATOR", new Vector3(2361.649f, 593.551f, 15.432491f), new Quaternion(-0.01194778f, 0.005154964f, 0.8922257f, 0.4514024f));//13
            api.createVehicle("SABREGT", new Vector3(2290.812f, 662.457f, 15.437609f), new Quaternion(-0.004348053f, 0.003731936f, 0.5506685f, 0.8347044f));//14
            api.createVehicle("TURISMO", new Vector3(2213.896f, 719.2641f, 15.458335f), new Quaternion(-0.002295473f, 0.001867346f, 0.3097835f, 0.9508025f));//15
            api.createVehicle("TRASH", new Vector3(2231.183f, 736.4599f, 15.459599f), new Quaternion(-0.003429666f, -0.002095187f, 0.9954517f, 0.09518221f));//16
            api.createVehicle("ANNIHILATOR", new Vector3(2323.937f, 794.5594f, 15.998639f), new Quaternion(-0.001141903f, 0.002282516f, -0.2242321f, 0.9745324f));//17
            api.createVehicle("TRASH", new Vector3(2257.443f, 721.6019f, 15.438978f), new Quaternion(0.0003481103f, -0.003353917f, 0.8838975f, -0.4676686f));//18
            var infernus = api.createVehicle("TURISMO", new Vector3(2464.762f, 589.6474f, 15.437915f), new Quaternion(-0.001692423f, -0.002709477f, 0.9179366f, -0.3967145f));//10
            api.createVehicle("BLISTA", new Vector3(2604.48f, 414.0821f, 5.438591f), new Quaternion(-0.002986294f, -0.002253091f, 0.9838318f, -0.1790559f));//20
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
            api.createVehicle("BLISTA", new Vector3(-193.0686f, 293.6498f, 14.57422f), new Quaternion(-0.005227661f, 0.03427088f, -0.01627137f, 0.9992664f)); //bus
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
            api.createVehicle("BLISTA", new Vector3(-179.097f, 193.3891f, 14.47749f), new Quaternion(0.003143757f, -0.01116914f, 0.9981435f, -0.05979124f)); //bus
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

            #endregion

            var npc1 = new ServerNPC("Babens", MIVSDK.ModelDictionary.getPedModelByName("F_Y_BANK_01"), new Vector3(-242.1259f, 277.121f, 14.78422f), 1.0f);

            var npc2 = new ServerNPC("Krupka Mateush", MIVSDK.ModelDictionary.getPedModelByName("F_Y_STRIPPERC01"), new Vector3(-219.1516f, 277.0148f, 14.79722f), 1.0f);

            //npc2.EnterVehicle(infernus);

            var npc3 = new ServerNPC("Janek Bobok", MIVSDK.ModelDictionary.getPedModelByName("F_Y_DOCTOR_01"), new Vector3(-219.1516f, 271.0148f, 14.79722f), 1.0f);

            setTimer(2000, delegate
            {
                Random random = new Random();
                npc1.WalkTo(npc1.Position + new Vector3(random.Next(-200, 200), random.Next(-200, 200), random.Next(-200, 200)));
                var player = api.getPlayer(0);
                //if (player != null) npc2.DriveTo(player.Position);
                if (player != null) npc3.RunTo(player.Position);
            });

            api.onPlayerKeyDown += api_onPlayerKeyDown;
        }

        void api_onPlayerSpawn(ServerPlayer player)
        {
            if (((PlayerData)player.metadata).inSkinSelectionMode)
            {
                player.Camera.Position = skinCameraPos;
                player.Camera.LookAt(skinPedPos);
                player.Position = skinPedPos;
                player.Heading = 242.1461f;
                player.Freezed = true;
                player.Model = ModelDictionary.getAllPedModels().Keys.First();
            }
            else
            {
                int random = new Random().Next(spawns.Length);
                player.Position = new Vector3(spawns[random].X, spawns[random].Y, spawns[random].Z);
                player.Heading = spawns[random].W;
            }
        }

        void api_onPlayerDisconnect(ServerPlayer player)
        {
            api.writeChat("Player " + player.Nick + " disconnected");
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
            if (command == "veh")
            {
                //Vector3 carpos = MathHelper.Around(player.Position, 4.0f);
                Vector3 carpos = player.Position + (MathHelper.HeadingToDirection(player.Heading) * 6.0f);
                api.createVehicle(param[0], carpos, MathHelper.HeadingToQuaternion(player.Heading));
                return;
            }
            if (command == "cam")
            {
                player.Camera.Reset();
                return;
            }
            if (command == "test")
            {
                var pos = new Vector3(2468.039f, 147.9008f, 5.838196f);
                player.requester.isObjectVisible(pos, (response) =>
                {
                    api.writeChat(response.ToString());
                });
                player.requester.worldToScreenProject(pos, (response) =>
                {
                    api.writeChat(response.X.ToString() + " " + response.Y.ToString());
                });

            }
        }

        private void api_onPlayerConnect(System.Net.EndPoint address, ServerPlayer player)
        {
            api.writeChat("Player " + player.Nick + " connected");
            api.writeChat(player, "Hello " + player.Nick);
            player.metadata = new PlayerData()
            {
                inSkinSelectionMode = true
            };
        }

        private void api_onPlayerKeyDown(ServerPlayer player, System.Windows.Forms.Keys key)
        {
            PlayerData data = (PlayerData)player.metadata;
            if (data.inSkinSelectionMode)
            {
                if (key == System.Windows.Forms.Keys.Left)
                {
                    var dict = ModelDictionary.getAllPedModels();
                    data.currentModelIndex--;
                    if (data.currentModelIndex < 0) data.currentModelIndex = (uint)(dict.Count - 1);
                    player.Model = dict.Keys.ToArray()[data.currentModelIndex];
                }
                if (key == System.Windows.Forms.Keys.Right)
                {
                    var dict = ModelDictionary.getAllPedModels();
                    data.currentModelIndex++;
                    if (data.currentModelIndex > dict.Count) data.currentModelIndex = (uint)0;
                    player.Model = dict.Keys.ToArray()[data.currentModelIndex];
                }
                if (key == System.Windows.Forms.Keys.Enter)
                {
                    data.inSkinSelectionMode = false;
                    player.Camera.Reset();
                    player.Freezed = false;
                    api_onPlayerSpawn(player);
                }
            }
        }

        private void api_onPlayerSendText(ServerPlayer player, string text)
        {
            api.writeChat(player.Nick + "(" + player.id + "): " + text);
            Console.WriteLine("# " + player.Nick + " [" + player.id + "]: " + text);
        }
    }
}