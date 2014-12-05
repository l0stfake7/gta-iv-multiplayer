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

        public void updatePed(byte id, UpdateDataStruct data, StreamedPed ped)
        {
            var posnew = new Vector3(data.pos_x, data.pos_y, data.pos_z - 1.0f);
            ped.position = posnew;
            ped.heading = data.heading;
            ped.direction = new Vector3(data.rot_x, data.rot_y, data.rot_z);
            if (ped.streamedIn && data.vehicle_id == 0)
            {
                if (ped.gameReference.isInVehicle())
                {
                    ped.gameReference.CurrentVehicle.PassengersLeaveVehicle(true);
                    ped.gameReference.CurrentVehicle.Delete();
                }

                float delta = posnew.DistanceTo(ped.gameReference.Position);
                Vector3 vdelta = posnew - ped.gameReference.Position;
                //ped.gameReference.Weapons.MP5.Ammo = 999;

                int healthDelta = data.ped_health - ped.gameReference.Health;
                ped.gameReference.Health = data.ped_health;
                ped.last_game_health = data.ped_health;

                if (healthDelta > 20 && healthDelta < 100)
                {
                    var bpf = new BinaryPacketFormatter(Commands.Player_damage);
                    bpf.add(new byte[1] { id });
                    //Client.instance.chatController.writeChat("damaged " + healthDelta) ;
                    Client.instance.serverConnection.write(bpf.getBytes());
                }

                ped.gameReference.Position = posnew;
                ped.gameReference.Heading = data.heading;

                if ((data.state & PlayerState.IsShooting) != 0)
                {
                    ped.animator.playAnimation(PedAnimations.Shoot);
                } else 
                if ((data.state & PlayerState.IsAiming) != 0)
                {
                    ped.animator.playAnimation(PedAnimations.Aim);
                } else 
                if ((data.state & PlayerState.IsRagdoll) != 0)
                {
                    ped.animator.playAnimation(PedAnimations.Ragdoll);
                } else 
                if ((data.vstate & VehicleState.IsEnteringVehicle) != 0)
                {
                    ped.animator.playAnimation(PedAnimations.EnterClosestVehicle);
                } else
                if ((data.state & PlayerState.IsCrouching) != 0)
                {
                    ped.animator.playAnimation(PedAnimations.Couch);
                }
                else
                {
                    if (new Vector3(data.vel_x, data.vel_y, data.vel_z).Length() > 2.2f)
                    {
                        ped.animator.playAnimation(PedAnimations.Run);
                    }
                    else if (new Vector3(data.vel_x, data.vel_y, data.vel_z).Length() > 0.3f)
                    {
                        ped.animator.playAnimation(PedAnimations.Walk);
                    }
                    else
                    {
                        ped.animator.playAnimation(PedAnimations.StandStill);
                    }

                }

                //ped.gameReference.Velocity = new Vector3(elemValue.vel_x, elemValue.vel_y, elemValue.vel_z);
                //ped.gameReference.Task.ClearAllImmediately();
            }
        }

        public void updateVehicle(byte id, UpdateDataStruct data, StreamedPed ped)
        {
            if (data.vehicle_id > 0)
            {
                var posnew = new Vector3(data.pos_x, data.pos_y, data.pos_z + 1.0f);
                StreamedVehicle veh = vehicleController.getById(data.vehicle_id);
                if (veh != null)
                {
                    if (veh.streamedIn)
                    {
                        if (ped != null && ped.streamedIn && !ped.gameReference.isInVehicle())
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
                        if ((data.vstate & VehicleState.IsAsPassenger) != 0) return;
                        veh.position = posnew;
                        veh.gameReference.Position = posnew;
                        veh.orientation = new Quaternion(data.rot_x, data.rot_y, data.rot_z, data.rot_a);
                        //veh.gameReference.ApplyForce(, Vector3.Zero);
                        veh.gameReference.RotationQuaternion = veh.orientation;
                        veh.gameReference.Velocity = new Vector3(data.vel_x, data.vel_y, data.vel_z);
                        if (veh.gameReference.Velocity.Length() > 2.0f)
                        {
                            ped.gameReference.Task.DrivePointRoute(veh.gameReference, 999.0f, posnew + veh.gameReference.Velocity);
                        }
                        else if ((data.vstate & VehicleState.IsBraking) != 0)
                        {
                            ped.gameReference.Task.DrivePointRoute(veh.gameReference, 999.0f, posnew - veh.gameReference.Velocity);
                        }
                        else 

                        veh.gameReference.Health = data.vehicle_health;
                    }
                }
            }
        }
    }
}