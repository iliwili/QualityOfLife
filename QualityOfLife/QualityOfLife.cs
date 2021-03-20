using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System.IO;
using System.Reflection;

namespace QualityOfLife
{
    [BepInPlugin("iliwili.quality_of_life", "Quality of Life", "1.0.0.0")]
    [HarmonyPatch]
    public class QualityOfLife : BaseUnityPlugin
    {
        public static ManualLogSource Log;
        private Harmony Harmony;
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
        public static ConfigEntry<int> BlastFurnaceFuelPerProduct;
        public static ConfigEntry<int> BlastFurnaceSecPerProduct;

        // [SMELTER.CHARCOALKILN]
        public static ConfigEntry<int> CharcoalKilnMultiplier;
        public static ConfigEntry<int> CharcoalKilnSecPerProduct;

        // [TELEPORT]
        public static ConfigEntry<bool> EnableTeleportOres;


        public static List<Character> Wolves = new List<Character>();
        public QualityOfLife()
        {
            Log = this.Logger;
            BepInEx.Logging.Logger.Sources.Add(Log);

            this.Harmony = new Harmony("iliwili.quality_of_life");

            Harmony.CreateAndPatchAll(typeof(SmelterMod));
            Harmony.CreateAndPatchAll(typeof(Teleport));

            Log.LogInfo("QualityOfLife loaded.");
        }

        void Awake()
        {
            EnableSmelterModification = this.Config.Bind<bool>("SMELTER", "EnableSmelterModification", true, "Enable smelter modification");

            SmelterOreMultiplier = this.Config.Bind<int>("SMELTER.SMELTER", "SmelterOreMultiplier", 2, "Smelter ore multiplier");
            SmelterFuelMultiplier = this.Config.Bind<int>("SMELTER.SMELTER", "SmelterFuelMultiplier", 2, "Smelter fuel multiplier");
            SmelterFuelPerProduct = this.Config.Bind<int>("SMELTER.SMELTER", "SmelterFuelPerProduct", 1, "Smelter fuel per burnt ore (lowest is 1 but it will still take 2 fuel per product, don't actualy know why it does that.)");
            SmelterSecPerProduct = this.Config.Bind<int>("SMELTER.SMELTER", "SmelterSecPerProduct", 1, "Smelter processing time per ore (lowest is 1)");

            BlastFurnaceOreMultiplier = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnaceOreMultiplier", 2, "Blast Furnace ore multiplier");
            BlastFurnaceFuelMultiplier = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnaceFuelMultiplier", 2, "Blast Furnace fuel multiplier");
            BlastFurnaceFuelPerProduct = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnacePerProduct", 1, "Blast furnace fuel per burnt ore (lowest is 1 but it will still take 2 fuel per product, don't actualy know why it does that.)");
            BlastFurnaceSecPerProduct = this.Config.Bind<int>("SMELTER.BLASTFURNACE", "BlastFurnaceSecPerProduct", 1, "Blast furnace processing time per ore (lowest is 1)");

            CharcoalKilnMultiplier = this.Config.Bind<int>("SMELTER.CHARCOALKILN", "CharcoalKilnMultiplier", 2, "Charcoal Kiln multiplier");
            CharcoalKilnSecPerProduct = this.Config.Bind<int>("SMELTER.CHARCOALKILN", "CharcoalKilnSecPerProduct", 1, "Charcoal Kiln processing time per wood (lowest is 1 but it will still take more than 1 second, don't actualy know why it does that.)");

            EnableTeleportOres = this.Config.Bind<bool>("TELEPORT", "EnableTeleportOres", true, "Enable/Disable the option to teleport with ores.");
        }
    }
}
