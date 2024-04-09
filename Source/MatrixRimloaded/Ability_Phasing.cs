using RimWorld;
using RimWorld.Planet;
using VanillaPsycastsExpanded;
using Verse;
using Verse.AI;

namespace MatrixRimloaded;

public class Ability_Phasing : Ability_TargetCorpse
{
    public override void Cast(params GlobalTargetInfo[] targets)
    {
        base.Cast(targets);
        foreach (var target in targets)
        {
            var thing = target.Thing as Corpse;
            Resurrect(thing);
        }
    }

    public override void WarmupToil(Toil toil)
    {
        toil.WithEffect(USH_DefOf.USH_HackingBody, TargetIndex.A);
    }

    protected void Resurrect(Corpse target)
    {
        var innerPawn = target.InnerPawn;
        var hediffs = innerPawn.health.hediffSet.hediffs;
        for (var index = hediffs.Count - 1; index >= 0; --index)
        {
            var localHediff = hediffs[index];
            if (localHediff is Hediff_MissingPart hediffMissingPart)
            {
                var part = hediffMissingPart.Part;
                innerPawn.health.RemoveHediff(localHediff);
                innerPawn.health.RestorePart(part);
                continue;
            }

            if (localHediff.def != VPE_DefOf.TraumaSavant &&
                (localHediff.def.isBad || localHediff is Hediff_Addiction) &&
                localHediff.def.everCurableByItem)
            {
                innerPawn.health.RemoveHediff(localHediff);
            }
        }

        ResurrectionUtility.TryResurrectWithSideEffects(innerPawn);
        if (!innerPawn.Spawned)
        {
            GenSpawn.Spawn(innerPawn, target.Position, target.Map);
        }
    }
}