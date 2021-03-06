using HarmonyLib;

namespace QualityOfLife
{
    class SmelterMod
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Smelter), "Awake")]
        public static void StartSmelter(ref Smelter __instance)
        {
            if (QualityOfLife.EnableSmelterModification.Value)
            {
                QualityOfLife.Log.LogInfo(__instance.m_name);
                switch (__instance.m_name)
                {
                    case "$piece_smelter":
                        __instance.m_maxOre *= QualityOfLife.SmelterOreMultiplier.Value;
                        __instance.m_maxFuel *= QualityOfLife.SmelterFuelMultiplier.Value;

                        if (QualityOfLife.SmelterFuelPerProduct.Value >= 1 && QualityOfLife.SmelterSecPerProduct.Value >= 1)
                        {
                            __instance.m_fuelPerProduct = QualityOfLife.SmelterFuelPerProduct.Value;
                            __instance.m_secPerProduct = QualityOfLife.SmelterSecPerProduct.Value;
                        }
                        else
                        {
                            QualityOfLife.Log.LogInfo("Smelter fuel per product or smelter sec per product is too low, lowest is 1.");
                        }
                        break;
                    case "$piece_blastfurnace":
                        __instance.m_maxOre *= QualityOfLife.BlastFurnaceOreMultiplier.Value;
                        __instance.m_maxFuel *= QualityOfLife.BlastFurnaceFuelMultiplier.Value;

                        if (QualityOfLife.BlastFurnaceFuelPerProduct.Value >= 1 && QualityOfLife.BlastFurnaceSecPerProduct.Value >= 1)
                        {
                            __instance.m_fuelPerProduct = QualityOfLife.BlastFurnaceFuelPerProduct.Value;
                            __instance.m_secPerProduct = QualityOfLife.BlastFurnaceSecPerProduct.Value;
                        }
                        else
                        {
                            QualityOfLife.Log.LogInfo("Blast Furnace fuel per product or smelter sec per product is too low, lowest is 1.");
                        }
                        break;
                    case "$piece_charcoalkiln":
                        __instance.m_maxOre *= QualityOfLife.CharcoalKilnMultiplier.Value;

                        if (QualityOfLife.CharcoalKilnSecPerProduct.Value >= 1)
                        {
                            __instance.m_secPerProduct = QualityOfLife.CharcoalKilnSecPerProduct.Value;
                        }
                        else
                        {
                            QualityOfLife.Log.LogInfo("Charcoal Kiln sec per product is too low, lowest is 1.");
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                QualityOfLife.Log.LogInfo("Smelter mod is disabled.");
            }
        }
    }
}
