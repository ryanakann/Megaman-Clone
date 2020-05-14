using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationPropertyType {
    Int,
    Float,
    Bool,
    Trigger,
}

[System.Serializable]
public class AnimationProperty {
    public string name;
    public AnimationPropertyType type;
    public float value;
}