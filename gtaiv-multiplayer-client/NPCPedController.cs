// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
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