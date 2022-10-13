using HarmonyLib;
using Rocket.Core.Plugins;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MNoFlinchNoWobble
{
    public class MNoFlinchNoWobble : RocketPlugin<Config>
    {
        public Dictionary<ushort, ushort> EffectRep;
        public static MNoFlinchNoWobble Instance { get; private set; }
        protected override void Load()
        {
            Instance = this;
            if (Configuration.Instance.DisableFlinching) DamageTool.damagePlayerRequested += DamageTool_damagePlayerRequested;
            if (Configuration.Instance.DisableWobble) Patches.PatchAll();
            EffectRep = new Dictionary<ushort, ushort>();
            foreach (var x in Configuration.Instance.AntiWobbleReplacements)
            {
                EffectRep[x.ReplacedId] = x.ReplacementId;
            }
            Rocket.Core.Logging.Logger.Log($"{Name} {Assembly.GetName().Version} has been loaded!");
        }

        private void DamageTool_damagePlayerRequested(ref DamagePlayerParameters par, ref bool shouldAllow)
        {
            try
            {
                if (par.player.life.health - par.damage + Configuration.Instance.HPCalcOffset > 0)
                {
                    par.direction *= 0;
                }
            }
            catch (Exception ex)
            {
                // Hopefully this will never call, but i put it in a try/catch statement cuz if this errors it also cancels the damage
                Rocket.Core.Logging.Logger.LogWarning($"Global damage modifier error: {ex}");
            }
        }

        protected override void Unload()
        {
            DamageTool.damagePlayerRequested -= DamageTool_damagePlayerRequested;
            if (Configuration.Instance.DisableWobble) Patches.UnpatchAll();
        }
    }

    internal static class Patches
    {
        private static Harmony PatcherInstance;
        // run in Load()
        internal static void PatchAll()
        {
            PatcherInstance = new Harmony("MNoFlinchNoWobble");
            PatcherInstance.PatchAll();
        }
        // run in Unload()
        internal static void UnpatchAll()
        {
            PatcherInstance.UnpatchAll("MNoFlinchNoWobble");
        }

        // this one only does grenades
        [HarmonyPatch]
        internal static class FlinchingPatchesGrenade
        {
            [HarmonyPatch(typeof(Grenade))]
            [HarmonyPatch("Explode")]
            [HarmonyPrefix]
            static void Fliching(Grenade __instance)
            {
                __instance.explosion = ReplaceID(__instance.explosion);
            }
        }

        // this one is neccessary for charges, and can also overwrite effects from other things id assume
        [HarmonyPatch]
        internal static class FlinchingPatchesEffects
        {
            [HarmonyPatch(typeof(EffectManager))]
            [HarmonyPatch("sendEffect")]
            [HarmonyPatch(new Type[] { typeof(ushort), typeof(float), typeof(Vector3) })]
            [HarmonyPrefix]
            static void Fliching(ref ushort id)
            {
                id = ReplaceID(id);
            }
        }

        // finally, one for rocket launchers/tank cannons
        [HarmonyPatch]
        internal static class FlinchingPatchesRockets
        {
            [HarmonyPatch(typeof(SDG.Unturned.Rocket))]
            [HarmonyPatch("OnTriggerEnter")]
            [HarmonyPrefix]
            static void Fliching(SDG.Unturned.Rocket __instance)
            {
                __instance.explosion = ReplaceID(__instance.explosion);
            }
        }

        // not finally, traps and explosive bullets
        [HarmonyPatch]
        internal static class FlinchingPatchesTraps
        {
            [HarmonyPatch(typeof(EffectManager))]
            [HarmonyPatch("triggerEffect")]
            [HarmonyPrefix]
            static void Fliching(ref TriggerEffectParameters parameters)
            {
                if (parameters.asset == null) return;
                var pos = parameters.position;
                var distance = parameters.relevantDistance;
                var dir = parameters.direction;
                var id = parameters.relevantPlayerID;
                var reliable = parameters.reliable;
                var scale = parameters.scale;
                var replicate = parameters.shouldReplicate;
                var instigated = parameters.wasInstigatedByPlayer;
                parameters = new TriggerEffectParameters(ReplaceID(parameters.asset.id))
                {
                    position = pos,
                    relevantDistance = distance,
                    direction = dir,
                    relevantPlayerID = id,
                    reliable = reliable,
                    scale = scale,
                    shouldReplicate = replicate,
                    wasInstigatedByPlayer = instigated
                };
            }
        }

        public static ushort ReplaceID(ushort id)
        {
            if (MNoFlinchNoWobble.Instance.EffectRep.ContainsKey(id))
            {
                return MNoFlinchNoWobble.Instance.EffectRep[id];
            }
            return id;
        }
    }
}
