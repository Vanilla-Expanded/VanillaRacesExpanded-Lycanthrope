using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

using System.Collections.Generic;
using System;
using Verse.AI;

namespace VanillaRacesExpandedLycanthrope
{
    [HarmonyPatch(typeof(InteractionWorker))]
    [HarmonyPatch("Interacted")]
    public static class VanillaRacesExpandedLycanthrope_InteractionWorker_Interacted_Patch
    {
        static Random random = new Random();

        [HarmonyPostfix]
        static void CreateRelationOnDeepTalk(Pawn initiator, Pawn recipient, InteractionWorker __instance)
        {
            if (initiator.HasActiveGene(InternalDefOf.VRE_PackMentality) == true && recipient.HasActiveGene(InternalDefOf.VRE_PackMentality) == true)
            {
                if (__instance.interaction == InteractionDefOf.DeepTalk)
                {
                    if (random.NextDouble() > 0.8f)
                    {
                        initiator.relations.AddDirectRelation(InternalDefOf.VRE_PackMember, recipient);
                    }                       
                }
            }
        }
    }
}
