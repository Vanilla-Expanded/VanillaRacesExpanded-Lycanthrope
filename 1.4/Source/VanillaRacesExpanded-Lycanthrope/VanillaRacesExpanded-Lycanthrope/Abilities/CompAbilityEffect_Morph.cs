



using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse;
using System.Linq;



namespace VanillaRacesExpandedLycanthrope
{
    [StaticConstructorOnStartup]
    public class CompAbilityEffect_Morph : CompAbilityEffect
    {

        private new CompProperties_AbilityMorph Props => (CompProperties_AbilityMorph)props;

        List<Gene> morphedGenesToMaintain;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look(ref morphedGenesToMaintain, "morphedGenesToMaintain");
        }


        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {



            Pawn pawn = parent.pawn;

            List<Gene> genes = pawn.genes?.GenesListForReading;
            XenotypeDef xenotype = null;
            morphedGenesToMaintain = new List<Gene>();

            if (genes != null)
            {
                foreach (Gene gene in genes)
                {
                    if (gene.def.defName.Contains("VRE_Morphs"))
                    {
                        if (gene.Active)
                        {
                            MorphGeneDefExtension extension = gene.def.GetModExtension<MorphGeneDefExtension>();
                            if (extension != null)
                            {
                                morphedGenesToMaintain.Add(gene);
                                xenotype = extension.xenotype;
                                
                            }
                        }
                        else morphedGenesToMaintain.Add(gene);


                    }

                }
            }

            if (!pawn.health.hediffSet.HasHediff(InternalDefOf.VRE_Morphed))
            {
                pawn.health.AddHediff(InternalDefOf.VRE_Morphed);

                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_Morphed);

                HediffComp_Morphed comp = hediff.TryGetComp<HediffComp_Morphed>();

                List<GeneDef> endogenes = new List<GeneDef>();
                List<GeneDef> xenogenes = new List<GeneDef>();

                foreach (Gene gene in pawn.genes.Endogenes)
                {
                    endogenes.Add(gene.def);
                }
                foreach (Gene gene in pawn.genes.Xenogenes)
                {
                    xenogenes.Add(gene.def);
                }

                comp.endogenes = endogenes;
                comp.xenogenes = xenogenes;
                comp.xenotypeName = pawn.genes.xenotypeName;
                comp.xenotypeicon = pawn.genes.iconDef;

                if (pawn.genes?.GenesListForReading.Count > 0)
                {
                    foreach (Gene gene in pawn.genes?.GenesListForReading)
                    {
                        if (!morphedGenesToMaintain.Contains(gene)) { pawn.genes?.RemoveGene(gene); }

                    }
                }
                SetXenotypeNoClearing(pawn, xenotype);

                Hediff hediffModifications = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_MorphModifications);

                HediffComp_MorphModifications compModifications = hediffModifications.TryGetComp<HediffComp_MorphModifications>();

                if (compModifications.endogenes.Count > 0)
                {
                    foreach (GeneDef genedef in compModifications.endogenes) { pawn.genes.AddGene(genedef, false); }

                }
                if (compModifications.xenogenes.Count > 0)
                {
                    foreach (GeneDef genedef in compModifications.xenogenes) { pawn.genes.AddGene(genedef, true); }

                }


            }
            else
            {


                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_Morphed);
                HediffComp_Morphed comp = hediff.TryGetComp<HediffComp_Morphed>();

                Hediff hediffModifications = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_MorphModifications);
                HediffComp_MorphModifications compModifications = hediffModifications.TryGetComp<HediffComp_MorphModifications>();

                compModifications.endogenes.Clear();
                compModifications.xenogenes.Clear();

                List<GeneDef> modifiedEndogenes = new List<GeneDef>();
                List<GeneDef> modifiedXenogenes = new List<GeneDef>();

                foreach (Gene gene in pawn.genes.Endogenes)
                {
                    if (!xenotype.AllGenes.Contains(gene.def))
                    {
                        modifiedEndogenes.Add(gene.def);
                    }

                }
                foreach (Gene gene in pawn.genes.Xenogenes)
                {
                    if (!xenotype.AllGenes.Contains(gene.def))
                    {
                        modifiedXenogenes.Add(gene.def);
                    }
                }

                compModifications.endogenes = modifiedEndogenes;
                compModifications.xenogenes = modifiedXenogenes;



                if (pawn.genes?.GenesListForReading.Count > 0)
                {
                    foreach (Gene gene in pawn.genes?.GenesListForReading)
                    {
                        if (!morphedGenesToMaintain.Contains(gene)) { pawn.genes?.RemoveGene(gene); }
                    }
                }
                if (comp.endogenes.Count > 0)
                {
                    foreach (GeneDef genedef in comp.endogenes) { pawn.genes.AddGene(genedef, false); }

                }
                if (comp.xenogenes.Count > 0)
                {
                    foreach (GeneDef genedef in comp.xenogenes) { pawn.genes.AddGene(genedef, true); }

                }
                pawn.genes.xenotypeName=comp.xenotypeName;
                pawn.genes.iconDef=comp.xenotypeicon;

                pawn.health.RemoveHediff(pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_Morphed));


            }

        }


        public void SetXenotypeNoClearing(Pawn pawn, XenotypeDef xenotype)
        {
            pawn.genes.SetXenotypeDirect(xenotype);
            pawn.genes.iconDef = null;
            for (int i = 0; i < xenotype.genes.Count; i++)
            {
                pawn.genes.AddGene(xenotype.genes[i], !xenotype.inheritable);
            }
        }
    }

}



