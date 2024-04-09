using HarmonyLib;
using Verse;

namespace MatrixRimloaded.Patch;

[StaticConstructorOnStartup]
public static class MatrixRimloaded
{
    static MatrixRimloaded()
    {
        Log.Message("Matrix rimloading... loaded.");

        new Harmony("rimworld.mod.ushankas.matrix").PatchAll();
    }
}