
using RimWorld;
using Verse;

namespace VanillaRacesExpandedLycanthrope
{

    public class ThoughtWorker_PackMembers : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            if (pawn.Map == null)
            {
                return false;
            }

            if(!pawn.HasActiveGene(InternalDefOf.VRE_PackMentality))
            {
                return false;
            }

            else
            {
                return ThoughtState.ActiveAtStage(0);
            }

        }

        public override float MoodMultiplier(Pawn pawn)
        {
            if (pawn.Map != null && StaticCollectionsClass.map_PackMembers.ContainsKey(pawn.Map))
            {
                return StaticCollectionsClass.map_PackMembers[pawn.Map];
            }
            else return 1;


        }
    }
}
