using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    void AddEnemy(GameObject enemy);
    void RemoveEnemy(GameObject enemy);
}