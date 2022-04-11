using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpDisplay : MonoBehaviour
{
    Experience experience;
    // Start is called before the first frame update
    void Start()
    {
        experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = string.Format("{0:0}/{1:0}", experience.GetPoints(), experience.GetMaxExp());
    }
}