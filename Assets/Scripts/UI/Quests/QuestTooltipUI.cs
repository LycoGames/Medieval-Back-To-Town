using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform objectiveContainer;
    [SerializeField] private GameObject objectivePrefab;
    [SerializeField] private GameObject objectiveWithNumberPrefab;
    [SerializeField] private GameObject objectiveWithNumberInCompletePrefab;
    [SerializeField] private GameObject objectiveIncompletePrefab;
    [SerializeField] TextMeshProUGUI rewardText;

    public void Setup(QuestStatus status)
    {
        Quest quest = status.GetQuest();
        title.text = quest.GetTitle();

        foreach (Transform item in objectiveContainer)
        {
            Destroy(item.gameObject);
        }

        foreach (var objective in quest.GetObjectives())
        {
            GameObject prefab;
            if (objective.isCollectItemQuest ||
                objective.isKillEnemyQuest && status.IsObjectiveComplete(objective.reference))
            {
                prefab = objectiveWithNumberPrefab;
            }
            else if (objective.isCollectItemQuest ||
                     objective.isKillEnemyQuest && !status.IsObjectiveComplete(objective.reference))
            {
                prefab = objectiveWithNumberInCompletePrefab;
            }
            else if (status.IsObjectiveComplete(objective.reference))
                prefab = objectivePrefab;
            else
                prefab = objectiveIncompletePrefab;

            GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);

            if (objective.isCollectItemQuest)
            {
                TextMeshProUGUI[] textList = objectiveInstance.GetComponentsInChildren<TextMeshProUGUI>();
                textList[0].text = objective.description;
                //textList[1].text = status.GetCollectedItemCount(objective.reference);
                textList[1].text = GetCollectedItemCount(objective);
            }
            else if (objective.isKillEnemyQuest)
            {
                TextMeshProUGUI[] textList = objectiveInstance.GetComponentsInChildren<TextMeshProUGUI>();
                textList[0].text = objective.description;
                //textList[1].text = status.GetKilledEnemyCount(objective.reference);
                textList[1].text = GetKilledEnemyCount(objective);
            }
            else
            {
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
            }
        }

        rewardText.text = GetRewardText(quest);
    }

    private string GetKilledEnemyCount(Quest.Objective objective)
    {
        throw new System.NotImplementedException();
        //TODO
    }

    private string GetCollectedItemCount(Quest.Objective objective)
    {
        Inventory playerInventory = FindObjectOfType<Inventory>();
        if (playerInventory.HasItem(objective.itemToCollect))
        {
            int itemCount = playerInventory.GetNumberInSlot(playerInventory.GetSlot(objective.itemToCollect));
            return itemCount + "/" + objective.quantity;
        }

        return "0/" + objective.quantity;
    }

    private string GetRewardText(Quest quest)
    {
        var rewardTextString = "";
        foreach (var reward in quest.GetRewards())
        {
            Debug.Log(reward.item.GetDisplayName());
            if (rewardTextString != "")
            {
                rewardTextString += ", ";
            }

            if (reward.number > 1)
            {
                rewardTextString += reward.number + " ";
            }

            rewardTextString += reward.item.GetDisplayName();
        }

        if (rewardTextString == "")
        {
            rewardTextString = "No reward";
        }

        rewardTextString += ".";
        return rewardTextString;
    }
}