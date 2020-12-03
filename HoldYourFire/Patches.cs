using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace HoldYourFire
{
    public class Patches
    {
        [StaticConstructorOnStartup]
        class Main
        {
            static Main()
            {
                //Log.Message("Hello from Harmony in scope: com.github.harmony.rimworld.maarx.holdyourfire");
                var harmony = new Harmony("com.github.harmony.rimworld.maarx.holdyourfire");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
        }

        public static string[] defsNotToAutofire = new string[] {
            "Gun_IncendiaryLauncher",
            "Gun_SmokeLauncher",
            "Gun_EmpLauncher",
            "Gun_DoomsdayRocket",
            "Gun_TripleRocket",
            "Weapon_GrenadeMolotov",
            "Weapon_GrenadeEMP",
            "Weapon_GrenadeFrag"
        };

        [HarmonyPatch(typeof(Pawn_DraftController))]
        [HarmonyPatch("Drafted", MethodType.Setter)]
        class Patch_Pawn_DraftController_Drafted
        {
            static void Postfix(Pawn_DraftController __instance)
            {
                //Lazy safe-nav, might not have a primary weapon equipped at all.
                String weaponDefName = __instance.pawn.equipment.Primary?.def?.defName;
                if (weaponDefName != null && defsNotToAutofire.Contains(weaponDefName)) {
                    __instance.FireAtWill = false;
                }
            }
        }

        [HarmonyPatch(typeof(Pawn_DraftController))]
        [HarmonyPatch("Notify_PrimaryWeaponChanged")]
        class Patch_Pawn_DraftController_Notify_PrimaryWeaponChanged
        {
            static void Postfix(Pawn_DraftController __instance)
            {
                //Lazy safe-nav, might not have a primary weapon equipped at all.
                String weaponDefName = __instance.pawn.equipment.Primary?.def?.defName;
                if (weaponDefName != null && defsNotToAutofire.Contains(weaponDefName))
                {
                    __instance.FireAtWill = false;
                }
            }
        }
    }
}
