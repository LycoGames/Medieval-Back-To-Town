using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private Quest[] quests;

    public void GiveQuest()
    {
        QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        foreach (var quest in quests)
        {
            if (questList.HasQuest(quest) || questList.GetCompletedQuests().Contains(quest))
            {
                continue;
            }

            string conversantName = transform.GetComponent<AIConversant>().GetConversantName();
            conversantName = conversantName.Replace(" ", "");
            string compassName = "CompassPOI" + conversantName + " Variant(Clone)";
            GameObject compassPOI = GameObject.Find(compassName);
            if (compassPOI)
                Destroy(compassPOI);
            questList.AddQuest(quest);
            return;
        }
    }
}