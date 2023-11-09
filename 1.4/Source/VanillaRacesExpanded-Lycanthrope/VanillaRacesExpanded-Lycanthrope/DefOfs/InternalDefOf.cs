﻿using System;
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

        public static HediffDef VRE_Morphed;
        public static HediffDef VRE_MorphModifications;
        public static MorphGeneTemplateDef VRE_Morphs;

        public static AbilityDef VRE_Morph;


        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

       


    }
}