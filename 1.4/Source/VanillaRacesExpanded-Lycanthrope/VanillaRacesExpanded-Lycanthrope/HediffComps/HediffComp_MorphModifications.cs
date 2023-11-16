
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;



namespace VanillaRacesExpandedLycanthrope
{
    public class HediffComp_MorphModifications : HediffComp
    {
        private HediffCompProperties_MorphModifications Props => (HediffCompProperties_MorphModifications)props;

        public List<GeneDef> endogenes = new List<GeneDef>();

        public List<GeneDef> xenogenes = new List<GeneDef>();

        public string xenotypeName;

        public XenotypeIconDef xenotypeicon;



        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Collections.Look(ref this.endogenes, nameof(this.endogenes), LookMode.Def);
            Scribe_Collections.Look(ref this.xenogenes, nameof(this.xenogenes), LookMode.Def);
            Scribe_Values.Look(ref this.xenotypeName, nameof(this.xenotypeName));
            Scribe_Defs.Look(ref this.xenotypeicon, nameof(this.xenotypeicon));

        }






    }
}