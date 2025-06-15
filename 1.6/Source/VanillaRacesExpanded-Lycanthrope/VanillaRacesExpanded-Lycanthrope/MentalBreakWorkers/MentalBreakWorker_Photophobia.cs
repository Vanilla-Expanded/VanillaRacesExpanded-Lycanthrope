
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedLycanthrope
{
    public class MentalBreakWorker_Photophobia : MentalBreakWorker
    {
        public override bool BreakCanOccur(Pawn pawn)
        {
            if (!pawn.Spawned)
            {
                return false;
            }
            if (!base.BreakCanOccur(pawn))
            {
                return false;
            }
            return ThoughtWorker_Photophobia.Light(pawn);
        }
    }
}
