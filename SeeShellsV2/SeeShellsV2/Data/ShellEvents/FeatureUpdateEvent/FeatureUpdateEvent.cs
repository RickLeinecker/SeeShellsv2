using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeShellsV2.Data
{
    public class FeatureUpdateEvent : ShellEvent
    {
        public override string LongDescriptionPattern => "A large number of simultaneous registry-write timestamp edits across multiple shellbags suggests that a user may have installed a Windows Feature Update at {TIMESTAMP}.";
    }
}
