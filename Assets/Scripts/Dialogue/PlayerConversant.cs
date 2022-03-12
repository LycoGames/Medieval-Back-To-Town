using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerConversant : MonoBehaviour
{
    [SerializeField] float checkAIConversantRadius = 5f;
    [SerializeField] private float canInteractDistance = 3f;
    [SerializeField] private Vector2 Offset = new Vector2(0, 2);
    [SerializeField] private float targetingSense = 3f;

    [SerializeField] private List<AIConversant> nearAIConversants = new List<AIConversant>();

    private Dialogue currentDialogue;
    private DialogueNode currentNode = null;
    private bool isChoosing = false;

    private LayerMask NPCLayerMask;

    public event Action onConversationUpdated;

    //AI Conversant Interface Variables
    private Vector3 screenCenter;
    private Vector3 screenPos;
    private Vector3 cornerDistance;
    private Vector3 absCornerDistance;
    private Vector3 worldViewField;

    private Camera cam;

    private void Start()
    {
        NPCLayerMask = LayerMask.GetMask("NPC");
        cam = Camera.main;
    }

    private void Update()
    {
        CheckNearAIConversants();
        if (nearAIConversants.Any())
            PlayerConservantUserInterface();
    }

    public void StartDialogue(Dialogue newDialogue)
    {
        currentDialogue = newDialogue;
        currentNode = currentDialogue.GetRootNode();
        onConversationUpdated();
    }

    public void Quit()
    {
        currentDialogue = null;
        currentNode = null;
        isChoosing = false;
        onConversationUpdated();
    }

    public bool IsActive()
    {
        return currentDialogue != null;
    }

    public bool IsChoosing()
    {
        return isChoosing;
    }

    public string GetText()
    {
        if (currentNode == null)
        {
            return "";
        }

        return currentNode.GetText();
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
        return currentDialogue.GetPlayerChildren(currentNode);
    }

    public void SelectChoice(DialogueNode chosenNode)
    {
        currentNode = chosenNode;
        isChoosing = false;
        Next(); // Seçenek seçildikten sonra doğrudan ai responsa a geç yoksa seçilen şık ai response gibi gösterilir.
    }

    public void Next()
    {
        int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
        if (numPlayerResponses > 0)
        {
            isChoosing = true;
            onConversationUpdated();
            return;
        }

        DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
        int randomIndex = Random.Range(0, children.Length);
        currentNode = children[randomIndex];
        onConversationUpdated();
    }

    public bool HasNext()
    {
        return currentDialogue.GetAllChildren(currentNode).Any();
    }

    private void CheckNearAIConversants()
    {
        Collider[] hitColliders =
            Physics.OverlapSphere(transform.position, checkAIConversantRadius,
                NPCLayerMask);

        AIConversant aiConversant;

        foreach (AIConversant item in nearAIConversants)
        {
            item.GetComponentInChildren<NPCInteractUI>().SetActiveInteractInfo(false);
        }

        nearAIConversants.Clear();

        foreach (var item in hitColliders)
        {
            aiConversant = item.gameObject.GetComponent<AIConversant>();
            if (!nearAIConversants.Contains(aiConversant))
            {
                nearAIConversants.Add(aiConversant);
            }
        }
    }

    public int TargetIndex()
    {
        int[] distances = new int[nearAIConversants.Count];
        for (int i = 0; i < nearAIConversants.Count; i++)
        {
            distances[i] = (int) Vector2.Distance(
                cam.WorldToScreenPoint(nearAIConversants[i].transform.position),
                new Vector2(Screen.width / 2, Screen.height / 2));
        }

        int minDistance = Mathf.Min(distances);
        int index = 0;
        for (int i = 0; i < distances.Length; i++)
        {
            if (minDistance == distances[i])
                index = i;
        }

        return index;
    }

    private void PlayerConservantUserInterface()
    {
        screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

        Transform activeAIConservant = nearAIConversants[TargetIndex()].transform;

        screenPos = cam.WorldToScreenPoint(activeAIConservant.position + (Vector3) Offset);
        cornerDistance = screenPos - screenCenter;
        absCornerDistance = new Vector3(Mathf.Abs(cornerDistance.x), Mathf.Abs(cornerDistance.y),
            Mathf.Abs(cornerDistance.z));

        NPCInteractUI npcUI = activeAIConservant.GetComponentInChildren<NPCInteractUI>();

        if (absCornerDistance.x < screenCenter.x / targetingSense &&
            absCornerDistance.y < screenCenter.y / targetingSense && screenPos.x > 0 &&
            screenPos.y > 0 && screenPos.z > 0 //If target is in the middle of the screen
            && !Physics.Linecast(transform.position + (Vector3) Offset,
                activeAIConservant.position + (Vector3) Offset * 2, 5))
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, activeAIConservant.transform.position)) <
                canInteractDistance)
            {
                npcUI.SetActiveInteract(true);
            }
            else
            {
                npcUI.SetActiveInteract(false);
                npcUI.SetActiveInteractInfo(true);
            }
        }
        else
        {
            npcUI.SetActiveInteract(false);
            npcUI.SetActiveInteractInfo(false);
        }
    }
}