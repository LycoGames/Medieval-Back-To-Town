using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenedCoffinCounter : MonoBehaviour
{
    CoffinCounterText text;
    public static int counter;
    [SerializeField] bool playerHasTouchedOnMeBefore = false;

    void Start()
    {
        text = FindObjectOfType<CoffinCounterText>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (playerHasTouchedOnMeBefore == false)
            {
                OpenedCoffinCounter.counter++;
                playerHasTouchedOnMeBefore = true;
                text.ShowCoffinCounterText(counter);
            }
        }
    }

    public int GetOpenedCoffinCounter()
    {
        return counter;
    }
}
