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
    [SerializeField] private string playerName;
    [SerializeField] float checkAIConversantRadius = 5f;
    [SerializeField] private float canInteractDistance = 3f;
    [SerializeField] private Vector2 Offset = new Vector2(0, 2);
    [SerializeField] private float targetingSense = 3f;

    [SerializeField] private List<AIConversant> nearAIConversants = new List<AIConversant>();

    private Dialogue currentDialogue;
    private DialogueNode currentNode = null;
    private bool isChoosing = false;

    private LayerMask NPCLayerMask;

    private StateMachine stateMachine;
    private WStateMachine wStateMachine;

    public event Action onConversationUpdated;

    //AI Conversant Interface Variables
    private Vector3 screenCenter;
    private Vector3 screenPos;
    private Vector3 cornerDistance;
    private Vector3 absCornerDistance;
    private Vector3 worldViewField;

    private Camera cam;

    private AIConversant activeAIConservant = null;
    private Transform activeAIConservantTransform = null;
    private NPCInteractUI npcUI;

    private void Start()
    {
        NPCLayerMask = LayerMask.GetMask("NPC");
        cam = Camera.main;
        stateMachine = GetComponent<StateMachine>();
        if (stateMachine == null)
            wStateMachine = GetComponent<WStateMachine>();
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
        TriggerEnterAction();
        onConversationUpdated();
    }

    public void Quit()
    {
        currentDialogue = null;
        TriggerExitAction();
        currentNode = null;
        isChoosing = false;
        activeAIConservant = null;
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

    public AIConversant GetInteractableNPC()
    {
        if (stateMachine == null)
        {
            return wStateMachine.InteractableNpc;
        }

        return stateMachine.InteractableNpc;
    }

    public IEnumerable<DialogueNode> GetChoices()
    {
        return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
    }

    public void SelectChoice(DialogueNode chosenNode)
    {
        currentNode = chosenNode;
        TriggerEnterAction();
        isChoosing = false;
        Next(); // Seçenek seçildikten sonra doğrudan ai responsa a geç yoksa seçilen şık ai response gibi gösterilir.
    }

    public void Next()
    {
        int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
        if (numPlayerResponses > 0)
        {
            isChoosing = true;
            TriggerExitAction();
            onConversationUpdated();
            return;
        }

        DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
        int randomIndex = Random.Range(0, children.Length);
        TriggerExitAction();
        currentNode = children[randomIndex];
        TriggerEnterAction();
        onConversationUpdated();
    }


    public void ResetUI()
    {
        npcUI.SetActiveInteract(false);
        npcUI.SetActiveInteractInfo(false);
    }

    private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
    {
        foreach (var node in inputNode)
        {
            if (node.CheckCondition(GetEvaluators()))
            {
                yield return node;
            }
        }
    }

    private IEnumerable<IPredicateEvaluator> GetEvaluators()
    {
        return GetComponents<IPredicateEvaluator>();
    }

    public bool HasNext()
    {
        return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Any();
    }

    private void TriggerEnterAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.GetOnEnterAction());
        }
    }

    private void TriggerExitAction()
    {
        if (currentNode != null)
        {
            TriggerAction(currentNode.GetOnExitAction());
        }
    }

    private void TriggerAction(string action)
    {
        if (action == "") return;

        foreach (DialogueTrigger trigger in activeAIConservant.GetComponents<DialogueTrigger>())
        {
            trigger.Trigger(action);
        }
    }

    public string GetCurrentConversantName()
    {
        return isChoosing ? playerName : activeAIConservant.GetConversantName();
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


        if (activeAIConservant && activeAIConservant != nearAIConversants[TargetIndex()])
        {
            ResetUI();
        }


        activeAIConservant = nearAIConversants[TargetIndex()];
        activeAIConservantTransform = activeAIConservant.transform;
        npcUI = activeAIConservant.GetComponentInChildren<NPCInteractUI>();

        screenPos = cam.WorldToScreenPoint(activeAIConservantTransform.position + (Vector3) Offset);
        cornerDistance = screenPos - screenCenter;
        absCornerDistance = new Vector3(Mathf.Abs(cornerDistance.x), Mathf.Abs(cornerDistance.y),
            Mathf.Abs(cornerDistance.z));

        if (absCornerDistance.x < screenCenter.x / targetingSense &&
            absCornerDistance.y < screenCenter.y / targetingSense && screenPos.x > 0 &&
            screenPos.y > 0 && screenPos.z > 0 //If target is in the middle of the screen
            && !Physics.Linecast(transform.position + (Vector3) Offset,
                activeAIConservantTransform.position + (Vector3) Offset * 2, 5))
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, activeAIConservantTransform.position)) <
                canInteractDistance && activeAIConservant.CanInteractable())
            {
                npcUI.SetActiveInteract(true);
                if (stateMachine != null)
                    stateMachine.InteractableNpc = activeAIConservant.GetComponent<AIConversant>();
                else
                    wStateMachine.InteractableNpc = activeAIConservant.GetComponent<AIConversant>();
            }
            else if (activeAIConservant.CanInteractable())
            {
                npcUI.SetActiveInteract(false);
                npcUI.SetActiveInteractInfo(true);
                if (stateMachine != null)
                    stateMachine.InteractableNpc = null;
                else
                    wStateMachine.InteractableNpc = null;
            }
        }
        else
        {
            npcUI.SetActiveInteract(false);
            npcUI.SetActiveInteractInfo(false);
            if (stateMachine != null)
                stateMachine.InteractableNpc = null;
            else
                wStateMachine.InteractableNpc = null;
        }
    }
}