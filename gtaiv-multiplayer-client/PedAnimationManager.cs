using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;

namespace MIVClient
{
    public enum PedAnimations
    {
        NotStreamed,
        StandStill,
        Couch,
        Walk,
        Run,
        Ragdoll
    }
    public class PedAnimationManager
    {

        StreamedPed ped;
        PedAnimations currentAnimation;
        static AnimationSet animset;

        public PedAnimationManager(StreamedPed ped)
        {
            this.ped = ped;
            currentAnimation = PedAnimations.StandStill;
            animset = new AnimationSet("move_rpg");
        }

        public void playAnimation(PedAnimations anim)
        {
            if (!ped.streamedIn)
            {
                currentAnimation = PedAnimations.NotStreamed;
            }
            else if (currentAnimation != anim)
            {
                switch (anim)
                {
                    case PedAnimations.Run: ped.gameReference.Animation.Play(animset, "sprint", 0.5f, AnimationFlags.Unknown05);
                        break;
                    case PedAnimations.Walk: ped.gameReference.Animation.Play(animset, "walk", 1.0f, AnimationFlags.Unknown05);
                        break;
                    case PedAnimations.StandStill: ped.gameReference.Animation.Play(animset, "idle", 1.0f);
                        break;
                }
                currentAnimation = anim;
            }
        }
    }
}
