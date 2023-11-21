



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

        List<Gene> morphedGenesToMaintain = new List<Gene>();

        public List<GeneDef> endogenes = new List<GeneDef>();

        public List<GeneDef> xenogenes = new List<GeneDef>();

        public string xenotypeName;

        public XenotypeIconDef xenotypeicon;

        public XenotypeDef xenotype;



        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look<Gene>(ref morphedGenesToMaintain, nameof(morphedGenesToMaintain),LookMode.Reference);
            Scribe_Values.Look(ref MorphConditionSwitch, nameof(MorphConditionSwitch));
            Scribe_Values.Look(ref morphed, nameof(morphed));
            Scribe_Collections.Look(ref this.endogenes, nameof(this.endogenes), LookMode.Def);
            Scribe_Collections.Look(ref this.xenogenes, nameof(this.xenogenes), LookMode.Def);
            Scribe_Values.Look(ref this.xenotypeName, nameof(this.xenotypeName));
            Scribe_Defs.Look(ref this.xenotypeicon, nameof(this.xenotypeicon));
            Scribe_Defs.Look(ref this.xenotype, nameof(this.xenotype));
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

        public bool ContainsOfDef(List<Gene> genes,GeneDef def)
        {
            foreach(Gene gene in genes)
            {
                if(gene.def == def)
                {
                    return true;
                }
            }
            return false;

        }


        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {


            Log.Message("Applying, and morphed is "+morphed);
            Pawn pawn = parent.pawn;

            List<Gene> genes = pawn.genes?.GenesListForReading;

            morphedGenesToMaintain.Clear();

            if (genes != null)
            {
                foreach (Gene gene in genes)
                {
                    if (gene.def.defName.Contains("VRE_Morphs"))
                    {
                        if(!ContainsOfDef(morphedGenesToMaintain, gene.def))
                        {
                            if (gene.Active)
                            {
                                morphedGenesToMaintain.Add(gene);
                                if (this.xenotype == null)
                                {
                                    MorphGeneDefExtension extension = gene.def.GetModExtension<MorphGeneDefExtension>();
                                    if (extension != null)
                                    {
                                        xenotype = extension.xenotype;
                                    }
                                }

                            }
                            else { morphedGenesToMaintain.Add(gene); }
                        }
                    }
                }
            }
            

            // Morph from 1 to 2

            if (!morphed)
            {
                morphed=true;
              
                List<GeneDef> endogenes = new List<GeneDef>();
                List<GeneDef> xenogenes = new List<GeneDef>();

                foreach (Gene gene in pawn.genes.Endogenes)
                {
                    if (!ContainsOfDef(morphedGenesToMaintain, gene.def)) { endogenes.Add(gene.def); }
                        
                }
                foreach (Gene gene in pawn.genes.Xenogenes)
                {
                    if (!ContainsOfDef(morphedGenesToMaintain, gene.def)) { xenogenes.Add(gene.def); }
                }

                this.endogenes.Clear();
                this.xenogenes.Clear();
                foreach (GeneDef genedef in endogenes) { this.endogenes.Add(genedef); }
                foreach (GeneDef genedef in xenogenes) { this.xenogenes.Add(genedef); }
                this.xenotypeName = pawn.genes.xenotypeName;
                this.xenotypeicon = pawn.genes.iconDef;

              

                if (pawn.genes?.GenesListForReading.Count > 0)
                {
                    foreach (Gene gene in pawn.genes?.GenesListForReading)
                    {
                        if (!ContainsOfDef(morphedGenesToMaintain, gene.def)) { pawn.genes?.RemoveGene(gene); }

                    }
                }
                SetXenotypeNoClearing(pawn, xenotype);

            }

            // Morph back from 2 to 1

            else
            {
                morphed = false;

               

                List<GeneDef> modifiedEndogenes = new List<GeneDef>();
                List<GeneDef> modifiedXenogenes = new List<GeneDef>();

                foreach (GeneDef genedef in this.endogenes) { modifiedEndogenes.Add(genedef); }
                foreach (GeneDef genedef in this.xenogenes) { modifiedXenogenes.Add(genedef); }

                string modifiedXenotype = this.xenotypeName;
                XenotypeIconDef modifiedIcon = this.xenotypeicon;

                this.endogenes.Clear();
                this.xenogenes.Clear();

                foreach (Gene gene in pawn.genes.Endogenes)
                {
                    if (!ContainsOfDef(morphedGenesToMaintain, gene.def)) { this.endogenes.Add(gene.def); }

                }
                foreach (Gene gene in pawn.genes.Xenogenes)
                {
                    if (!ContainsOfDef(morphedGenesToMaintain, gene.def)) { this.xenogenes.Add(gene.def); }
                }

            

                if (pawn.genes.GenesListForReading.Count > 0)
                {
                   
                    foreach (Gene gene in pawn.genes.GenesListForReading)
                    {
                       
                        if (!ContainsOfDef(morphedGenesToMaintain, gene.def)) { 
                            pawn.genes.RemoveGene(gene);
                          
                        }
                    }
                }
                if (modifiedEndogenes.Count > 0)
                {
                    foreach (GeneDef genedef in modifiedEndogenes) { pawn.genes.AddGene(genedef, false); }

                }
                if (modifiedXenogenes.Count > 0)
                {
                    foreach (GeneDef genedef in modifiedXenogenes) { pawn.genes.AddGene(genedef, true); }

                }
                this.xenotypeName = pawn.genes.xenotypeName;
                this.xenotypeicon = pawn.genes.iconDef;



                pawn.genes.xenotypeName = modifiedXenotype;
                pawn.genes.iconDef = modifiedIcon;

                
            }
        }


        public void SetXenotypeNoClearing(Pawn pawn, XenotypeDef xenotype)
        {
            pawn.genes.SetXenotypeDirect(xenotype);
            pawn.genes.iconDef = null;
            for (int i = 0; i < xenotype.genes.Count; i++)
            {
                if (!ContainsOfDef(morphedGenesToMaintain, xenotype.genes[i])) { pawn.genes.AddGene(xenotype.genes[i], !xenotype.inheritable); }               
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



