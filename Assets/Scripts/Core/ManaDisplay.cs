using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaDisplay : MonoBehaviour
{
    Mana mana;
    // Start is called before the first frame update
    void Start()
    {
        mana = GameObject.FindGameObjectWithTag("Player").GetComponent<Mana>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = string.Format("{0:0}/{1:0}", mana.GetMana(), mana.GetMaxMana());
    }
}
