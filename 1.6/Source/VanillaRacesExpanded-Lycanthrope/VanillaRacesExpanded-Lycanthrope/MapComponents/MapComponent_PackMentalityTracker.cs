using System;
using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace VanillaRacesExpandedLycanthrope
{
    public class MapComponent_PackMentalityTracker : MapComponent
    {

        public int tickCounter = 0;
        public int tickInterval = 12000;
        public static Dictionary<Map, int> map_PackMembers_backup = new Dictionary<Map, int>();
        List<Map> list2;
        List<int> list3;

        public MapComponent_PackMentalityTracker(Map map) : base(map)
        {

        }

        public override void FinalizeInit()
        {
            StaticCollectionsClass.map_PackMembers = map_PackMembers_backup;
            base.FinalizeInit();
        }

        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_Collections.Look(ref map_PackMembers_backup, "map_PackMembers_backup", LookMode.Reference, LookMode.Value, ref list2, ref list3);

            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounter_map_PackMembers", 0, true);

        }

        

        public override void MapComponentTick()
        {

            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                StaticCollectionsClass.map_PackMembers.Clear();
              
                foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
                {
                    if (Utils.ContainsAnyRelationOfDef(pawn,InternalDefOf.VRE_PackMember))
                    {
                        StaticCollectionsClass.AddMapPackMembers(this.map,1);
                    }

                }



                map_PackMembers_backup = StaticCollectionsClass.map_PackMembers;

                tickCounter = 0;
            }



        }

      


    }


}



