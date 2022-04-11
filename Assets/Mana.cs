using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{

    LazyValue<float> mana;

    // Start is called before the first frame update
    void Awake()
    {
        mana = new LazyValue<float>(GetMaxMana);
    }

    void Update()
    {
        if (mana.value < GetMaxMana())
        {
            mana.value += GetRegenRate() * Time.deltaTime;
            if (mana.value >= GetMaxMana())
            {
                mana.value = GetMaxMana();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("mana: " + mana.value);
            print("max mana: " + GetMaxMana());
        }
    }

    public float GetMaxMana()
    {
        return GetComponent<BaseStats>().GetStat(Stat.Mana);
    }

    public float GetMana()
    {
        return mana.value;
    }

    public float GetRegenRate()
    {
        return GetComponent<BaseStats>().GetStat(Stat.ManaRegenRate);
    }

    public bool UseMana(float manaToUse)
    {
        if (manaToUse > mana.value)
        {
            return false;
        }
        mana.value -= manaToUse;
        return true;
    }
}