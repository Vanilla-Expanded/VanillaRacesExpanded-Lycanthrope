using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;
using System;
using VanillaRacesExpandedHighmate;
using VanillaRacesExpandedPhytokin;

namespace VanillaRacesExpandedLycanthrope
{
    [StaticConstructorOnStartup]
    public class CompAbilityEffect_Morph : CompAbilityEffect
    {

        public List<GeneDef> morphConditionGenes = new List<GeneDef>() { InternalDefOf.VRE_Morphs_AdulthoodMorphing,InternalDefOf.VRE_Morphs_NocturnalMorphing,
        InternalDefOf.VRE_Morphs_SeasonalMorphing,InternalDefOf.VRE_Morphs_DamageMorphing, InternalDefOf.VRE_Morphs_RandomMorphing};

        private new CompProperties_AbilityMorph Props => (CompProperties_AbilityMorph)props;

        public bool MorphConditionSwitch = false;

        public bool morphed = false;

        List<Gene> morphedGenesToMaintain = new List<Gene>();

        public List<GeneDef> endogenes = new List<GeneDef>();

        public List<GeneDef> xenogenes = new List<GeneDef>();

        public XenotypeDef xenotype;
        public XenotypeDef xenotypeOriginal;

        public int randomMorphTime = Rand.RangeInclusive(36, 2400);
        public int randomMorphCounter = 0;

        public float hemogenValueForm1 = 1;
        public float hemogenValueForm2 = 1;

        public float DeathrestValueForm1 = 1;
        public float DeathrestValueForm2 = 1;

        public float killThirstValueForm1 = 1;
        public float killThirstValueForm2 = 1;

        public float lovinValueForm1 = 1;
        public float lovinValueForm2 = 1;

        public float psyfocusValueForm1 = 1;
        public float psyfocusValueForm2 = 1;

        public float wastepacksValueForm1 = 1;
        public float wastepacksValueForm2 = 1;

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Collections.Look<Gene>(ref morphedGenesToMaintain, nameof(morphedGenesToMaintain), LookMode.Reference);
            Scribe_Values.Look(ref MorphConditionSwitch, nameof(MorphConditionSwitch));
            Scribe_Values.Look(ref morphed, nameof(morphed));
            Scribe_Collections.Look(ref this.endogenes, nameof(this.endogenes), LookMode.Def);
            Scribe_Collections.Look(ref this.xenogenes, nameof(this.xenogenes), LookMode.Def);
            Scribe_Values.Look(ref this.randomMorphTime, nameof(this.randomMorphTime));
            Scribe_Values.Look(ref this.randomMorphCounter, nameof(this.randomMorphCounter));
            Scribe_Defs.Look(ref this.xenotype, nameof(this.xenotype));
            Scribe_Defs.Look(ref this.xenotypeOriginal, nameof(this.xenotypeOriginal));
            Scribe_Values.Look(ref this.hemogenValueForm1, nameof(this.hemogenValueForm1));
            Scribe_Values.Look(ref this.hemogenValueForm2, nameof(this.hemogenValueForm2));
            Scribe_Values.Look(ref this.DeathrestValueForm1, nameof(this.DeathrestValueForm1));
            Scribe_Values.Look(ref this.DeathrestValueForm2, nameof(this.DeathrestValueForm2));
            Scribe_Values.Look(ref this.killThirstValueForm1, nameof(this.killThirstValueForm1));
            Scribe_Values.Look(ref this.killThirstValueForm2, nameof(this.killThirstValueForm2));
            Scribe_Values.Look(ref this.lovinValueForm1, nameof(this.lovinValueForm1));
            Scribe_Values.Look(ref this.lovinValueForm2, nameof(this.lovinValueForm2));
            Scribe_Values.Look(ref this.psyfocusValueForm1, nameof(this.psyfocusValueForm1));
            Scribe_Values.Look(ref this.psyfocusValueForm2, nameof(this.psyfocusValueForm2));
            Scribe_Values.Look(ref this.wastepacksValueForm1, nameof(this.wastepacksValueForm1));
            Scribe_Values.Look(ref this.wastepacksValueForm2, nameof(this.wastepacksValueForm2));
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

        public void StoreNeedValues(int form, Pawn pawn, bool storeOrRestore)
        {
            Gene_Hemogen gene_Hemogen = pawn.genes?.GetFirstGeneOfType<Gene_Hemogen>();
            if (gene_Hemogen != null)
            {
                if (form == 1)
                {
                    if (storeOrRestore) { hemogenValueForm1 = gene_Hemogen.Value; }
                    else { gene_Hemogen.Value = hemogenValueForm2; }
                }
                else if (form == 2)
                {
                    if (storeOrRestore) { hemogenValueForm2 = gene_Hemogen.Value; }
                    else { gene_Hemogen.Value = hemogenValueForm1; }
                }
            }

            Gene_Deathrest gene_Deathrest = pawn.genes?.GetFirstGeneOfType<Gene_Deathrest>();
            if (gene_Deathrest != null)
            {
                if (form == 1)
                {
                    if (storeOrRestore) { DeathrestValueForm1 = gene_Deathrest.DeathrestNeed.CurLevel; }
                    else { gene_Deathrest.DeathrestNeed.CurLevel = DeathrestValueForm2; }
                }
                else if (form == 2)
                {
                    if (storeOrRestore) { DeathrestValueForm2 = gene_Deathrest.DeathrestNeed.CurLevel; }
                    else { gene_Deathrest.DeathrestNeed.CurLevel = DeathrestValueForm1; }
                }
            }

            Need_KillThirst killthirst = pawn.needs?.TryGetNeed<Need_KillThirst>();
            if (killthirst != null)
            {
                if (form == 1)
                {
                    if (storeOrRestore) { killThirstValueForm1 = killthirst.CurLevel; }
                    else { killthirst.CurLevel = killThirstValueForm2; }
                }
                else if (form == 2)
                {
                    if (storeOrRestore) { killThirstValueForm2 = killthirst.CurLevel; }
                    else { killthirst.CurLevel = killThirstValueForm1; }
                }
            }

            Pawn_PsychicEntropyTracker psy = pawn.psychicEntropy;
            if (psy != null)
            {
                if (form == 1)
                {
                    if (storeOrRestore) { psyfocusValueForm1 = psy.CurrentPsyfocus; }
                    else { ReflectionCache.psyfocus(psy) = psyfocusValueForm2; }
                }
                else if (form == 2)
                {
                    if (storeOrRestore) { psyfocusValueForm2 = psy.CurrentPsyfocus; }
                    else { ReflectionCache.psyfocus(psy) = psyfocusValueForm1; }
                }
            }

            try
            {
                ((Action)(() =>
                {
                    if (ModLister.HasActiveModWithName("Vanilla Races Expanded - Highmate"))
                    {
                        if (VanillaRacesExpandedHighmate.Utils.HasLovinNeed(pawn))
                        {
                            if (form == 1)
                            {
                                if (storeOrRestore) { lovinValueForm1 = VanillaRacesExpandedHighmate.Utils.GetLovinNeed(pawn); }
                                else { VanillaRacesExpandedHighmate.Utils.SetLovinNeed(pawn, lovinValueForm2); }
                            }
                            else if (form == 2)
                            {
                                if (storeOrRestore) { lovinValueForm2 = VanillaRacesExpandedHighmate.Utils.GetLovinNeed(pawn); }
                                else { VanillaRacesExpandedHighmate.Utils.SetLovinNeed(pawn, lovinValueForm1); }
                            }
                        }
                    }
                }))();
            }
            catch (TypeLoadException) { }

            try
            {
                ((Action)(() =>
                {
                    if (ModLister.HasActiveModWithName("Vanilla Races Expanded - Phytokin"))
                    {

                        Gene_Resource_Wastepacks gene_Wastepacks = pawn.genes?.GetFirstGeneOfType<Gene_Resource_Wastepacks>();
                        if (gene_Wastepacks != null)
                        {
                            if (form == 1)
                            {
                                if (storeOrRestore) { wastepacksValueForm1 = gene_Wastepacks.Resource.Value; }
                                else { gene_Wastepacks.Resource.Value = wastepacksValueForm2; }
                            }
                            else if (form == 2)
                            {
                                if (storeOrRestore) { wastepacksValueForm2 = gene_Wastepacks.Resource.Value; }
                                else { gene_Wastepacks.Resource.Value = wastepacksValueForm1; }
                            }
                        }
                    }
                }))();
            }
            catch (TypeLoadException) { }


        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {



            Pawn pawn = parent.pawn;
            if (pawn.Map != null)
            {
                Effecter effecter = InternalDefOf.CocoonDestroyed.SpawnAttached(pawn, pawn.Map);
                effecter.Trigger(pawn, null);
                IntRange numberOfFilth = new IntRange(1,2);
                for (int i = 0; i < numberOfFilth.RandomInRange; i++)
                {
                    IntVec3 c;
                    CellFinder.TryFindRandomReachableCellNearPosition(pawn.Position, pawn.Position, pawn.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);

                    FilthMaker.TryMakeFilth(c, pawn.Map, ThingDefOf.Filth_Blood);
                }

                InternalDefOf.VRE_MorphSound.PlayOneShot(new TargetInfo(pawn.Position, pawn.Map, false));
            }

            List<Gene> genes = pawn.genes?.GenesListForReading;

            morphedGenesToMaintain.Clear();

            if (genes != null)
            {
                foreach (Gene gene in genes)
                {
                    if (gene.def.defName.Contains("VRE_Morphs"))
                    {
                        if (!Utils.ContainsOfDef(morphedGenesToMaintain, gene.def))
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
                morphed = true;

                StoreNeedValues(1, pawn, true);

                List<GeneDef> endogenes = new List<GeneDef>();
                List<GeneDef> xenogenes = new List<GeneDef>();

                foreach (Gene gene in pawn.genes.Endogenes)
                {
                    if (!Utils.ContainsOfDef(morphedGenesToMaintain, gene.def)) { endogenes.Add(gene.def); }

                }
                foreach (Gene gene in pawn.genes.Xenogenes)
                {
                    if (!Utils.ContainsOfDef(morphedGenesToMaintain, gene.def)) { xenogenes.Add(gene.def); }
                }

                this.endogenes.Clear();
                this.xenogenes.Clear();
                foreach (GeneDef genedef in endogenes) { this.endogenes.Add(genedef); }
                foreach (GeneDef genedef in xenogenes) { this.xenogenes.Add(genedef); }

                if (this.xenotypeOriginal == null)
                {

                    this.xenotypeOriginal = pawn.genes.Xenotype;
                }


                if (pawn.genes?.GenesListForReading.Count > 0)
                {
                    foreach (Gene gene in pawn.genes?.GenesListForReading)
                    {
                        if (!Utils.ContainsOfDef(morphedGenesToMaintain, gene.def)) { pawn.genes?.RemoveGene(gene); }

                    }
                }
                SetXenotypeNoClearing(pawn, xenotype);
                StoreNeedValues(1, pawn, false);
            }

            // Morph back from 2 to 1

            else
            {
                morphed = false;

                StoreNeedValues(2, pawn, true);

                List<GeneDef> modifiedEndogenes = new List<GeneDef>();
                List<GeneDef> modifiedXenogenes = new List<GeneDef>();

                foreach (GeneDef genedef in this.endogenes) { modifiedEndogenes.Add(genedef); }
                foreach (GeneDef genedef in this.xenogenes) { modifiedXenogenes.Add(genedef); }



                this.endogenes.Clear();
                this.xenogenes.Clear();

                foreach (Gene gene in pawn.genes.Endogenes)
                {
                    if (!Utils.ContainsOfDef(morphedGenesToMaintain, gene.def)) { this.endogenes.Add(gene.def); }

                }
                foreach (Gene gene in pawn.genes.Xenogenes)
                {
                    if (!Utils.ContainsOfDef(morphedGenesToMaintain, gene.def)) { this.xenogenes.Add(gene.def); }
                }



                if (pawn.genes.GenesListForReading.Count > 0)
                {

                    foreach (Gene gene in pawn.genes.GenesListForReading)
                    {

                        if (!Utils.ContainsOfDef(morphedGenesToMaintain, gene.def))
                        {
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

                SetXenotypeNoClearing(pawn, xenotypeOriginal);
                StoreNeedValues(2, pawn, false);
            }
        }

        public void SetXenotypeNoClearing(Pawn pawn, XenotypeDef xenotype)
        {
            pawn.genes.SetXenotypeDirect(xenotype);

            for (int i = 0; i < xenotype.genes.Count; i++)
            {
                if (!Utils.ContainsOfDef(morphedGenesToMaintain, xenotype.genes[i])) { pawn.genes.AddGene(xenotype.genes[i], !xenotype.inheritable); }
            }
            List<Gene> genes = pawn.genes?.GenesListForReading;
            if (genes.Count > 0)
            {
                foreach (Gene gene in genes)
                {
                    if (gene?.def.defName.Contains("VRE_Morphs") == true)
                    {
                        if (xenotype?.AllGenes?.Contains(gene.def) == true)
                        {
                            Gene otherGene = gene.overriddenByGene;

                            gene.overriddenByGene = null;
                            if (otherGene != null)
                            {
                                otherGene.overriddenByGene = gene;
                            }

                        }
                    }
                }
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
                if (this.parent.pawn.HasActiveGene(InternalDefOf.VRE_Morphs_AdulthoodMorphing))
                {

                    if (!MorphConditionSwitch && this.parent.pawn.ageTracker.AgeBiologicalYears >= 16)
                    {
                        this.Apply(this.parent.pawn, null);
                        MorphConditionSwitch = true;
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
                    if (pawnHealth < 0.80)
                    {
                        if (!MorphConditionSwitch)
                        {
                            this.Apply(this.parent.pawn, null);
                            MorphConditionSwitch = true;
                        }
                    }
                    else if (pawnHealth > 0.90)
                    {
                        if (MorphConditionSwitch)
                        {
                            this.Apply(this.parent.pawn, null);
                            MorphConditionSwitch = false;
                        }
                    }
                }
                if (this.parent.pawn.HasActiveGene(InternalDefOf.VRE_Morphs_RandomMorphing))
                {
                    randomMorphCounter++;
                    if (randomMorphCounter >= randomMorphTime)
                    {
                        randomMorphTime = Rand.RangeInclusive(36, 2400);
                        this.Apply(this.parent.pawn, null);
                        randomMorphCounter = 0;
                    }
                }
            }
        }
    }
}



