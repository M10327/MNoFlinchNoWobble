using Rocket.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MNoFlinchNoWobble
{
    public class Config : IRocketPluginConfiguration
    {
        public bool DisableFlinching;
        public bool DisableWobble;
        public byte HPCalcOffset;
        public List<AntiWobbleExplosionReplacements> AntiWobbleReplacements;
        public void LoadDefaults()
        {
            DisableFlinching = true;
            DisableWobble = true;
            HPCalcOffset = 10;
            AntiWobbleReplacements = new List<AntiWobbleExplosionReplacements>()
            {
                new AntiWobbleExplosionReplacements()
                {
                    ReplacedId = 20,
                    ReplacementId = 18530
                },
                new AntiWobbleExplosionReplacements()
                {
                    ReplacedId = 34,
                    ReplacementId = 18531
                },
                new AntiWobbleExplosionReplacements()
                {
                    ReplacedId = 45,
                    ReplacementId = 18532
                },
                new AntiWobbleExplosionReplacements()
                {
                    ReplacedId = 53,
                    ReplacementId = 18533
                },
                new AntiWobbleExplosionReplacements()
                {
                    ReplacedId = 54,
                    ReplacementId = 18534
                },
                new AntiWobbleExplosionReplacements()
                {
                    ReplacedId = 123,
                    ReplacementId = 18535
                },
                new AntiWobbleExplosionReplacements()
                {
                    ReplacedId = 136,
                    ReplacementId = 18536
                }
            };
        }
    }

    public class AntiWobbleExplosionReplacements
    {
        [XmlAttribute("ReplacedId")]
        public ushort ReplacedId;
        [XmlAttribute("ReplacementId")]
        public ushort ReplacementId;
    }
}
