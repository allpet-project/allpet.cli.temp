﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AllPet.peer.tcp 
{
    public class PeerV2
    {
        public static IPeer CreatePeer()
        {
            return new light.asynctcp.ServerModule();
        }
    }
}
