using RimWorld.Planet;
using Verse;
using VFECore.Abilities;

namespace MatrixRimloaded;

public class Ability_Implode : Ability
{
    public override void Cast(params GlobalTargetInfo[] targets)
    {
        base.Cast(targets);
        foreach (var target in targets)
        {
            var implosion = ThingMaker.MakeThing(ThingDef.Named("USH_ImplosionExpanding"));
            GenSpawn.Spawn(implosion, target.Cell, pawn.Map);
        }
    }
}