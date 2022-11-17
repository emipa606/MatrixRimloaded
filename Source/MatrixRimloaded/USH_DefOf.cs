using RimWorld;
using Verse;

namespace MatrixRimloaded;

[DefOf]
public static class USH_DefOf
{
    public static ThingDef USH_JumpingPawn;

    public static SoundDef USH_JumpOfFaith_Land;

    public static FleckDef USH_JumpOfFaithFleck;
    public static FleckDef USH_PsychicFlightFleck;

    public static EffecterDef USH_HackingBody;

    public static HediffDef USH_BulletDodge;


    static USH_DefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(USH_DefOf));
    }
}