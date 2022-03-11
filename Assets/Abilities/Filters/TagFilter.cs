using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Tag Filter", menuName = "Abilities/Filters/Tag", order = 0)]
public class TagFilter : FilterStrategy
{
    [SerializeField] string tagToFilter = "";

    public override IEnumerable<GameObject> Filter(IEnumerable<GameObject> objectsToFilter)
    {
        foreach (var gameObject in objectsToFilter)
        {
            if (gameObject.CompareTag(tagToFilter))
            {
                yield return gameObject;
            }
        }
    }

}