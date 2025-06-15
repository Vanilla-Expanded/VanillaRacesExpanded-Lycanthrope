
using System.Collections.Generic;
using RimWorld;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace VanillaRacesExpandedLycanthrope
{
    public class QuestNode_Root_LycanShip : QuestNode
    {
        private const string QuestTag = "VRE_WolfmenTransport";

        private const int TicksToShuttleArrival = 180;

        private const int TicksToReinforcements = 10000;

        private const int TicksToBeginAssault = 5000;

        private static readonly SimpleCurve PointsToThrallCountCurve = new SimpleCurve
    {
        new CurvePoint(0f, 2f),
        new CurvePoint(2000f, 5f)
    };

        private static readonly SimpleCurve PointsToReinforcementsCountCurve = new SimpleCurve
    {
        new CurvePoint(2000f, 0f),
        new CurvePoint(2500f, 2f),
        new CurvePoint(5000f, 4f),
        new CurvePoint(8000f, 6f)
    };

        protected override void RunInt()
        {
            if (!ModLister.CheckBiotech("Sanguophage ship"))
            {
                return;
            }


            Quest quest = QuestGen.quest;
            Slate slate = QuestGen.slate;
            Map map = QuestGen_Get.GetMap();
           
            float x = slate.Get("points", 0f);

            string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID("VRE_WolfmenTransport");
            string attackedSignal = QuestGenUtility.HardcodedSignalWithQuestID("shuttlePawns.TookDamageFromPlayer");
            string defendTimeoutSignal = QuestGen.GenerateNewSignal("DefendTimeout");
            string beginAssaultSignal = QuestGen.GenerateNewSignal("BeginAssault");
            string assaultBeganSignal = QuestGen.GenerateNewSignal("AssaultBegan");
            slate.Set("map", map);
            List<FactionRelation> list = new List<FactionRelation>();
            foreach (Faction item3 in Find.FactionManager.AllFactionsListForReading)
            {
                list.Add(new FactionRelation(item3, FactionRelationKind.Neutral));
            }
            Faction faction = FactionGenerator.NewGeneratedFactionWithRelations(InternalDefOf.VRE_WolfmenFaction, list, hidden: true);
            faction.temporary = true;
            Find.FactionManager.Add(faction);
            quest.ReserveFaction(faction);
            List<Pawn> shuttlePawns = new List<Pawn>();
            Pawn pawn = quest.GeneratePawn(new PawnGenerationRequest(InternalDefOf.VRE_WolfmenPawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: true, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: true));
            pawn.health.forceDowned = true;
            shuttlePawns.Add(pawn);
            slate.Set("lycan", pawn);
            int num = Mathf.RoundToInt(PointsToThrallCountCurve.Evaluate(x));
            PawnGenerationRequest request = new PawnGenerationRequest(InternalDefOf.VRE_WolfmenPawnKindDef, faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: true, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: true);
            for (int i = 0; i < num; i++)
            {
                Pawn item = quest.GeneratePawn(request);
                shuttlePawns.Add(item);
            }
            slate.Set("WolfmenCount", num);
            slate.Set("shuttlePawns", shuttlePawns);
            Thing thing = ThingMaker.MakeThing(ThingDefOf.ShuttleCrashed_Exitable);
            quest.SetFaction(Gen.YieldSingle(thing), faction);
            TryFindShuttleCrashPosition(map, thing.def.size, out var shuttleCrashPosition);
            TransportShip transportShip = quest.GenerateTransportShip(TransportShipDefOf.Ship_ShuttleCrashing, shuttlePawns, thing).transportShip;
            quest.AddShipJob_WaitTime(transportShip, 60, leaveImmediatelyWhenSatisfied: false).showGizmos = false;
            quest.AddShipJob(transportShip, ShipJobDefOf.Unload);
            QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);


            quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, useColonistsFromCaravanArg: false, QuestPart.SignalListenMode.OngoingOnly, shuttlePawns, filterDeadPawnsFromLookTargets: false, "[wolfmanShuttleCrashedLetterText]", null, "[wolfmanShuttleCrashedLetterLabel]");
            quest.AddShipJob_Arrive(transportShip, map.Parent, null, shuttleCrashPosition, ShipJobStartMode.Force_DelayCurrent, faction);
            quest.DefendPoint(map.Parent, pawn,shuttleCrashPosition, shuttlePawns, faction, null, null, 5f);




            

            string text = QuestGenUtility.HardcodedSignalWithQuestID("shuttlePawns.TookDamageFromPlayer");
            string text2 = QuestGenUtility.HardcodedSignalWithQuestID("shuttlePawns.Arrested");
            string text3 = QuestGenUtility.HardcodedSignalWithQuestID("shuttlePawns.Killed");
            string inSignal = QuestGenUtility.HardcodedSignalWithQuestID("shuttlePawns.LeftMap");
            quest.AnySignal(new string[3] { text,text2,text3 }, delegate
            {

                quest.Letter(LetterDefOf.NegativeEvent, null, null, null, null, useColonistsFromCaravanArg: false, QuestPart.SignalListenMode.OngoingOnly, shuttlePawns, filterDeadPawnsFromLookTargets: false, "[letterTextBeggarsBetrayed]", null, "[letterLabelBeggarsBetrayed]");
                QuestPart_FactionRelationChange part = new QuestPart_FactionRelationChange
                {
                    faction = faction,
                    relationKind = FactionRelationKind.Hostile,
                    canSendHostilityLetter = false,
                    inSignal = QuestGen.slate.Get<string>("inSignal")
                };
                quest.AddPart(part);
                quest.End(QuestEndOutcome.Fail, 0, null, null);

            });
            

            quest.Delay(30000, delegate
            {
                
             
                quest.Leave(shuttlePawns, null, sendStandardLetter: false, leaveOnCleanup: false);
              

            }, null);

            quest.AllPawnsDespawned(shuttlePawns, delegate
            {
                QuestGen_End.End(quest, QuestEndOutcome.Success);
            }, null, inSignal);


        }

        protected override bool TestRunInt(Slate slate)
        {
            Map map = QuestGen_Get.GetMap();
            if (map == null)
            {
                return false;
            }



            if (!TryFindShuttleCrashPosition(map, ThingDefOf.ShuttleCrashed.size, out var _))
            {
                return false;
            }
            return true;
        }

        private bool TryFindShuttleCrashPosition(Map map, IntVec2 size, out IntVec3 spot)
        {
            if (DropCellFinder.FindSafeLandingSpot(out spot, null, map, 35, 15, 25, size, ThingDefOf.ShuttleCrashed.interactionCellOffset))
            {
                return true;
            }
            return false;
        }
    }
}