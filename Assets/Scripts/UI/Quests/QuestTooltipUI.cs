using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestTooltipUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform objectiveContainer;
    [SerializeField] private GameObject objectivePrefab;
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
            GameObject prefab = status.IsObjectiveComplete(objective.reference)
                ? objectivePrefab
                : objectiveIncompletePrefab;
            GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
            TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
            objectiveText.text = objective.description;
        }

        rewardText.text = GetRewardText(quest);
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