using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaDisplay : MonoBehaviour
{
    Mana mana;

    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        mana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
    }
}