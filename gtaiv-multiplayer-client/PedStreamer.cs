using GTA;
using System;
using System.Collections.Generic;

//using MIVSDK;

namespace MIVClient
{
    public class PedStreamer
    {
        public List<StreamedPed> peds;

        private Font nickfont;
        private System.Drawing.Color nickcolor, healthcolor, chatcolor;

        private Client client;

        public PedStreamer(Client client)
        {
            nickfont = new Font("Consolas", 24, FontScaling.Pixel);
            nickcolor = System.Drawing.Color.LightYellow;
            healthcolor = System.Drawing.Color.FromArgb(190, 255, 33, 33);
            chatcolor = System.Drawing.Color.White;
            this.client = client;
            peds = new List<StreamedPed>();
        }

        public void add(StreamedPed ped)
        {
            peds.Add(ped);
        }

        public void delete(StreamedPed ped)
        {
            peds.Remove(ped);
        }


        public void updateGfx()
        {
            foreach (StreamedPed ped in this.peds)
            {
                try
                {
                    if (ped.streamedIn && ped.gameReference.Exists())
                    {
                        var projected = (Vector2)World.WorldToScreenProject(ped.gameReference.Position);

                        var rect = new System.Drawing.RectangleF(projected.X - 30, projected.Y - 50, 60, 30);
                        ped.nickDraw.textbox = rect;

                        var rect2 = new System.Drawing.RectangleF(projected.X - 30, projected.Y - 30, 60, 10);
                        ped.healthDraw.box = rect2;

                        var rect3 = new System.Drawing.RectangleF(projected.X - 30, projected.Y - 10, 60, 30);
                        ped.chatDraw.textbox = rect3;
                    }
                }
                catch
                { }
            }
        }

        public void update()
        {
            Vector3 playerPos = client.getPlayerPed().Position;
            foreach (StreamedPed ped in this.peds)
            {
                try
                {
                    float distance = playerPos.DistanceTo(ped.position);
                    //client.chatController.writeChat(playerPos.X.ToString() + " " + playerPos.Y.ToString() + " " + playerPos.Z.ToString() + " ");
                    if (distance < 300.0f)
                    {
                        if (!ped.streamedIn || ped.gameReference == null || !ped.gameReference.Exists())
                        {
                            ped.gameReference = World.CreatePed(ped.model, ped.position, RelationshipGroup.NetworkPlayer_01);
                            ped.blip = Blip.AddBlip(ped.gameReference);
                            ped.blip.Color = BlipColor.LightOrange;
                            ped.blip.Display = BlipDisplay.MapOnly;
                            ped.blip.Icon = BlipIcon.Misc_Destination;
                            ped.blip.Name = "Player";
                            ped.streamedIn = true;
                            ped.gameReference.Heading = ped.heading;
                            var projected = (Vector2)World.WorldToScreenProject(ped.position);

                            var rect = new System.Drawing.RectangleF(projected.X - 30, projected.Y - 50, 60, 30);
                            ped.nickDraw = new ClientTextView(rect, TextAlignment.Center, ped.networkname, nickfont, nickcolor);

                            var rect2 = new System.Drawing.RectangleF(projected.X - 30, projected.Y - 30, 60, 10);
                            ped.healthDraw = new ClientRectangleView(rect2, healthcolor);

                            var rect3 = new System.Drawing.RectangleF(projected.X - 30, projected.Y - 10, 60, 30);
                            ped.chatDraw = new ClientTextView(rect3, TextAlignment.Center, "", nickfont, chatcolor);

                            //ped.gameReference.GiveFakeNetworkName(ped.networkname, System.Drawing.Color.White);
                            var rand = new System.Random();
                            //ped.gameReference.GiveFakeNetworkName(ped.networkname, System.Drawing.Color.FromArgb(255, rand.Next(70, 255), rand.Next(70, 255), rand.Next(70, 255)));
                            //ped.gameReference.Invincible = true;
                            Weapon weapon = Weapon.Rifle_M4;
                            ped.gameReference.Weapons.AssaultRifle_M4.Ammo = 999;
                            ped.gameReference.Weapons.AssaultRifle_M4.AmmoInClip = 999;
                            ped.gameReference.Weapons.Select(weapon);
                            ped.gameReference.SenseRange = 0;
                            ped.gameReference.Task.GuardCurrentPosition();
                            //ped.gameReference.Task.LookAt(playerPos, 9999);
                        }
                        else
                        {
                           
                        }
                    }
                    else if (ped.streamedIn)
                    {
                        ped.blip.Delete();
                        ped.nickDraw.destroy();
                        ped.healthDraw.destroy();
                        ped.chatDraw.destroy();
                        ped.gameReference.Delete();
                        ped.gameReference = null;
                        ped.hasNetworkName = false;
                        ped.streamedIn = false;
                    }
                }
                catch (Exception e)
                {
                    client.chatController.writeChat("PEDSTREAMER: " + e.Message);
                }
            }/*
            var pedss = World.GetPeds(client.getPlayerPed().Position, 200.0f);
            foreach (Ped a in pedss) if (a.Exists() && a.isAlive && a != client.getPlayerPed() && peds.Count(ax => ax.gameReference != null && ax.gameReference == a) == 0) a.Delete();*/
        }
    }

    public class StreamedPed
    {
        public PedAnimationManager animator;
        public Blip blip;
        public Ped gameReference;
        public bool hasNetworkName;
        public float heading;
        public string model, networkname;
        public Vector3 position, direction;
        public bool streamedIn;
        public int last_game_health;
        public ClientTextView nickDraw;
        public ClientRectangleView healthDraw;
        public ClientTextView chatDraw;
        private string currentChatMessage;
        private DateTime lastChatTime;

        public string CurrentChatMessage
        {
            get
            {
                if ((DateTime.Now - lastChatTime).Seconds > 5)
                {
                    currentChatMessage = "";
                }
                return currentChatMessage;
            }
            set
            {
                lastChatTime = DateTime.Now;
                currentChatMessage = value;
            }
        }

        private PedStreamer streamer;

        public StreamedPed(PedStreamer streamer, string model, string networkname, Vector3 position, float heading)
        {
            this.streamer = streamer;
            this.position = position;
            this.heading = heading;
            this.networkname = networkname;
            this.model = model;
            direction = Vector3.Zero;
            streamedIn = false;
            hasNetworkName = false;
            streamer.add(this);
            animator = new PedAnimationManager(this);
        }

        public void delete()
        {
            streamer.delete(this);
        }
    }
}