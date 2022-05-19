using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatsListUI : MonoBehaviour
{
    [SerializeField] private StatsItemUI statPrefab;
    private StatsEquipableItem item;

    private const string STAT_DAMAGE_TEXT = "Damage";
    private const string STAT_DEFENCE_TEXT = "Defence";
    private const string STAT_HEALTH_TEXT = "Health";
    private const string STAT_MANA_TEXT = "Mana";
    private const string STAT_ABILITYPOWER_TEXT = "Ability Power";
    private const string STAT_EXPERIENCEREWARD_TEXT = "Experience Reward";
    private const string STAT_MOVEMENTSPEED_TEXT = "Movement Speed";
    private const string STAT_MANAREGENRATE_TEXT = "Mana Regen Rate";
    private const string STAT_TOTALTRAITPOINTS_TEXT = "Total Trait Points";
    private const string STAT_EXPERIENCETOLEVELUP_TEXT = "Experience to Levelup";
    private const string STAT_CRITICCHANCE_TEXT = "Critic Chance";
    private const string STAT_ACCURACY_TEXT = "Accuracy";
    // Start is called before the first frame update


    public void SetItem(StatsEquipableItem item)
    {
        this.item = item;
        Redraw();
    }

    private void Redraw()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }

        if (item == null)
        {
            Debug.LogError("Item Boş!!!");
            return;
        }
        if (item.GetAdditiveModifiers(Stat.Damage).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.Damage))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_DAMAGE_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.Defence).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.Defence))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_DEFENCE_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.CriticChance).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.CriticChance))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_CRITICCHANCE_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.Accuracy).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.Accuracy))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_ACCURACY_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.Health).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.Health))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_HEALTH_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.Mana).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.Mana))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_MANA_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.AbilityPower).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.AbilityPower))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_ABILITYPOWER_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.ExperienceReward).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.ExperienceReward))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_EXPERIENCEREWARD_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.MovementSpeed).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.MovementSpeed))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_MOVEMENTSPEED_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.ManaRegenRate).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.ManaRegenRate))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_MANAREGENRATE_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.TotalTraitPoints).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.TotalTraitPoints))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_TOTALTRAITPOINTS_TEXT, value);
            }
        }

        if (item.GetAdditiveModifiers(Stat.ExperienceToLevelUp).Any())
        {
            foreach (var value in item.GetAdditiveModifiers(Stat.ExperienceToLevelUp))
            {
                StatsItemUI uiInstance = Instantiate<StatsItemUI>(statPrefab, transform);
                uiInstance.Setup(STAT_EXPERIENCETOLEVELUP_TEXT, value);
            }
        }

        /*foreach (QuestStatus status in questList.GetStatuses())
        {
            QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, transform);
            uiInstance.Setup(status);
        }*/
    }
}