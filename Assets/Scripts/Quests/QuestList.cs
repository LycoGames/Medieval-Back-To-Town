using System;
using System.Collections.Generic;
using System.Linq;
using CompassNavigatorPro;
using RPG.Saving;
using UnityEditor;
using UnityEngine;

public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
{
    private List<QuestStatus> statuses = new List<QuestStatus>();

    private List<Quest> completedQuests = new List<Quest>();

    public event Action onUpdate;

    public void AddQuest(Quest quest)
    {
        if (HasQuest(quest)) return;
        QuestStatus newStatus = new QuestStatus(quest);
        UpdateNavigations(newStatus);
        statuses.Add(newStatus);
        if (onUpdate != null)
        {
            onUpdate();
        }
    }

    private void UpdateNavigations(QuestStatus newStatus)
    {
        foreach (var objective in newStatus.GetObjectives())
        {
            if (objective.compassProPOINpc == null) continue;
            if (objective.isCollectItemQuest || objective.isKillEnemyQuest)
            {
                if (objective.compassProPOILocation == null)
                    continue;
                CreateLocationPOI(objective);
            }
            else
            {
                if (objective.compassProPOINpc == null)
                    continue;
                CreateNPCPOI(objective);
            }
        }
    }

    private void CreateNPCPOI(Quest.Objective objective)
    {
        GameObject compassPOIInstance =
            Instantiate(objective.compassProPOINpc, objective.compassProPOINpc.transform.position, Quaternion.identity,
                GameObject.FindGameObjectWithTag("POIParent").transform);
    }

    private void CreateLocationPOI(Quest.Objective objective)
    {
        GameObject compassPOIInstance =
            Instantiate(objective.compassProPOINpc, objective.compassProPOINpc.transform.position, Quaternion.identity,
                GameObject.FindGameObjectWithTag("POIParent").transform);
    }

    public void CompleteObjective(Quest quest, string objective)
    {
        QuestStatus status = GetQuestStatus(quest);
        status.CompleteObjective(objective);
        if (status.IsComplete())
        {
            GiveReward(quest);
            completedQuests.Add(quest);
        }

        onUpdate?.Invoke();
    }

    public void UpdateKillObjectiveStatus(GameObject enemy, int value)
    {
        foreach (var questStatus in GetStatuses())
        {
            if (!questStatus.HasKillObjectives())
                return;
            foreach (var objectiveStatus in questStatus.GetKillObjectives())
            {
                if (enemy.name.Substring(0, enemy.name.Length - 7) == objectiveStatus.Enemy.name)
                {
                    if (questStatus.UpdateKillObjectiveStatus(objectiveStatus.Reference, 1))
                    {
                        GameObject prefab = questStatus.GetObjectiveByReference(objectiveStatus.Reference)
                            .compassProPOINpc;
                        Instantiate(prefab, prefab.transform.position, Quaternion.identity,
                            GameObject.FindGameObjectWithTag("POIParent").transform);
                        string compassName = "CompassPOI" + questStatus.GetQuest().name + " Variant(Clone)";
                        GameObject compassPOI = GameObject.Find(compassName);
                        if (compassPOI)
                            Destroy(compassPOI);
                    }
                }
            }
        }
        /*  var status = GetQuestStatus(quest);
          status.UpdateKillObjectiveStatus(objective, value);*/
    }

    public void UpdateCollectObjectiveStatus(InventoryItem item, int value)
    {
        foreach (var questStatus in GetStatuses())
        {
            if (!questStatus.HasCollectObjectives())
                return;
            foreach (var objectiveStatus in questStatus.GetCollectObjectives())
            {
                if (item == objectiveStatus.Item)
                {
                    if (questStatus.UpdateCollectObjectiveStatus(objectiveStatus.Reference, value))
                    {
                    }
                }
            }
        }
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
            EquipableItem equipableItem = reward.item as EquipableItem;
            if (equipableItem)
            {
                if (equipableItem.GetAllowedCharacterClasses().Length > 0 &&
                    equipableItem.GetAllowedCharacterClasses()[0] == CharacterClass.Warrior &&
                    PlayerPrefs.GetString("CharacterPref") == "Archer")
                    continue;
                if (equipableItem.GetAllowedCharacterClasses().Length > 0 &&
                    equipableItem.GetAllowedCharacterClasses()[0] == CharacterClass.Archer &&
                    PlayerPrefs.GetString("CharacterPref") == "Warrior")
                    continue;
            }

            bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
            if (!success)
            {
                GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
            }
        }
    }

    public List<Quest> GetCompletedQuests()
    {
        return completedQuests;
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
            case "CompletedObjective":
                return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
            case "CompletedQuest":
                return completedQuests.Contains(Quest.GetByName(parameters[0]));
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