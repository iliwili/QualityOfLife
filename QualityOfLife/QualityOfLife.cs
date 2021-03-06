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
        public static ConfigEntry<int> SmelterFuelPerProduct;
        public static ConfigEntry<int> SmelterSecPerProduct;
        
        // [SMELTER.BLASTFURNACE]
        public static ConfigEntry<int> BlastFurnaceOreMultiplier;
        public static ConfigEntry<int> BlastFurnaceFuelMultiplier;
        public static ConfigEntry<int> BlastFurnacePerProduct;
        public static ConfigEntry<int> BlastFurnaceSecPerProduct;

        // [SMELTER.CHARCOALKILN]
        public static ConfigEntry<int> CharcoalKilnMultiplier;
        public static ConfigEntry<int> CharcoalKilnSecPerProduct;

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
            // TODO: This doesn't work in the config file
            SmelterFuelPerProduct = this.Config.Bind<int>("SMELTER.SMELTER", "SmelterFuelPerProduct", 2, "Smelter fuel per burnt ore (lowest is 1)");
            SmelterSecPerProduct = this.Config.Bind<int>("SMELTER.SMELTER", "SmelterSecPerProduct", 5, "Smelter processing time per ore (lowest is 1)");

            BlastFurnaceOreMultiplier = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnaceOreMultiplier", 2, "Blast Furnace ore multiplier");
            BlastFurnaceFuelMultiplier = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnaceFuelMultiplier", 2, "Blast Furnace fuel multiplier");
            BlastFurnacePerProduct = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnacePerProduct", 2, "Blast furnace fuel per burnt ore (lowest is 1)");
            BlastFurnaceSecPerProduct = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnaceSecPerProduct", 5, "Blast furnace processing time per ore (lowest is 1)");

            CharcoalKilnMultiplier = this.Config.Bind<int>("SMELTER.CHARCOALKILN", "CharcoalKilnMultiplier", 2, "Charcoal Kiln multiplier");
            CharcoalKilnSecPerProduct = this.Config.Bind<int>("SMELTER.CHARCOALKILN", "CharcoalKilnSecPerProduct", 2, "Charcoal Kiln processing time per wood (lowest is 1)");
        }

        // TODO: Fix a seperate class for the smelters function
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

                        if (SmelterFuelPerProduct.Value > 1 && SmelterSecPerProduct.Value > 1)
                        {
                            __instance.m_fuelPerProduct = SmelterFuelPerProduct.Value;
                            __instance.m_secPerProduct = SmelterSecPerProduct.Value;
                        } else
                        {
                            Log.LogInfo("Smelter fuel per product or smelter sec per product is too low, lowest is 1.");
                        }
                        break;
                    case "$piece_blastfurnace":
                        __instance.m_maxOre *= BlastFurnaceOreMultiplier.Value;
                        __instance.m_maxFuel *= BlastFurnaceFuelMultiplier.Value;

                        if (BlastFurnacePerProduct.Value > 1 && BlastFurnaceSecPerProduct.Value > 1)
                        {
                            __instance.m_fuelPerProduct = BlastFurnacePerProduct.Value;
                            __instance.m_secPerProduct = BlastFurnaceSecPerProduct.Value;
                        }
                        else
                        {
                            Log.LogInfo("Blast Furnace fuel per product or smelter sec per product is too low, lowest is 1.");
                        }
                        break;
                    case "$piece_charcoalkiln":
                        __instance.m_maxOre *= CharcoalKilnMultiplier.Value;

                        if (CharcoalKilnSecPerProduct.Value > 1)
                        {
                            __instance.m_secPerProduct = CharcoalKilnSecPerProduct.Value;
                        }
                        else
                        {
                            Log.LogInfo("Charcoal Kiln sec per product is too low, lowest is 1.");
                        }
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
