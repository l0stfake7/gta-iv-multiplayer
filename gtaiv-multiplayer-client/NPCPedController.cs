namespace MIVClient
{
    public class NPCPedController : IDControllerBase<StreamedPed>
    {
        public void update()
        {
            foreach (var ped in dict.Values)
            {
                if (ped.IsStreamedIn())
                {
                    if (ped.gameReference.isRagdoll || ped.gameReference.isDead || !ped.gameReference.isAliveAndWell)
                    {
                        ped.StreamOut();
                        continue;
                    }
                    if (ped.gameReference.Position.DistanceTo(ped.position) > 10.0f)
                    {
                        ped.gameReference.Position = ped.position;
                    }
                    ped.animator.refreshAnimation();
                }
            }
        }
    }
}