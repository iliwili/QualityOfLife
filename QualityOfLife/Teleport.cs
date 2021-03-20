using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

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


        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "TeleportTo")]
        public static void TeleportTo(ref Player __instance)
        {
            QualityOfLife.Wolves.Clear();

            List<Character> characters = new List<Character>();
            Character.GetCharactersInRange(__instance.transform.position, (float) 20.0, characters);

            foreach (Character character in characters)
            {
                if (character.IsTamed() || character.name.Contains("Wolf_cub"))
                {
                    MonsterAI component = character.GetComponent<MonsterAI>();
                    if (component.GetFollowTarget() && component.GetFollowTarget().Equals((object) __instance.gameObject))
                    {
                        QualityOfLife.Wolves.Add(character);
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "UpdateTeleport")]
        public static void UpdateTeleport(ref bool ___m_teleporting, ref float ___m_teleportTimer, ref Vector3 ___m_teleportTargetPos, ref Quaternion ___m_teleportTargetRot)
        {
            if (!___m_teleporting || (double)___m_teleportTimer <= 2.0)
                return;

            foreach (Character character in QualityOfLife.Wolves)
            {
                Vector2 vector2 = new Vector2(1f, 0.0f) + Random.insideUnitCircle;
                Vector3 vector3 = new Vector3(vector2.x, vector2.y, 0.5f);
                Vector3 dir = ___m_teleportTargetRot * Vector3.forward;
                character.transform.position = ___m_teleportTargetPos + vector3;
                character.transform.rotation = ___m_teleportTargetRot;
                character.SetLookDir(dir);
            }
        }
    }
}
