using System;
using System.Collections.Generic;
using System.Text;

using Unity;

namespace SeeShellsV2.Services
{
    public interface ICsvImporter
    {
        public void Import(string path);
    }
}
