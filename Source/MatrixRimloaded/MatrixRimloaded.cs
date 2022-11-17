using HarmonyLib;
using Verse;

namespace MatrixRimloaded.Patch;

[StaticConstructorOnStartup]
public static class MatrixRimloaded
{
    static MatrixRimloaded()
    {
        Log.Message("Matrix rimloading... loaded.");

        var harmony = new Harmony("rimworld.mod.ushankas.matrix");
        harmony.PatchAll();
    }
}