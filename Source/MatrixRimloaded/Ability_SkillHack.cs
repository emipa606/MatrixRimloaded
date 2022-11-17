﻿using RimWorld;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Ability = VFECore.Abilities.Ability;

namespace MatrixRimloaded;

public class Ability_SkillHack : Ability
{
    public SkillDef skillToLearn;

    public override void DoAction()
    {
        Find.WindowStack.Add(new Dialog_ChooseMatrixSkill(this));
        base.DoAction();
    }

    public override void Cast(params GlobalTargetInfo[] targets)
    {
        base.Cast(targets);
        if (skillToLearn == null)
        {
            pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
            Messages.Message("USH_NoSkillChosen".Translate(), CasterPawn, MessageTypeDefOf.NeutralEvent, false);
        }

        foreach (var target in targets)
        {
            var targetP = target.Thing as Pawn;
            targetP?.skills.GetSkill(skillToLearn).Learn(35000f, true);
        }
    }

    public override void WarmupToil(Toil toil)
    {
        toil.WithEffect(() => USH_DefOf.USH_HackingBody,
            () => firstTarget.Pawn.OccupiedRect().ClosestCellTo(CasterPawn.Position));
    }
}