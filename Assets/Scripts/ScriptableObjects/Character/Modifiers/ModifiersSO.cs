using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModifiersSO : ScriptableObject
{
    public abstract void ApplyModifier(GameObject character, int value);
}
