using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestObjective 
{
    //kills quests
    public int TargetKillCount;
    public MonsterPrototype.eMonsterID TargetType;

    //explore quests
    public int TargetExploreRoomsCount;
    [HideInInspector]
    public List<Room> ExploredRooms = new List<Room>();

    bool isComplete;
    public bool IsComplete { get { return isComplete; } }

    int ObjectiveCount;

    public bool CompleteObjective()
    {
        ObjectiveCount++;

        //kill quest
        if(TargetKillCount > 0 && ObjectiveCount >= TargetKillCount)
        {
            isComplete = true;
        }

        //explore
        if(TargetExploreRoomsCount > 0 && ObjectiveCount >= TargetExploreRoomsCount)
        {
            isComplete = true;
        }

        UIController.Instance.PlayerHUD.UpdateQuestText();

        return false;
    }

    public void Reset()
    {
        ObjectiveCount = 0;
        ExploredRooms.Clear();
        isComplete = false;
    }

    public string GetObjectiveText()
    {
        string retVal = "None";

        if(TargetKillCount > 0)
        {
            retVal = "Kill " + TargetType.ToString() + " : " + ObjectiveCount + " / " + TargetKillCount;
        }

        if(TargetExploreRoomsCount > 0)
        {
            retVal = "Explore Rooms : " + ExploredRooms.Count + " / " + TargetExploreRoomsCount;
        }

        return retVal;
    }
}