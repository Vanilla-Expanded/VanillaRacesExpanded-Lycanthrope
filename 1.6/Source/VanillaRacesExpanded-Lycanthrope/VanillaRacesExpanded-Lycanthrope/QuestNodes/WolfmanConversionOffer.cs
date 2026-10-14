using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedLycanthrope
{
    public class QuestPart_WolfmanConversionOffer : QuestPart
    {
        public Pawn leader;
        public string offerStartSignal;
        public string offerEndSignal;
        public string offerTimeoutSignal;

        private bool offerActive;

        public override void Notify_QuestSignalReceived(Signal signal)
        {
            base.Notify_QuestSignalReceived(signal);

            if (signal.tag == offerStartSignal)
            {
                offerActive = true;
            }
            else if (signal.tag == offerEndSignal || signal.tag == offerTimeoutSignal)
            {
                offerActive = false;
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref leader, "leader");
            Scribe_Values.Look(ref offerStartSignal, "offerStartSignal");
            Scribe_Values.Look(ref offerEndSignal, "offerEndSignal");
            Scribe_Values.Look(ref offerTimeoutSignal, "offerTimeoutSignal");
            Scribe_Values.Look(ref offerActive, "offerActive", defaultValue: false);
        }

        public static bool IsOfferActiveFor(Pawn pawn)
        {
            if (pawn == null || Find.QuestManager == null)
            {
                return false;
            }

            return Find.QuestManager.QuestsListForReading.Any(quest =>
                quest.State == QuestState.Ongoing &&
                quest.PartsListForReading.OfType<QuestPart_WolfmanConversionOffer>().Any(part =>
                    part.leader == pawn && part.offerActive));
        }
    }

    public class FloatMenuOptionProvider_WolfmanReimplant : FloatMenuOptionProvider
    {
        protected override bool Drafted => true;

        protected override bool Undrafted => true;

        protected override bool Multiselect => false;

        protected override bool RequiresManipulation => true;

        protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
        {
            Pawn recipient = context.FirstSelectedPawn;
            if (recipient == null || !recipient.IsColonistPlayerControlled || recipient.Downed || recipient.genes == null)
            {
                return null;
            }

            if (!QuestPart_WolfmanConversionOffer.IsOfferActiveFor(clickedPawn) || clickedPawn.Downed || clickedPawn.genes == null)
            {
                return null;
            }

            if (!clickedPawn.genes.HasGene(InternalDefOf.VRE_GermlineReimplanter) ||
                clickedPawn.HostileTo(recipient) ||
                clickedPawn.genes.Xenotype == recipient.genes.Xenotype ||
                !recipient.CanReach(clickedPawn, PathEndMode.ClosestTouch, Danger.Deadly) ||
                !CompAbilityEffect_ReimplantGermline.PawnIdeoCanAcceptReimplant(clickedPawn, recipient))
            {
                return null;
            }

            return new FloatMenuOption(
                "VRE_AcceptWolfmanGermline".Translate(),
                delegate
                {
                    if (recipient.Spawned && clickedPawn.Spawned && !clickedPawn.Downed)
                    {
                        CompAbilityEffect_ReimplantGermline.GiveVoluntaryReimplantJob(recipient, clickedPawn);
                    }
                },
                clickedPawn,
                Color.white);
        }
    }
}
