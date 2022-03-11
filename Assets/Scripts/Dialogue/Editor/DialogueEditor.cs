using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
    private Dialogue selectedDialogue = null;
    private GUIStyle nodeStyle;

    [MenuItem(("Window/Dialogue Editor"))]
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }

    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
        if (dialogue != null)
        {
            ShowEditorWindow();
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChange;

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        nodeStyle.normal.textColor = Color.white;
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

    private void OnSelectionChange()
    {
        Dialogue newDialogue = Selection.activeObject as Dialogue;
        if (newDialogue)
        {
            selectedDialogue = newDialogue;
            Repaint();
        }
    }

    private void OnGUI()
    {
        if (selectedDialogue)
        {
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                OnGUINode(node);
            }
        }
        else
        {
            EditorGUILayout.LabelField("No Dialogue Selected.");
        }
    }

    private void OnGUINode(DialogueNode node)
    {
        GUILayout.BeginArea(node.position, nodeStyle);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);
        string newText = EditorGUILayout.TextField(node.text);
        string newUniqueID = EditorGUILayout.TextField(node.uniqueID);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
            node.text = newText;
            node.uniqueID = newUniqueID;
        }

        GUILayout.EndArea();
    }
}