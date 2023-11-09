
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;



namespace VanillaRacesExpandedLycanthrope
{
    public class HediffComp_Morphed : HediffComp
    {
        private HediffCompProperties_Morphed Props => (HediffCompProperties_Morphed)props;

        public List<GeneDef> endogenes = new List<GeneDef>();

        public List<GeneDef> xenogenes = new List<GeneDef>();

        public string xenotypeName;

        public XenotypeIconDef xenotypeicon;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Collections.Look(ref this.endogenes, nameof(this.endogenes));
            Scribe_Collections.Look(ref this.xenogenes, nameof(this.xenogenes));
            Scribe_Values.Look(ref this.xenotypeName, nameof(this.xenotypeName));
            Scribe_Defs.Look(ref this.xenotypeicon, nameof(this.xenotypeicon));

        }






    }
}