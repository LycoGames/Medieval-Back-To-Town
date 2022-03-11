using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour
{
    [SerializeField] float maxMana;
    [SerializeField] float _mana;
    float _manaRegenarete = 2;

    // Start is called before the first frame update
    void Awake()
    {
        _mana = maxMana;
    }

    void Update()
    {
        if (_mana < maxMana)
        {
            _mana += _manaRegenarete * Time.deltaTime;
            if (_mana >= maxMana)
            {
                _mana = maxMana;
            }
        }
    }

    public float GetMaxMana()
    {
        return maxMana;
    }

    public float GetMana()
    {
        return _mana;
    }

    public bool UseMana(float manaToUse)
    {
        if (manaToUse > _mana)
        {
            return false;
        }
        _mana -= manaToUse;
        return true;
    }
}