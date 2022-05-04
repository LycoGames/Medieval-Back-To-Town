using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPredicateNumberEvaluator
{
    int? Evaluate(string predicate, string[] parameters);
}