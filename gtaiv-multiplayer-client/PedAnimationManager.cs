// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using GTA;
using System;

namespace MIVClient
{
    public enum PedAnimations
    {
        NotStreamed,
        StandStill,
        Couch,
        Aim,
        AimWalk,
        Walk,
        WalkTo,
        Jump,
        Climb,
        Run,
        RunTo,
        Ragdoll,
        Shoot,
        EnterClosestVehicle
    }

    public class PedAnimationManager
    {
        private static AnimationSet animset;
        private static AnimationSet animset2;
        private PedAnimations currentAnimation;
        private DateTime lastAnimationStarted;
        private object[] lastParams;
        private Vector3 lastRunToCoord, lastWalkToCoord;
        private StreamedPed ped;

        public PedAnimationManager(StreamedPed ped)
        {
            this.ped = ped;
            currentAnimation = PedAnimations.StandStill;
            animset = new AnimationSet("move_m@casual");
            animset2 = new AnimationSet("gun@ak47");
            lastRunToCoord = Vector3.Zero;
            lastWalkToCoord = Vector3.Zero;
            lastAnimationStarted = DateTime.Now;
        }

        public void playAnimation(PedAnimations anim, params object[] param)
        {
            lastParams = param;
            if (!ped.IsStreamedIn())
            {
                currentAnimation = PedAnimations.NotStreamed;
                return;
            }
            if (currentAnimation != anim)
            {
                lastAnimationStarted = DateTime.Now;
                currentAnimation = anim;
                if (anim == PedAnimations.RunTo && ped.gameReference.Position.DistanceTo((Vector3)param[0]) > 1.0f)
                {
                    if ((lastRunToCoord).DistanceTo((Vector3)param[0]) > 1.0f)
                    {
                        //ped.gameReference.Animation.Play(animset, "holster_2_aim", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        ped.gameReference.Task.RunTo((Vector3)param[0]);
                        lastRunToCoord = (Vector3)param[0];
                    }
                    return;
                }
                if (anim == PedAnimations.WalkTo && ped.gameReference.Position.DistanceTo((Vector3)param[0]) > 1.0f)
                {
                    if ((lastWalkToCoord).DistanceTo((Vector3)param[0]) > 1.0f)
                    {
                        //ped.gameReference.Animation.Play(animset, "holster_2_aim", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        ped.gameReference.Task.GoTo((Vector3)param[0]);
                        lastWalkToCoord = (Vector3)param[0];
                    }
                    return;
                }
                ped.gameReference.PreventRagdoll = true;
                ped.gameReference.Task.ClearAllImmediately();
                switch (anim)
                {
                    case PedAnimations.Run: ped.gameReference.Animation.Play(animset, "sprint", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        break;

                    case PedAnimations.Aim:
                        //ped.gameReference.Animation.Play(animset, "holster_2_aim", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        ped.gameReference.Task.AimAt(ped.gameReference.Position + (ped.cameraDirection * 100.0f), 9999999);
                        ped.gameReference.Position = ped.position;
                        currentAnimation = PedAnimations.NotStreamed;
                        break;

                    case PedAnimations.Couch: ped.gameReference.Animation.Play(animset2, "unholster_crouch", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        break;

                    case PedAnimations.Climb:
                        GTA.Native.Function.Call("TASK_CLIMB", ped.gameReference, 1);
                        GTA.Native.Function.Call("TASK_CLIMB", ped.gameReference, true);
                        break;
                    case PedAnimations.Jump:
                        GTA.Native.Function.Call("TASK_JUMP", ped.gameReference, 1);
                        GTA.Native.Function.Call("TASK_JUMP", ped.gameReference, true);
                        break;

                    case PedAnimations.Shoot:
                        ped.gameReference.Accuracy = 99;
                        //ped.gameReference.Task.ShootAt(Client.instance.getPlayerPed(), ShootMode.Continuous, 9992);
                        //ped.gameReference.ShootAt(ped.gameReference.Position + ped.gameReference.Direction);
                        Vector3 pos = ped.gameReference.Position + (ped.cameraDirection * 100.0f);
                        ped.gameReference.Position = ped.position;
                        //GTA.Native.Function.Call("TRIGGER_PTFX_ON_PED_BONE", "muz_smg", Player.Character, 0.3, 0.0, -0.12, 90.0, 0.0, 0.0, 0x38A1, 0.5);
                        GTA.Native.Function.Call("FIRE_PED_WEAPON", ped.gameReference, pos.X, pos.Y, pos.Z);
                        GTA.Native.Function.Call("TASK_SHOOT_AT_COORD", ped.gameReference, pos.X, pos.Y, pos.Z, (Int32)4, 999992);
                        //AlternateHook.call(MIVSDK.AlternateHookRequest.PedCommands.FIRE_PED_WEAPON, ped.gameReference, pos.X, pos.Y, pos.Z);
                        ped.gameReference.Heading = ped.heading;
                        ped.gameReference.Task.AimAt(ped.gameReference.Position + (ped.cameraDirection * 100.0f), 9999999);
                        currentAnimation = PedAnimations.Aim;
                        break;

                    case PedAnimations.EnterClosestVehicle:
                        //ped.gameReference.Task.ShootAt(Client.instance.getPlayerPed(), ShootMode.Continuous, 9992);
                        ped.gameReference.Task.EnterVehicle(World.GetClosestVehicle(ped.gameReference.Position, 10.0f), VehicleSeat.Driver);
                        break;

                    case PedAnimations.Ragdoll: ped.gameReference.ForceRagdoll(1000, false);
                        break;

                    case PedAnimations.Walk: ped.gameReference.Animation.Play(animset, "walk", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        break;

                    case PedAnimations.StandStill: ped.gameReference.Animation.Play(animset, "idle", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        break;
                }
            }
        }

        public void refreshAnimation()
        {
            playAnimation(currentAnimation, lastParams);
        }

        public void refreshAnimationForce()
        {
            currentAnimation = PedAnimations.NotStreamed;
            //playAnimation(currentAnimation, lastParams);
        }
    }
}