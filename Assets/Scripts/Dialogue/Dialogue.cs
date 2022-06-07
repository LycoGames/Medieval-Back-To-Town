using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private List<DialogueNode> nodes = new List<DialogueNode>();

    [SerializeField] private Vector2 newNodeOffset = new Vector2(250, 0);


    private Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();
    
    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        nodeLookup.Clear();
        foreach (DialogueNode node in GetAllNodes())
        {
            nodeLookup[node.name] = node;
        }
    }

    public IEnumerable<DialogueNode> GetAllNodes()
    {
        return nodes;
    }

    public DialogueNode GetRootNode()
    {
        return nodes[0];
    }

    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
    {
        foreach (string childID in parentNode.GetChildren())
        {
            if (nodeLookup.ContainsKey(childID))
            {
                yield return nodeLookup[childID];
            }
        }
    }

    public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode)
    {
        foreach (DialogueNode node in GetAllChildren(currentNode))
        {
            if (node.IsPlayerSpeaking())
            {
                yield return node;
            }
        }
    }

    public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode)
    {
        foreach (DialogueNode node in GetAllChildren(currentNode))
        {
            if (!node.IsPlayerSpeaking())
            {
                yield return node;
            }
        }
    }

#if UNITY_EDITOR
    public void CreateNode(DialogueNode parent)
    {
        DialogueNode newNode = MakeNode(parent);
        Undo.RegisterCreatedObjectUndo(newNode, "Created Dialogue Node");
        Undo.RecordObject(this, "Added Dialogue Node");
        AddNode(newNode);
    }


    public void DeleteNode(DialogueNode nodeToDelete)
    {
        Undo.RecordObject(this, "Delete Dialogue Node");
        nodes.Remove(nodeToDelete);
        OnValidate();
        CleanDanglingChildren(nodeToDelete);
        Undo.DestroyObjectImmediate(nodeToDelete);
    }

    private DialogueNode MakeNode(DialogueNode parent)
    {
        DialogueNode newNode = CreateInstance<DialogueNode>();
        newNode.name = Guid.NewGuid().ToString();
        if (parent != null)
        {
            parent.AddChild(newNode.name);
            newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
            newNode.SetPosition(parent.GetRect().position + newNodeOffset);
        }

        return newNode;
    }

    private void AddNode(DialogueNode newNode)
    {
        nodes.Add(newNode);
        OnValidate();
    }

    private void CleanDanglingChildren(DialogueNode nodeToDelete)
    {
        foreach (DialogueNode node in GetAllNodes())
        {
            node.RemoveChild(nodeToDelete.name);
        }
    }
#endif
    // About the save a file to harddrive
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        if (nodes.Count == 0)
        {
            DialogueNode newNode = MakeNode(null);
            AddNode(newNode);
        }

        if (AssetDatabase.GetAssetPath(this) != "")
        {
            foreach (DialogueNode node in GetAllNodes())
            {
                if (AssetDatabase.GetAssetPath(node) == "")
                {
                    AssetDatabase.AddObjectToAsset(node, this);
                }
            }
        }
#endif
    }

    //Load file from harddrive
    public void OnAfterDeserialize()
    {
    }
}