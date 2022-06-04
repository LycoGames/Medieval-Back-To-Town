using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpDisplay : MonoBehaviour
{
    Experience experience;
    BaseStats baseStats;
    private TMP_Text text;
    float requiredEXPToLevelUp;
    
    // Start is called before the first frame update
    void Start()
    {
        experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
        text = GetComponent<TMP_Text>();
        baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
        requiredEXPToLevelUp = experience.GetMaxExp();
        baseStats.onLevelUp += Reset;
    }

    void Reset()
    {
        requiredEXPToLevelUp = experience.GetMaxExp() - experience.GetPoints();
    }

    // Update is called once per frame
    void Update()
    {
        float percentage = (experience.expPointsCurrentToPrevious() / requiredEXPToLevelUp);
        text.text = percentage.ToString("P1", CultureInfo.InvariantCulture);
        // text.text = string.Format("{0:0}/{1:0}", experience.GetPoints(), experience.GetMaxExp());
    }
}