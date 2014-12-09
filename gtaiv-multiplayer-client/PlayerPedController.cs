using GTA;
using System.Linq;
using System.Collections.Generic;

namespace MIVClient
{
    public class PlayerPedController : IDControllerBase<StreamedPed>
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