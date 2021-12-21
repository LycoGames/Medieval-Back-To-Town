using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using System;



public class DialogueEditor : EditorWindow
{

    Dialogue selectedDialogue = null;
    GUIStyle nodeStyle;
    DialogueNode draggingNode = null;
    Vector2 draggingOffset;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }

    [OnOpenAsset(1)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue; //dialog scripti var mı yoksa null göndericek
        if (dialogue != null)
        {
            //ShowEditorWindow();
            ShowThisWindow(dialogue);
            return true;
        }
        return false;
    }
    public static void ShowThisWindow(Dialogue dialogue)
    {
        DialogueEditor window = (DialogueEditor)GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        window.selectedDialogue = dialogue;
    }

    private void OnEnable()
    {//https://docs.unity3d.com/ScriptReference/Selection.html
     // Her obje degisiminde OnSelecitionChanged calıstırılacak.
        Selection.selectionChanged += OnSelectionChanged; //fonksiyonu listeye ekledigimden () yapmıyorum.... unutma 

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        // nodeStyle.normal.background = Texture2D.grayTexture;
        nodeStyle.normal.textColor = Color.blue; // calısmıyor ? :D
        nodeStyle.padding = new RectOffset(20, 20, 20, 20);
        nodeStyle.border = new RectOffset(10, 10, 10, 10);

    }



    private void OnSelectionChanged()
    {
        Dialogue newDialogue = Selection.activeObject as Dialogue; // bu dialogue tipinde mi ?
        if (newDialogue != null)
        {
            selectedDialogue = newDialogue;
            Repaint();
        }
    }

    //https://docs.unity3d.com/ScriptReference/EditorGUILayout.html
    //https://docs.unity3d.com/ScriptReference/EditorGUI.html
    void OnGUI() //update tarzı bir fonksiyon update kadar çok çalışmıyor editöre tıklancıkta bir kaç kez calısıır.
    {
        if (selectedDialogue == null)
        {
            EditorGUILayout.LabelField("No dialogue selected");
        }

        else
        {
            ProcessEvents();
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                OnGuiNode(node);
            }

        } 

        // EditorGUI.LabelField(new Rect(10,10,200,200),"AN Apple"); //üste üste gelebilriler pozzisyonları iyi vermen lazım
        // EditorGUILayout.LabelField("Apple"); //editör içine yazılanlar. layout normalinden farklı auto yapıyor.
    }


    private void ProcessEvents()
    {
        if (Event.current.type == EventType.MouseDown && draggingNode==null)
        {
            draggingNode = GetNodeAtPoint(Event.current.mousePosition);
            if (draggingNode != null)
            {
                draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
            }
        }
        else if (Event.current.type == EventType.MouseDrag && draggingNode !=null)
        {
            Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
            draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseUp && draggingNode!=null)
        {
            draggingNode = null;
        }
    }

    private DialogueNode GetNodeAtPoint(Vector2 point)
    {
        DialogueNode foundNode = null;
        foreach (DialogueNode node in selectedDialogue.GetAllNodes())
        {
            if (node.rect.Contains(point))
            {
                foundNode = node;
            }
        }
        return foundNode;
    }

    private void OnGuiNode(DialogueNode node)
    {
        GUILayout.BeginArea(node.rect, nodeStyle);
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel); //node yazısını beyaz
        string newText = EditorGUILayout.TextField(node.text);
        string newUniqueID = EditorGUILayout.TextField(node.uniqueID);

        if (EditorGUI.EndChangeCheck()) //degisik var mı yok mu bool döndürüyor.
        {
            Undo.RecordObject(selectedDialogue, "Update Dialogue Text"); //yapılan degisiklikleri kayıt için

            node.text = newText;
            node.uniqueID = newUniqueID;
        }
        GUILayout.EndArea();
    }
}
