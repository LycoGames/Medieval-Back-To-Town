using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] Text damageText = null;

    public void DestroyText()
    {
       // Destroy(gameObject);
    }

    public void SetDamageTextValue(float dmg)
    {
        damageText.text = string.Format("{0:0}", dmg);
    }
}
