using HarmonyLib;
using RimWorld;
using Verse;

namespace MatrixRimloaded.Patch;

[HarmonyPatch]
public static class Projectile_Patch
{
    [HarmonyPatch(typeof(Projectile), "CanHit")]
    [HarmonyPrefix]
    public static bool CanHit_Prefix(Bullet __instance)
    {
        if (__instance.intendedTarget == null)
        {
            return true;
        }

        if (__instance.intendedTarget.Pawn == null)
        {
            return true;
        }

        return !__instance.intendedTarget.Pawn.health.hediffSet.HasHediff(USH_DefOf.USH_BulletDodge);
    }
}