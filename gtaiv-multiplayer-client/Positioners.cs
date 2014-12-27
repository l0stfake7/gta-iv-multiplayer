// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using GTA;
using MIVSDK;

namespace MIVClient
{
    public partial class Client : Script
    {
        public void preparePed(Ped ped)
        {
            if (ped.Exists())
            {
                ped.Invincible = true;
                ped.WillFlyThroughWindscreen = false;
                ped.PreventRagdoll = true;
            }
        }

        public void prepareVehicle(Vehicle vehicle)
        {
            if (vehicle.Exists())
            {
                vehicle.EngineRunning = true;
                vehicle.InteriorLightOn = true;
                vehicle.HazardLightsOn = true;
                vehicle.Repair();
            }
        }

        public void prepareVehicle(StreamedVehicle vehicle)
        {
            if (vehicle.gameReference != null) prepareVehicle(vehicle.gameReference);
        }

        public void prepareVehicle(StreamedPed ped)
        {
            if (ped.gameReference != null) preparePed(ped.gameReference);
        }

        public void updatePed(uint id, UpdateDataStruct data, StreamedPed ped)
        {
            var posnew = new Vector3(data.pos_x, data.pos_y, data.pos_z - 1.0f);
            ped.position = posnew;
            ped.heading = data.heading;
            ped.direction = new Vector3(data.rot_x, data.rot_y, data.rot_z);
            ped.cameraDirection = new Vector3(data.camdir_x, data.camdir_y, data.camdir_z);
            if (ped.IsStreamedIn() && data.vehicle_id == 0)
            {
                if (ped.gameReference.isInVehicle())
                {
                    ped.gameReference.CurrentVehicle.PassengersLeaveVehicle(true);
                    //ped.gameReference.CurrentVehicle.Delete();
                }

                float delta = posnew.DistanceTo(ped.gameReference.Position);
                Vector3 vdelta = posnew - ped.gameReference.Position;
                //ped.gameReference.Weapons.MP5.Ammo = 999;

                int healthDelta = data.ped_health - ped.gameReference.Health;
                ped.gameReference.Health = data.ped_health;
                ped.last_game_health = data.ped_health;

                if (data.weapon > 0)
                {
                    if (ped.gameReference.Weapons.Current != (Weapon)data.weapon)
                    {
                        ped.gameReference.Weapons.FromType((Weapon)data.weapon).Ammo = 999;
                        ped.gameReference.Weapons.FromType((Weapon)data.weapon).AmmoInClip = 999;
                        ped.gameReference.Weapons.Select((Weapon)data.weapon);
                    }
                }
                else
                {
                    ped.gameReference.Weapons.RemoveAll();
                }

                if (healthDelta > 20 && healthDelta < 100)
                {
                    var bpf = new BinaryPacketFormatter(Commands.Player_damage);
                    bpf.Add(id);
                    //Client.instance.chatController.writeChat("damaged " + healthDelta) ;
                    Client.instance.serverConnection.write(bpf.getBytes());
                }

                bool cancelPositionUpdate = false;

                if ((data.state & PlayerState.IsShooting) != 0)
                {
                    ped.animator.playAnimation(PedAnimations.Shoot);
                    cancelPositionUpdate = true;
                }
                else
                    if ((data.state & PlayerState.IsAiming) != 0)
                    {
                        ped.animator.playAnimation(PedAnimations.Aim);
                        cancelPositionUpdate = true;
                    }
                    else
                        if ((data.state & PlayerState.IsRagdoll) != 0 || data.ped_health <= 0)
                        {
                            ped.animator.playAnimation(PedAnimations.Ragdoll);
                            ped.gameReference.Velocity = new Vector3(data.vel_x, data.vel_y, data.vel_z);
                            cancelPositionUpdate = true;
                        }
                        else
                            if ((data.vstate & VehicleState.IsEnteringVehicle) != 0)
                            {
                                ped.animator.playAnimation(PedAnimations.EnterClosestVehicle);
                                cancelPositionUpdate = true;
                            }
                            else
                                if ((data.state & PlayerState.IsCrouching) != 0)
                                {
                                    ped.animator.playAnimation(PedAnimations.Couch);
                                }
                                else
                                {
                                    if ((data.vstate & VehicleState.IsAccelerating) != 0)
                                    {
                                        if ((data.vstate & VehicleState.IsSprinting) != 0)
                                        {
                                            ped.animator.playAnimation(PedAnimations.Run);
                                        }
                                        else
                                        {
                                            ped.animator.playAnimation(PedAnimations.Walk);
                                        }
                                    }
                                    else
                                    {
                                        ped.animator.playAnimation(PedAnimations.StandStill);
                                    }
                                }

                if (!cancelPositionUpdate)
                {
                    ped.gameReference.Position = posnew;
                }
                ped.gameReference.Heading = data.heading;

                //ped.gameReference.Velocity = new Vector3(elemValue.vel_x, elemValue.vel_y, elemValue.vel_z);
                //ped.gameReference.Task.ClearAllImmediately();
            }
        }

        public void updateVehicle(uint id, UpdateDataStruct data, StreamedPed ped)
        {
            if (data.vehicle_id > 0)
            {
                var posnew = new Vector3(data.pos_x, data.pos_y, data.pos_z);
                StreamedVehicle veh = vehicleController.GetInstance(data.vehicle_id);
                if (veh != null)
                {
                    if (veh.IsStreamedIn())
                    {
                        if (ped != null && ped.IsStreamedIn() && !ped.gameReference.isInVehicle())
                        {
                            if ((data.state & PlayerState.IsPassenger1) != 0)
                            {
                                ped.gameReference.WarpIntoVehicle(veh.gameReference, VehicleSeat.RightFront);
                            }
                            else if ((data.state & PlayerState.IsPassenger2) != 0)
                            {
                                ped.gameReference.WarpIntoVehicle(veh.gameReference, VehicleSeat.LeftRear);
                            }
                            else if ((data.state & PlayerState.IsPassenger3) != 0)
                            {
                                ped.gameReference.WarpIntoVehicle(veh.gameReference, VehicleSeat.RightFront);
                            }
                            else
                            {
                                ped.gameReference.WarpIntoVehicle(veh.gameReference, VehicleSeat.Driver);
                            }
                        }

                        int healthDelta = data.ped_health - ped.gameReference.Health;
                        ped.gameReference.Health = data.ped_health;
                        ped.last_game_health = data.ped_health;

                        if (healthDelta > 20 && healthDelta < 100)
                        {
                            var bpf = new BinaryPacketFormatter(Commands.Player_damage);
                            bpf.Add(id);
                            //Client.instance.chatController.writeChat("damaged " + healthDelta) ;
                            Client.instance.serverConnection.write(bpf.getBytes());
                        }

                        int vehicleHealthDelta = data.vehicle_health - veh.gameReference.Health;
                        veh.gameReference.Health = data.vehicle_health;
                        veh.last_game_health = data.vehicle_health;

                        if (vehicleHealthDelta > 20 && vehicleHealthDelta < 2000 && data.vehicle_id > 0)
                        {
                            var bpf = new BinaryPacketFormatter(Commands.Vehicle_damage, id, data.vehicle_id, vehicleHealthDelta);
                            Client.instance.serverConnection.write(bpf.getBytes());
                        }

                        if ((data.vstate & VehicleState.IsAsPassenger) != 0) return;
                        veh.position = posnew;
                        if (veh.gameReference.Position.DistanceTo(posnew) > 1.0f)
                        {
                            veh.gameReference.Position = posnew;
                        }
                        //veh.gameReference.Position = posnew;
                        veh.orientation = new Quaternion(data.rot_x, data.rot_y, data.rot_z, data.rot_a);
                        //veh.gameReference.ApplyForce(, Vector3.Zero);
                        veh.gameReference.RotationQuaternion = veh.orientation;
                        var vel = new Vector3(data.vel_x, data.vel_y, data.vel_z);
                        if (System.Math.Abs(veh.gameReference.Velocity.Length() - vel.Length()) > 6.0f)
                        {
                            veh.gameReference.ApplyForce(vel);
                        }
                        if ((data.vstate & VehicleState.IsBraking) == 0)
                        {
                            ped.gameReference.Task.DrivePointRoute(veh.gameReference, 999.0f, posnew - veh.gameReference.Velocity);
                        }
                        else
                        {
                            ped.gameReference.Task.DrivePointRoute(veh.gameReference, 999.0f, posnew + veh.gameReference.Velocity);
                        }
                        if ((data.state & PlayerState.IsShooting) != 0)
                        {
                            Vector3 pos = veh.gameReference.Position + veh.gameReference.Direction;
                            GTA.Native.Function.Call("FIRE_PED_WEAPON", ped.gameReference, pos.X, pos.Y, pos.Z);
                            GTA.Native.Function.Call("TASK_SHOOT_AT_COORD", ped.gameReference, pos.X, pos.Y, pos.Z, (Int32)4, 999992);
                        }
                    }
                }
            }
        }
    }
}