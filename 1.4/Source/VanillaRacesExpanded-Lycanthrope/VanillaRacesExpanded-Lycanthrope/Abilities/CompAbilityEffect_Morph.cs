



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

        public List<GeneDef> morphConditionGenes = new List<GeneDef>() { InternalDefOf.VRE_Morphs_AdulthoodMorphing,InternalDefOf.VRE_Morphs_NocturnalMorphing,
        InternalDefOf.VRE_Morphs_SeasonalMorphing,InternalDefOf.VRE_Morphs_DamageMorphing};

        private new CompProperties_AbilityMorph Props => (CompProperties_AbilityMorph)props;

        public bool MorphConditionSwitch = false;

        public bool morphed = false;

        List<Gene> morphedGenesToMaintain;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look<Gene>(ref morphedGenesToMaintain, nameof(morphedGenesToMaintain),LookMode.Deep);
            Scribe_Values.Look(ref MorphConditionSwitch, nameof(MorphConditionSwitch));
            Scribe_Values.Look(ref morphed, nameof(morphed));
        }

        public override bool ShouldHideGizmo
        {
            get
            {
                foreach (GeneDef gene in morphConditionGenes)
                {
                    if (this.parent.pawn.HasActiveGene(gene))
                    {
                        return true;
                    }
                }
                return false;
            }

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
                            morphedGenesToMaintain.Add(gene);
                            MorphGeneDefExtension extension = gene.def.GetModExtension<MorphGeneDefExtension>();
                            if (extension != null)
                            {
                                xenotype = extension.xenotype;
                            }
                        }
                        else if (!morphedGenesToMaintain.Contains(gene)) { morphedGenesToMaintain.Add(gene); } 


                    }

                }
            }

            // Morph from 1 to 2

            if (!morphed)
            {
                morphed=true;

                Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_Morphed);

                HediffComp_Morphed comp = hediff.TryGetComp<HediffComp_Morphed>();

                List<GeneDef> endogenes = new List<GeneDef>();
                List<GeneDef> xenogenes = new List<GeneDef>();

                foreach (Gene gene in pawn.genes.Endogenes)
                {
                    if (!morphedGenesToMaintain.Contains(gene)) { endogenes.Add(gene.def); }
                        
                }
                foreach (Gene gene in pawn.genes.Xenogenes)
                {
                    if (!morphedGenesToMaintain.Contains(gene))
                    {
                        xenogenes.Add(gene.def);
                    }
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

            // Morph back from 2 to 1

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
                    if (!xenotype.AllGenes.Contains(gene.def)&&!morphedGenesToMaintain.Contains(gene))
                    {
                        modifiedEndogenes.Add(gene.def);
                    }

                }
                foreach (Gene gene in pawn.genes.Xenogenes)
                {
                    if (!xenotype.AllGenes.Contains(gene.def) && !morphedGenesToMaintain.Contains(gene))
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
                pawn.genes.xenotypeName = comp.xenotypeName;
                pawn.genes.iconDef = comp.xenotypeicon;

                morphed = false;
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

        public override void CompTick()
        {
            base.CompTick();

            if (this.parent.pawn.IsHashIntervalTick(1000))
            {
                if (this.parent.pawn.HasActiveGene(InternalDefOf.VRE_Morphs_NocturnalMorphing))
                {

                    float currentTime = GenLocalDate.DayTick(this.parent.pawn);
                    if (currentTime > 45000 || currentTime < 15000)
                    {
                        if (!MorphConditionSwitch) {
                            this.Apply(this.parent.pawn, null);
                            MorphConditionSwitch = true;
                        }                      
                    }
                    else
                    { 
                        if (MorphConditionSwitch)
                        {
                            this.Apply(this.parent.pawn, null);
                            MorphConditionSwitch = false;
                        }
                    }
                }
                if (this.parent.pawn.HasActiveGene(InternalDefOf.VRE_Morphs_AdulthoodMorphing))
                {

                    if (this.parent.pawn.ageTracker.AgeBiologicalYears >= 16)
                    {
                        this.Apply(this.parent.pawn, null);
                    }
                }
                if (this.parent.pawn.HasActiveGene(InternalDefOf.VRE_Morphs_SeasonalMorphing))
                {

                    float currentTime = GenLocalDate.DayOfYear(this.parent.pawn);
                    if (currentTime > 31 && currentTime < 60)
                    {
                        if (!MorphConditionSwitch)
                        {
                            this.Apply(this.parent.pawn, null);
                            MorphConditionSwitch = true;
                        }
                    }
                    else
                    {
                        if (MorphConditionSwitch)
                        {
                            this.Apply(this.parent.pawn, null);
                            MorphConditionSwitch = false;
                        }
                    }
                }
                if (this.parent.pawn.HasActiveGene(InternalDefOf.VRE_Morphs_DamageMorphing))
                {

                    float pawnHealth = this.parent.pawn.health.summaryHealth.SummaryHealthPercent;
                    if (pawnHealth < 0.40)
                    {
                        if (!MorphConditionSwitch)
                        {
                            this.Apply(this.parent.pawn, null);
                            MorphConditionSwitch = true;
                        }
                    }
                    else if(pawnHealth > 0.80)
                    {
                        if (MorphConditionSwitch)
                        {
                            this.Apply(this.parent.pawn, null);
                            MorphConditionSwitch = false;
                        }
                    }
                }
            }
        }
    }
}



