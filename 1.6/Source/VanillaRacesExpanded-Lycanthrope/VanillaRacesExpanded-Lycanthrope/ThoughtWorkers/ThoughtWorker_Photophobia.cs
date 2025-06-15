
using RimWorld;
using Verse;

namespace VanillaRacesExpandedLycanthrope
{
    public class ThoughtWorker_Photophobia : ThoughtWorker
    {

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!ModsConfig.BiotechActive)
            {
                return ThoughtState.Inactive;
            }
            if (p.genes == null || !p.HasActiveGene(InternalDefOf.VRE_Photophobia) || !Light(p))
            {
                return ThoughtState.Inactive;
            }
            return ThoughtState.ActiveAtStage(0);
        }

        public static bool Light(Pawn pawn)
        {
            Map mapHeld = pawn.MapHeld;
            if (mapHeld == null)
            {
                return false;
            }

            IntVec3 positionHeld = pawn.PositionHeld;



            if (pawn.Map.glowGrid.GroundGlowAt(positionHeld) >= 0.11f)
            {
                return true;
            }

            return false;
        }
    }
}
