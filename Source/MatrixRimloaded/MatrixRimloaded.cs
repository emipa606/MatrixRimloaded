using System.Reflection;
using HarmonyLib;
using Verse;

namespace MatrixRimloaded.Patch;

[StaticConstructorOnStartup]
public static class MatrixRimloaded
{
    static MatrixRimloaded()
    {
        new Harmony("rimworld.mod.ushankas.matrix").PatchAll(Assembly.GetExecutingAssembly());
    }
}