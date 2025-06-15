
using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;


namespace VanillaRacesExpandedLycanthrope
{

    public static class StaticCollectionsClass
    {

        // A list of maps and nunber of pack members on it
        public static Dictionary<Map, int> map_PackMembers = new Dictionary<Map, int>();
  

        public static void AddMapPackMembers(Map map, int number)
        {
            if (map != null)
            {
                if (!map_PackMembers.ContainsKey(map))
                {
                    map_PackMembers.Add(map, 0);
                }
                map_PackMembers[map] += number;
            }
        }
    }
}
