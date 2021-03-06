using HarmonyLib;

namespace QualityOfLife
{
    class Teleport
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ItemDrop), "Awake")]
        public static void TeleportOres(ref ItemDrop __instance)
        {
            if (QualityOfLife.EnableTeleportOres.Value)
            {
                __instance.m_itemData.m_shared.m_teleportable = true;
            }
        }
    }
}
