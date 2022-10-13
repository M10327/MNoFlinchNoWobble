using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNoFlinchNoWobble
{
    public class Config : IRocketPluginConfiguration
    {
        public bool DisableFlinching;
        public bool DisableWobble;
        public void LoadDefaults()
        {
            DisableFlinching = true;
            DisableWobble = true;
        }
    }
}
