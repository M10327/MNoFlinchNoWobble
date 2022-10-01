﻿using HarmonyLib;
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
    public class MNoFlinchNoWobble : RocketPlugin
    {
        protected override void Load()
        {
            DamageTool.damagePlayerRequested += DamageTool_damagePlayerRequested;
            Patches.PatchAll();
        }

        private void DamageTool_damagePlayerRequested(ref DamagePlayerParameters par, ref bool shouldAllow)
        {
            try
            {
                if (par.player.life.health - par.damage > 0)
                {
                    par.direction = new UnityEngine.Vector3(0, 0, 0);
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
            Patches.UnpatchAll();
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

        public static ushort ReplaceID(ushort id)
        {
            switch (id)
            {
                case 20:
                    return 18530;
                case 34:
                    return 18531;
                case 45:
                    return 18532;
                case 53:
                    return 18533;
                case 54:
                    return 18534;
                case 123:
                    return 18535;
                case 136:
                    return 18536;
                default:
                    return id;
            }
        }
    }
}