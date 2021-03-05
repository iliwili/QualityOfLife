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
        private static ManualLogSource Log;
        // [SMELTER]
        public static ConfigEntry<bool> EnableSmelterModification;
        // [SMELTER.SMELTER]]
        public static ConfigEntry<int> SmelterOreMultiplier;
        public static ConfigEntry<int> SmelterFuelMultiplier;
        
        // [SMELTER.BLASTFURNACE]
        public static ConfigEntry<int> BlastFurnaceOreMultiplier;
        public static ConfigEntry<int> BlastFurnaceFuelMultiplier;

        // [SMELTER.CHARCOALKILN]
        public static ConfigEntry<int> CharcoalKilnMultiplier;

        public QualityOfLife()
        {
            Log = this.Logger;
            BepInEx.Logging.Logger.Sources.Add(Log);

            new Harmony("iliwili.quality_of_life").PatchAll();
            Log.LogInfo("QualityOfLife loaded.");
        }

        void Awake()
        {
            EnableSmelterModification = this.Config.Bind<bool>("SMELTER", "EnableSmelterModification", true, "Enable smelter modification");

            SmelterOreMultiplier = this.Config.Bind<int>("SMELTER.SMELTER", "SmelterOreMultiplier", 2, "Smelter ore multiplier");
            SmelterFuelMultiplier = this.Config.Bind<int>("SMELTER.SMELTER", "SmelterFuelMultiplier", 2, "Smelter fuel multiplier");

            BlastFurnaceOreMultiplier = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnaceOreMultiplier", 2, "Blast Furnace ore multiplier");
            BlastFurnaceFuelMultiplier = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnaceFuelMultiplier", 2, "Blast Furnace fuel multiplier");

            CharcoalKilnMultiplier = this.Config.Bind<int>("SMELTER.CHARCOALKILN", "CharcoalKilnMultiplier", 2, "Charcoal Kiln multiplier");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Smelter), "Awake")]
        public static void StartSmelter(ref Smelter __instance)
        {
            if (EnableSmelterModification.Value)
            {
                switch (__instance.m_name)
                {
                    case "$piece_smelter":
                        __instance.m_maxOre *= SmelterOreMultiplier.Value;
                        __instance.m_maxFuel *= SmelterFuelMultiplier.Value;
                        break;
                    case "$piece_blastfurnace":
                        __instance.m_maxOre *= BlastFurnaceOreMultiplier.Value;
                        __instance.m_maxFuel *= BlastFurnaceFuelMultiplier.Value;
                        break;
                    case "$piece_charcoalkiln":
                        __instance.m_maxOre *= CharcoalKilnMultiplier.Value;
                        break;
                    default:
                        break;
                }
            } else
            {
                Log.LogInfo("Smelter mod is disabled.");
            }
        }
    }
}
