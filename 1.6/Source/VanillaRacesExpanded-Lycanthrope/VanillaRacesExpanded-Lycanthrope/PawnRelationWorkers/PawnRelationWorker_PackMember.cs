
using RimWorld;
using Verse;
namespace VanillaRacesExpandedLycanthrope
{
    public class PawnRelationWorker_PackMember : PawnRelationWorker
    {
      

        public override void CreateRelation(Pawn pawn, Pawn other, ref PawnGenerationRequest request)
        {
            pawn.relations.AddDirectRelation(InternalDefOf.VRE_PackMember, other);
           
        }
    }
}
