using RimWorld;
using UnityEngine;
using Verse;
using System;
using System.Collections.Generic;
using System.Linq;


namespace VanillaRacesExpandedLycanthrope
{
    public class VanillaRacesExpandedLycanthrope_Settings : ModSettings

    {


        public bool VRE_DisablePhotophobiaMessage = false;



        private static Vector2 scrollPosition = Vector2.zero;

        public override void ExposeData()
        {
            base.ExposeData();


            Scribe_Values.Look(ref VRE_DisablePhotophobiaMessage, "VRE_DisablePhotophobiaMessage", false);



        }
        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();

            var scrollContainer = inRect.ContractedBy(10);
            scrollContainer.height -= listingStandard.CurHeight;
            scrollContainer.y += listingStandard.CurHeight;
            Widgets.DrawBoxSolid(scrollContainer, Color.grey);
            var innerContainer = scrollContainer.ContractedBy(1);
            Widgets.DrawBoxSolid(innerContainer, new ColorInt(42, 43, 44).ToColor);
            var frameRect = innerContainer.ContractedBy(5);
            frameRect.y += 15;
            frameRect.height -= 15;
            var contentRect = frameRect;
            contentRect.x = 0;
            contentRect.y = 0;
            contentRect.width -= 20;

            contentRect.height = 950f;

            Widgets.BeginScrollView(frameRect, ref scrollPosition, contentRect, true);
            listingStandard.Begin(contentRect.AtZero());

                       listingStandard.CheckboxLabeled("VRE_DisablePhotophobiaMessage".Translate(), ref VRE_DisablePhotophobiaMessage, "VRE_DisablePhotophobiaMessage_Description".Translate());
           
            listingStandard.End();
            Widgets.EndScrollView();

            base.Write();

        }

    }


}
