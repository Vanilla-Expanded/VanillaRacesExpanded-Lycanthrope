using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Linq;
using UnityEngine;
using Verse.Sound;

namespace VanillaRacesExpandedLycanthrope
{
    [StaticConstructorOnStartup]
    public class CompAbilityEffect_WarHowl : CompAbilityEffect
    {      

        private new CompProperties_AbilityWarHowl Props => (CompProperties_AbilityWarHowl)props;     

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {

            base.Apply(target, dest);
            Pawn pawn = target.Pawn;
            if (pawn != null && pawn.Map !=null)
            {
                FleckMaker.AttachedOverlay(pawn, FleckDefOf.PsycastAreaEffect, Vector3.zero, 3f, -1f);
                InternalDefOf.VRE_WarHowl_Cast.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));

                List<Pawn> list = pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);

                foreach (Pawn item in list)
                {
                    if (!item.RaceProps.Humanlike || item.Dead || item.health == null || !(item.Position.DistanceTo(parent.pawn.Position) <= 10))
                    {
                        continue;
                    }
                    Hediff hediff = item.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_WarHowlHediff);
                    if (hediff == null)
                    {
                        hediff = item.health.AddHediff(InternalDefOf.VRE_WarHowlHediff, item.health.hediffSet.GetBrain());
                        hediff.Severity = 0.1f;

                    }
                    else
                    {
                        hediff.Severity += 0.1f;
                    }
                    HediffComp_Disappears hediffComp_Disappears = hediff.TryGetComp<HediffComp_Disappears>();
                    if (hediffComp_Disappears != null)
                    {
                        hediffComp_Disappears.ticksToDisappear = hediffComp_Disappears.disappearsAfterTicks;
                    }
                    
                    
                }

            }

        }
       
    }
}



