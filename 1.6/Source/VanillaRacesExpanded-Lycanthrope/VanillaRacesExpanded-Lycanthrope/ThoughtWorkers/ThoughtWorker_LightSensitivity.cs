
using RimWorld;
using Verse;
namespace VanillaRacesExpandedLycanthrope
{
    public class ThoughtWorker_LightSensitivity : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            
            if (p.Spawned && p.Map.glowGrid.GroundGlowAt(p.Position) >= 0.11f)
            {
                return true;
            }
            return ThoughtState.Inactive;
        }
    }
}
