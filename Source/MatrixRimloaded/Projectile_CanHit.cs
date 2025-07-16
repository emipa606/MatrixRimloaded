using HarmonyLib;
using RimWorld;
using Verse;

namespace MatrixRimloaded.Patch;

[HarmonyPatch(typeof(Projectile), "CanHit")]
public static class Projectile_CanHit
{
    public static bool Prefix(Bullet __instance)
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