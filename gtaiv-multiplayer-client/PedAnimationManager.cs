using GTA;

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
        private StreamedPed ped;

        public PedAnimationManager(StreamedPed ped)
        {
            this.ped = ped;
            currentAnimation = PedAnimations.StandStill;
            animset = new AnimationSet("move_m@casual");
            animset2 = new AnimationSet("gun@ak47");
        }

        public void playAnimation(PedAnimations anim, params object[] param)
        {
            if (!ped.streamedIn || ped.gameReference == null || !ped.gameReference.Exists())
            {
                currentAnimation = PedAnimations.NotStreamed;
            }
            if (currentAnimation != anim)
            {
                ped.gameReference.Task.ClearAllImmediately();
                switch (anim)
                {
                    case PedAnimations.Run: ped.gameReference.Animation.Play(animset, "sprint", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        break;

                    case PedAnimations.Aim:
                        //ped.gameReference.Animation.Play(animset, "holster_2_aim", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        ped.gameReference.Task.AimAt(ped.gameReference.Position + ped.direction, 9999);
                        break;
                    case PedAnimations.RunTo:
                        //ped.gameReference.Animation.Play(animset, "holster_2_aim", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        ped.gameReference.Task.RunTo((Vector3)param[0]);
                        break;

                    case PedAnimations.Couch: ped.gameReference.Animation.Play(animset2, "unholster_crouch", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        break;
                    case PedAnimations.Shoot:
                        ped.gameReference.Accuracy = 0;
                        ped.gameReference.Task.ShootAt(Client.instance.getPlayerPed(), ShootMode.Continuous, 9992);
                        //ped.gameReference.ShootAt(ped.gameReference.Position + ped.gameReference.Direction);
                        break;
                    case PedAnimations.EnterClosestVehicle:
                        //ped.gameReference.Task.ShootAt(Client.instance.getPlayerPed(), ShootMode.Continuous, 9992);
                        ped.gameReference.Task.EnterVehicle(World.GetClosestVehicle(ped.gameReference.Position, 10.0f), VehicleSeat.Driver);
                        break;

                    case PedAnimations.Ragdoll: ped.gameReference.ForceRagdoll(1, false);
                        break;

                    case PedAnimations.Walk: ped.gameReference.Animation.Play(animset, "walk", 1.0f, AnimationFlags.Unknown01 | AnimationFlags.Unknown05);
                        break;

                    case PedAnimations.StandStill: ped.gameReference.Animation.Play(animset, "idle", 1.0f);
                        break;
                }
                currentAnimation = anim;
            }
        }
    }
}