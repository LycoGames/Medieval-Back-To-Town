using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    LazyValue<float> mana;
    ManaBar manaBar;

    private float tempMana = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        mana = new LazyValue<float>(GetMaxMana);
        manaBar = GetComponent<ManaBar>();
    }

    void Start()
    {
        manaBar.SetMaxMana(GetMaxMana());
        GetComponent<BaseStats>().onLevelUp += SetNewMaxMana;
    }

    /*
        void OnEnable()
        {
            GetComponent<BaseStats>().onLevelUp += SetNewMaxMana;
        }
        void OnDisable()
        {
            GetComponent<BaseStats>().onLevelUp -= SetNewMaxMana;
        }
    */

    void Update()
    {
        if (mana.value < GetMaxMana())
        {
            mana.value += GetRegenRate() * Time.deltaTime;
            if (mana.value >= GetMaxMana())
            {
                mana.value = GetMaxMana();
            }

            manaBar.SetMana(GetMana());
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // print("mana: " + mana.value);
            // print("max mana: " + GetMaxMana());
        }
    }

    public void SetNewMaxMana()
    {
        // print("ilk satır");
        manaBar.SetMaxMana(GetComponent<BaseStats>().GetStat(Stat.Mana));
        // print("maxmana " + GetComponent<BaseStats>().GetStat(Stat.Mana));
    }

    public void AddMana(float value)
    {
        mana.value = Mathf.Min(mana.value + value, GetMaxMana());
        manaBar.SetMana(mana.value);
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