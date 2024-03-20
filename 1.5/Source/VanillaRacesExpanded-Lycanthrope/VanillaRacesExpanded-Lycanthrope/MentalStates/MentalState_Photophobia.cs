
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedLycanthrope
{
    public class MentalState_Photophobia : MentalState
    {
        private int lastLightSeenTick = -1;

        protected override bool CanEndBeforeMaxDurationNow => false;

        public override RandomSocialMode SocialModeMax()
        {
            return RandomSocialMode.Off;
        }

        public override void MentalStateTick()
        {
            base.MentalStateTick();
            if (pawn.IsHashIntervalTick(30))
            {
                if (lastLightSeenTick < 0 || ThoughtWorker_Photophobia.Light(pawn))
                {
                    lastLightSeenTick = Find.TickManager.TicksGame;
                }
                if (lastLightSeenTick >= 0 && Find.TickManager.TicksGame >= lastLightSeenTick + def.minTicksBeforeRecovery)
                {
                    RecoverFromState();
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastLightSeenTick, "lastLightSeenTick", -1);
        }

        public override TaggedString GetBeginLetterText()
        {
            if (def.beginLetter.NullOrEmpty())
            {
                return null;
            }
            if (VanillaRacesExpandedLycanthrope_Mod.settings.VRE_DisablePhotophobiaMessage)
            {
                return null;
            }
            return def.beginLetter.Formatted(pawn.NameShortColored, pawn.Named("PAWN")).AdjustedFor(pawn).Resolve()
                .CapitalizeFirst();
        }
    }
}
