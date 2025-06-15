using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;

namespace VanillaRacesExpandedLycanthrope
{
    [HarmonyPatch(typeof(Pawn_InteractionsTracker))]
    [HarmonyPatch("StartSocialFight")]
    public static class VanillaRacesExpandedLycanthrope_Pawn_InteractionsTracker_StartSocialFight_Patch
    {
        [HarmonyPostfix]
        static void CreateRelationOnSocialFight(Pawn otherPawn, Pawn ___pawn)
        {
            if (___pawn.HasActiveGene(InternalDefOf.VRE_PackMentality) == true && otherPawn.HasActiveGene(InternalDefOf.VRE_PackMentality) == true)
            {

                ___pawn.relations.AddDirectRelation(InternalDefOf.VRE_PackMember, otherPawn);
                
            }
        }
    }
}
