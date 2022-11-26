using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZeroPointModule
{
    [HarmonyPatch(typeof(GeneratedBuildings))]
    [HarmonyPatch(nameof(GeneratedBuildings.LoadGeneratedBuildings))]
    public class GeneratedBuildings_LoadGeneratedBuildings_Patch
    {
        public static void Prefix()
        {
            Debug.Log(" === ZeroPointModuleMod_GeneratedBuildings_LoadGeneratedBuildings Prefix === ");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.ZEROPOINTMODULE.NAME", "ZP Module");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.ZEROPOINTMODULE.DESC", "Zero Point Energy Module. The energy of the vacuum feeds this battery.");
            Strings.Add("STRINGS.BUILDINGS.PREFABS.ZEROPOINTMODULE.EFFECT", "An inexhaustible energy source");
            ModUtil.AddBuildingToPlanScreen("Power", ZeroPointModuleConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Db))]
    [HarmonyPatch(nameof(Db.Initialize))]
    public class Db_Initialize_Patch
    {
        public static void Postfix()
        {
            Db.Get().Techs.Get("RenewableEnergy").unlockedItemIDs.Add(ZeroPointModuleConfig.ID);
        }
    }

    [HarmonyPatch(typeof(Battery), "ConsumeEnergy", new Type[] { typeof(float), typeof(bool) })]
    internal class ZeroPointModuleMod_Battery_ConsumeEnergy
    {
        private static bool Prefix(Battery __instance)
        {
            //Debug.Log(" ===ZeroPointModuleMod_Battery_ConsumeEnergy loaded === ");

            if (__instance.gameObject.GetComponent<KPrefabID>().PrefabTag == ZeroPointModuleConfig.ID)
            {
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Battery), "OnSpawn")]
    internal class ZeroPointModuleMod_Battery_OnSpawn
    {
        private static void Prefix(Battery __instance)
        {
            Debug.Log(" ===ZeroPointModuleMod_Battery_OnSpawn loaded === ");

            if (__instance.gameObject.GetComponent<KPrefabID>().PrefabTag == ZeroPointModuleConfig.ID)
            {
                AccessTools.Field(typeof(Battery), "joulesAvailable").SetValue(__instance, 40000f);
            }

        }
    }
}
