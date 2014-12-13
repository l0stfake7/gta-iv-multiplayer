// Copyright 2014 Adrian Chlubek. This file is part of GTA Multiplayer IV project.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
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
                    //ped.animator.refreshAnimation();
                }
            }
        }
    }
}