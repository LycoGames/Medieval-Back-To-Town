using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffinCounterText : MonoBehaviour
{
    [SerializeField] GameObject text;

    float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 3f)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void ShowCoffinCounterText(int counter)
    {
        timer = 0;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        text.gameObject.GetComponent<Text>().text = string.Format("Fugitive Soul Found " + counter.ToString());
    }
}
