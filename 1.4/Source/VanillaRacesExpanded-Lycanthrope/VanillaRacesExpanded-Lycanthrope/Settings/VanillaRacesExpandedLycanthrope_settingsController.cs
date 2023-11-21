
using RimWorld;
using UnityEngine;
using Verse;


namespace VanillaRacesExpandedLycanthrope
{




    public class VanillaRacesExpandedLycanthrope_Mod : Mod
    {
        public static VanillaRacesExpandedLycanthrope_Settings settings;

        public VanillaRacesExpandedLycanthrope_Mod(ModContentPack content) : base(content)
        {
            settings = GetSettings<VanillaRacesExpandedLycanthrope_Settings>();
        }
        public override string SettingsCategory() => "VRE - Lycanthrope";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }





    }
}

