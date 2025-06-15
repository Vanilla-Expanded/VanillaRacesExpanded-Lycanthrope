using RimWorld;
using System.Collections.Generic;
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
            foreach (GeneDef gene in xenotype.AllGenes)
            {
                if (gene == geneDef)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool ContainsOfDef(List<Gene> genes, GeneDef def)
        {
            foreach (Gene gene in genes)
            {
                if (gene.def == def)
                {
                    return true;
                }
            }
            return false;

        }
        public static bool ContainsAnyRelationOfDef(Pawn pawn, PawnRelationDef relation)
        {
            foreach (DirectPawnRelation directPawnRelation in pawn.relations?.DirectRelations)
            {
                if (directPawnRelation?.def == relation)
                {
                    return true;
                }

            }
            return false;

        }
    }
}