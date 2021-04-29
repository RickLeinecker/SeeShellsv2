using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class RemovableDriveConnectEvent : ShellEvent, IShellEvent
    {
        public override string LongDescriptionPattern => "A shellbag that documents a removable storage device was found. The shellbag was last writen by Windows at {TIMESTAMP}, suggesting that {USER} connected or disconnected {PLACE} at this time.";
    }
}
