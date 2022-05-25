using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestStatus
{
    private Quest quest;
    private List<CollectObjectiveStatus> collectObjectiveStatuses;
    private List<KillObjectiveStatus> killObjectiveStatuses;
    private List<string> completedObjectives = new List<string>();


    public class CollectObjectiveStatus
    {
        public string Reference { get; set; }
        public InventoryItem Item { get; set; }
        public int Number { get; set; }
    }

    public class KillObjectiveStatus
    {
        public string Reference { get; set; }
        public GameObject Enemy { get; set; }
        public int Number { get; set; }
    }


    [System.Serializable]
    class QuestStatusRecord
    {
        public string questName;
        public List<string> completedObjective;
    }

    public QuestStatus(Quest quest)
    {
        this.quest = quest;
        foreach (var objective in quest.GetObjectives())
        {
            if (objective.isCollectItemQuest)
            {
                collectObjectiveStatuses ??= new List<CollectObjectiveStatus>();
                collectObjectiveStatuses.Add(SetupCollectObjectiveStatus(objective));
            }
            else if (objective.isKillEnemyQuest)
            {
                killObjectiveStatuses ??= new List<KillObjectiveStatus>();
                killObjectiveStatuses.Add(SetupKillObjectiveStatus(objective));
            }
        }
    }


    public QuestStatus(object objectState)
    {
        QuestStatusRecord state = objectState as QuestStatusRecord;
        quest = Quest.GetByName(state.questName);
        completedObjectives = state.completedObjective;
    }

    public Quest GetQuest()
    {
        return quest;
    }

    public IEnumerable<Quest.Objective> GetObjectives()
    {
        return quest.GetObjectives();
    }

    public int GetCompletedCount()
    {
        return completedObjectives.Count;
    }

    public bool IsObjectiveComplete(string objective)
    {
        return completedObjectives.Contains(objective);
    }

    public void CompleteObjective(string objective)
    {
        if (quest.HasObjective(objective))
        {
            completedObjectives.Add(objective);
        }
    }

    public bool UpdateKillObjectiveStatus(string objective, int value)
    {
        foreach (var objectiveStatus in killObjectiveStatuses.Where(objectiveStatus =>
                     objectiveStatus.Reference == objective))
        {
            if (GetObjectiveByReference(objective).number > objectiveStatus.Number)
            {
                objectiveStatus.Number += value;
                return GetObjectiveByReference(objective).number == objectiveStatus.Number;
            }
        }

        return false;
    }


    public bool UpdateCollectObjectiveStatus(string objective, int value)
    {
        foreach (var objectiveStatus in collectObjectiveStatuses.Where(objectiveStatus =>
                     objectiveStatus.Reference == objective))
        {
            objectiveStatus.Number += value;
            return GetObjectiveByReference(objective).quantity == objectiveStatus.Number;
        }

        return false;
    }

    public int GetKillObjectiveStatus(string objective)
    {
        foreach (var objectiveStatus in killObjectiveStatuses.Where(objectiveStatus =>
                     objectiveStatus.Reference == objective))
        {
            return objectiveStatus.Number;
        }

        return -1;
    }

    public IEnumerable<KillObjectiveStatus> GetKillObjectives()
    {
        return killObjectiveStatuses;
    }

    public IEnumerable<CollectObjectiveStatus> GetCollectObjectives()
    {
        return collectObjectiveStatuses;
    }

    public Quest.Objective GetObjectiveByReference(string reference)
    {
        foreach (var objective in quest.GetObjectives())
        {
            if (objective.reference == reference)
                return objective;
        }

        return null;
    }

    public int GetCollectObjectiveStatus(string objective)
    {
        foreach (var objectiveStatus in collectObjectiveStatuses.Where(objectiveStatus =>
                     objectiveStatus.Reference == objective))
        {
            return objectiveStatus.Number;
        }

        return -1;
    }

    public bool IsComplete()
    {
        foreach (var objective in quest.GetObjectives())
        {
            if (!completedObjectives.Contains(objective.reference))
            {
                return false;
            }
        }

        return true;
    }

    public object CaptureState()
    {
        var state = new QuestStatusRecord();
        state.questName = quest.name;
        state.completedObjective = completedObjectives;
        return state;
    }

    private CollectObjectiveStatus SetupCollectObjectiveStatus(Quest.Objective objective)
    {
        var collectObjectiveStatus = new CollectObjectiveStatus
        {
            Reference = objective.reference,
            Item = objective.itemToCollect,
        };
        return collectObjectiveStatus;
    }

    private KillObjectiveStatus SetupKillObjectiveStatus(Quest.Objective objective)
    {
        var killObjectiveStatus = new KillObjectiveStatus
        {
            Reference = objective.reference,
            Enemy = objective.enemy,
        };
        return killObjectiveStatus;
    }

    private bool CanCompleteCollectObjective(string reference)
    {
        int collectedItem = GetCollectObjectiveStatus(reference);
        Quest.Objective objective = GetObjectiveByReference(reference);
        return collectedItem >= objective.quantity;
    }

    public bool? Evaluate(string predicate, string[] parameters)
    {
        switch (predicate)
        {
            case "HasEnoughInventoryItem":
                return CanCompleteCollectObjective(parameters[0]);
        }

        return null;
    }

    public bool HasKillObjectives()
    {
        return killObjectiveStatuses != null;
    }

    public bool HasCollectObjectives()
    {
        return collectObjectiveStatuses != null;
    }
}