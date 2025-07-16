using RimWorld;
using UnityEngine;
using VEF.Abilities;
using Verse;
using Verse.Sound;

namespace MatrixRimloaded;

public class USH_JumpingPawn : AbilityPawnFlyer
{
    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        //FlyingPawn.DrawNowAt(GetDrawPos(drawLoc), flip);
    }

    private Vector3 GetDrawPos(Vector3 drawLoc)
    {
        var drawPos = drawLoc;
        var x = ticksFlying / (float)ticksFlightTime;
        drawPos.y = AltitudeLayer.Skyfaller.AltitudeFor();
        return drawPos + (Vector3.forward * (x - Mathf.Pow(x, 2)) * 15f);
    }

    protected override void RespawnPawn()
    {
        var flyingPawn = FlyingPawn;
        base.RespawnPawn();
        USH_DefOf.USH_JumpOfFaith_Land.PlayOneShot(flyingPawn);
        FleckMaker.ThrowSmoke(flyingPawn.DrawPos, flyingPawn.Map, 1f);
        FleckMaker.ThrowDustPuffThick(flyingPawn.DrawPos, flyingPawn.Map, 2f, new Color(1f, 1f, 1f, 2.5f));
    }
}