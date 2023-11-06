using RimWorld;
using Verse;

namespace VanillaRacesExpandedLycanthrope
{
    public static class Utils
    {
        public static bool HasActiveGene(this Pawn pawn, GeneDef geneDef)
        {
            if (pawn.genes is null) return false;
            return pawn.genes.GetGene(geneDef)?.Active ?? false;
        }

        public static bool XenotypeContainsGene(XenotypeDef xenotype, GeneDef geneDef)
        {
            foreach(GeneDef gene in xenotype.AllGenes)
            {
                if(gene == geneDef)
                {
                    return true;
                }
            }
            return false;
        }
    }
}