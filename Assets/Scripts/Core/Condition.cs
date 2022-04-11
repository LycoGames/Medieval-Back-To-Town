using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Condition
{
    [SerializeField] private Disjunction[] and;

    public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
    {
        foreach (var dis in and)
        {
            if (!dis.Check(evaluators))
            {
                return false;
            }
        }

        return true;
    }

    [System.Serializable]
    class Disjunction
    {
        [SerializeField] private Predicate[] or;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (var pred in or)
            {
                if (pred.Check(evaluators))
                {
                    return true;
                }
            }

            return false;
        }
    }

    [System.Serializable]
    class Predicate
    {
        [SerializeField] private string predicate;
        [SerializeField] private string[] parameters;
        [SerializeField] private bool negate = false;

        public bool Check(IEnumerable<IPredicateEvaluator> evaluators)
        {
            foreach (var evaluator in evaluators)
            {
                bool? result = evaluator.Evaluate(predicate, parameters);
                if (result == null)
                {
                    continue;
                }

                if (result == negate) return false;
            }

            return true;
        }
    }
}