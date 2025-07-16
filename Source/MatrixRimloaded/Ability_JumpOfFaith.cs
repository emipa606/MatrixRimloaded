using RimWorld;
using RimWorld.Planet;
using Verse;
using Ability = VEF.Abilities.Ability;

namespace MatrixRimloaded;

public class Ability_JumpOfFaith : Ability
{
    public override void Cast(params GlobalTargetInfo[] targets)
    {
        MakeStaticFleck(pawn.DrawPos, pawn.Map, USH_DefOf.USH_JumpOfFaithFleck,
            def.castFleckScaleWithRadius ? GetRadiusForPawn() : def.castFleckScale, def.castFleckSpeed);
        var map = Caster.Map;
        var flyer = (USH_JumpingPawn)PawnFlyer.MakeFlyer(USH_DefOf.USH_JumpingPawn, CasterPawn, targets[0].Cell,
            null, null, true, target: targets[0].Cell);
        flyer.ability = this;
        GenSpawn.Spawn(flyer, Caster.Position, map);
        base.Cast(targets);
    }
}