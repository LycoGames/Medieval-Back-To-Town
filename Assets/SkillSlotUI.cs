using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{

    [SerializeField] Image cooldownOverlay = null;
    CooldownStore cooldownStore;
    [SerializeField] Ability ability;
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cooldownStore = player.GetComponent<CooldownStore>();
    }

    private void Update()
    {
        if (ability)
        {
            cooldownOverlay.fillAmount = cooldownStore.GetFractionRemaining(ability);
        }

    }
}
