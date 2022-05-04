using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTargetText : MonoBehaviour
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

    public void ShowNoTargetText()
    {
        timer = 0;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }
}
