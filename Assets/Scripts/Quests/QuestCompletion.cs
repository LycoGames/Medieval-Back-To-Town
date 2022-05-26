using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompletion : MonoBehaviour
{
    [SerializeField] private List<CompletionData> completionDatas;

    [Serializable]
    private struct CompletionData
    {
        public Quest quest;
        public string objective;
    }

    public void CompleteObjective()
    {
        QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        foreach (CompletionData data in completionDatas)
        {
            if (!questList.HasQuest(data.quest)) continue;
            if (questList.GetCompletedQuests().Contains(data.quest)) continue;
            questList.CompleteObjective(data.quest, data.objective);
            string conversantName = transform.GetComponent<AIConversant>().GetConversantName();
            conversantName = conversantName.Replace(" ", "");
            string compassName = "CompassPOI" + conversantName + " Variant(Clone)";
            GameObject compassPOI = GameObject.Find(compassName);
            if (compassPOI)
                Destroy(compassPOI);
            return;
        }
    }
}