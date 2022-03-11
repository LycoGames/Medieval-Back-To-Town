using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    public string uniqueID;
    public string text;
    public string[] children;
    public Rect position;
}