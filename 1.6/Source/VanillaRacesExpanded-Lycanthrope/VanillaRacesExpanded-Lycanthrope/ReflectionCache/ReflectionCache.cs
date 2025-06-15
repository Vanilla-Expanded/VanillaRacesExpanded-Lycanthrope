using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;



namespace VanillaRacesExpandedLycanthrope
{
    public class ReflectionCache
    {

        public static readonly AccessTools.FieldRef<Pawn_PsychicEntropyTracker, float> psyfocus =
          AccessTools.FieldRefAccess<Pawn_PsychicEntropyTracker, float>(AccessTools.Field(typeof(Pawn_PsychicEntropyTracker), "currentPsyfocus"));
    
    }
}
