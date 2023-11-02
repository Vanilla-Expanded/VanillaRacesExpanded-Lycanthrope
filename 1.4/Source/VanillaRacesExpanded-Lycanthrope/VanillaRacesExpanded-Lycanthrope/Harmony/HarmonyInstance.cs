using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;
using HarmonyLib;
using System.Reflection;

namespace VanillaRacesExpandedLycanthrope
{

    //Setting the Harmony instance
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.VELycanthropes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }


    }

}
