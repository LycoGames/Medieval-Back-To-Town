using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField] ProgressionCharacterClass[] characterClasses = null;

    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

    public float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
        BuildLookup();

        float[] levels = lookupTable[characterClass][stat];

        if (levels.Length == 0)
        {
            return 0;
        }
        if (levels.Length < level)
        {
            return levels[levels.Length - 1];
        }

        return levels[level - 1];
    }

    public int GetLevels(Stat stat, CharacterClass characterClass)
    {
        BuildLookup();

        float[] levels = lookupTable[characterClass][stat];
        return levels.Length;
    }

    private void BuildLookup()
    {
        if (lookupTable != null) return;

        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

        foreach (ProgressionCharacterClass progressionClass in characterClasses)
        {
            var statLoookupTable = new Dictionary<Stat, float[]>();

            foreach (ProgressionStat progressionStat in progressionClass.stats)
            {
                statLoookupTable[progressionStat.stat] = progressionStat.levels;
            }

            lookupTable[progressionClass.characterClass] = statLoookupTable;
        }
    }

    [System.Serializable]
    class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public ProgressionStat[] stats;
        //public float[] health;
    }

    [System.Serializable]
    class ProgressionStat
    {
        public Stat stat;
        public float[] levels;
    }
}
