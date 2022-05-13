using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupAlert : MonoBehaviour
{

    public void SetText(string name)
    {
        string itemName = name;
        GetComponent<Text>().text = string.Format("Edindin: " + itemName);
    }

}
