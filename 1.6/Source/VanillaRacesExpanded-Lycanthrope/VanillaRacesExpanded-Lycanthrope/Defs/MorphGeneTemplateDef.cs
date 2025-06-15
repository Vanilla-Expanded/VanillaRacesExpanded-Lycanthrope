
using System;
using System.Collections.Generic;
using RimWorld;
using Verse;


namespace VanillaRacesExpandedLycanthrope
{
    public class MorphGeneTemplateDef : Def
    {

        public Type geneClass = typeof(Gene);

        public int biostatCpx;

        public int biostatMet;

        public int biostatArc;

        public float minAgeActive;

        public GeneCategoryDef displayCategory;

        public int displayOrderOffset;

        public float selectionWeight = 0f;

        public AbilityDef ability;


        [MustTranslate]
        public string labelShortAdj;

        [NoTranslate]
        public string iconPath;

        [NoTranslate]
        public string exclusionTagPrefix;

        public override IEnumerable<string> ConfigErrors()
        {
            foreach (string item in base.ConfigErrors())
            {
                yield return item;
            }
            if (!typeof(Gene).IsAssignableFrom(geneClass))
            {
                yield return "geneClass is not Gene or child thereof.";
            }
        }
    }
}
