using System;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;

namespace QualityOfLife
{
    [BepInPlugin("iliwili.quality_of_life", "Quality of Life", "1.0.0.0")]
    [HarmonyPatch]
    public class QualityOfLife : BaseUnityPlugin
    {
        private static ManualLogSource log;
        private static ConfigFile config;

        public QualityOfLife()
        {
            log = this.Logger;
            config = this.Config;
            BepInEx.Logging.Logger.Sources.Add(log);

            new Harmony("iliwili.quality_of_life").PatchAll();
            log.LogInfo("QualityOfLife loaded.");
        }

        void Awake()
        {
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Smelter), "Awake")]
        public static void StartSmelter(ref Smelter __instance)
        {
            log.LogInfo(__instance.m_name);
            __instance.m_maxOre *= 10;
            __instance.m_maxFuel *= 4;
        }
    }
}
