using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCompletion : MonoBehaviour
{
    [SerializeField] private Quest quest;
    [SerializeField] private string objective;


    public void CompleteObjective()
    {
        QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
        questList.CompleteObjective(quest, objective);
        string conversantName = gameObject.transform.parent.GetComponent<AIConversant>().GetConversantName();
        conversantName = conversantName.Replace(" ", "");
        string compassName = "CompassPOI" + conversantName;
        GameObject compassPOI = GameObject.Find("compassName");
        if (compassPOI)
            Destroy(compassPOI);
    }
}