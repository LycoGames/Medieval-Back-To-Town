using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Saving;
using UnityEditor;
using UnityEngine;

public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
{
    private List<QuestStatus> statuses = new List<QuestStatus>();

    public event Action onUpdate;

    public void AddQuest(Quest quest)
    {
        if (HasQuest(quest)) return;
        QuestStatus newStatus = new QuestStatus(quest);
        statuses.Add(newStatus);
        if (onUpdate != null)
        {
            onUpdate();
        }
    }

    public void CompleteObjective(Quest quest, string objective)
    {
        QuestStatus status = GetQuestStatus(quest);
        status.CompleteObjective(objective);
        if (status.IsComplete())
        {
            GiveReward(quest);
        }

        onUpdate?.Invoke();
    }

    public void UpdateKillObjectiveStatus(GameObject enemy, int value)
    {
        foreach (var questStatus in GetStatuses())
        {
            foreach (var objectiveStatus in questStatus.GetKillObjectives())
            {
                if (enemy.name.Substring(0, enemy.name.Length - 7) == objectiveStatus.Enemy.name)
                {
                    questStatus.UpdateKillObjectiveStatus(objectiveStatus.Reference, 1);
                }
            }
        }
        /*  var status = GetQuestStatus(quest);
          status.UpdateKillObjectiveStatus(objective, value);*/
    }

    public void UpdateCollectObjectiveStatus(Quest quest, string objective, int value)
    {
        var status = GetQuestStatus(quest);
        status.UpdateKillObjectiveStatus(objective, value);
    }

    public bool HasQuest(Quest quest)
    {
        return GetQuestStatus(quest) != null;
    }

    public IEnumerable<QuestStatus> GetStatuses()
    {
        return statuses;
    }

    private QuestStatus GetQuestStatus(Quest quest)
    {
        foreach (QuestStatus status in statuses)
        {
            if (status.GetQuest() == quest)
            {
                return status;
            }
        }

        return null;
    }

    private void GiveReward(Quest quest)
    {
        foreach (var reward in quest.GetRewards())
        {
            bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
            if (!success)
            {
                GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
            }
        }
    }

    public object CaptureState()
    {
        List<object> state = new List<object>();
        foreach (QuestStatus status in statuses)
        {
            state.Add(status.CaptureState());
        }

        return state;
    }

    public void RestoreState(object state)
    {
        List<object> stateList = state as List<object>;
        if (stateList == null) return;

        statuses.Clear();
        foreach (object objectState in stateList)
        {
            statuses.Add(new QuestStatus(objectState));
        }
    }

    public bool? Evaluate(string predicate, string[] parameters)
    {
        switch (predicate)
        {
            case "HasQuest":
                return HasQuest(Quest.GetByName(parameters[0]));
            case "CompletedQuest":
                return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
            case "HasKilledEnoughEnemy":
                return GetKillObjectiveResultWithEnemyName(parameters[0]);
        }

        return null;
    }

    private bool? GetKillObjectiveResultWithEnemyName(string parameter)
    {
        foreach (var status in statuses)
        {
            foreach (var objectiveStatus in status.GetKillObjectives())
            {
                if (objectiveStatus.Enemy.name == parameter)
                    return objectiveStatus.Number == status.GetObjectiveByReference(objectiveStatus.Reference).number;
            }

            return null;
        }

        return null;
    }
}