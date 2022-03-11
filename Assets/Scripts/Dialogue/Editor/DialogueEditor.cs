using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
    private Dialogue selectedDialogue = null;
    [NonSerialized] private GUIStyle nodeStyle;
    [NonSerialized] private GUIStyle playerNodeStyle;
    [NonSerialized] private DialogueNode draggingNode = null;
    [NonSerialized] private Vector2 draggingOffset;
    [NonSerialized] private DialogueNode creatingNode = null;
    [NonSerialized] private DialogueNode deletingNode = null;
    [NonSerialized] private DialogueNode linkingParentNode = null;
    private Vector2 scrollPosition;
    [NonSerialized] private bool draggingCanvas = false;
    [NonSerialized] private Vector2 draggingCanvasOffset;

    private const float canvasSize = 4000;
    private const float backgroundSize = 50;

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

        playerNodeStyle = new GUIStyle();
        playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        playerNodeStyle.normal.textColor = Color.white;
        playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
        playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

    private void OnSelectionChange()
    {
        Dialogue newDialogue = Selection.activeObject as Dialogue;
        if (newDialogue != null)
        {
            selectedDialogue = newDialogue;
            Repaint();
        }
    }

    private void OnGUI()
    {
        if (selectedDialogue)
        {
            ProcessEvents();

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

            Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
            Texture2D backgroundTex = Resources.Load("background") as Texture2D;
            Rect texCoords = new Rect(0, 0, canvasSize / backgroundSize, canvasSize / backgroundSize);
            GUI.DrawTextureWithTexCoords(canvas, backgroundTex, texCoords);

            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }

            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
            }

            EditorGUILayout.EndScrollView();

            if (creatingNode != null)
            {
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
            }

            if (deletingNode != null)
            {
                selectedDialogue.DeleteNode(deletingNode);
                deletingNode = null;
            }
        }
        else
        {
            EditorGUILayout.LabelField("No Dialogue Selected.");
        }
    }


    private void ProcessEvents()
    {
        if (Event.current.type == EventType.MouseDown && draggingNode == null)
        {
            draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
            if (draggingNode != null)
            {
                draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                Selection.activeObject = draggingNode;
            }
            else
            {
                draggingCanvas = true;
                draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                Selection.activeObject = selectedDialogue;
            }
        }
        else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
        {
            draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);
            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
        {
            scrollPosition = draggingCanvasOffset - Event.current.mousePosition;
            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseUp && draggingNode != null)
        {
            draggingNode = null;
        }
        else if (Event.current.type == EventType.MouseUp && draggingCanvas)
        {
            draggingCanvas = false;
        }
    }


    private void DrawNode(DialogueNode node)
    {
        GUIStyle style = nodeStyle;
        if (node.IsPlayerSpeaking())
        {
            style = playerNodeStyle;
        }

        GUILayout.BeginArea(node.GetRect(), style);

        node.SetText(EditorGUILayout.TextField(node.GetText()));

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Delete"))
        {
            deletingNode = node;
        }

        DrawLinkButtons(node);

        if (GUILayout.Button("Create"))
        {
            creatingNode = node;
        }

        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    private void DrawLinkButtons(DialogueNode node)
    {
        if (linkingParentNode == null)
        {
            if (GUILayout.Button("link"))
            {
                linkingParentNode = node;
            }
        }
        else if (linkingParentNode == node)
        {
            if (GUILayout.Button("cancel"))
            {
                linkingParentNode = null;
            }
        }
        else if (linkingParentNode.GetChildren().Contains(node.name))
        {
            if (GUILayout.Button("unlink"))
            {
                linkingParentNode.RemoveChild(node.name);
                linkingParentNode = null;
            }
        }
        else
        {
            if (GUILayout.Button("child"))
            {
                linkingParentNode.AddChild(node.name);
                linkingParentNode = null;
            }
        }
    }

    private void DrawConnections(DialogueNode node)
    {
        Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
        foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
        {
            Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
            Vector3 controlPointOffset = endPosition - startPosition;
            controlPointOffset.y = 0;
            controlPointOffset.x *= 0.8f;
            Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset,
                endPosition - controlPointOffset, Color.white, null, 4f);
        }
    }

    private DialogueNode GetNodeAtPoint(Vector2 point)
    {
        DialogueNode foundNode = null;
        foreach (DialogueNode node in selectedDialogue.GetAllNodes())
        {
            if (node.GetRect().Contains(point))
            {
                foundNode = node;
            }
        }

        return foundNode;
    }
}