using GTA;
using System;
using System.Linq;
using System.Collections.Generic;

//using MIVSDK;

namespace MIVClient
{
    public class PedStreamer
    {
        public List<StreamedPed> peds;

        private GTA.Font nickfont, fa_font;
        private System.Drawing.Color nickcolor, chatcolor;

        private Client client;

        public PedStreamer(Client client)
        {
            nickfont = new Font("Segoe UI", 24, FontScaling.Pixel, false, false);
            nickfont.Effect = FontEffect.None;
            nickcolor = System.Drawing.Color.LightYellow;
            chatcolor = System.Drawing.Color.White;
            fa_font = new Font("FotntAwesome", 24, FontScaling.Pixel, false, false);
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
        public void updateSlow()
        {
            var pedss = World.GetPeds(client.getPlayerPed().Position, 200.0f);
            foreach (Ped a in pedss)
            {
                if (a.Exists() && a.isAlive && a != client.getPlayerPed() && peds.Count(ax => ax.gameReference != null && ax.gameReference == a) == 0)
                {
                    a.Delete();
                }
            }
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
                        var peddelta = ped.gameReference.Position - client.getPlayerPed().Position;
                        float distance = peddelta.Length();
                        //float distance_from_centerscreen = (projected - new Vector2(Game.Resolution.Width, Game.Resolution.Height)).Length();
                        int alpha = (int)Math.Round((255.0f * (distance / -80.0f + 1.0f)));
                        if (alpha > 255) alpha = 255;
                        if (alpha < 0) alpha = 0;
                        if (projected.X < -120 || projected.X > Game.Resolution.Width || projected.Y < -50 || projected.Y > Game.Resolution.Height || 
                            (peddelta + Game.CurrentCamera.Direction).Length() < distance || !Game.CurrentCamera.isSphereVisible(ped.gameReference.Position, 3.0f))
                        {
                            ped.nickDraw.destroy();
                            ped.healthDraw.destroy();
                            ped.healthDraw2.destroy();
                            ped.chatDraw.destroy();
                            ped.nickDraw = null;
                            ped.healthDraw = null;
                            ped.healthDraw2 = null;
                            ped.chatDraw = null;
                        }
                        else
                        {
                            var rect = new System.Drawing.RectangleF(projected.X - 100, projected.Y - 50, 200, 30);
                            var rect2 = new System.Drawing.RectangleF(projected.X - 37, projected.Y - 22, 37*2, 11);
                            var rect22 = new System.Drawing.RectangleF(projected.X - 35, projected.Y - 20, (35.0f * 2.0f) * (ped.last_game_health < 0 ? 0 : ped.last_game_health / 100.0f), 7);
                            var rect3 = new System.Drawing.RectangleF(projected.X - 30, projected.Y - 10, 60, 30);
                            var chaticonframe = new System.Drawing.RectangleF(projected.X - 30, projected.Y - 80, 60, 30);
                            if (ped.nickDraw == null)
                            {
                                ped.nickDraw = new ClientTextView(rect, TextAlignment.Center, ped.networkname, nickfont, System.Drawing.Color.FromArgb(alpha, 255, 255, 255));
                                ped.healthDraw = new ClientRectangleView(rect2, System.Drawing.Color.FromArgb(alpha, 0, 0, 0));
                                ped.healthDraw2 = new ClientRectangleView(rect22, System.Drawing.Color.FromArgb(alpha, 80, 80, 255));
                                ped.chatDraw = new ClientTextView(rect3, TextAlignment.Center, "", nickfont, System.Drawing.Color.FromArgb(alpha, 255, 255, 255));
                            }
                            else
                            {
                                ped.nickDraw.textbox = rect;
                                ped.nickDraw.color = System.Drawing.Color.FromArgb(alpha, 255, 255, 255);
                                ped.healthDraw.box = rect2;
                                ped.healthDraw.color = System.Drawing.Color.FromArgb(alpha, 0, 0, 0);
                                ped.healthDraw2.box = rect22;
                                ped.healthDraw2.color = System.Drawing.Color.FromArgb(alpha, 80, 80, 255);
                                ped.chatDraw.textbox = rect3;
                                ped.chatDraw.color = System.Drawing.Color.FromArgb(alpha, 255, 255, 255);
                            }

                        }

                        
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
                            ped.gameReference.BlockGestures = true;
                            ped.gameReference.BlockPermanentEvents = true;
                            ped.gameReference.Task.AlwaysKeepTask = true;
                            ped.gameReference.CowerInsteadOfFleeing = true;

                            //ped.gameReference.Task.LookAt(playerPos, 9999);
                        }
                        else
                        {
                            if (ped.last_game_health > 0 && (ped.gameReference.isDead || !ped.gameReference.isAlive || ped.gameReference.Health == 0))
                            {
                                ped.blip.Delete();
                                ped.nickDraw.destroy();
                                ped.healthDraw.destroy();
                                ped.healthDraw2.destroy();
                                ped.chatDraw.destroy();
                                ped.iconDraw.destroy();
                                ped.nickDraw = null;
                                ped.gameReference.Delete();
                                ped.gameReference = null;
                                ped.hasNetworkName = false;
                                ped.streamedIn = false;
                            }
                        }
                    }
                    else if (ped.streamedIn)
                    {
                        ped.blip.Delete();
                        ped.nickDraw.destroy();
                        ped.healthDraw.destroy();
                        ped.healthDraw2.destroy();
                        ped.chatDraw.destroy();
                        ped.iconDraw.destroy();
                        ped.nickDraw = null;
                        ped.gameReference.Delete();
                        ped.gameReference = null;
                        ped.hasNetworkName = false;
                        ped.streamedIn = false;
                    }
                }
                catch (Exception e)
                {
                    Game.Console.Print("PEDSTREAMER: " + e.Message);
                    Game.Log("PEDSTREAMER: " + e.Message);
                }
            }
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
        public bool streamedIn, immortal;
        public int last_game_health;
        public uint vehicle_id;
        public ClientTextView nickDraw, iconDraw;
        public ClientRectangleView healthDraw, healthDraw2;
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
            vehicle_id = 0;
            animator = new PedAnimationManager(this);
        }

        public void delete()
        {
            streamer.delete(this);
        }
    }
}