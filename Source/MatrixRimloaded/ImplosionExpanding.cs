using Verse;

namespace MatrixRimloaded;

public class ImplosionExpanding : Thing
{
    public override void PostMake()
    {
        base.PostMake();
        var distortion = (Mote)ThingMaker.MakeThing(ThingDef.Named("DistortionBubble"));
        GenSpawn.Spawn(distortion, Position, Map);
        Destroy();
    }
}