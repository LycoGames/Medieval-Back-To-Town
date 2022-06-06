using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupAlert : MonoBehaviour
{
    [SerializeField] GameObject text;

    float timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            //text.SetActive(false);
        }
    }
    public void SetText(string name)
    {
     //   text.SetActive(true);
        timer = 0;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        string itemName = name;
        text.GetComponent<Text>().text = string.Format("You have obtained: " + itemName);
    }

}
