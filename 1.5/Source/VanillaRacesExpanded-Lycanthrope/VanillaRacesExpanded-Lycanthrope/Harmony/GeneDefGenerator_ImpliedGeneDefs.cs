using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Verse;
using RimWorld;

using HarmonyLib;


namespace VanillaRacesExpandedLycanthrope
{




    [HarmonyPatch(typeof(GeneDefGenerator))]
    [HarmonyPatch("ImpliedGeneDefs")]

    public static class VanillaRacesExpandedLycanthrope_GeneDefGenerator_ImpliedGeneDefs_Patch
    {
        [HarmonyPostfix]
        public static IEnumerable<GeneDef> Postfix(IEnumerable<GeneDef> values)
        {
            List<GeneDef> resultingList = values.ToList();


            MorphGeneTemplateDef template = InternalDefOf.VRE_Morphs;

            List <XenotypeDef> listOfXenotypes = DefDatabase<XenotypeDef>.AllDefs.Where(element => !Utils.XenotypeContainsGene(element,DefDatabase<GeneDef>.GetNamedSilentFail("VREA_Power"))
            && element.defName!= "AG_RandomCustom").ToList();

         

            foreach (XenotypeDef xenotype in listOfXenotypes)
            {
                resultingList.Add(GetFromXenotype(template,xenotype, xenotype.index));
            }




            return resultingList;




        }

        public static GeneDef GetFromXenotype(MorphGeneTemplateDef template,XenotypeDef def, int displayOrderBase)
        {


            GeneDef geneDef = new GeneDef
            {
                defName = template.defName + "_" + def.defName,
                geneClass = template.geneClass,
                label = template.label.Formatted(def.label),
                iconPath = def.iconPath,
                description = template.description.Formatted(def.LabelCap),
                labelShortAdj = template.labelShortAdj.Formatted(def.label),
                selectionWeight = template.selectionWeight,
                biostatCpx = template.biostatCpx,
                biostatMet = template.biostatMet,
                biostatArc = template.biostatArc,
                displayCategory = template.displayCategory,
                displayOrderInCategory = displayOrderBase + template.displayOrderOffset,
                minAgeActive = template.minAgeActive,
                modContentPack = template.modContentPack,
                modExtensions = new List<DefModExtension> {
                    new VanillaGenesExpanded.GeneExtension {
                        backgroundPathArchite = "UI/Icons/Genes/GeneBackground_MorphGene"
                    },
                    new MorphGeneDefExtension {
                        xenotype = def
                    }
                },

                abilities = new List<AbilityDef> { template.ability },
                descriptionHyperlinks = new List<DefHyperlink> { new DefHyperlink { def = template.ability } }
            };


            if (!template.exclusionTagPrefix.NullOrEmpty())
            {
                geneDef.exclusionTags = new List<string> { template.exclusionTagPrefix };
            }

            return geneDef;

        }





    }

}
