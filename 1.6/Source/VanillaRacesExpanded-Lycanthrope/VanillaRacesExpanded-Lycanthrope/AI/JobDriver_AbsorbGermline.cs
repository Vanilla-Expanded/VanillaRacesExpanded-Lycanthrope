
using RimWorld;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedLycanthrope
{
    public class JobDriver_AbsorbGermline : JobDriver
    {
        public const int TicksToAbsorb = 60;

        public Pawn Target => job.targetA.Thing as Pawn;

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            
                this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
                yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
                yield return Toils_General.WaitWith(TargetIndex.A, TicksToAbsorb, useProgressBar: true);
                yield return Toils_General.Do(delegate
                {
                    if (Target.HomeFaction != null && pawn.Faction == Faction.OfPlayer)
                    {
                        Faction.OfPlayer.TryAffectGoodwillWith(Target.HomeFaction, -50, canSendMessage: true, !Target.HomeFaction.temporary, HistoryEventDefOf.AbsorbedXenogerm);
                    }
                    QuestUtility.SendQuestTargetSignals(Target.questTags, "XenogermAbsorbed", Target.Named("SUBJECT"));
                    CompAbilityEffect_ReimplantGermline.ReimplantGermline(Target, pawn);
                });
            
        }
    }
}