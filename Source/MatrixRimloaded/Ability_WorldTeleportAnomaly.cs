using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using VanillaPsycastsExpanded.Skipmaster;
using Verse;
using Verse.AI;
using Verse.Sound;
using Ability = VFECore.Abilities.Ability;

namespace MatrixRimloaded;

public class Ability_WorldTeleportAnomaly : Ability
{
    public override void DoAction()
    {
        var localPawn = PawnsToSkip().FirstOrDefault(p => p.IsQuestLodger());
        if (localPawn != null)
        {
            Dialog_MessageBox.CreateConfirmation("FarskipConfirmTeleportingLodger".Translate(localPawn.Named("PAWN")),
                base.DoAction);
        }
        else
        {
            base.DoAction();
        }
    }

    private IEnumerable<Pawn> PawnsToSkip()
    {
        var caravan = pawn.GetCaravan();
        if (caravan != null)
        {
            foreach (var pawnsToSkip in caravan.pawns)
            {
                yield return pawnsToSkip;
            }
        }
        else
        {
            var homeMap = pawn.Map.IsPlayerHome;
            foreach (var thing in GenRadial
                         .RadialDistinctThingsAround(pawn.Position, pawn.Map, GetRadiusForPawn(), true))
            {
                if (thing is Pawn { Dead: false } pawnsToSkip &&
                    (pawnsToSkip.IsColonist || pawnsToSkip.IsPrisonerOfColony ||
                     !homeMap && pawnsToSkip.RaceProps.Animal && pawnsToSkip.Faction is
                         { IsPlayer: true }))
                {
                    yield return pawnsToSkip;
                }
            }
        }
    }

    private Pawn AlliedPawnOnMap(Map targetMap)
    {
        return targetMap.mapPawns.AllPawnsSpawned.FirstOrDefault(p => !p.NonHumanlikeOrWildMan() && p.IsColonist &&
                                                                      p.HomeFaction == Faction.OfPlayer &&
                                                                      !PawnsToSkip().Contains(p));
    }

    private bool ShouldEnterMap(GlobalTargetInfo target)
    {
        if (target.WorldObject is Caravan caravan && caravan.Faction == pawn.Faction)
        {
            return false;
        }

        return target.WorldObject is MapParent { HasMap: true } mapParent &&
               (AlliedPawnOnMap(mapParent.Map) != null || mapParent.Map == pawn.Map);
    }

    public override bool CanHitTargetTile(GlobalTargetInfo target)
    {
        var distanceBetweenTargets = Find.WorldGrid.TraversalDistanceBetween(
            CasterPawn.GetCaravan() != null ? CasterPawn.GetCaravan().Tile : Caster.Map.Tile, target.Tile);
        return distanceBetweenTargets < GetRangeForPawn() + 1 && distanceBetweenTargets > -1;
    }

    public override bool IsEnabledForPawn(out string reason)
    {
        if (!base.IsEnabledForPawn(out reason))
        {
            return false;
        }

        if (pawn.GetCaravan() is not { ImmobilizedByMass: true })
        {
            return true;
        }

        reason = "CaravanImmobilizedByMass".Translate();
        return false;
    }

    public override void WarmupToil(Toil toil)
    {
        base.WarmupToil(toil);
        toil.AddPreTickAction(delegate
        {
            if (pawn.jobs.curDriver.ticksLeftThisToil != 5)
            {
                return;
            }

            foreach (var p in PawnsToSkip())
            {
                if (p == CasterPawn)
                {
                    MakeStaticFleck(CasterPawn.DrawPos, CasterPawn.Map, USH_DefOf.USH_PsychicFlightFleck,
                        def.castFleckScaleWithRadius ? GetRadiusForPawn() : def.castFleckScale, def.castFleckSpeed);
                }

                var unused = Caster.Map;
                FleckMaker.ThrowSmoke(p.DrawPos, p.Map, 1f);
                FleckMaker.ThrowDustPuffThick(p.DrawPos, p.Map, 2f, new Color(1f, 1f, 1f, 2.5f));
            }
        });
    }

    public override void Cast(params GlobalTargetInfo[] targets)
    {
        var caravan = pawn.GetCaravan();
        var targetMap = targets[0].WorldObject is MapParent mapParent ? mapParent.Map : null;
        var targetCell = IntVec3.Invalid;
        var list = PawnsToSkip().ToList();
        if (pawn.Spawned)
        {
            SoundDefOf.Psycast_Skip_Pulse.PlayOneShot(new TargetInfo(targets[0].Cell, pawn.Map));
        }

        if (targetMap != null && AlliedPawnOnMap(targetMap) is { Position: var alliedPawnCell })
        {
            targetCell = alliedPawnCell;
        }

        var unused = def.GetModExtension<AbilityExtension_Clamor>();
        if (targetCell.IsValid)
        {
            foreach (var pawn3 in list)
            {
                if (pawn3.Spawned)
                {
                    pawn3.teleporting = true;
                    pawn3.ExitMap(false, Rot4.Invalid);
                    pawn3.teleporting = false;
                }

                CellFinder.TryFindRandomSpawnCellForPawnNear(targetCell, targetMap, out var intVec, 4, cell =>
                    cell != targetCell && cell.GetRoom(targetMap) == targetCell.GetRoom(targetMap));
                GenSpawn.Spawn(pawn3, intVec, targetMap);
                if (pawn3.drafter != null && pawn3.IsColonistPlayerControlled)
                {
                    pawn3.drafter.Drafted = true;
                }

                pawn3.Notify_Teleported();
                if (pawn3.IsPrisoner)
                {
                    pawn3.guest.WaitInsteadOfEscapingForDefaultTicks();
                }

                FleckMaker.ThrowSmoke(pawn3.DrawPos, pawn3.Map, 1f);
                FleckMaker.ThrowDustPuffThick(pawn3.DrawPos, pawn3.Map, 2f, new Color(1f, 1f, 1f, 2.5f));
                if ((pawn3.IsColonist || pawn3.RaceProps.packAnimal) && pawn3.Map.IsPlayerHome)
                {
                    pawn3.inventory.UnloadEverything = true;
                }
            }

            if (Find.WorldSelector.IsSelected(caravan))
            {
                Find.WorldSelector.Deselect(caravan);
                CameraJumper.TryJump(targetCell, targetMap);
            }

            caravan?.Destroy();
        }
        else if (targets[0].WorldObject is Caravan caravan2 && caravan2.Faction == pawn.Faction)
        {
            if (caravan != null)
            {
                caravan.pawns.TryTransferAllToContainer(caravan2.pawns);
                caravan2.Notify_Merged(new List<Caravan>
                {
                    caravan
                });
                caravan.Destroy();
            }
            else
            {
                foreach (var pawn4 in list)
                {
                    caravan2.AddPawn(pawn4, true);
                    pawn4.ExitMap(false, Rot4.Invalid);
                }
            }
        }
        else if (caravan != null)
        {
            caravan.Tile = targets[0].Tile;
            caravan.pather.StopDead();
        }
        else
        {
            CaravanMaker.MakeCaravan(list, pawn.Faction, targets[0].Tile, false);
            foreach (var pawn5 in list)
            {
                pawn5.ExitMap(false, Rot4.Invalid);
            }
        }


        base.Cast(targets);
    }

    public override void GizmoUpdateOnMouseover()
    {
        var radiusForPawn = GetRadiusForPawn();
        GenDraw.DrawRadiusRing(pawn.Position, radiusForPawn, def.radiusRingColor);
    }
}