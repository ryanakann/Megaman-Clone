using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimationCriterion {
    public string property;
    public string comparator;
    public float value;
}

[System.Serializable]
public class AnimationTransition {
    public string entryState;
    public string exitState;
    public AnimationCriterion[] conditions;

    public bool Evaluate (AnimatorController controller) {
        foreach(AnimationCriterion condition in conditions) {
            // Debug.Log($"\t{condition.property}{condition.comparator}{condition.value}...");
            System.Type type = controller.GetPropertyType(condition.property);
            if (type == typeof(int)) {
                if (false == Evaluate(controller.GetInt(condition.property), condition)) {
                    Debug.Log($"\t{condition.property} {condition.comparator} {condition.value} not met...");
                    return false;
                }
            } else if (type == typeof(float)) {
                if (false == Evaluate(controller.GetFloat(condition.property), condition)) {
                    Debug.Log($"\t{condition.property} {condition.comparator} {condition.value} not met...");
                    return false;
                }
            } else if (type == typeof(bool)) {
                if (false == Evaluate(controller.GetBool(condition.property), condition)) {
                    Debug.Log($"\t{condition.property} {condition.comparator} {condition.value} not met...");
                    return false;
                }
            }
        }
        Debug.Log($"Switching from {entryState} to {exitState}!");
        return true;
    }

    private bool Evaluate (int value, AnimationCriterion criterion) {
        switch(criterion.comparator) {
            case "=": //equal to
                return value == (int)criterion.value;
            case "==":
                return value == (int)criterion.value;
            case "<": //less than
                return value < (int)criterion.value;
            case "<=": //less than or equal to
                return value <= (int)criterion.value;
            case ">": //greater than
                return value > (int)criterion.value;
            case ">=": //greater than or equal to
                return value >= (int)criterion.value;
        }
        return false;
    }

    private bool Evaluate (float value, AnimationCriterion criterion) {
        switch(criterion.comparator) {
            case "=": //equal to
                return value == (float)criterion.value;
            case "==":
                return value == (float)criterion.value;
            case "<": //less than
                return value < (float)criterion.value;
            case "<=": //less than or equal to
                return value <= (float)criterion.value;
            case ">": //greater than
                return value > (float)criterion.value;
            case ">=": //greater than or equal to
                return value >= (float)criterion.value;
        }
        return false;
    }

    private bool Evaluate (bool value, AnimationCriterion criterion) {
        return (value == false && criterion.value <= 0f ) || (value == true && criterion.value > 0f);
    }
}