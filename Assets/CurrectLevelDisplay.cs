using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrectLevelDisplay : MonoBehaviour
{
    Text text;
    BaseStats baseStats;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = baseStats.GetLevel().ToString();
    }
}
