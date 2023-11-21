using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaRacesExpandedLycanthrope
{
    [DefOf]
    public static class InternalDefOf
    {

       
        public static HediffDef VRE_MorphModifications;

        public static MorphGeneTemplateDef VRE_Morphs;

        public static AbilityDef VRE_Morph;

        public static GeneDef VRE_Morphs_NocturnalMorphing;
        public static GeneDef VRE_Morphs_AdulthoodMorphing;
        public static GeneDef VRE_Morphs_SeasonalMorphing;
        public static GeneDef VRE_Morphs_DamageMorphing;
        public static GeneDef VRE_Photophobia;

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

       


    }
}