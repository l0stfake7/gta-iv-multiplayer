using GTA;
using System.Collections.Generic;

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
                    ped.animator.refreshAnimation();
                }
            }
        }

    }
}