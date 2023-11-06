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

        public static MorphGeneTemplateDef VRE_Morphs;

        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

       


    }
}