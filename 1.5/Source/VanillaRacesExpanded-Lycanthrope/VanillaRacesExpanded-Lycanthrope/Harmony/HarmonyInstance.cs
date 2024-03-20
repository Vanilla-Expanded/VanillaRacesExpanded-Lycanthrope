using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;
using System.Reflection.Emit;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Verse.AI;
using RimWorld.Planet;



namespace VanillaRacesExpandedLycanthrope
{

    public class VanillaRacesExpandedLycanthrope : Mod
    {
        public VanillaRacesExpandedLycanthrope(ModContentPack content) : base(content)
        {
            var harmony = new Harmony("com.vrelycans");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

}

