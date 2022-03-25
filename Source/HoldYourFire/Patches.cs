using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using Verse;

namespace HoldYourFire;

public class Patches
{
    public static string[] defsNotToAutofire =
    {
        "Gun_IncendiaryLauncher",
        "Gun_SmokeLauncher",
        "Gun_EmpLauncher",
        "Gun_DoomsdayRocket",
        "Gun_TripleRocket",
        "Weapon_GrenadeMolotov",
        "Weapon_GrenadeEMP",
        "Weapon_GrenadeFrag"
    };

    [StaticConstructorOnStartup]
    private class Main
    {
        static Main()
        {
            //Log.Message("Hello from Harmony in scope: com.github.harmony.rimworld.maarx.holdyourfire");
            var harmony = new Harmony("com.github.harmony.rimworld.maarx.holdyourfire");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    [HarmonyPatch(typeof(Pawn_DraftController))]
    [HarmonyPatch("Drafted", MethodType.Setter)]
    private class Patch_Pawn_DraftController_Drafted
    {
        private static void Postfix(Pawn_DraftController __instance)
        {
            //Lazy safe-nav, might not have a primary weapon equipped at all.
            var weaponDefName = __instance.pawn.equipment.Primary?.def?.defName;
            if (weaponDefName != null && defsNotToAutofire.Contains(weaponDefName))
            {
                __instance.FireAtWill = false;
            }
        }
    }

    [HarmonyPatch(typeof(Pawn_DraftController))]
    [HarmonyPatch("Notify_PrimaryWeaponChanged")]
    private class Patch_Pawn_DraftController_Notify_PrimaryWeaponChanged
    {
        private static void Postfix(Pawn_DraftController __instance)
        {
            //Lazy safe-nav, might not have a primary weapon equipped at all.
            var weaponDefName = __instance.pawn.equipment.Primary?.def?.defName;
            if (weaponDefName != null && defsNotToAutofire.Contains(weaponDefName))
            {
                __instance.FireAtWill = false;
            }
        }
    }
}