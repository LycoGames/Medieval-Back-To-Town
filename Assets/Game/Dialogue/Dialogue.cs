using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
public class Dialogue : ScriptableObject
{
    [SerializeField]
    List<DialogueNode> nodes; //list arraye göre daha kolay .add

#if UNITY_EDITOR //sadece editorde calısırken calısması için. Oyundayken calısmayacak.
    void Awake()
    {
        if (nodes.Count == 0)
        {
            nodes.Add(new DialogueNode()); //oto node ekleyecek 

        }
    }
#endif

    public IEnumerable<DialogueNode> GetAllNodes() //ienumerable ile array içinde dönmek gibi bir derdim yok.
    {
        return nodes;
    }

    public DialogueNode GetRootNode()
    {
        return nodes[0];
    }

}
